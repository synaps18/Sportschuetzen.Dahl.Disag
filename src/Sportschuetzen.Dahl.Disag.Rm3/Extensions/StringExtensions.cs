using System.Globalization;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Evaluation;
using Sportschuetzen.Dahl.Disag.Models.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

/// <summary>
///     Extension methods for <see cref="string" />
/// </summary>
public static class StringExtensions
{
	/// <summary>
	///     Parses a string to a <see cref="DisagResponse" />
	/// </summary>
	/// <param name="rawData"></param>
	/// <returns></returns>
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

	/// <summary>
	///     Removes the CR from a string
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static string RemoveCr(this string data)
	{
		data.Debug($"Removing CR: [{data}]");
		var removed = data.Remove(data.Length - 1);
		data.Debug($"Removed CR: [{removed}]");

		return removed;
	}

	/// <summary>
	///     Parses a string to a byte array
	/// </summary>
	/// <param name="data"></param>
	/// <param name="withChecksum"></param>
	/// <returns></returns>
	public static byte[] ToByteArray(this string data, bool withChecksum)
	{
		data.Debug($"Parsing string [{data}] to byte array {(withChecksum ? "with" : "without")} checksum");
		var dataToSend = withChecksum ? new byte[data.Length + 1] : new byte[data.Length];

		for (var i = 0; i < data.Length; i++)
			dataToSend[i] = Convert.ToByte(data[i]);

		if (withChecksum) dataToSend[data.Length] = data.ToCharArray().CalCheckSum();

		return dataToSend;
	}

	/// <summary>
	///     Parses a string to a <see cref="DisagShot" />
	/// </summary>
	/// <param name="serialString"></param>
	/// <returns></returns>
	public static DisagShot ToDisagSchuss(this string serialString)
	{
		/*
		     Format SCH=[ShotNumber];[ShotValue];[DivisorValue];[Angle];[Flag]
		                    0          1           2          3       4
		*/

		var parameterList = serialString.Split(';');

		var shotNumber = parameterList[0];
		var shotValue = parameterList[1];
		var shotDivisor = parameterList[2];
		var shotAngle = parameterList[3];
		var shotFlag = parameterList[4];

		var shot = new DisagShot
		{
			Number = double.Parse(shotNumber, CultureInfo.InvariantCulture),
			Value = double.Parse(shotValue, CultureInfo.InvariantCulture),
			DivisorValue = double.Parse(shotDivisor, CultureInfo.InvariantCulture),
			Angle = double.Parse(shotAngle, CultureInfo.InvariantCulture),
			Validity = Enum.Parse<EDisagValidity>(shotFlag.Substring(0, 1))
		};

		return shot;
	}
}