using Sportschuetzen.Dahl.Disag.Models.Evaluation;
using Sportschuetzen.Dahl.Disag.Rm3.Constants;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class StripeSequence : Sequence<DisagSeries>
{
	private readonly DisagSeries _disagSeries;

	/// <summary>
	/// </summary>
	/// <param name="serialHandler"></param>
	/// <param name="parameter"></param>
	public StripeSequence(SerialHandler serialHandler, SeriesParameter parameter) : base(serialHandler)
	{
		this.Debug("Stripe Sequence constructor");
		_disagSeries = new DisagSeries
		{
			TotalShots = parameter.NumberOfStrips * (int)parameter.StripType,
			ShotsPerBullsEye = parameter.ShotsPerBullsEye,
			StripType = parameter.StripType,
			ShotEvaluation = parameter.ShotEvaluation,

			Printing = parameter.Print
		};
	}

	private void NewSchuss(string serialShot)
	{
		this.Debug("New Shot arrived");
		var disagSchuss = serialShot.ToDisagSchuss();

		var bullsEye = new DisagBullsEye();
		bullsEye.Shots.Add(disagSchuss);

		_disagSeries.Stripes?.Last().BullsEyes.Add(bullsEye);
	}

	private void NewStripe()
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
				this.Debug($"SCH received, Strip type [{e.Parameter}]");
				NewSchuss(e.Parameter);
				break;
			case ReceiveCommandConstants.WSC:
				this.Debug($"WSC empfangen, DISAG wartet auf Scheibe mit Schußzahl [{e.Parameter}]");


				if (e.Parameter.Contains('-'))
				{
					await Task.Delay(200);
					await SerialHandler.Send(0, 0);
				}
				else
				{
					NewStripe();
				}

				break;
			case ReceiveCommandConstants.WSE:
				this.Debug("WSE empfangen, DISAG Letzte Scheibe wurde gesendet.");
				ReleaseAwaitingData();
				return;
			case ReceiveCommandConstants.STA:
				this.Debug("STA gesendet, DISAG startet auswertung.");
				return;
			default:
				this.Error($"Unknown command! {e.Command}");
				break;
		}
	}
}