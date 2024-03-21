using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

/// <summary>
///     Extension methods for byte arrays
/// </summary>
public static class ByteArrayExtensions
{
	/// <summary>
	///     Adds Carriage Return to byte array
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static byte[] AddCr(this byte[] data)
	{
		data.Debug("Adding CR");
		var withCr = new byte[data.Length + 1];

		for (var i = 0; i < data.Length; i++) withCr[i] = data[i];

		withCr[data.Length] = (byte)EDisagHex.CR;

		return withCr;
	}
}