using Microsoft.AspNetCore.Mvc;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Evaluation;
using Sportschuetzen.Dahl.Disag.Rm3;

namespace Sportschuetzen.Dahl.Disag.Server.Controllers;

/// <summary>
///     Controller for the DisagRm3
/// </summary>
[ApiController]
[Route("RM3")]
public class Rm3Controller : ControllerBase
{
	private readonly IDisagRm3 _rm3;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="rm3"></param>
	public Rm3Controller(IDisagRm3 rm3)
	{
		_rm3 = rm3;
	}

	/// <summary>
	///     Cancels the current operation
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/Cancel")]
	public async Task Cancel()
	{
		try
		{
			await _rm3.CancelAsync();
		}
		catch (Exception exception)
		{
			throw new Exception("Could not cancel", exception);
		}
	}


	/// <summary>
	///     Connects to the Disag
	/// </summary>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/Connect")]
	public void Connect()
	{
		try
		{
			var connected = _rm3.Connect();
			if (!connected)
				throw new Exception("Could not connect to RM3");
		}
		catch (Exception exception)
		{
			throw new Exception("Could not connect to RM3", exception);
		}
	}

	/// <summary>
	///     Disconnects from the Disag
	/// </summary>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/Disconnect")]
	public void Disconnect()
	{
		try
		{
			_rm3.Disconnect();
		}
		catch (Exception exception)
		{
			throw new Exception("Could not disconnect", exception);
		}
	}

	/// <summary>
	///     Ends the current operation
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/End")]
	public async Task End()
	{
		try
		{
			await _rm3.EndAsync();
		}
		catch (Exception exception)
		{
			throw new Exception("Could not end", exception);
		}
	}

	/// <summary>
	///     Sets Disag to Win Mode
	/// </summary>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/Fer")]
	public void Fer()
	{
		try
		{
			_rm3.FEr();
		}
		catch (Exception exception)
		{
			throw new Exception("Could not set Disag to Win Mode", exception);
		}
	}

	/// <summary>
	///     Sets Disag to normal Mode
	/// </summary>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/Fern")]
	public void Fern()
	{
		try
		{
			_rm3.FErn();
		}
		catch (Exception exception)
		{
			throw new Exception("Could not set Disag to Normal Mode", exception);
		}
	}

	/// <summary>
	///     Gets the serial number of the Disag
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[HttpGet("Get/Serial")]
	public async Task<string> GetSerialOfDisag()
	{
		try
		{
			var serial = await _rm3.GetSerialAsync();
			return serial;
		}
		catch (Exception exception)
		{
			throw new Exception("Could not get serial", exception);
		}
	}

	/// <summary>
	///     Gets a new series from the Disag
	/// </summary>
	/// <param name="streifen"> Count of stripes </param>
	/// <param name="schuss"> Count of shots each </param>
	/// <param name="type"> Type of stripe </param>
	/// <param name="shotEvaluation">GR = Ganze Ringe, ZR = Zehntel Ringe, KR = Keine Ringe</param>
	/// <param name="aufdruck"> Aufdruck </param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[HttpGet("Get/Serie")]
	public async Task<DisagSeries> GetSerie([FromQuery] int streifen, [FromQuery] int schuss,
		[FromQuery] EStripType type, [FromQuery] EShotEvaluation shotEvaluation, [FromQuery] string aufdruck = "")
	{
		try
		{
			var serie = await _rm3.GetSeries(new SeriesParameter(streifen, schuss, type, shotEvaluation, aufdruck));
			return serie;
		}
		catch (Exception exception)
		{
			throw new Exception("Could not get type", exception);
		}
	}

	/// <summary>
	///     Gets the type of the Disag
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[HttpGet("Get/Type")]
	public async Task<string> GetTypeOfDisag()
	{
		try
		{
			var type = await _rm3.GetTypeAsync();
			return type;
		}
		catch (Exception exception)
		{
			throw new Exception("Could not get type", exception);
		}
	}

	/// <summary>
	///     Repeats the last operation
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[HttpPost("Do/Repeat")]
	public async Task Repeat()
	{
		try
		{
			await _rm3.RepeatAsync();
		}
		catch (Exception exception)
		{
			throw new Exception("Could not repeat", exception);
		}
	}
}