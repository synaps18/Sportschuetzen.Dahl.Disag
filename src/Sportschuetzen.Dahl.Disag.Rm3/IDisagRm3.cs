using Sportschuetzen.Dahl.Disag.Rm3.Auswertung;

namespace Sportschuetzen.Dahl.Disag.Rm3;

/// <summary>
///     Interface for the DisagRm3
/// </summary>
public interface IDisagRm3
{
	/// <summary>
	///     Event that is raised when the Disag is working
	/// </summary>
	event EventHandler<bool>? IsWorkingChanged;

	/// <summary>
	///     Event that is raised when the connection to the Disag changes
	/// </summary>
	event EventHandler<bool>? OnConnectionChanged;

	/// <summary>
	///     Cancels the current operation
	/// </summary>
	/// <returns></returns>
	Task CancelAsync();

	/// <summary>
	///     Connects to the Disag
	/// </summary>
	/// <returns></returns>
	bool Connect();

	/// <summary>
	///     Disconnects from the Disag
	/// </summary>
	/// <returns></returns>
	bool Disconnect();

	/// <summary>
	///     Ends the current operation
	/// </summary>
	/// <returns></returns>
	Task EndAsync();

	/// <summary>
	///     Sets Disag to Win Mode
	/// </summary>
	Task FEr();

	/// <summary>
	///     Sets Disag to Win Mode
	/// </summary>
	Task FErn();

	/// <summary>
	///     Gets a new series from the Disag
	/// </summary>
	/// <param name="parameter"></param>
	/// <returns></returns>
	Task<DisagSerie> GetSeries(SeriesParameter parameter);

	/// <summary>
	///     Gets the serial number of the Disag
	/// </summary>
	/// <returns></returns>
	Task<string> GetSerialAsync();

	/// <summary>
	///     Gets the type of the Disag
	/// </summary>
	/// <returns></returns>
	Task<string> GetTypeAsync();

	/// <summary>
	///     Repeats the last operation
	/// </summary>
	/// <returns></returns>
	Task RepeatAsync();
}