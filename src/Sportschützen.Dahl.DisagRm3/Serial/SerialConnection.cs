using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Extensions;
using Sportschützen.Dahl.DisagRm3.Structs;

namespace Sportschützen.Dahl.DisagRm3.Serial;

public class SerialConnection : IDisposable
{
    private readonly CancellationTokenSource _awaitStxSource = new();

    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly SerialPort _serialPort;
    private string _rawData = string.Empty;
    private Task? _awaitStxTask;

    private readonly System.Timers.Timer _observeConnectiontimer = new()
    {
        Interval = 100
    };

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
        _serialPort.ErrorReceived += SerialPortOnErrorReceived;
        _serialPort.PinChanged += SerialPortOnPinChanged;

        _observeConnectiontimer.Elapsed += ObserveConnectiontimer_OnElapsed;
        _observeConnectiontimer.Start();

        Initialize();
    }       

    private bool _lastConnectionState;
    private void ObserveConnectiontimer_OnElapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            if (_serialPort.IsOpen == _lastConnectionState) return;

            _lastConnectionState = _serialPort.IsOpen;
            RaiseOnConnectionChanged(_serialPort.IsOpen);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    public void Dispose()
    {
        _serialPort.Close();
        _serialPort.Dispose();

        _serialPort.DataReceived -= SerialPort_DataReceived;
        _serialPort.ErrorReceived -= SerialPortOnErrorReceived;
        _serialPort.PinChanged -= SerialPortOnPinChanged;

        this.Debug("Disposed!");
    }

    public void SetBaud(EDisagBaudrate baud)
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

    public async Task WriteToSerial(byte[] dataToSend, EDisagBaudrate baud, bool cr, bool awaitStx = false)
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

                    await WriteToSerial(new[] { (byte)EDisagHex.ACK }, EDisagBaudrate.B_38400, false);

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

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var sp = (SerialPort)sender;

        var buffer = new byte[sp.BytesToRead];

        sp.Read(buffer, 0, buffer.Length);

        CheckReceivedBytes(buffer);
    }

    private void SerialPortOnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
        //TODO Dieses Event ist nur da um festzustellen, ob auf dem Port ein Event gefeuert wird, sollte die Verbinudng abreißen
        this.Warning($"SerialPortOnErrorReceived: Event type [{e.EventType}]");
    }

    private void SerialPortOnPinChanged(object sender, SerialPinChangedEventArgs e)
    {
        //TODO Dieses Event ist nur da um festzustellen, ob auf dem Port ein Event gefeuert wird, sollte die Verbinudng abreißen
        this.Warning($"SerialPortOnPinChanged: Event type [{e.EventType}]");
    }

    public event EventHandler<DisagResponse>? OnDataReceived;
    public event EventHandler<EDisagHex>? OnHexReceived;
    public event EventHandler<bool>? OnConnectionChanged;

    protected virtual void RaiseOnConnectionChanged(bool e)
    {
        OnConnectionChanged?.Invoke(this, e);
    }
}