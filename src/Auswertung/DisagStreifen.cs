using Sportschützen.Dahl.DisagRm3.Enum;

namespace Sportschützen.Dahl.DisagRm3.Auswertung;

public class DisagStreifen
{
    public DisagStreifen(EScheibentyp scheibentyp, int schussProSpiegel)
    {
        this.Scheibentyp = scheibentyp;
        Spiegel = new List<DisagSpiegel>();

        //Anzahl von Spiegeln für Stripes erstellen
        //for (var i = 0; i < (int)scheibentyp; i++)
        //{
        //    var spiegel = new DisagSpiegel();
        //    spiegel.Schüsse.Add(new DisagSchuss(schussProSpiegel));
        //    Spiegel.Add(spiegel);
        //}
    }

    public List<DisagSpiegel> Spiegel { get; set; }

    public EScheibentyp Scheibentyp { get; }

    public double Gesamtergebnis => Spiegel.Sum(spiegel => spiegel.Schüsse.Sum(schuss => schuss.Ringwert));
}