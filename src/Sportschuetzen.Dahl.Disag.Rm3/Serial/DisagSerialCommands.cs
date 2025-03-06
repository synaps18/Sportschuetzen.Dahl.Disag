using System.Diagnostics;
using System.IO.Ports;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Structs;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;

namespace Sportschuetzen.Dahl.Disag.Rm3.Serial;

public class DisagSerialCommands : IDisposable
{
	private readonly string _comPort;
	private readonly SemaphoreSlim _semaphore = new(1, 1);

	private SerialPort GetSerialPort(string comPort)
	{
		return new SerialPort(comPort)
		{
			Parity = Parity.None,
			StopBits = StopBits.One,
			DataBits = 8,
			Handshake = Handshake.None
		};
	}

	public DisagSerialCommands(string comPort)
	{
		_comPort = comPort;
	}
	
	public async Task<DisagCommand> Await_Disag_Response(CancellationToken? token = null)
	{
		var receivedString = await InvokeSerialWithReturn(port =>
		{
			var sw30S = new Stopwatch();

			while (true)
			{
				sw30S.Start();
				while (port.BytesToRead == 0 && sw30S.ElapsedMilliseconds < 30000)
				{
					token?.ThrowIfCancellationRequested();
				}
				sw30S.Stop();

				if (sw30S.ElapsedMilliseconds >= 30000)
				{
					port.WriteDisagHex(EDisagHex.NAK);
					throw new TimeoutException("Timeout while waiting for STA (Start series analysis");
				}

				var received = new List<byte>();

				while (received.LastOrDefault() != (byte)EDisagHex.CR)
				{
					var receivedChunk = ReadFromSerialPort(port);
					received.AddRange(receivedChunk);
				}

				//if (!receivedArray.ValidateChecksum())
				//{
				//	port.WriteDisagHex(EDisagHex.NAK);
				//	continue;
				//}

				port.WriteDisagHex(EDisagHex.ACK);


				var receivedArray = new string(received.ToArray().ToCharArray());

				return receivedArray ?? string.Empty;
			}
		});

		return receivedString.ToDisagResponse(false);
	}

	/// <summary>
	/// Follows the protocol to send a command by computer to the Disag device
	/// </summary>
	/// <param name="command"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	public async Task Send_Disag_Command(EDisagCommand command, CancellationToken? token = null)
	{
		await Send_Enq(token);
		await Send_Command(command.ToString(), token);
	}

	public async Task Send_Mode(EDisagMode mode)
	{
		var stringedMode = mode.ToString();
		var dataAsByteArray = stringedMode.ToByteArray(false);
		var dataToSend = dataAsByteArray.AddCr();

		await InvokeSerial(port =>
		{
			port.Write(dataToSend, 0, dataToSend.Length);
		}, EDisagBaudRate.B2400);
	}


	/// <summary>
	/// Follows the protocol to send a command by computer to the Disag device
	/// </summary>
	/// <param name="command"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	public async Task Send_Disag_Command(string command, CancellationToken? token = null)
	{
		await Send_Enq(token);
		await Send_Command(command, token);
	}

	private async Task Send_Command(string commandString, CancellationToken? token = null)
	{
		await InvokeSerial(port =>
		{
			var data = commandString.ToByteArray(true);
			data = data.AddCr();

			var sw300Ms = new Stopwatch();

			var counter = 0;

			while(true) 
			{
				token?.ThrowIfCancellationRequested();
				if (++counter > 5) throw new Exception("Received not expected value");

				port.Write(data, 0, data.Length);
				
				sw300Ms.Restart();
				while (port.BytesToRead == 0 && sw300Ms.ElapsedMilliseconds < 300)
				{
					token?.ThrowIfCancellationRequested();
				}
				sw300Ms.Stop();

				if (sw300Ms.ElapsedMilliseconds >= 300) continue;
				
				var received = ReadFromSerialPort(port).FirstOrDefault();

				if (received == (byte)EDisagHex.NAK) continue;
				if(received != (byte)EDisagHex.ACK) throw new Exception($"Received not expected value. Received {(EDisagHex)received} instead of {EDisagHex.ACK}");
				break;
			}
		});
	}

	private async Task Send_Enq(CancellationToken? token = null)
	{
		await InvokeSerial(port =>
		{
			var sw100Ms = new Stopwatch();
			var sw30S = new Stopwatch();

			while(true) 
			{
				token?.ThrowIfCancellationRequested();
				if (sw30S.ElapsedMilliseconds >= 30000) throw new Exception("No response from Disag");
				sw30S.Start();

				port.WriteDisagHex(EDisagHex.ENQ);

				sw100Ms.Restart();
				while (port.BytesToRead == 0 && sw100Ms.ElapsedMilliseconds <= 100)
				{
					token?.ThrowIfCancellationRequested();
				}
				sw100Ms.Stop();
				sw30S.Stop();

				if(sw100Ms.ElapsedMilliseconds >= 100) 
					continue;

				var received = ReadFromSerialPort(port).FirstOrDefault();
				if (received != (byte)EDisagHex.STX) throw new Exception($"Received not expected value. Received {received} instead of {EDisagHex.STX}");
				break;
			}
		});
	}

	private Task<string> InvokeSerialWithReturn(Func<SerialPort, string> invoke, EDisagBaudRate baud = EDisagBaudRate.B38400)
	{
		EnterSemaphore();

		try
		{
			using var serialPort = GetSerialPort(_comPort);
			serialPort.OpenSerialPort(baud);
			var received = invoke(serialPort);
			return Task.FromResult(received);
		}
		finally
		{
			LeaveSemaphore();
		}
	}

	private Task InvokeSerial(Action<SerialPort> invoke, EDisagBaudRate baud = EDisagBaudRate.B38400)
	{
		EnterSemaphore();

		try
		{
			using var serialPort = GetSerialPort(_comPort);
			serialPort.OpenSerialPort(baud);
			invoke(serialPort);
		}
		finally
		{
			LeaveSemaphore();
		}

		return Task.CompletedTask;
	}

	private byte[] ReadFromSerialPort(SerialPort sender)
	{
		var buffer = new byte[sender.BytesToRead];
		sender.Read(buffer, 0, buffer.Length);
		return buffer;
	}

	private void EnterSemaphore()
	{
		if(!_semaphore.Wait(0))
		{
			throw new SemaphoreFullException("Semaphore full on serial port handler");
		};
	}

	private void LeaveSemaphore()
	{
		_semaphore.Release();
	}

	public void Dispose()
	{
		_semaphore.Dispose();
	}
}