using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL4;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.RailThorn.Logging;

public class GLLogger : Logger
{
    private static readonly string LOG_FILE = $"GL_LOG_{GetFileGuid()}.log";
    private static readonly Lazy<GLLogger> _GL_LOGGER = new(() => new());
    private static readonly object OBJ_LOCK = new();

    private string LogDir { get; } = Path.Combine(LOG_DIR, "gl_logs");

    internal static GLLogger Instance => _GL_LOGGER.Value;
    internal static DebugProc DebugMessageDelegate = GLLog;

    private RunMode RunMode { get; }
    private string LogFile { get; }

    private GLLogger()
    {
        CheckLogDirectory(LogDir);
        LogLevel = LogLevel.GLDebug;

        LoggerManager.Instance.Add(this);
        //  TODO:   use IApplicationContext interface
        RunMode = LoggerManager.Instance.RunMode;
        LogFile = Path.Combine(LogDir, LOG_FILE);

        if (!File.Exists(LogFile))
        {
            var logMsg = Encode($"*** GL_LOGGER *** Open LogFile @{DateTime.UtcNow} ***\n");
            using (var logFile = File.Create(LogFile, logMsg.Length, FileOptions.None))
            {
                logFile.Write(logMsg);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source">Source of the debugging message.</param>
    /// <param name="type">Type of the debugging message.</param>
    /// <param name="id">ID associated with the message.</param>
    /// <param name="severity">Severity of the message.</param>
    /// <param name="length">Length of the string in pMessage.</param>
    /// <param name="pMessage">Pointer to message string.</param>
    /// <param name="pUserParam">Disregarded</param>
    public static void GLLog(DebugSource source, DebugType type, int id,
                      DebugSeverity severity, int length, IntPtr pMessage, IntPtr pUserParam)
    {
        // In order to access the string pointed to by pMessage, you can use Marshal
        // class to copy its contents to a C# string without unsafe code. You can
        // also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
        string message = Marshal.PtrToStringAnsi(pMessage, length);

        // The rest of the function is up to you to implement, however a debug output
        // is always useful.
        Instance.WriteLine($"[{severity} source={source} type={type} id={id}] {message}");
    }

    public void LogGLError(string methodName)
    {
        OpenTK.Graphics.OpenGL4.ErrorCode errCode = GL.GetError();
        if (errCode != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
        {
            WriteLogEntry($"[{LoadStatus.Error}] [{methodName}] {errCode}");
        }
    }
    public override void WriteLine(string message)
    {
        if (RunMode == RunMode.Debug)
        {
            if (message.IndexOf($"[GLDebug]") >= 0)
            { WriteLogEntry($"[{nameof(GLLogger)}] {message}\n"); }
        }
    }

    private void WriteLogEntry(string message)
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
