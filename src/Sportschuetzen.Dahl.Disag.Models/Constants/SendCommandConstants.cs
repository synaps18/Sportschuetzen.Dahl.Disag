namespace Sportschuetzen.Dahl.Disag.Models.Constants;

/// <summary>
///     Befehle die an DisagService gesendet werden können
/// </summary>
internal static class SendCommandConstants
{
	/// <summary>
	///     Scheibentype
	/// </summary>
	public static readonly string SCH = "SCH";

	/// <summary>
	///     Ringauswertung
	/// </summary>
	public static readonly string RIA = "RIA";

	/// <summary>
	///     Kalibriereinstellung
	/// </summary>
	public static readonly string KAL = "KAL";

	/// <summary>
	///     Ringberechnung
	/// </summary>
	public static readonly string RIB = "RIB";

	/// <summary>
	///     Teilerauswertung
	/// </summary>
	public static readonly string TEA = "TEA";

	/// <summary>
	///     Teilergrenze
	/// </summary>
	public static readonly string TEG = "TEG";

	/// <summary>
	///     Schusszahl pro Streifen
	/// </summary>
	public static readonly string SSC = "SSC";

	/// <summary>
	///     Schusszahl Gesamt
	/// </summary>
	public static readonly string SGE = "SGE";

	/// <summary>
	///     Schusszahl pro Zwischensumme
	/// </summary>
	public static readonly string SZI = "SZI";

	/// <summary>
	///     Kein Scheibenaufdruck
	/// </summary>
	public static readonly string KSD = "KSD";

	/// <summary>
	///     Teiler auf dem Streifen nur markieren
	/// </summary>
	public static readonly string TEM = "TEM";

	/// <summary>
	///     Nach der Endsumme kann noch ein Text aufgedruckt werden.
	///     DRT=XXXXXXXXXX ASCII-Zeichen
	/// </summary>
	public static readonly string DRT = "DRT";
}