using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Constants;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class MachineTypeSequence : Sequence<string>
{
	private string? _receivedMachineType;

	public MachineTypeSequence(SerialHandler serialHandler) : base(serialHandler)
	{
	}

	protected override async Task<string> SequenceToCall()
	{
		await SerialHandler.Send(EDisagCommand.TYP);

		await AwaitDataAsync();

		return _receivedMachineType!;
	}

	protected override async void SerialHandler_OnDataReceived(object? sender, DisagResponse e)
	{
		base.SerialHandler_OnDataReceived(sender, e);

		if (e.Command == EDisagCommand.TYP.ToString())
		{
			_receivedMachineType = e.Parameter;
		}
		else if (e.Command == ReceiveCommandConstants.WSC)
		{
			await Task.Delay(200);
			await SerialHandler.Send(EDisagCommand.ABR);

			ReleaseAwaitingData();
		}
		else
		{
			this.Error($"Not expected command: {e.Command}");
			throw new Exception($"Not expected command: {e.Command}!");
		}
	}
}