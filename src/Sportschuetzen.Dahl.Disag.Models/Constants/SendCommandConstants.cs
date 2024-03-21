namespace Sportschuetzen.Dahl.Disag.Models.Constants;

/// <summary>
///     Befehle die an DisagService gesendet werden können
/// </summary>
internal static class SendCommandConstants
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