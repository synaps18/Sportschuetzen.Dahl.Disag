using Sportschuetzen.Dahl.Disag.Models.Auswertung;

namespace Sportschuetzen.Dahl.Disag.Rm3;

///<inheritdoc />
public class DisagRm3Simulation : IDisagRm3
{
	///<inheritdoc />
	public event EventHandler<bool>? ConnectionChanged;

	///<inheritdoc />
	public event EventHandler<bool>? IsWorkingChanged;

	private const int DisagDefaultTimeout = 200;

	///<inheritdoc />
	public async Task CancelAsync()
	{
		await InvokeDisag(() => Task.CompletedTask);
	}

	///<inheritdoc />
	public bool Connect()
	{
		var result = InvokeDisag(() => true);
		OnConnectionChanged(result);
		return result;
	}

	///<inheritdoc />
	public bool Disconnect()
	{
		var result = InvokeDisag(() => false);
		OnConnectionChanged(result);
		return result;
	}

	///<inheritdoc />
	public async Task EndAsync()
	{
		await InvokeDisag(() => Task.CompletedTask);
	}

	///<inheritdoc />
	public async Task FEr()
	{
		await InvokeDisag(() => Task.CompletedTask);
	}

	///<inheritdoc />
	public async Task FErn()
	{
		await InvokeDisag(() => Task.CompletedTask);
	}

	///<inheritdoc />
	public async Task<string> GetSerialAsync()
	{
		return await InvokeDisag(() => Task.FromResult("123 FAKE"));
	}

	///<inheritdoc />
	public async Task<DisagSerie> GetSeries(SeriesParameter parameter)
	{
		var result = await InvokeDisag(() => Task.FromResult(new DisagSerie
		{
			Streifen = new List<DisagStreifen>
			{
				new()
				{
					Spiegel = new List<DisagSpiegel>
					{
						new()
						{
							Schüsse = new List<DisagSchuss>
							{
								new()
							}
						}
					}
				}
			}
		}), 3000);

		return result;
	}

	///<inheritdoc />
	public async Task<string> GetTypeAsync()
	{
		return await InvokeDisag(() => Task.FromResult("TYPE FAKE"));
	}

	///<inheritdoc />
	public async Task RepeatAsync()
	{
		await InvokeDisag(() => Task.CompletedTask);
	}

	private void InvokeDisag(Action action, int timeout = DisagDefaultTimeout)
	{
		OnIsWorkingChanged(true);
		Thread.Sleep(timeout);
		action();
		OnIsWorkingChanged(false);
	}

	private async Task InvokeDisag(Func<Task> action, int timeout = DisagDefaultTimeout)
	{
		OnIsWorkingChanged(true);
		await Task.Delay(timeout);
		await action();
		OnIsWorkingChanged(false);
	}

	private T InvokeDisag<T>(Func<T> action, int timeout = DisagDefaultTimeout)
	{
		OnIsWorkingChanged(true);
		Thread.Sleep(timeout);
		var result = action();
		OnIsWorkingChanged(false);
		return result;
	}

	private async Task<T> InvokeDisag<T>(Func<Task<T>> action, int timeout = DisagDefaultTimeout)
	{
		OnIsWorkingChanged(true);
		await Task.Delay(timeout);
		var result = await action();
		OnIsWorkingChanged(false);
		return result;
	}

	protected virtual void OnConnectionChanged(bool e)
	{
		ConnectionChanged?.Invoke(this, e);
	}

	protected virtual void OnIsWorkingChanged(bool e)
	{
		IsWorkingChanged?.Invoke(this, e);
	}
}