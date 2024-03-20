using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Auswertung;

public class DisagSchuss
{
	/// <summary>
	///     Wert des Ringes
	/// </summary>
	public double Ringwert { get; set; }

	//Public Eigenschaften
	/// <summary>
	///     Die gewertete Schussnummer
	/// </summary>
	public double Schussnummer { get; set; }

	/// <summary>
	///     Wert des Teilers
	/// </summary>
	public double Teilerwert { get; set; }

	/// <summary>
	///     Wert des Winkels
	/// </summary>
	public double Winkel { get; set; }

	/// <summary>
	///     Gültigkeit der Auswertung des Ringes
	/// </summary>
	public EDisagFlag Flag { get; set; }

	/// <summary>
	///     Die Anzahl der Schüsse auf den Shot
	/// </summary>
	public int AnzahlSchuss { get; set; }
}