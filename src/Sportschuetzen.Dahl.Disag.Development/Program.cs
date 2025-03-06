using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Evaluation;

namespace Sportschuetzen.Dahl.Disag.Development
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			using var disag = new Rm3.Serial.Disag();

			//await disag.Send_Fer_Mode();

			//Quits the Fer Mode
			//await disag.Send_End_Program();

			//var series = await disag.Send_Request_Series(new SeriesParameter(1, 1, EStripType.LG5, EShotEvaluation.ZR));

			//var series2 = await disag.Send_Request_Series(new SeriesParameter(1, 1, EStripType.LG5, EShotEvaluation.ZR));

			var serialNumber = await disag.Send_Get_Serial_Number();
			var type = await disag.Send_Get_Machine_Type();
			//Console.WriteLine(serialNumber);
			Console.Read();
		}
	}
}
