using System.Globalization;
using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Exceptions;
using Sportschützen.Dahl.DisagRm3.Extensions;

namespace Sportschützen.Dahl.DisagRm3.Auswertung;

public class DisagSchuss
{
    /// <summary>
    ///     Wert des Ringes
    /// </summary>
    public double Ringwert { get; }

    //Public Eigenschaften
    /// <summary>
    ///     Die gewertete Schussnummer
    /// </summary>
    public double Schussnummer { get; }

    /// <summary>
    ///     Wert des Teilers
    /// </summary>
    public double Teilerwert { get; }

    /// <summary>
    ///     Wert des Winkels
    /// </summary>
    public double Winkel { get; }

    /// <summary>
    ///     Die Anzahl der Schüsse auf den Shot
    /// </summary>
    public int AnzahlSchuss { get; }

    /// <summary>
    ///     Gültigkeit der Auswertung des Ringes
    /// </summary>
    public EDisagFlag Flag { get; }
    //Private Eigenschaften

    /// <summary>
    ///     Roh-Shot für Stripes erstellen
    /// </summary>
    /// <param name="schussProSpiegel">Anzahl Schüsse pro Stripes</param>
    public DisagSchuss(int schussProSpiegel)
    {
        AnzahlSchuss = schussProSpiegel;
    }

    /// <summary>
    ///     Shot für Stripes mit Werten erstellen. Wird von DisagService erstellt!
    ///     Beispiel DisagService sendet: SCH=22;9;720.5;272;G
    ///     Format SCH =[Schußnr];[Ringwert];[Teilerwert];[Winkel];[Flag]
    /// </summary>
    /// <param name="schußnr">Numerierung des Schusses</param>
    /// <param name="ringwert">Die Wertung des Spiegels</param>
    /// <param name="teilerwert"></param>
    /// <param name="winkel"></param>
    /// <param name="flag">Gültigkeit</param>
    public DisagSchuss(string schußnr, string ringwert, string teilerwert, string winkel, string flag)
    {
        try
        {
            Schussnummer = double.Parse(schußnr, CultureInfo.InvariantCulture);
            Ringwert = double.Parse(ringwert, CultureInfo.InvariantCulture);
            Teilerwert = double.Parse(teilerwert, CultureInfo.InvariantCulture);
            Winkel = double.Parse(winkel, CultureInfo.InvariantCulture);
            Flag = System.Enum.Parse<EDisagFlag>(flag.Substring(0, 1));

            this.Debug($"Schussnumer: {schußnr} => {Schussnummer}, Ringwert: {ringwert} => {Ringwert}, Teilerwert: {teilerwert} => {Teilerwert}, Winkel: {winkel} => {Winkel}, Flag: {flag} => {Flag}");
        }
        catch (ConvertDataToSpiegelException e)
        {
            this.Error("Conversion error", e);
            throw;
        }
    }
}