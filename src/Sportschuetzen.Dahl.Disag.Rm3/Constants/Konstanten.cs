namespace Sportschuetzen.Dahl.Disag.Rm3.Constants;

/// <summary>
///     Befehle die vom DisagService empfangen werden können
/// </summary>
internal static class DISAG_Befehle_Empfangen
{
    /// <summary>
    ///     Wartet auf Streifen
    ///     WSC=99: Werte entspricht der Schußzahl pro Spiegel, 1..15 je nach Scheibentype erlaubt.
    ///     Nach diesem Befehl kann kann ein neuer Einstellstring gesendet werden.
    ///     Ist das Vorzeichen negativ wartet die RM auf EDITIERUNG des Ergebnisses
    /// </summary>
    public const string WSC = "WSC";

    /// <summary>
    ///     Meldung
    ///     MEL=Bitte Streifen zum Bedrucken einlegen
    /// </summary>
    public const string MEL = "MEL";

    /// <summary>
    ///     Auswerte Start
    /// </summary>
    public const string STA = "STA";

    /// <summary>
    ///     Wartet auf Stripes Ende
    ///     WSE wird geschickt nachdem die letzte Stripes gewertet wurd.
    ///     Danach nimmt die RM keine Stripes mehr an, bis eine neue Einstellung geschickt wurde.
    /// </summary>
    public const string WSE = "WSE"; //Warte auf Stripes Ende

    /// <summary>
    ///     Schuss
    /// </summary>
    public const string SCH = "SCH";
}

/// <summary>
///     Befehle die an DisagService gesendet werden können
/// </summary>
internal static class DISAG_Befehle_Senden
{
    /// <summary>
    ///     Scheibentype
    /// </summary>
    public const string SCH = "SCH";

    /// <summary>
    ///     Ringauswertung
    /// </summary>
    public const string RIA = "RIA";

    /// <summary>
    ///     Kalibriereinstellung
    /// </summary>
    public const string KAL = "KAL";

    /// <summary>
    ///     Ringberechnung
    /// </summary>
    public const string RIB = "RIB";

    /// <summary>
    ///     Teilerauswertung
    /// </summary>
    public const string TEA = "TEA";

    /// <summary>
    ///     Teilergrenze
    /// </summary>
    public const string TEG = "TEG";

    /// <summary>
    ///     Schusszahl pro Streifen
    /// </summary>
    public const string SSC = "SSC";

    /// <summary>
    ///     Schusszahl Gesamt
    /// </summary>
    public const string SGE = "SGE";

    /// <summary>
    ///     Schusszahl pro Zwischensumme
    /// </summary>
    public const string SZI = "SZI";

    /// <summary>
    ///     Kein Scheibenaufdruck
    /// </summary>
    public const string KSD = "KSD";

    /// <summary>
    ///     Teiler auf dem Streifen nur markieren
    /// </summary>
    public const string TEM = "TEM";

    /// <summary>
    ///     Nach der Endsumme kann noch ein Text aufgedruckt werden.
    ///     DRT=XXXXXXXXXX ASCII-Zeichen
    /// </summary>
    public const string DRT = "DRT";
}

/// <summary>
///     Auswahl verschiedener Scheibentypen
/// </summary>
internal static class DISAG_Scheibentyp
{
    /// <summary>
    ///     LG 10er Band
    /// </summary>
    public const string LG10 = "LG10";

    /// <summary>
    ///     LG 5er Band
    /// </summary>
    public const string LG5 = "LG5";

    /// <summary>
    ///     LG Einzelscheibe
    /// </summary>
    public const string LGES = "LGES";

    /// <summary>
    ///     LP
    /// </summary>
    public const string LP = "LP";

    /// <summary>
    ///     Zimmerstuzen 15m
    /// </summary>
    public const string ZS = "ZS";

    /// <summary>
    ///     Laufende Stripes; ein Shot
    /// </summary>
    public const string LS1 = "LS1";

    /// <summary>
    ///     Laufende Stripes; doppel Shot
    /// </summary>
    public const string LS2 = "LS2";

    /// <summary>
    ///     50m Stripes
    /// </summary>
    public const string KK50 = "KK50";

    /// <summary>
    ///     100m - Stripes für Groß und Kleinkaliber
    /// </summary>
    public const string GK10 = "GK10";

    /// <summary>
    ///     Kombischeibe 5-kreisig mit weißem Scheibenspiegel
    /// </summary>
    public const string GK5 = "GK5";

    /// <summary>
    ///     LP Schnellfeuer
    /// </summary>
    public const string LPSF = "LPSF";

    /// <summary>
    ///     Schnellfeuer- und Duell Stripes.
    /// </summary>
    public const string SCHFE = "SCHFE";

    /// <summary>
    ///     Benutzerdefiniert 1
    /// </summary>
    public const string USE1 = "USE1";

    /// <summary>
    ///     Benutzerdefiniert 2
    /// </summary>
    public const string USE2 = "USE2";
}