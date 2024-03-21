namespace Sportschuetzen.Dahl.Disag.Rm3.Exceptions;

/// <summary>
///     Exception that is thrown when the received data could not be parsed
/// </summary>
public class ParseReceivedDataException : Exception
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ParseReceivedDataException" /> class.
	/// </summary>
	public ParseReceivedDataException()
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ParseReceivedDataException" /> class with a specified error message.
	/// </summary>
	/// <param name="message"></param>
	public ParseReceivedDataException(string message)
		: base(message)
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ParseReceivedDataException" /> class with a specified error message
	///     and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public ParseReceivedDataException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}