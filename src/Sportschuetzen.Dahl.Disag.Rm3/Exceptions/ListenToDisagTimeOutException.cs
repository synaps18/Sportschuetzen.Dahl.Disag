namespace Sportschuetzen.Dahl.Disag.Rm3.Exceptions;

/// <summary>
///     Exception thrown when the time out for listening to disag has been reached
/// </summary>
public class ListenToDisagTimeOutException : Exception
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ListenToDisagTimeOutException" /> class.
	/// </summary>
	public ListenToDisagTimeOutException()
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ListenToDisagTimeOutException" /> class.
	/// </summary>
	/// <param name="message"></param>
	public ListenToDisagTimeOutException(string message)
		: base(message)
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ListenToDisagTimeOutException" /> class.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public ListenToDisagTimeOutException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}