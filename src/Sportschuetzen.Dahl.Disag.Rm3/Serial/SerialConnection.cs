using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;
using Timer = System.Timers.Timer;

namespace Sportschuetzen.Dahl.Disag.Rm3.Serial;

/// <summary>
///     Represents a serial connection to the Disag RM3
/// </summary>
public class SerialConnection : IDisposable
{
	/// <summary>
	///     Event that is triggered when the connection state changes
	/// </summary>
	public event EventHandler<bool>? ConnectionChanged;

	/// <summary>
	///     Event that is triggered when data is received
	/// </summary>
	public event EventHandler<DisagResponse>? OnDataReceived;

	/// <summary>
	///     Event that is triggered when hex data is received
	/// </summary>
	public event EventHandler<EDisagHex>? OnHexReceived;

	private readonly CancellationTokenSource _awaitStxSource = new();
	private readonly SemaphoreSlim _semaphore = new(1, 1);
	private readonly SerialPort _serialPort;

	private readonly Timer _observeConnectionTimer = new()
	{
		Interval = 100
	};

	private bool _lastConnectionState;
	private string _rawData = string.Empty;
	private Task? _awaitStxTask;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="comPort"></param>
	public SerialConnection(string comPort)
	{
		_serialPort = new SerialPort(comPort)
		{
			Parity = Parity.None,
			StopBits = StopBits.One,
			DataBits = 8,
			Handshake = Handshake.None
		};

		_serialPort.DataReceived += SerialPort_DataReceived;
		_serialPort.ErrorReceived += SerialPort_OnErrorReceived;
		_serialPort.PinChanged += SerialPort_OnPinChanged;

		_observeConnectionTimer.Elapsed += ObserveConnectionTimerOnElapsed;
		_observeConnectionTimer.Start();

		Initialize();
	}

	/// <summary>
	///     Connects to the Disag
	/// </summary>
	/// <returns></returns>
	public bool Connect()
	{
		if (_serialPort.IsOpen) _serialPort.Close();

		_serialPort.Open();
		return _serialPort.IsOpen;
	}

	/// <summary>
	///     Disconnects from the Disag
	/// </summary>
	/// <returns></returns>
	public bool Disconnect()
	{
		if (!_serialPort.IsOpen) return true;

		_serialPort.Close();
		return !_serialPort.IsOpen;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_serialPort.Close();
		_serialPort.DataReceived -= SerialPort_DataReceived;
		_serialPort.ErrorReceived -= SerialPort_OnErrorReceived;
		_serialPort.PinChanged -= SerialPort_OnPinChanged;
		_serialPort.Dispose();

		this.Debug("Disposed!");
	}

	/// <summary>
	///     Sets the baud rate of the serial port
	/// </summary>
	/// <param name="baud"></param>
	public void SetBaud(EDisagBaudRate baud)
	{
		if (_serialPort.BaudRate == (int)baud) return;

		this.Info($"Setting BauRate to {(int)baud}, old Baud: {_serialPort.BaudRate}");

		try
		{
			if (_serialPort.IsOpen) _serialPort.Close();

			_serialPort.BaudRate = (int)baud;
		}
		catch (Exception exception)
		{
			this.Error("Failed to set baud!", exception);
			throw;
		}
	}

	/// <summary>
	///     Writes data to the serial port
	/// </summary>
	/// <param name="dataToSend"></param>
	/// <param name="baud"></param>
	/// <param name="cr"></param>
	/// <param name="awaitStx"></param>
	/// <returns></returns>
	public async Task WriteToSerial(byte[] dataToSend, EDisagBaudRate baud, bool cr, bool awaitStx = false)
	{
		await _semaphore.WaitAsync();

		SetBaud(baud);

		if (cr)
			dataToSend = dataToSend.AddCr();

		try
		{
			if (!_serialPort.IsOpen)
				_serialPort.Open();


			this.Debug($"Writing to serial adapter [{_serialPort.PortName}], Baud [{_serialPort.BaudRate}], CR: {cr}");
			_serialPort.Write(dataToSend, 0, dataToSend.Length);

			if (awaitStx) await AwaitStxAsync();
		}
		catch (Exception exception)
		{
			this.Error("Failed to write data to serial port!", exception);
			throw;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	/// <summary>
	///     Awaits the STX (Start of Text) byte
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	private async Task AwaitStxAsync()
	{
		if (_awaitStxTask?.Status == TaskStatus.Running)
		{
			this.Error("Cannot wait two times for STX!");
			throw new Exception("Cannot wait two times for STX!");
		}

		_awaitStxTask = Task.Run(() =>
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			this.Debug("Awaiting STX");
			while (!_awaitStxSource.IsCancellationRequested)
			{
				if (stopwatch.ElapsedMilliseconds < 100) continue;
				this.Warning("STX not received!");
				return;
			}

			this.Debug("STX received!");
		});

		try
		{
			await _awaitStxTask.WaitAsync(CancellationToken.None);
		}
		catch (Exception e)
		{
			this.Error("Task was canceled while waiting on it!", e);
		}
	}

	private async void CheckReceivedBytes(byte[] data)
	{
		//1 BYTE => 1 Char

		foreach (var item in data)
			switch (item)
			{
				case (byte)EDisagHex.CR:
					this.Debug("CR received!");
					var response = _rawData.ParseToDisagResponse();

					await WriteToSerial(new[] { (byte)EDisagHex.ACK }, EDisagBaudRate.B38400, false);

					OnDataReceived?.Invoke(this, response);
					_rawData = string.Empty;
					break;
				case (byte)EDisagHex.ACK:
					this.Debug("ACK received!");
					OnHexReceived?.Invoke(this, EDisagHex.ACK);
					break;
				case (byte)EDisagHex.STX:
					this.Debug("STX received!");
					_awaitStxSource.Cancel();
					break;
				case (byte)EDisagHex.NAK:
					this.Debug("NAK received");
					throw new Exception("NAK Received!");
				case (byte)EDisagHex.EOT:
					this.Debug("EOT received");
					break;
				case (byte)EDisagHex.ENQ:
					this.Debug("ENQ received");
					break;
				default:
					this.Debug($"Data received: [{(char)item}]");
					_rawData += (char)item;
					break;
			}
	}

	private void Initialize()
	{
		if (_serialPort.IsOpen) _serialPort.Close();
		this.Debug("Initialized");
	}

	private void ObserveConnectionTimerOnElapsed(object? sender, ElapsedEventArgs e)
	{
		try
		{
			if (_serialPort.IsOpen == _lastConnectionState) return;

			_lastConnectionState = _serialPort.IsOpen;
			OnConnectionChanged(_serialPort.IsOpen);
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception);
		}
	}

	protected virtual void OnConnectionChanged(bool e)
	{
		ConnectionChanged?.Invoke(this, e);
	}

	private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		var sp = (SerialPort)sender;

		var buffer = new byte[sp.BytesToRead];

		sp.Read(buffer, 0, buffer.Length);

		CheckReceivedBytes(buffer);
	}

	private void SerialPort_OnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
	{
		//TODO This event is only there to determine whether an event is fired on the port if the connection is lost
		this.Warning($"SerialPortOnErrorReceived: Event type [{e.EventType}]");
	}

	private void SerialPort_OnPinChanged(object sender, SerialPinChangedEventArgs e)
	{
		//TODO This event is only there to determine whether an event is fired on the port if the connection is lost
		this.Warning($"SerialPortOnPinChanged: Event type [{e.EventType}]");
	}
}