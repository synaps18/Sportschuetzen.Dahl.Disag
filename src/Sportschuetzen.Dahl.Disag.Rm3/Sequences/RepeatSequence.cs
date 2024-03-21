using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Structs;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class RepeatSequence : Sequence<string>
{
	private string? _receivedData;

	public RepeatSequence(SerialHandler serialHandler) : base(serialHandler)
	{
	}

	protected override async Task<string> SequenceToCall()
	{
		await SerialHandler.Send(EDisagCommand.WID);

		await AwaitDataAsync();

		return _receivedData!;
	}

	protected override void SerialHandler_OnDataReceived(object? sender, DisagResponse e)
	{
		base.SerialHandler_OnDataReceived(sender, e);

		_receivedData = !string.IsNullOrEmpty(e.Parameter) ? e.Parameter : e.Command;
		ReleaseAwaitingData();
	}
}