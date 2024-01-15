using Microsoft.Extensions.Logging;

namespace CS2GSI.TestApp;

public class Logger : ILogger
{
    private readonly LogLevel _enabledLoglevel;
    private readonly ConsoleColor _defaultForegroundColor = Console.ForegroundColor;
    private readonly ConsoleColor _defaultBackgroundColor = Console.BackgroundColor;

    public Logger(LogLevel logLevel = LogLevel.Information)
    {
        _enabledLoglevel = logLevel;
    }
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        Console.ForegroundColor = ForegroundColorForLogLevel(logLevel);
        Console.BackgroundColor = BackgroundColorForLogLevel(logLevel);
        Console.Write(logLevel.ToString()[..3].ToUpper());
        Console.ResetColor();
        // ReSharper disable once LocalizableElement
        Console.Write($" [{DateTime.UtcNow:HH:mm:ss.fff}] ");
        Console.WriteLine(formatter.Invoke(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _enabledLoglevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    private ConsoleColor ForegroundColorForLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Error or LogLevel.Critical => ConsoleColor.Black,
            LogLevel.Debug => ConsoleColor.Black,
            LogLevel.Information => ConsoleColor.White,
            _ => _defaultForegroundColor
        };
    }
    
    private ConsoleColor BackgroundColorForLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Error or LogLevel.Critical => ConsoleColor.Red,
            LogLevel.Debug => ConsoleColor.Yellow,
            LogLevel.Information => ConsoleColor.Black,
            _ => _defaultBackgroundColor
        };
    }
}