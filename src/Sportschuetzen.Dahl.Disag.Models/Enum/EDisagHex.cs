namespace Sportschuetzen.Dahl.Disag.Models.Enum;

/// <summary>
///     Protocol commands in HEX
/// </summary>
public enum EDisagHex
{
	/// <summary>
	///     End of Transmission
	/// </summary>
	EOT = 0x04,

	/// <summary>
	///     Enquiry
	/// </summary>
	ENQ = 0x05,

	/// <summary>
	///     Acknowledge
	/// </summary>
	ACK = 0x06,

	/// <summary>
	///     Start of text
	/// </summary>
	STX = 0x02,

	/// <summary>
	///     Negative Acknowledge
	/// </summary>
	NAK = 0x15,

	/// <summary>
	///     Carriage Return
	/// </summary>
	CR = 0x0D
}