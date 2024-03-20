using Sportschuetzen.Dahl.Disag.Rm3.Enum;

namespace Sportschuetzen.Dahl.Disag.Rm3.Auswertung;

public class DisagSerie
{
    public double Gesamtergebnis => Streifen?.Sum(item => item.Gesamtergebnis) ?? 0.0;

    public ERingauswertung Ringauswertung { get; }

    public EScheibentyp Scheibentyp { get; }

    public int AnzahlSchussGesamt { get; }

    public int AnzahlSchussProSpiegel { get; }

    public int AnzahlStreifen => Streifen!.Count;

    public List<DisagStreifen>? Streifen { get; set; } = new();

    public string Aufdruck { get; }

    /// <summary>
    ///     Neue Roh-Series anlegen
    /// </summary>
    public DisagSerie(SeriesParameter parameter)
    {
        AnzahlSchussGesamt = parameter.AnzahlStreifen * (int)parameter.Scheibentyp;
        AnzahlSchussProSpiegel = parameter.SchussProSpiegel;
        Scheibentyp = parameter.Scheibentyp;
        Ringauswertung = parameter.Ringauswertung;

        Aufdruck = parameter.Aufdruck;
    }

    public string GetSerialString()
    {
        var s = string.Empty;
        s += $"SCH={Scheibentyp};";
        s += $"RIA={Ringauswertung};";
        s += $"SSC={AnzahlSchussProSpiegel};";
        s += $"SGE={AnzahlSchussGesamt}";

        if (!string.IsNullOrEmpty(Aufdruck))
            s += $";DRT={Aufdruck}";
        return s;
    }
}