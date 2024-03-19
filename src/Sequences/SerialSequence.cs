using Sportschützen.Dahl.DisagRm3.Enum;
using Sportschützen.Dahl.DisagRm3.Extensions;
using Sportschützen.Dahl.DisagRm3.Serial;
using Sportschützen.Dahl.DisagRm3.Structs;

namespace Sportschützen.Dahl.DisagRm3.Sequences;

internal abstract class SerialSequence<T> : IDisposable
{
    private readonly CancellationTokenSource _tokenSource = new();
    protected readonly SerialWrapper SerialWrapper;

    private int _awaitCounter;
    private Task? _awaitingDataTask;

    private bool _isBusy;
    protected bool AwaitingData = true;

    protected SerialSequence(SerialWrapper serialWrapper)
    {
        SerialWrapper = serialWrapper;
        serialWrapper.OnDataReceived += SerialWrapper_OnDataReceived;
    }

    public virtual void Dispose()
    {
        this.Info($"Disposing [{this.GetType()}]");
        SerialWrapper.OnDataReceived -= SerialWrapper_OnDataReceived;
    }

    protected virtual void SerialWrapper_OnDataReceived(object? sender, DisagResponse e)
    {
        KeepWaitingDataAlive();
    }

    public async Task<T> RequestData()
    {
        if (_isBusy) throw new Exception("Two sequences not allowed!");

        _isBusy = true;
        var result = await SequenceToCall();
        _isBusy = false;

        return result;
    }

    protected abstract Task<T> SequenceToCall();

    protected void ReleaseAwaitingData(int timeout = 0)
    {
        _tokenSource.CancelAfter(timeout);
    }

    private void KeepWaitingDataAlive()
    {
        _awaitCounter = 0;
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
}