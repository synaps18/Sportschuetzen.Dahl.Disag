using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Serial;

namespace Sportschützen.Dahl.DisagRm3.Sequences;

internal class CancelSequence : SerialSequence<string>
{
    public CancelSequence(SerialWrapper serialWrapper) : base(serialWrapper)
    {
        SerialWrapper.OnHexReceived += SerialWrapperOnHexReceived;
    }

    public override void Dispose()
    {
        SerialWrapper.OnHexReceived -= SerialWrapperOnHexReceived;
        base.Dispose();
    }

    private void SerialWrapperOnHexReceived(object? sender, EDisagHex e)
    {
        if (e == EDisagHex.ACK) ReleaseAwaitingData();
    }

    protected override async Task<string> SequenceToCall()
    {
        await SerialWrapper.Send(EDisagBefehle.ABR);

        await AwaitDataAsync();

        return string.Empty;
    }
}