using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Evaluation;

/// <summary>
///     Represents a strip with bulls eyes
/// </summary>
public class DisagStrip
{
	/// <summary>
	///     Score of the strip
	/// </summary>
	public double Score => BullsEyes.Sum(bullsEye => bullsEye.Shots.Sum(shot => shot.Value));

	/// <summary>
	///     Type of the strip
	/// </summary>
	public EStripType Type { get; set; }

	/// <summary>
	///     Bulls eyes of the strip
	/// </summary>
	public List<DisagBullsEye> BullsEyes { get; set; } = new();
}