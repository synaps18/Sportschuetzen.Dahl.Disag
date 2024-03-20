using Sportschuetzen.Dahl.Disag.Rm3.Auswertung;
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
    /// <param name="serialWrapper"></param>
    /// <param name="parameter"></param>
    public StreifenSequence(SerialWrapper serialWrapper, SeriesParameter parameter) : base(serialWrapper)
    {
        this.Debug("Stripe Sequence constructor");
        _disagSerie = new DisagSerie(parameter);
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
                    await SerialWrapper.Send(0, 0);
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
        var serialString = _disagSerie.GetSerialString();

        this.Debug($"Send Stripe Info: {serialString}");
        await SerialWrapper.Send(serialString);

        await AwaitDataAsync();

        return _disagSerie;
    }

    private void NewStripe()
    {
        this.Debug("New Stripe arrived");
        var stripe = new DisagStreifen(_disagSerie.Scheibentyp, _disagSerie.AnzahlSchussProSpiegel);
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