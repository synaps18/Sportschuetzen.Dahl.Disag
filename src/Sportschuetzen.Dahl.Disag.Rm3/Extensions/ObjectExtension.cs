using System.Runtime.CompilerServices;
using Serilog;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

public static class ObjectExtension
{
    public static void Debug(this object obj, string message, [CallerMemberName] string callermeber = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Debug($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}");
    }

    public static void Debug(this object obj, string message, Exception exception,
        [CallerMemberName] string callermeber = "", [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Debug($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}", exception);
    }

    public static void Error(this object obj, string message, [CallerMemberName] string callermeber = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Error($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}");
    }

    public static void Error(this object obj, string message, Exception exception,
        [CallerMemberName] string callermeber = "", [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Error($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}", exception);
    }

    public static void Info(this object obj, string message, [CallerMemberName] string callermeber = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Information($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}");
    }

    public static void Info(this object obj, string message, Exception exception,
        [CallerMemberName] string callermeber = "", [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Information($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}",
            exception);
    }

    public static void Warning(this object obj, string message, [CallerMemberName] string callermeber = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Warning($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}");
    }

    public static void Warning(this object obj, string message, Exception exception,
        [CallerMemberName] string callermeber = "", [CallerLineNumber] int lineNumber = 0)
    {
        Log.Logger.Warning($"Class [{obj.GetType()}], Caller [{callermeber}], Line [{lineNumber}], {message}",
            exception);
    }
}