using OpenTK.Windowing.Common;

using ForgeWorks.GlowFork;

namespace ForgeWorks.TauBetaDelta;

/// <summary>
/// This is the first state to start the game.
/// We will use the Load function to install the AssetManager
/// </summary>
public class InitializeState : GameState
{
    public InitializeState() : base()
    {
        Name = nameof(InitializeState);
        Next = nameof(LoadingState);

        View = new InitialView(this)
        {
            ClientSize = (1200, 900),
            ViewPort = (1200, 900),
            WindowBorder = WindowBorder.Resizable,
            WindowState = WindowState.Maximized,
            ViewRefresh = OnViewRefresh
        };

        Clock.Start();
    }

    public override void Load()
    {
        //  TODO: install AssetManager
    }

    public override void Unload()
    {
        //  nothing to unload here (yet)...
    }

    private void OnViewRefresh()
    {
        ChangeState(Next);
    }
}
