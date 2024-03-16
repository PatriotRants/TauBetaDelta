using ForgeWorks.TauBetaDelta.Presentation;

namespace ForgeWorks.TauBetaDelta.Extensibility;

public interface IGameState : IDisposable
{
    string Name { get; }
    IView View { get; }
}
