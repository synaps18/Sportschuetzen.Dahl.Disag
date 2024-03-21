using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Evaluation;

/// <summary>
///     Represents a shot with and its values
/// </summary>
public class DisagShot
{
	/// <summary>
	///     Angle of the shot
	/// </summary>
	public double Angle { get; set; }

	/// <summary>
	///     Divisor value of the shot
	/// </summary>
	public double DivisorValue { get; set; }

	/// <summary>
	///     Number of the shot
	/// </summary>
	public double Number { get; set; }

	/// <summary>
	///     Value of the shot
	/// </summary>
	public double Value { get; set; }

	/// <summary>
	///     Validity of the shot
	/// </summary>
	public EDisagValidity Validity { get; set; }
}