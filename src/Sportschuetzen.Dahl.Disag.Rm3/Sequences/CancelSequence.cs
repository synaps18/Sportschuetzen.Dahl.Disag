using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class CancelSequence : Sequence<string>
{
	public CancelSequence(SerialHandler serialHandler) : base(serialHandler)
	{
		SerialHandler.OnHexReceived += SerialHandler_OnHexReceived;
	}

	public override void Dispose()
	{
		SerialHandler.OnHexReceived -= SerialHandler_OnHexReceived;
		base.Dispose();
	}

	protected override async Task<string> SequenceToCall()
	{
		await SerialHandler.Send(EDisagCommand.ABR);

		await AwaitDataAsync();

		return string.Empty;
	}

	private void SerialHandler_OnHexReceived(object? sender, EDisagHex e)
	{
		if (e == EDisagHex.ACK) ReleaseAwaitingData();
	}
}