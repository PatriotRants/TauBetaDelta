using ForgeWorks.RailThorn;

namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IGame
{
    string Name { get; }
    string Title { get; }

    void ChangeState(string nextState);
    IGameState GetState();
}
