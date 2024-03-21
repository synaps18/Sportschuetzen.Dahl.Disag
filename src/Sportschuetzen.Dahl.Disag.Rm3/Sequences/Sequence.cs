using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Serial;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

internal abstract class Sequence<T> : IDisposable
{
	private readonly CancellationTokenSource _tokenSource = new();
	protected readonly SerialHandler SerialHandler;

	private bool _isBusy;
	protected bool AwaitingData = true;

	private int _awaitCounter;
	private Task? _awaitingDataTask;

	protected Sequence(SerialHandler serialHandler)
	{
		SerialHandler = serialHandler;
		serialHandler.OnDataReceived += SerialHandler_OnDataReceived;
	}

	public virtual void Dispose()
	{
		this.Info($"Disposing [{GetType()}]");
		SerialHandler.OnDataReceived -= SerialHandler_OnDataReceived;
	}

	public async Task<T> RequestData()
	{
		if (_isBusy) throw new Exception("Two sequences not allowed!");

		_isBusy = true;
		var result = await SequenceToCall();
		_isBusy = false;

		return result;
	}


	protected async Task AwaitDataAsync()
	{
		var token = _tokenSource.Token;

		if (_awaitingDataTask?.Status == TaskStatus.Running)
		{
			this.Error("Already waiting for data!");
			return;
		}

		_awaitingDataTask = Task.Run(async () =>
		{
			this.Debug("Awaiting Data");
			while (AwaitingData & !token.IsCancellationRequested)
				try
				{
					await Task.Delay(10);
					if (++_awaitCounter > 1000)
					{
						this.Error("Failed waiting data!");
						return;
					}
				}
				catch (Exception e)
				{
					this.Error("Error waiting data", e);
					throw;
				}

			AwaitingData = true;
		});

		await _awaitingDataTask.WaitAsync(CancellationToken.None);
	}

	private void KeepWaitingDataAlive()
	{
		_awaitCounter = 0;
	}

	protected void ReleaseAwaitingData(int timeout = 0)
	{
		_tokenSource.CancelAfter(timeout);
	}

	protected abstract Task<T> SequenceToCall();

	protected virtual void SerialHandler_OnDataReceived(object? sender, DisagResponse e)
	{
		KeepWaitingDataAlive();
	}
}