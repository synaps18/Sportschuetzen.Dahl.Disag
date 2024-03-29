namespace Sportschuetzen.Dahl.Disag.Models.Constants;

/// <summary>
///     Befehle die vom DisagService empfangen werden können
/// </summary>
public static class ReceiveCommandConstants
{
    /// <summary>
    ///     Wartet auf Streifen
    ///     WSC=99: Werte entspricht der Schußzahl pro Spiegel, 1..15 je nach Scheibentype erlaubt.
    ///     Nach diesem Befehl kann kann ein neuer Einstellstring gesendet werden.
    ///     Ist das Vorzeichen negativ wartet die RM auf EDITIERUNG des Ergebnisses
    /// </summary>
    public static readonly string WSC = "WSC";

    /// <summary>
    ///     Meldung
    ///     MEL=Bitte Streifen zum Bedrucken einlegen
    /// </summary>
    public static readonly string MEL = "MEL";

    /// <summary>
    ///     Auswerte Start
    /// </summary>
    public static readonly string STA = "STA";

    /// <summary>
    ///     Wartet auf Stripes Ende
    ///     WSE wird geschickt nachdem die letzte Stripes gewertet wurd.
    ///     Danach nimmt die RM keine Stripes mehr an, bis eine neue Einstellung geschickt wurde.
    /// </summary>
    public static readonly string WSE = "WSE"; //Warte auf Stripes Ende

    /// <summary>
    ///     Schuss
    /// </summary>
    public static readonly string SCH = "SCH";
}