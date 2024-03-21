namespace Sportschuetzen.Dahl.Disag.Models.Enum;

/// <summary>
/// Commands to send to disag
/// </summary>
public enum EDisagCommand
{
	/// <summary>
	///     Read the type of disag
	/// </summary>
	TYP,

	/// <summary>
	///     Read the serial number of the disag
	/// </summary>
	SNR,

	/// <summary>
	///     Cancel: this command can be used to cancel a running series.
	/// </summary>
	ABR,

	/// <summary>
	///     End: Exits mode 'Fer' and switches to manual mode. The program is therefore exited!
	/// </summary>
	END,

	/// <summary>
	///     Number of shots per target for next target only
	/// </summary>
	SNS,

	/// <summary>
	///     Repeat (Edit mode)
	/// </summary>
	WID,

	/// <summary>
	///     Edit shots
	/// </summary>
	EDI,

	/// <summary>
	///     Exits remote mode (Fern)
	/// </summary>
	EXIT
}