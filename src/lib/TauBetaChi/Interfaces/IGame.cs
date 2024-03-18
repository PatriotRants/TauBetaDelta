namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IGame : IDisposable
{
    string Name { get; }
    string Title { get; }

    void ChangeState(string nextState);
    IGameState GetState();
}
