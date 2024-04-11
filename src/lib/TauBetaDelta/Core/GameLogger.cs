using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

internal sealed partial class Game
{
    private class GameLogger : Logger
    {
        private static readonly string LOG_FILE = $"tbd_{GetFileGuid()}.log";
        private static readonly object OBJ_LOCK = new();

        private string LogDir { get; } = LOG_DIR;
        private string LogFile { get; }

        internal GameLogger(LogLevel logLevel)
        {
            LogLevel = logLevel;
            LogFile = Path.Combine(LogDir, LOG_FILE);

            if (!File.Exists(LogFile))
            {
                var logMsg = Encode($"*** Game Logger *** Open LogFile @{DateTime.UtcNow} ***\n");
                using (var logFile = File.Create(LogFile, logMsg.Length, FileOptions.None))
                {
                    logFile.Write(logMsg);
                }
            }
        }

        public override void WriteLine(string message)
        {
            lock (OBJ_LOCK)
            {
                using (var logFile = File.OpenWrite(LogFile))
                {
                    //  move to end of file
                    logFile.Seek(0, SeekOrigin.End);
                    logFile.Write(Encode($"{message}\n"));
                }
            }
        }
        public override void Dispose()
        {
            // ... ???
        }
    }
}