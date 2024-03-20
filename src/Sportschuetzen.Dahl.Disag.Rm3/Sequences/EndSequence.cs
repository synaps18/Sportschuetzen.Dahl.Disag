using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class EndSequence : SerialSequence<string>
{
    private string? _receivedData;

    public EndSequence(SerialHandler serialHandler) : base(serialHandler)
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
        await SerialHandler.Send(EDisagBefehle.END);

        await AwaitDataAsync();

        return _receivedData!;
    }
}