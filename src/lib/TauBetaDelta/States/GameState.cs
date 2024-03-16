using ForgeWorks.GlowFork.Automata;
using ForgeWorks.GlowFork.Graphics;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Collections;
using ForgeWorks.TauBetaDelta.Presentation;
using ForgeWorks.TauBetaDelta.Extensibility;

namespace ForgeWorks.TauBetaDelta;

public abstract partial class GameState : State, IGameState
{
    private static readonly Resources RESOURCES = Registry.Get<Resources>();

    protected static readonly IGame GAME = Registry.Get<Game>();
    protected static readonly LoggerManager LOGGER = RESOURCES.LoggerManager;
    protected static readonly ShaderManager SHADERS = RESOURCES.ShaderManager;
    protected static readonly AssetManager ASSETS = RESOURCES.AssetManager;

    protected GameView View { get; init; }

    public string Title => GAME.Title;
    IView IGameState.View => View;

    protected GameState() { }

    public abstract void Init();
    public abstract void Dispose();

    protected void ChangeState(string nextState)
    {
        GAME.ChangeState(nextState);
    }

}