using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class CancelSequence : SerialSequence<string>
{
    public CancelSequence(SerialHandler serialHandler) : base(serialHandler)
    {
        SerialHandler.OnHexReceived += SerialHandlerOnHexReceived;
    }

    public override void Dispose()
    {
        SerialHandler.OnHexReceived -= SerialHandlerOnHexReceived;
        base.Dispose();
    }

    private void SerialHandlerOnHexReceived(object? sender, EDisagHex e)
    {
        if (e == EDisagHex.ACK) ReleaseAwaitingData();
    }

    protected override async Task<string> SequenceToCall()
    {
        await SerialHandler.Send(EDisagBefehle.ABR);

        await AwaitDataAsync();

        return string.Empty;
    }
}