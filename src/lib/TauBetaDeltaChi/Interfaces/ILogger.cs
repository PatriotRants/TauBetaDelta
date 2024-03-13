namespace ForgeWorks.TauBetaDelta.Logging;

public interface ILogger
{
    LogLevel LogLevel { get; }

    void WriteLine(string message);
}
