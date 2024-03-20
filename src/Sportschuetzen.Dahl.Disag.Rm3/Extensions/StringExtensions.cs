using Sportschuetzen.Dahl.Disag.Rm3.Auswertung;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

public static class StringExtensions
{
    public static string RemoveCr(this string data)
    {
        data.Debug($"Removing CR: [{data}]");
        var removed = data.Remove(data.Length - 1);
        data.Debug($"Removed CR: [{removed}]");

        return removed;
    }

    public static byte[] ToByteArray(this string data, bool withChecksum)
    {
        data.Debug($"Parsing string [{data}] to byte array {(withChecksum ? "with" : "without")} checksum");
        var dataToSend = withChecksum ? new byte[data.Length + 1] : new byte[data.Length];

        for (var i = 0; i < data.Length; i++)
            dataToSend[i] = Convert.ToByte(data[i]);

        if (withChecksum) dataToSend[data.Length] = data.ToCharArray().CalCheckSum();

        return dataToSend;
    }

    public static DisagResponse ParseToDisagResponse(this string rawData)
    {
        var cleanedData = rawData.RemoveCr();
        var response = new DisagResponse();

        rawData.Debug($"Parsing [{rawData}] to {typeof(DisagResponse)}");
        if (cleanedData.Contains("="))
        {
            response.Command = cleanedData.Split("=")[0];
            response.Parameter = cleanedData.Split("=")[1];
        }
        else
        {
            response.Command = cleanedData;
        }

        return response;
    }

    public static DisagSchuss ToDisagSchuss(this string serialString)
    {
        /*
             Format SCH=[Schußnr];[Ringwert];[Teilerwert];[Winkel];[Flag]
                            0          1           2          3       4
        */

        var parameterList = serialString.Split(';');

        var shotNumber = parameterList[0];
        var shotValue = parameterList[1];
        var shotDivisor = parameterList[2];
        var shotAngle = parameterList[3];
        var shotFlag = parameterList[4];

        var shot = new DisagSchuss(shotNumber, shotValue, shotDivisor, shotAngle, shotFlag);

        return shot;
    }
}