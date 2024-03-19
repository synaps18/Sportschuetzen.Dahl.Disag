namespace Sportschützen.Dahl.DisagRm3.Exceptions;

public class ConvertDataToSpiegelException : Exception
{
    public ConvertDataToSpiegelException()
    {
    }

    public ConvertDataToSpiegelException(string message)
        : base(message)
    {
    }

    public ConvertDataToSpiegelException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}