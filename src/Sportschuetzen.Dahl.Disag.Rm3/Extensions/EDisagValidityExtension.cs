using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

/// <summary>
///     Extension for the <see cref="EDisagValidity" /> enum
/// </summary>
public static class EDisagValidityExtension
{
	/// <summary>
	///     Creates an <see cref="EDisagValidity" /> from a string
	/// </summary>
	/// <param name="validity"></param>
	/// <param name="str"></param>
	/// <returns></returns>
	public static EDisagValidity FromString(this EDisagValidity validity, string str)
	{
		return EDisagValidity.G;
	}
}