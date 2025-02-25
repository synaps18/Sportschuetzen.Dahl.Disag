using System.IO.Ports;

namespace Sportschuetzen.Dahl.Disag.Rm3.Sequences;

public class SerialSequencer
{
	private readonly SerialPort _serial;

	public SerialSequencer(SerialPort serial)
	{
		_serial = serial;
	}

	public bool SendEnq()
	{
		var finished = false;
		var buffer = new List<byte>();

		void SerialOnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			var sp = (SerialPort)sender;

			var localBuffer = new byte[sp.BytesToRead];

			sp.Read(localBuffer, 0, localBuffer.Length);

			buffer.AddRange(localBuffer);

			finished = true;
		}

		_serial.DataReceived += SerialOnDataReceived;

		try
		{
			//SEND ENQ

			while (!finished)
			{
			}


		}
		catch(Exception _)
		{
			return false;
		}
		finally
		{
			_serial.DataReceived -= SerialOnDataReceived;
		}

		return true;
	}

	
}