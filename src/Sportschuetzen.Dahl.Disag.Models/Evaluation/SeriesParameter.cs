using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Evaluation;

/// <summary>
///     Parameter set for a series
/// </summary>
public class SeriesParameter
{
	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="numberOfStrips"></param>
	/// <param name="shotsPerBullsEye"></param>
	/// <param name="stripType"></param>
	/// <param name="shotEvaluation"></param>
	/// <param name="print"></param>
	public SeriesParameter(int numberOfStrips, int shotsPerBullsEye, EStripType stripType,
		EShotEvaluation shotEvaluation,
		string print = "")
	{
		NumberOfStrips = numberOfStrips;
		ShotsPerBullsEye = shotsPerBullsEye;
		StripType = stripType;
		ShotEvaluation = shotEvaluation;
		Print = print;
	}

	/// <summary>
	///     Evaluation of the shots
	/// </summary>
	public EShotEvaluation ShotEvaluation { get; }

	/// <summary>
	///     Type of the strip
	/// </summary>
	public EStripType StripType { get; }

	/// <summary>
	///     Number of strips in the series
	/// </summary>
	public int NumberOfStrips { get; }

	/// <summary>
	///     Number of shots per bulls eye
	/// </summary>
	public int ShotsPerBullsEye { get; }

	/// <summary>
	///     Text to print on he strip
	/// </summary>
	public string Print { get; }
}