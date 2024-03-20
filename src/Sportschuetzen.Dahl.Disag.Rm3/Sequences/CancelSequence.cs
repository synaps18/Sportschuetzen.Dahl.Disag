using Sportschuetzen.Dahl.Disag.Rm3.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

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