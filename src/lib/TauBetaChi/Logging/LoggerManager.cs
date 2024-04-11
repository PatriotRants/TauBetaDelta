using ForgeWorks.RailThorn;

namespace ForgeWorks.TauBetaDelta.Logging;

public delegate void ResourceLogger(LoadStatus status, string logEntry);

public class LoggerManager : IDisposable
{
    private static readonly Lazy<LoggerManager> _LOGGER_MGR = new(() => new());

    internal static LoggerManager Instance => _LOGGER_MGR.Value;
    internal static Queue<Exception> Exceptions { get; } = new();

    private List<ILogger> Loggers { get; } = new();

    public RunMode RunMode { get; private set; }

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
        if (logLevel == LogLevel.GLDebug)
        {
            //  assumption: only on GLDebug Logger
            var logger = Loggers
                .FirstOrDefault(l => l.LogLevel == logLevel);
            logger?.WriteLine($"[GLDebug] {message}");

            return;
        }

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
    public void Post(Exception exception, string message)
    {
        Exceptions.Enqueue(exception);

        Post(LoadStatus.Error, message);
    }
    public void Dispose()
    {
        foreach (ILogger logger in Loggers)
        {
            logger.Dispose();
        }
    }
    public void SetRunMode(RunMode runMode)
    {
        RunMode = runMode;
    }
}

internal class DefaultLogger : Logger
{
    private static readonly string LOG_FILE = $"GL_LOG_{GetFileGuid()}.log";

    private string LogDir { get; } = Path.Combine(LOG_DIR, "errors");
    private string LogFile { get; }

    internal DefaultLogger()
    {
        CheckLogDirectory(LogDir);
        LogLevel = LogLevel.Error;
        LogFile = Path.Combine(LogDir, LOG_FILE);
    }

    public override void WriteLine(string message)
    {
        //  we are only going to need this if we run into an exception
        if (!File.Exists(LogFile))
        {
            var logMsg = Encode($"*** Default Error Logger *** Open LogFile @{DateTime.UtcNow} ***\n");
            using (var logFile = File.Create(LogFile, logMsg.Length, FileOptions.None))
            {
                logFile.Write(logMsg);
            }
        }

        using (var logFile = File.OpenWrite(LogFile))
        {
            //  move to end of file
            logFile.Seek(0, SeekOrigin.End);
            logFile.Write(Encode($"*** {message} ***\n"));
            //  if we are writing here then we should have a call stack with an exception
            while (LoggerManager.Exceptions.TryDequeue(out Exception exception))
            {
                logFile.Write(Encode($"{exception}\n"));
            }
        }
    }

    public override void Dispose()
    {
        //  nothing to do here ...
    }
}
