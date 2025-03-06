namespace Sportschuetzen.Dahl.Disag.Models.Structs;

/// <summary>
///     Response from Disag
/// </summary>
public struct DisagCommand
{
	/// <summary>
	///     Command from Disag
	/// </summary>
	public string Command;

	/// <summary>
	///     Parameter of command
	/// </summary>
	public string Parameter;

	public override string ToString()
	{
		return Command + "=" + Parameter;
	}
}