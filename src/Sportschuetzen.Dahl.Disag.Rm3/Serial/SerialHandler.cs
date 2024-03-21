using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Serial;

/// <summary>
///     Handles the serial connection to the Disag RM3
/// </summary>
public class SerialHandler : IDisposable
{
	/// <summary>
	///     Event that is triggered when the connection state changes
	/// </summary>
	public event EventHandler<bool>? OnConnectionChanged
	{
		add => _serialConnection.ConnectionChanged += value;
		remove => _serialConnection.ConnectionChanged -= value;
	}

	/// <summary>
	///     Event that is triggered when data is received
	/// </summary>
	public event EventHandler<DisagResponse>? OnDataReceived
	{
		add => _serialConnection.OnDataReceived += value;
		remove => _serialConnection.OnDataReceived -= value;
	}

	/// <summary>
	///     Event that is triggered when hex data is received
	/// </summary>
	public event EventHandler<EDisagHex>? OnHexReceived
	{
		add => _serialConnection.OnHexReceived += value;
		remove => _serialConnection.OnHexReceived -= value;
	}

	private readonly SerialConnection _serialConnection;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="comPort"></param>
	public SerialHandler(string comPort)
	{
		_serialConnection = new SerialConnection(comPort);
		_serialConnection.SetBaud(EDisagBaudRate.B38400);
	}

	/// <summary>
	///     Connects to the Disag
	/// </summary>
	/// <returns></returns>
	public bool Connect()
	{
		return _serialConnection.Connect();
	}

	/// <summary>
	///     Disconnects from the Disag
	/// </summary>
	/// <returns></returns>
	public bool Disconnect()
	{
		return _serialConnection.Disconnect();
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_serialConnection.Dispose();
	}

	/// <summary>
	///     Sends data to the Disag
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public async Task Send(string data)
	{
		await Send(EDisagHex.ENQ);
		var dataAsByteArray = data.ToByteArray(true);

		this.Debug("Sending STRING: " + data);
		await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudRate.B38400, true);
	}

	/// <summary>
	///     Sends a command to the Disag
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	public async Task Send(EDisagCommand command)
	{
		await Send(EDisagHex.ENQ);
		var dataAsByteArray = command.ToString().ToByteArray(true);

		this.Debug("Sending command: " + command);
		await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudRate.B38400, true);
	}

	/// <summary>
	///     Sends the EDI command to the Disag
	/// </summary>
	/// <param name="ediTotalShots"></param>
	/// <param name="ediShotsLastStrip"></param>
	/// <returns></returns>
	public async Task Send(int ediTotalShots, int ediShotsLastStrip)
	{
		await Send(EDisagHex.ENQ);
		var edi = $"{EDisagCommand.EDI}={ediTotalShots};{ediShotsLastStrip}";
		var dataAsByteArray = edi.ToByteArray(true);

		this.Debug("Sending command: " + edi);
		await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudRate.B38400, true);
	}

	/// <summary>
	///     Sends the MODE command to the Disag
	/// </summary>
	/// <param name="mode"></param>
	/// <returns></returns>
	public async Task Send(EDisagMode mode)
	{
		var stringedMode = mode.ToString();
		var dataAsByteArray = stringedMode.ToByteArray(false);

		this.Debug("Sending MODE: " + mode);
		await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudRate.B2400, true);
	}

	/// <summary>
	///     Sends a HEX to the Disag
	/// </summary>
	/// <param name="hex"></param>
	/// <returns></returns>
	public async Task Send(EDisagHex hex)
	{
		this.Debug("Sending HEX: " + hex);
		var waitForStx = hex == EDisagHex.ENQ;

		await _serialConnection.WriteToSerial(new[] { (byte)hex }, EDisagBaudRate.B38400, false, waitForStx);
	}
}