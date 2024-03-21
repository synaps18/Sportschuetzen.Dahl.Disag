using Serilog;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Evaluation;
using Sportschuetzen.Dahl.Disag.Rm3.Sequences;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3;

/// <inheritdoc />
public class DisagRm3 : IDisagRm3
{
	/// <inheritdoc />
	public event EventHandler<bool>? ConnectionChanged
	{
		add => _serialHandler.OnConnectionChanged += value;
		remove => _serialHandler.OnConnectionChanged -= value;
	}

	/// <inheritdoc />
	public event EventHandler<bool>? IsWorkingChanged;

	private const string LOG_FILE = @"C:\ProgramData\DisagRm3\log.txt";

	private readonly SerialHandler _serialHandler;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="comPort"> Comport to use for disag </param>
	public DisagRm3(string comPort)
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File(LOG_FILE)
			.CreateLogger();

		_serialHandler = new SerialHandler(comPort);
	}

	/// <inheritdoc />
	public async Task CancelAsync()
	{
		using var sequence = new CancelSequence(_serialHandler);
		await RunSequence(sequence);
	}

	/// <inheritdoc />
	public bool Connect()
	{
		return _serialHandler.Connect();
	}

	/// <inheritdoc />
	public bool Disconnect()
	{
		return _serialHandler.Disconnect();
	}

	/// <inheritdoc />
	public async Task EndAsync()
	{
		using var sequence = new EndSequence(_serialHandler);
		await RunSequence(sequence);
	}

	/// <inheritdoc />
	public async Task FEr()
	{
		await _serialHandler.Send(EDisagMode.W);
	}

	/// <inheritdoc />
	public async Task FErn()
	{
		await _serialHandler.Send(EDisagMode.V);
	}

	/// <inheritdoc />
	public async Task<string> GetSerialAsync()
	{
		using var sequence = new NumberSequence(_serialHandler);
		var data = await RunSequence(sequence);
		return data;
	}

	/// <inheritdoc />
	public async Task<DisagSeries> GetSeries(SeriesParameter parameter)
	{
		using var sequence = new StripSequence(_serialHandler, parameter);
		var data = await RunSequence(sequence);
		return data;
	}

	/// <inheritdoc />
	public async Task<string> GetTypeAsync()
	{
		using var sequence = new MachineTypeSequence(_serialHandler);
		var data = await RunSequence(sequence);
		return data;
	}

	/// <inheritdoc />
	public async Task RepeatAsync()
	{
		await _serialHandler.Send(EDisagCommand.WID);
	}

	protected virtual void OnIsWorkingChanged(bool e)
	{
		IsWorkingChanged?.Invoke(this, e);
	}

	private async Task<T> RunSequence<T>(Sequence<T> sequence)
	{
		OnIsWorkingChanged(true);
		var data = await sequence.RequestData();
		OnIsWorkingChanged(false);
		return data;
	}
}