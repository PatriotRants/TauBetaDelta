using OpenTK.Windowing.Desktop;

using ForgeWorks.ShowBird;

using ForgeWorks.GlowFork;
using ForgeWorks.GlowFork.Automata;

using ForgeWorks.TauBetaDelta.Collections;
using ForgeWorks.TauBetaDelta.Extensibility;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ForgeWorks.TauBetaDelta;

internal sealed partial class Game : IRegistryItem, IGame
{
    private static readonly INetwork NETWORK = Registry.Get<Network>();

    private readonly Lazy<StateMachine> _stateMachine;

    private StateMachine StateMachine => _stateMachine.Value;

    internal IGLFWGraphicsContext WindowContext => _window.Context;

    public GameState GameState => StateMachine.GetCurrent<GameState>();
    public string Name { get; }
    public string Title { get; }

    internal Game(string name, string title)
    {
        Name = name;
        Title = title;

        _stateMachine = new(() => new StateMachine(GameState.Empty)
            .WithStates(nameof(StartupState), nameof(LoadingState)));

        Clock.Instance.Tick += GameClockTick;
    }

    void IGame.ChangeState(string nextState)
    {
        //  TODO: transition from current state to new state
        StateMachine.ChangeState(nextState);
    }
    IGameState IGame.GetState()
    {
        return GameState;
    }

    internal void Initialize()
    {
        //  setting to initial state
        if (StateMachine.Current == GameState.Empty)
        {
            StateMachine.ChangeState<StartupState>();
            _window = new Window(((IGameState)GameState).View);
        }

        StateMachine.StateChanged += OnStateChanged;
    }

    internal void Start()
    {
        //  start game
        _window.Run();
    }
    internal void Stop()
    {
        _window.Close();
    }

    private void GameClockTick(TimeSpan span, DateTime time)
    {

    }
    private void OnStateChanged(State oldState)
    {
        //  load the new state
        GameState.Init();
        //  update window client view (scene)
        _window.ChangeView(((IGameState)GameState).View);
        //  dispose old state
        ((GameState)oldState).Dispose();
    }

    internal void CenterWindow()
    {
        _window.CenterWindow();
    }
    internal KeyboardState GetKeyboardState()
    {
        return _window.KeyboardState;
    }
    internal MouseState GetMouseState()
    {
        return _window.MouseState;
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    public void Dispose()
    {
        NETWORK.StopNetwork();
    }
}