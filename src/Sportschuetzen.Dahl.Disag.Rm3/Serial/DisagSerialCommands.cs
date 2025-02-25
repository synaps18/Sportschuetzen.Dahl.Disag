using System.Diagnostics;
using System.IO.Ports;
using Sportschuetzen.Dahl.Disag.Models.Enum;
using Sportschuetzen.Dahl.Disag.Models.Structs;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;

namespace Sportschuetzen.Dahl.Disag.Rm3.Serial;

public class DisagSerialCommands
{
	private readonly SerialPort _serialPort;
	private readonly SemaphoreSlim _semaphore = new(1, 1);


	public DisagSerialCommands(string com)
	{
		_serialPort = new SerialPort(com)
		{
			Parity = Parity.None,
			StopBits = StopBits.One,
			DataBits = 8,
			Handshake = Handshake.None
		};
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

				var received = ReadFromSerialPort(port);
				if(!received.ValidateChecksum())
				{
					port.WriteDisagHex(EDisagHex.NAK);
					continue;
				}

				port.WriteDisagHex(EDisagHex.ACK);

				return received.RemoveChecksumAndCr().AsString();
			}
		});

		return receivedString.ToDisagResponse();
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

		await InvokeSerial(port =>
		{
			port.Write(dataAsByteArray, 0, 1);
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
			data.AddCr();

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
				while (_serialPort.BytesToRead == 0 && sw100Ms.ElapsedMilliseconds <= 100)
				{
					token?.ThrowIfCancellationRequested();
				}
				sw100Ms.Stop();
				sw30S.Stop();

				if(sw100Ms.ElapsedMilliseconds >= 100) continue;

				var received = ReadFromSerialPort(port).FirstOrDefault();
				if (received != (byte)EDisagHex.STX) throw new Exception($"Received not expected value. Received {(EDisagHex)received} instead of {EDisagHex.STX}");
			}
		});
	}

	private Task<string> InvokeSerialWithReturn(Func<SerialPort, string> invoke, EDisagBaudRate baud = EDisagBaudRate.B38400)
	{
		EnterSemaphore();

		try
		{
			OpenSerialPort(baud);
			var received = invoke(_serialPort);
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
			OpenSerialPort(baud);
			invoke(_serialPort);
		}
		finally
		{
			LeaveSemaphore();
		}

		return Task.CompletedTask;
	}


	private void OpenSerialPort(EDisagBaudRate baud)
	{
		if(_serialPort.IsOpen && _serialPort.BaudRate == (int)baud)
		{
			return;
		}

		CloseSerialPort();

		_serialPort.BaudRate = (int)baud;
		_serialPort.Open();
	}

	private void CloseSerialPort()
	{
		if (_serialPort.IsOpen)
		{
			_serialPort.Close();
		}
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
}