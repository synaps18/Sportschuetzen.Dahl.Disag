using Sportschützen.Dahl.DisagRm3.Auswertung;

namespace Sportschützen.Dahl.DisagRm3;

public interface IDisagRm3
{
	event EventHandler<bool>? OnConnectionChanged;
	event EventHandler<bool>? IsWorkingChanged;
	bool Connect();
	Task CancelAsync();
	Task EndAsync();

	/// <summary>
	///     Sets Disag to Win Mode
	/// </summary>
	Task FEr();

	/// <summary>
	///     Sets Disag to Win Mode
	/// </summary>
	Task FErn();

	Task<string> GetSerialAsync();
	Task<string> GetTypeAsync();
	Task RepeatAsync();
	Task<DisagSerie> SetNewSeries(SeriesParameter parameter);
}