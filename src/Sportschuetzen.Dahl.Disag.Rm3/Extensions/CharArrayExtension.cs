namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

/// <summary>
///     Extension methods for char array
/// </summary>
public static class CharArrayExtension
{
	/// <summary>
	///     Calculates the checksum of an <see cref="char" /> array
	/// </summary>
	/// <param name="packetData"></param>
	/// <returns></returns>
	public static byte CalCheckSum(this char[] packetData)
	{
		byte checkSumByte = 0x00;

		foreach (var c in packetData) checkSumByte ^= Convert.ToByte(c);

		if (checkSumByte < 32) checkSumByte += 32;
		packetData.Debug($"Checksum calculated byte: [{checkSumByte}], casted as char => [{(char)checkSumByte}]");

		return checkSumByte;
	}
}