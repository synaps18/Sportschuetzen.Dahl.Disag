namespace Sportschuetzen.Dahl.Disag.Models.Evaluation;

/// <summary>
///     Represents a bulls eye with shots
/// </summary>
public class DisagBullsEye
{
	/// <summary>
	///     Shots on he bulls eye
	/// </summary>
	public List<DisagShot> Shots { get; set; } = new();
}