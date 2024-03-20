namespace Sportschuetzen.Dahl.Disag.Rm3.Exceptions;

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