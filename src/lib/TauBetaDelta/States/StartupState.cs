using OpenTK.Windowing.Common;

using ForgeWorks.GlowFork;
using OpenTK.Graphics.OpenGL4;
using ForgeWorks.GlowFork.Logging;

namespace ForgeWorks.TauBetaDelta;

/// <summary>
/// This is the first state to start the game.
/// We will use the Load function to install the AssetManager
/// </summary>
public class StartupState : GameState
{
    public StartupState() : base()
    {
        Name = nameof(StartupState);
        Next = nameof(LoadingState);

        View = new StartupView(this)
        {
            ClientSize = (800, 600),
            ViewPort = (800, 600),
            WindowBorder = WindowBorder.Hidden,
            WindowState = WindowState.Normal,
            IsVisible = false
        };
        View.Load += OnViewLoaded;
    }

    public override void Init()
    {
        //  this will actually never get called for this state because
        //  this state is assigned before the Game has sub to event
    }
    public override void Dispose()
    {
        View.Load -= OnViewLoaded;
        View.Dispose();
    }

    private void OnViewLoaded()
    {
        //  initialize GL error callback - see https://opentk.net/learn/appendix_opengl/debug_callback.html?tabs=debug-context-3%2Cdelegate-gl%2Cenable-gl
        if (GAME.RunMode == RunMode.Debug)
        {
            GL.DebugMessageCallback(GLLogger.DebugMessageDelegate, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
        }

        //  TODO: install AssetManager
        ChangeState(Next);
    }
}
