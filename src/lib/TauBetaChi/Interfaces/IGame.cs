using ForgeWorks.GlowFork;
using ForgeWorks.RailThorn;

namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IGame
{
    string Name { get; }
    string Title { get; }
    RunMode RunMode { get; }

    void ChangeState(string nextState);
    IGameState GetState();
}
