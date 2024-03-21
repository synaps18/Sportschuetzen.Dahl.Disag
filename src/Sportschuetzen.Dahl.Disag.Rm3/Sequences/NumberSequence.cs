using Sportschuetzen.Dahl.Disag.Models.Constants;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Structs;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class NumberSequence : Sequence<string>
{
	private string? _receivedSerialNumber;

	public NumberSequence(SerialHandler serialHandler) : base(serialHandler)
	{
	}

	protected override async Task<string> SequenceToCall()
	{
		await SerialHandler.Send(EDisagCommand.SNR);

		await AwaitDataAsync();

		return _receivedSerialNumber!;
	}

	protected override async void SerialHandler_OnDataReceived(object? sender, DisagResponse e)
	{
		base.SerialHandler_OnDataReceived(sender, e);

		if (e.Command == EDisagCommand.SNR.ToString())
		{
			_receivedSerialNumber = e.Parameter;
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