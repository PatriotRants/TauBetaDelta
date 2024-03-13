using ForgeWorks.TauBetaDelta.Presentation;

namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IGameState
{
    string Name { get; }
    IView View { get; }
}
