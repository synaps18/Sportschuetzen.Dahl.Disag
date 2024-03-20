using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Constants;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class MachineTypeSequence : SerialSequence<string>
{
    private string? _receivedMachineType;

    public MachineTypeSequence(SerialWrapper serialWrapper) : base(serialWrapper)
    {
    }

    protected override async void SerialWrapper_OnDataReceived(object? sender, DisagResponse e)
    {
        base.SerialWrapper_OnDataReceived(sender, e);

        if (e.Command == EDisagBefehle.TYP.ToString())
        {
            _receivedMachineType = e.Parameter;
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
        await SerialWrapper.Send(EDisagBefehle.TYP);

        await AwaitDataAsync();

        return _receivedMachineType!;
    }
}