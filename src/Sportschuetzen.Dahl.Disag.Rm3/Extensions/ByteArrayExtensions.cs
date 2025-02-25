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


	public static byte[] RemoveChecksumAndCr(this byte[] bytes)
	{
		return bytes.SkipLast(2).ToArray();
	}

	public static bool ValidateChecksum(this byte[] bytes)
	{
		var lastElementIsCr = bytes.LastOrDefault() == (byte)EDisagHex.CR;
		var receivedChecksum = lastElementIsCr ? bytes[^2] : bytes[^1];

		var bytesWithoutChecksum = bytes.RemoveChecksumAndCr().ToCharArray();
		var calculatedChecksum = bytesWithoutChecksum.CalCheckSum();

		return receivedChecksum == calculatedChecksum;
	}

	public static char[] ToCharArray(this byte[] bytes)
	{
		return bytes.RemoveChecksumAndCr().Select(a => (char)a).ToArray();
	}

	public static string AsString(this byte[] bytes)
	{
		var characters = bytes.Select(a => (char)a);
		var rawString = new string(characters.ToArray());

		var stringWithoutCr = rawString.RemoveCr();
		var stringWithoutChecksum = rawString.RemoveChecksum();

		return new string(characters.ToArray());
	}
}