using Common.SingleThreadedExecutors;
using System.Runtime.CompilerServices;

namespace Common.Logging;

/// <summary>
/// Allows simple, colored, timestamped, and line & caller managed logging to the console and timestamped log files
/// </summary>
public static class LogWriter
{
    public static LogLevel OutputLevel { get; set; } = LogLevel.DEBUG;
    public static bool ShouldConsoleLog { get; set; } = true;

    private static readonly string exePath = Directory.GetCurrentDirectory() ?? throw new Exception();
    private static readonly string logName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".log";
    private static readonly string logDirectoryPath = Path.Combine(exePath, "Logs");
    private static readonly string logPath = Path.Combine(logDirectoryPath, logName);

    private static readonly SingleThreadedActionExecutor<LogLevel, string, string, string, int> actionExecutor = new(Log);

    static LogWriter()
    {
        // Ensure log directory exists
        if (!Directory.Exists(logDirectoryPath))
        {
            _ = Directory.CreateDirectory(logDirectoryPath);
        }
    }

    // Called via actionExecutor
    private static void Log(LogLevel logLevel, string message, string callerFilePath, string callerMemberName, int callerLineNumber)
    {
        if (logLevel >= OutputLevel)
        {
            string callerInfoString = $"{callerMemberName} in {callerFilePath.Replace(exePath + "\\", "")} at line {callerLineNumber}";
            File.AppendAllText(logPath, $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} [{logLevel}] {{Called from {callerInfoString}}} << {message} >>{Environment.NewLine}");

            if (ShouldConsoleLog)
            {
                switch (logLevel)
                {
                    case LogLevel.DEBUG:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    case LogLevel.WARN:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogLevel.ERROR:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;

                    case LogLevel.FATAL:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss} {logLevel} from {callerInfoString}] {message}");
                Console.ResetColor();
            }
        }
    }

    /// <summary>
    /// Logs info to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogInfo(string? message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.INFO, message ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs debug statements to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogDebug(string? message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.DEBUG, message ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs warnings to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogWarn(string? message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.WARN, message ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs errors to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogError(string? message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.ERROR, message ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs fatal items to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogFatal(string? message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.FATAL, message ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs info to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogInfo(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.INFO, message?.ToString() ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs debug statements to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogDebug(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.DEBUG, message?.ToString() ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs warnings to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogWarn(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.WARN, message?.ToString() ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs errors to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogError(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.ERROR, message?.ToString() ?? "null", callerFilePath, callerMemberName, callerLineNumber);

    /// <summary>
    /// Logs info to the Console and log file
    /// </summary>
    /// <param name="message">Message that should be logged</param>
    /// <param name="callerMemberName">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerFilePath">Automatically provided by .NET. DO NOT USE!</param>
    /// <param name="callerLineNumber">Automatically provided by .NET. DO NOT USE!</param>
    public static void LogFatal(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.FATAL, message?.ToString() ?? "null", callerFilePath, callerMemberName, callerLineNumber);
}
