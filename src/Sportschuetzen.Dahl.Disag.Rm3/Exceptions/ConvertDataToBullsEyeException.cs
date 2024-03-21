namespace Sportschuetzen.Dahl.Disag.Rm3.Exceptions;

/// <summary>
///     Exception thrown when the data could not be converted to a bulls eye
/// </summary>
public class ConvertDataToBullsEyeException : Exception
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ConvertDataToBullsEyeException" /> class.
	/// </summary>
	public ConvertDataToBullsEyeException()
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ConvertDataToBullsEyeException" /> class with a specified error
	///     message.
	/// </summary>
	/// <param name="message"></param>
	public ConvertDataToBullsEyeException(string message)
		: base(message)
	{
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ConvertDataToBullsEyeException" /> class with a specified error
	///     message and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public ConvertDataToBullsEyeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}