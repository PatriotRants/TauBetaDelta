using OpenTK.Mathematics;
using OpenTK.Graphics.ES11;

using ForgeWorks.GlowFork;
using ForgeWorks.GlowFork.Automata;

using ForgeWorks.TauBetaDelta.Extensibility;
using OpenTK.Windowing.Desktop;

namespace ForgeWorks.TauBetaDelta;

internal sealed partial class Game : IRegistryItem, IGame
{
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
            .WithStates("InitializeState", "LoadingState"));

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
            StateMachine.ChangeState<InitializeState>();
            _window = new Window(((IGameState)GameState).View);
        }

        StateMachine.StateChanged += OnStateChanged;
    }

    internal void Start()
    {
        //  start game
        _window.Run();
    }

    private void GameClockTick(TimeSpan span, DateTime time)
    {

    }
    private void OnStateChanged(State oldState)
    {
        //  unload old state
        ((GameState)oldState).Unload();
        //  load the new state
        GameState.Load();
        //  update window client view
        _window.SetView(((IGameState)GameState).View);
    }
}