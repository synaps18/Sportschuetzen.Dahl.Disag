namespace Sportschützen.Dahl.DisagRm3.Exceptions;

public class ParseReceivedDataException : Exception
{
    public ParseReceivedDataException()
    {
    }

    public ParseReceivedDataException(string message)
        : base(message)
    {
    }

    public ParseReceivedDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}