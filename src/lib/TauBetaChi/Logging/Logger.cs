using System.Text;

namespace ForgeWorks.TauBetaDelta.Logging;

public abstract class Logger : ILogger
{
    protected const string LOG_DIR = "./logs";

    public LogLevel LogLevel { get; protected init; }


    static Logger()
    {
        CheckLogDirectory(LOG_DIR);
    }

    public abstract void WriteLine(string message);

    public abstract void Dispose();

    protected static string GetFileGuid()
    {
        var guidBytes = Guid.NewGuid()
            .ToByteArray()
            .TakeLast(4)
            .ToArray();

        var guid = BitConverter.ToString(guidBytes)
            .Replace("-", string.Empty);

        return guid.ToUpper();
    }
    protected static byte[] Encode(string logMessage)
    {
        return Encoding.UTF8.GetBytes(logMessage);
    }
    protected static bool CheckLogDirectory(string logDirectory)
    {
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        return Directory.Exists(logDirectory);
    }
}