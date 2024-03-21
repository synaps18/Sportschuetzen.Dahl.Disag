using Sportschuetzen.Dahl.Disag.Models.Constants;
using Sportschuetzen.Dahl.Disag.Models.Evaluation;
using Sportschuetzen.Dahl.Disag.Models.Structs;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class StripSequence : Sequence<DisagSeries>
{
	private readonly DisagSeries _disagSeries;

	/// <summary>
	/// </summary>
	/// <param name="serialHandler"></param>
	/// <param name="parameter"></param>
	internal StripSequence(SerialHandler serialHandler, SeriesParameter parameter) : base(serialHandler)
	{
		this.Debug("Strip Sequence constructor");
		_disagSeries = new DisagSeries
		{
			TotalShots = parameter.NumberOfStrips * (int)parameter.StripType,
			ShotsPerBullsEye = parameter.ShotsPerBullsEye,
			StripType = parameter.StripType,
			ShotEvaluation = parameter.ShotEvaluation,

			Printing = parameter.Print
		};
	}

	private void NewShot(string serialShot)
	{
		this.Debug("New Shot arrived");
		var disagSchuss = serialShot.ToDisagSchuss();

		var bullsEye = new DisagBullsEye();
		bullsEye.Shots.Add(disagSchuss);

		_disagSeries.Stripes?.Last().BullsEyes.Add(bullsEye);
	}

	private void NewStrip()
	{
		this.Debug("New Stripe arrived");
		var stripe = new DisagStrip
		{
			Type = _disagSeries.StripType
		};
		_disagSeries.Stripes?.Add(stripe);
	}

	protected override async Task<DisagSeries> SequenceToCall()
	{
		var serialString = _disagSeries.ToString();

		this.Debug($"Send Stripe Info: {serialString}");
		await SerialHandler.Send(serialString);

		await AwaitDataAsync();

		return _disagSeries;
	}

	protected override async void SerialHandler_OnDataReceived(object? sender, DisagResponse e)
	{
		base.SerialHandler_OnDataReceived(sender, e);

		switch (e.Command)
		{
			case ReceiveCommandConstants.SCH:
				this.Debug($"{ReceiveCommandConstants.SCH} received, Strip type '{e.Parameter}'");
				NewShot(e.Parameter);
				break;
			case ReceiveCommandConstants.WSC:
				this.Debug($"{ReceiveCommandConstants.WSC} received, disag is waiting for strip with number of shots '{e.Parameter}'");
				if (e.Parameter.Contains('-'))
				{
					await Task.Delay(200);
					await SerialHandler.Send(0, 0);
				}
				else
				{
					NewStrip();
				}

				break;
			case ReceiveCommandConstants.WSE:
				this.Debug($"{ReceiveCommandConstants.WSE} received, disag finished sending the last strip");
				ReleaseAwaitingData();
				return;
			case ReceiveCommandConstants.STA:
				this.Debug($"{ReceiveCommandConstants.STA} received, disag starts evaluating series.");
				return;
			default:
				this.Error($"Unknown command received! {e.Command}");
				break;
		}
	}
}