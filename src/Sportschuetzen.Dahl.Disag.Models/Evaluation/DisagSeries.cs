using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Models.Evaluation;

/// <summary>
///     Represents a series with strips
/// </summary>
public class DisagSeries
{
	/// <summary>
	///     Score of the series
	/// </summary>
	public double Score => Stripes?.Sum(item => item.Score) ?? 0.0;

	/// <summary>
	///     Evaluation of the shots
	/// </summary>
	public EShotEvaluation ShotEvaluation { get; set; }

	/// <summary>
	///     Type of the strips
	/// </summary>
	public EStripType StripType { get; set; }

	/// <summary>
	///     Number of shots per bulls eye
	/// </summary>
	public int ShotsPerBullsEye { get; set; }

	/// <summary>
	///     Total number of shots in the series
	/// </summary>
	public int TotalShots { get; set; }

	/// <summary>
	///     Strips of the series
	/// </summary>
	public List<DisagStrip>? Stripes { get; set; } = new();

	/// <summary>
	///     Text to print on each the strip
	/// </summary>
	public string Printing { get; set; } = string.Empty;

	/// <summary>
	///     Returns a disag compatible string representation of the series
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		var s = string.Empty;
		s += $"SCH={StripType};";
		s += $"RIA={ShotEvaluation};";
		s += $"SSC={ShotsPerBullsEye};";
		s += $"SGE={TotalShots}";

		if (!string.IsNullOrEmpty(Printing))
			s += $";DRT={Printing}";
		return s;
	}
}