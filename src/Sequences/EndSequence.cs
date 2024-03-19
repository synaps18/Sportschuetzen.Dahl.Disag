using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Serial;
using Sportschützen.Dahl.DisagRm3.Structs;

namespace Sportschützen.Dahl.DisagRm3.Sequences;

internal class EndSequence : SerialSequence<string>
{
    private string? _receivedData;

    public EndSequence(SerialWrapper serialWrapper) : base(serialWrapper)
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
        await SerialWrapper.Send(EDisagBefehle.END);

        await AwaitDataAsync();

        return _receivedData!;
    }
}