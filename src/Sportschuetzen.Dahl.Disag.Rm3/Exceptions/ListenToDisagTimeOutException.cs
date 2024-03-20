namespace Sportschuetzen.Dahl.Disag.Rm3.Exceptions;

public class ListenToDisagTimeOutException : Exception
{
    public ListenToDisagTimeOutException()
    {
    }

    public ListenToDisagTimeOutException(string message)
        : base(message)
    {
    }

    public ListenToDisagTimeOutException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}