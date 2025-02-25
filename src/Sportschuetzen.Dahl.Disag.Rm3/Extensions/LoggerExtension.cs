using System.Runtime.CompilerServices;
using Serilog;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

/// <summary>
///     Extension methods for <see cref="object" />"
/// </summary>
public static class LoggerExtension
{
	public static bool EnableLog = false;
	/// <summary>
	///     Debug log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Debug(this object obj, string message, [CallerMemberName] string caller = "",
		[CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Debug($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}");
	}

	/// <summary>
	///     Debug log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="exception"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Debug(this object obj, string message, Exception exception,
		[CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Debug($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}", exception);
	}

	/// <summary>
	///     Error log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Error(this object obj, string message, [CallerMemberName] string caller = "",
		[CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Error($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}");
	}

	/// <summary>
	///     Error log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="exception"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Error(this object obj, string message, Exception exception,
		[CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Error($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}", exception);
	}

	/// <summary>
	///     Info log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Info(this object obj, string message, [CallerMemberName] string caller = "",
		[CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Information($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}");
	}

	/// <summary>
	///     Info log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="exception"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Info(this object obj, string message, Exception exception,
		[CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Information($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}",
			exception);
	}

	/// <summary>
	///     Warning log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Warning(this object obj, string message, [CallerMemberName] string caller = "",
		[CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Warning($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}");
	}

	/// <summary>
	///     Warning log
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	/// <param name="exception"></param>
	/// <param name="caller"></param>
	/// <param name="lineNumber"></param>
	public static void Warning(this object obj, string message, Exception exception,
		[CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
	{
		if (!EnableLog)
		{
			return;
		}
		Log.Logger.Warning($"Class [{obj.GetType()}], Caller [{caller}], Line [{lineNumber}], {message}",
			exception);
	}
}