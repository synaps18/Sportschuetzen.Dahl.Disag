namespace Sportschützen.Dahl.DisagRm3.Extensions;

public static class CharArrayExtension
{
    public static byte CalCheckSum(this char[] packetData)
    {
        byte checkSumByte = 0x00;

        foreach (var c in packetData) checkSumByte ^= Convert.ToByte(c);

        if (checkSumByte < 32) checkSumByte += 32;
        packetData.Debug($"Checksum calculated byte: [{checkSumByte}], casted as char => [{(char)checkSumByte}]");

        return checkSumByte;
    }
}