using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Auswertung;

public class DisagStreifen
{
	public DisagStreifen()
	{
		//Empty constructor
	}

    public List<DisagSpiegel> Spiegel { get; set; } = new();

    public EScheibentyp Scheibentyp { get; set; }

    public double Gesamtergebnis => Spiegel.Sum(spiegel => spiegel.Schüsse.Sum(schuss => schuss.Ringwert));
}