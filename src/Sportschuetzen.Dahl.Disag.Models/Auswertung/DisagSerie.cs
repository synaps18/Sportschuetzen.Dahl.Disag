using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Auswertung;

public class DisagSerie
{
    public double Gesamtergebnis => Streifen?.Sum(item => item.Gesamtergebnis) ?? 0.0;

    public ERingauswertung Ringauswertung { get; set; }

    public EScheibentyp Scheibentyp { get; set; }

    public int AnzahlSchussGesamt { get; set; }

    public int AnzahlSchussProSpiegel { get; set; }

    public int AnzahlStreifen => Streifen!.Count;

    public List<DisagStreifen>? Streifen { get; set; } = new();

    public string Aufdruck { get; set; }

    public override string ToString()
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