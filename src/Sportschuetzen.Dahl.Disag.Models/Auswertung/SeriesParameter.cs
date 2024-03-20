using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Auswertung;

public class SeriesParameter
{
    public int AnzahlStreifen { get; }
    public int SchussProSpiegel { get; }
    public EScheibentyp Scheibentyp { get; }
    public ERingauswertung Ringauswertung { get; }
    public string Aufdruck { get; }

    public SeriesParameter(int anzahlStreifen, int schussProSpiegel, EScheibentyp scheibentyp, ERingauswertung ringauswertung,
        string aufdruck = "")
    {
        AnzahlStreifen = anzahlStreifen;
        SchussProSpiegel = schussProSpiegel;
        Scheibentyp = scheibentyp;
        Ringauswertung = ringauswertung;
        Aufdruck = aufdruck;
    }
}