
using ForgeWorks.GlowFork;

namespace ForgeWorks.TauBetaDelta.Logging;

public delegate void ResourceLogger(LoadStatus status, string logEntry);

public class LoggerManager : IDisposable
{
    private static readonly Lazy<LoggerManager> INSTANCE = new(() => new());

    private List<ILogger> Loggers { get; } = new();

    internal static LoggerManager Instance => INSTANCE.Value;

    private LoggerManager()
    {
        Add(new DefaultLogger());
    }

    public void Add(ILogger logger)
    {
        Loggers.Add(logger);
    }

    public void Post(LogLevel logLevel, string message)
    {
        foreach (var logger in Loggers.Where(l => l.LogLevel <= logLevel))
        {
            logger.WriteLine($"{$"[{logLevel}]",-15}{message}");
        }
    }

    public void Post(LoadStatus loadStatus, string message)
    {
        switch (loadStatus)
        {
            case LoadStatus.Okay:
            case LoadStatus.Ready:
                Post(LogLevel.Default, message);

                break;
            case LoadStatus.Error:
                Post(LogLevel.Error, $"*** {message}");

                break;
            case LoadStatus.Unknown:
            default:
                Post(LogLevel.Debug, message);

                break;
        }
    }

    public void Dispose()
    {
        foreach (ILogger logger in Loggers)
        {
            logger.Dispose();
        }
    }
}

internal class DefaultLogger : ILogger
{

    public LogLevel LogLevel { get; } = LogLevel.Debug;

    internal DefaultLogger()
    {

    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void Dispose()
    {
        //  nothing to do here ...
    }
}
