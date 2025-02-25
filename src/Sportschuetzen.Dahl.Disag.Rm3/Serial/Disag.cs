using System.IO.Ports;
using Sportschuetzen.Dahl.Disag.Models.Constants;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Evaluation;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;

namespace Sportschuetzen.Dahl.Disag.Rm3.Serial;

public class Disag
{
	private readonly DisagSerialCommands _disagSerialCommands;

	public Disag(string com = "COM3")
	{
		_disagSerialCommands = new DisagSerialCommands(com);
	}

	public async Task<DisagSeries> Send_Request_Series(SeriesParameter parameter, CancellationToken? token = null)
	{
		var series = new DisagSeries
		{
			TotalShots = parameter.NumberOfStrips * (int)parameter.StripType,
			ShotsPerBullsEye = parameter.ShotsPerBullsEye,
			StripType = parameter.StripType,
			ShotEvaluation = parameter.ShotEvaluation,

			Printing = parameter.Print
		};

		await _disagSerialCommands.Send_Disag_Command(series.ToString(), token);
		var expectedWsc= await _disagSerialCommands.Await_Disag_Response(token);
		if (expectedWsc.Command != ReceiveCommandConstants.WSC)
		{
			throw new Exception($"Received not expected value. Received {expectedWsc.Command} instead of {ReceiveCommandConstants.WSC}");
		}

		var staCommand = await _disagSerialCommands.Await_Disag_Response(token);
		if (staCommand.Command != ReceiveCommandConstants.STA)
		{
			throw new Exception($"Received not expected value. Received {staCommand.Command} instead of {ReceiveCommandConstants.STA}");
		}


		var serie = new DisagSeries();
		while (true)
		{
			var command = await _disagSerialCommands.Await_Disag_Response(token);
			switch (command.Command)
			{
				case ReceiveCommandConstants.WSC:
					serie.Stripes.Add(new DisagStrip());
					if(command.Parameter.Contains('-'))
						await Send_Edit(0, 0);
					continue;
				case ReceiveCommandConstants.STA:
					 
					continue;
				case ReceiveCommandConstants.SCH:
					serie.Stripes.Last().BullsEyes.Last().Shots.Add(command.Parameter.ToDisagSchuss());
					continue;
				case ReceiveCommandConstants.WSE:
					return serie;
			}
		}
	}


	public async Task Send_Fer_Mode()
	{
		await _disagSerialCommands.Send_Mode(EDisagMode.W);
	}

	public async Task Send_Fern_Mode()
	{
		await _disagSerialCommands.Send_Mode(EDisagMode.V);
	}

	public async Task Send_Edit(int ediTotalShots, int ediShotsLastStrip)
	{
		var edi = $"{EDisagCommand.EDI}={ediTotalShots};{ediShotsLastStrip}";
		await _disagSerialCommands.Send_Disag_Command(edi);
	}

	public async Task Send_End_Program()
	{
		await _disagSerialCommands.Send_Disag_Command(EDisagCommand.END);
	}

	public async Task Send_Cancel_Series()
	{
		await _disagSerialCommands.Send_Disag_Command(EDisagCommand.ABR);
	}

	public async Task<string> Send_Get_Serial()
	{
		await _disagSerialCommands.Send_Disag_Command(EDisagCommand.SNR);
		var serial = await _disagSerialCommands.Await_Disag_Response();
		return serial.Parameter;
	}

	public async Task<string> Send_Get_Type()
	{
		await _disagSerialCommands.Send_Disag_Command(EDisagCommand.TYP);
		var serial = await _disagSerialCommands.Await_Disag_Response();
		return serial.Parameter;
	}

	/// <summary>
	///    Sends the command to print a text on the Disag
	/// </summary>
	/// <param name="text">MAX 10 characters!</param>
	/// <returns></returns>
	public async Task Send_Print(string text)
	{
		await _disagSerialCommands.Send_Disag_Command(EDisagCommand.DRT);
	}
}