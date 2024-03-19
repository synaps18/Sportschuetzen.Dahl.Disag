using Serilog;
using Sportschützen.Dahl.DisagRm3.Auswertung;
using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Sequences;
using Sportschützen.Dahl.DisagRm3.Serial;

namespace Sportschützen.Dahl.DisagRm3;

public class DisagRm3 : IDisagRm3
{
    private readonly SerialWrapper _serialWrapper;

    public event EventHandler<bool>? OnConnectionChanged
    {
        add => _serialWrapper.OnConnectionChanged += value;
        remove => _serialWrapper.OnConnectionChanged -= value;
    }

    public event EventHandler<bool>? IsWorkingChanged;

    public DisagRm3(string comPort)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("C:\\ProgramData\\SpSchManager\\log.txt")
            .CreateLogger();

        _serialWrapper = new SerialWrapper(comPort);
    }

    public bool Connect()
    {
	    return _serialWrapper.Connect();
    }

    public async Task CancelAsync()
    {
        using var sequence = new CancelSequence(_serialWrapper);
        var data = await RunSequence(sequence);
    }

    private async Task<T> RunSequence<T>(SerialSequence<T> sequence)
    {
        RaiseIsWorkingChanged(true);
        var data = await sequence.RequestData();
        RaiseIsWorkingChanged(false);
        return data;
    }

    public async Task EndAsync()
    {
        using var sequence = new EndSequence(_serialWrapper);
        var data = await RunSequence(sequence);
    }

    /// <summary>
    ///     Sets Disag to Win Mode
    /// </summary>
    public async Task FEr()
    {
        await _serialWrapper.Send(EDisagMode.W);
    }

    /// <summary>
    ///     Sets Disag to Win Mode
    /// </summary>
    public async Task FErn()
    {
        await _serialWrapper.Send(EDisagMode.V);
    }

    public async Task<string> GetSerialAsync()
    {
        using var sequence = new SerialNumberSequence(_serialWrapper);
        var data = await RunSequence(sequence);
        return data;
    }

    public async Task<string> GetTypeAsync()
    {
        using var sequence = new MachineTypeSequence(_serialWrapper);
        var data = await RunSequence(sequence);
        return data;
    }

    public async Task RepeatAsync()
    {
        await _serialWrapper.Send(EDisagBefehle.WID);
    }

    public async Task<DisagSerie> SetNewSeries(SeriesParameter parameter)
    {
        using var sequence = new StreifenSequence(_serialWrapper, parameter);
        var data = await RunSequence(sequence);
        return data;
    }

    protected virtual void RaiseIsWorkingChanged(bool e)
    {
        IsWorkingChanged?.Invoke(this, e);
    }
}