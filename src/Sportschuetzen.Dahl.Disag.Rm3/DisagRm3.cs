using Serilog;
using Sportschuetzen.Dahl.Disag.Rm3.Auswertung;
using Sportschuetzen.Dahl.Disag.Rm3.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Sequences;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3;

/// <inheritdoc />
public class DisagRm3 : IDisagRm3
{
	/// <inheritdoc />
	public event EventHandler<bool>? IsWorkingChanged;

	/// <inheritdoc />
	public event EventHandler<bool>? OnConnectionChanged
	{
		add => _serialWrapper.OnConnectionChanged += value;
		remove => _serialWrapper.OnConnectionChanged -= value;
	}

	private readonly SerialWrapper _serialWrapper;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="comPort"> Comport to use for disag </param>
	public DisagRm3(string comPort)
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File("C:\\ProgramData\\DisagRm3\\log.txt")
			.CreateLogger();

		_serialWrapper = new SerialWrapper(comPort);
	}

	/// <inheritdoc />
	public async Task CancelAsync()
	{
		using var sequence = new CancelSequence(_serialWrapper);
		var data = await RunSequence(sequence);
	}

	/// <inheritdoc />
	public bool Connect()
	{
		return _serialWrapper.Connect();
	}

	/// <inheritdoc />
	public bool Disconnect()
	{
		return _serialWrapper.Disconnect();
	}

	/// <inheritdoc />
	public async Task EndAsync()
	{
		using var sequence = new EndSequence(_serialWrapper);
		var data = await RunSequence(sequence);
	}

	/// <inheritdoc />
	public async Task FEr()
	{
		await _serialWrapper.Send(EDisagMode.W);
	}

	/// <inheritdoc />
	public async Task FErn()
	{
		await _serialWrapper.Send(EDisagMode.V);
	}

	/// <inheritdoc />
	public async Task<DisagSerie> GetSeries(SeriesParameter parameter)
	{
		using var sequence = new StreifenSequence(_serialWrapper, parameter);
		var data = await RunSequence(sequence);
		return data;
	}

	/// <inheritdoc />
	public async Task<string> GetSerialAsync()
	{
		using var sequence = new SerialNumberSequence(_serialWrapper);
		var data = await RunSequence(sequence);
		return data;
	}

	/// <inheritdoc />
	public async Task<string> GetTypeAsync()
	{
		using var sequence = new MachineTypeSequence(_serialWrapper);
		var data = await RunSequence(sequence);
		return data;
	}

	/// <inheritdoc />
	public async Task RepeatAsync()
	{
		await _serialWrapper.Send(EDisagBefehle.WID);
	}

	protected virtual void RaiseIsWorkingChanged(bool e)
	{
		IsWorkingChanged?.Invoke(this, e);
	}

	private async Task<T> RunSequence<T>(SerialSequence<T> sequence)
	{
		RaiseIsWorkingChanged(true);
		var data = await sequence.RequestData();
		RaiseIsWorkingChanged(false);
		return data;
	}
}