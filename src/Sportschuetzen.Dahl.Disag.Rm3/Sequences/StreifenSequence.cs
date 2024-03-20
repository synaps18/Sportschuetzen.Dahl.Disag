using Sportschuetzen.Dahl.Disag.Models.Auswertung;
using Sportschuetzen.Dahl.Disag.Rm3.Constants;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal class StreifenSequence : SerialSequence<DisagSerie>
{
    private readonly DisagSerie _disagSerie;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serialHandler"></param>
    /// <param name="parameter"></param>
    public StreifenSequence(SerialHandler serialHandler, SeriesParameter parameter) : base(serialHandler)
    {
        this.Debug("Stripe Sequence constructor");
        _disagSerie = new DisagSerie
        {
	        AnzahlSchussGesamt = parameter.AnzahlStreifen * (int)parameter.Scheibentyp,
	        AnzahlSchussProSpiegel = parameter.SchussProSpiegel,
	        Scheibentyp = parameter.Scheibentyp,
	        Ringauswertung = parameter.Ringauswertung,

	        Aufdruck = parameter.Aufdruck
	};
    }

    protected override async void SerialWrapper_OnDataReceived(object? sender, DisagResponse e)
    {
        base.SerialWrapper_OnDataReceived(sender, e);

        switch (e.Command)
        {
            case DISAG_Befehle_Empfangen.SCH:
                this.Debug($"SCH empfangen, Scheibentyp [{e.Parameter}]");
                NewSchuss(e.Parameter);
                break;
            case DISAG_Befehle_Empfangen.WSC:
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
            case DISAG_Befehle_Empfangen.WSE:
                this.Debug("WSE empfangen, DISAG Letzte Scheibe wurde gesendet.");
                ReleaseAwaitingData();
                return;
            case DISAG_Befehle_Empfangen.STA:
                this.Debug("STA gesendet, DISAG startet auswertung.");
                return;
            default:
                this.Error($"Unknown command! {e.Command}");
                break;
        }
    }

    protected override async Task<DisagSerie> SequenceToCall()
    {
        var serialString = _disagSerie.ToString();

        this.Debug($"Send Stripe Info: {serialString}");
        await SerialHandler.Send(serialString);

        await AwaitDataAsync();

        return _disagSerie;
    }

    private void NewStripe()
    {
        this.Debug("New Stripe arrived");
        var stripe = new DisagStreifen()
        {
            Scheibentyp = _disagSerie.Scheibentyp,
		};
        _disagSerie.Streifen?.Add(stripe);
    }

    private void NewSchuss(string serialShot)
    {
        this.Debug("New Shot arrived");
        var disagSchuss = serialShot.ToDisagSchuss();

        var spiegel = new DisagSpiegel();
        spiegel.Schüsse.Add(disagSchuss);
        
        _disagSerie.Streifen?.Last().Spiegel?.Add(spiegel);
    }
}