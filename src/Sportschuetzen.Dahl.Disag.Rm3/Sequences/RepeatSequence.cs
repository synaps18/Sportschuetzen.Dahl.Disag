using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class RepeatSequence : SerialSequence<string>
{
    private string? _receivedData;

    public RepeatSequence(SerialWrapper serialWrapper) : base(serialWrapper)
    {
    }

    protected override void SerialWrapper_OnDataReceived(object? sender, DisagResponse e)
    {
        base.SerialWrapper_OnDataReceived(sender, e);

        _receivedData = !string.IsNullOrEmpty(e.Parameter) ? e.Parameter : e.Command;
        ReleaseAwaitingData();
    }

    protected override async Task<string> SequenceToCall()
    {
        await SerialWrapper.Send(EDisagBefehle.WID);

        await AwaitDataAsync();

        return _receivedData!;
    }
}