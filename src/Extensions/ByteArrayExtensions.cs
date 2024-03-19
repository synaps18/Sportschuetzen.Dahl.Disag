using Sportschützen.Dahl.DisagRm3.Enum;

namespace Sportschützen.Dahl.DisagRm3.Extensions;

public static class ByteArrayExtensions
{
    /// <summary>
    /// Adds Carriage Return to byte array
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