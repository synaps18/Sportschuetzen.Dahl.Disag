using System.IO.Ports;
using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

public static class SerialPortExtensions
{
	public static void WriteDisagHex(this SerialPort port, EDisagHex disagHex)
	{
		var array = new[] { (byte)disagHex };
		port.Write(array, 0, array.Length);
	}

	public static void OpenSerialPort(this SerialPort port, EDisagBaudRate baud)
	{
		if (port.IsOpen && port.BaudRate == (int)baud)
		{
			return;
		}

		port.CloseSerialPort();

		port.BaudRate = (int)baud;
		port.Open();
	}

	public static void CloseSerialPort(this SerialPort port)
	{
		if (port.IsOpen)
		{
			port.Close();
		}
	}
}