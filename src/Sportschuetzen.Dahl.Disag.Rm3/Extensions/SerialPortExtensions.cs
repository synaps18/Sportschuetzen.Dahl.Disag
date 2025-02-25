using System.IO.Ports;
using Sportschuetzen.Dahl.Disag.Models.Enum;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

public static class SerialPortExtensions
{
	public static void WriteDisagHex(this SerialPort port, EDisagHex disagHex)
	{
		port.Write(new[] { (byte)disagHex }, 0, 1);
	}
}