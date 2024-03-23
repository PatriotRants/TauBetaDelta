namespace ForgeWorks.TauBetaDelta.Logging;

public interface ILogger : IDisposable
{
    LogLevel LogLevel { get; }

    void WriteLine(string message);
}
