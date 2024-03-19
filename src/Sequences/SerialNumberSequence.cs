using Sportschützen.Dahl.DisagRm3.Constants;
using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Extensions;
using Sportschützen.Dahl.DisagRm3.Serial;
using Sportschützen.Dahl.DisagRm3.Structs;

namespace Sportschützen.Dahl.DisagRm3.Sequences;

internal class SerialNumberSequence : SerialSequence<string>
{
    private string? _receivedSerialNumer;

    public SerialNumberSequence(SerialWrapper serialWrapper) : base(serialWrapper)
    {
    }

    protected override async void SerialWrapper_OnDataReceived(object? sender, DisagResponse e)
    {
        base.SerialWrapper_OnDataReceived(sender, e);

        if (e.Command == EDisagBefehle.SNR.ToString())
        {
            _receivedSerialNumer = e.Parameter;
        }
        //Warum auch immer, es kommt immer nach der Seriennummer ein WSC command... Deshalb hier dann abbrechen!
        else if (e.Command == DISAG_Befehle_Empfangen.WSC)
        {
            await Task.Delay(200);
            await SerialWrapper.Send(EDisagBefehle.ABR);

            ReleaseAwaitingData();
        }
        else
        {
            this.Error($"Not expected command: {e.Command}");
            throw new Exception($"Not expected command: {e.Command}!");
        }
    }

    protected override async Task<string> SequenceToCall()
    {
        await SerialWrapper.Send(EDisagBefehle.SNR);

        await AwaitDataAsync();

        return _receivedSerialNumer!;
    }
}