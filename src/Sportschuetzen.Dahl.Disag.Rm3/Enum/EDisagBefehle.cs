namespace Sportschuetzen.Dahl.Disag.Rm3.Enum;

public enum EDisagBefehle
{
    /// <summary>
    /// Maschinentyp auslesen
    /// </summary>
    TYP,
    /// <summary>
    /// Seriennummer auslesen
    /// </summary>
    SNR,
    /// <summary>
    /// Abbrechen: mit diesem Befehl kann eine laufende Serie abgebrochen werden.
    /// </summary>
    ABR,
    /// <summary>
    /// Ende: Verlässt Fer und welchselt in den manuellen Modus. Programm wird also verlassen! 
    /// </summary>
    END,
    /// <summary>
    /// Schusszahl pro Scheibe nur für nächste Scheibe
    /// </summary>
    SNS,
    /// <summary>
    /// Wiederholung (Edit Modus)
    /// </summary>
    WID,
    /// <summary>
    /// Schüsse Editieren
    /// </summary>
    EDI,
    /// <summary>
    /// Verlässt Fernmodus
    /// </summary>
    EXIT
}