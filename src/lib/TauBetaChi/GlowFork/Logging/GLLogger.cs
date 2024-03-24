using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL4;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.GlowFork.Logging;

public class GLLogger : ILogger
{
    private static readonly Lazy<GLLogger> _GL_LOGGER = new(() => new());

    internal static GLLogger Instance => _GL_LOGGER.Value;
    internal static DebugProc DebugMessageDelegate = GLLog;

    private GLLogger()
    {
        LoggerManager.Instance.Add(this);
    }


    public LogLevel LogLevel => LogLevel.GLDebug;

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
            WriteLine($"[{LoadStatus.Error}] [{methodName}] {errCode}");
        }

    }
    public void WriteLine(string message)
    {
        Console.WriteLine($"[{nameof(GLLogger)}] {message}");
    }

    public void Dispose()
    {
        //  nothing to do here ...
    }
}
