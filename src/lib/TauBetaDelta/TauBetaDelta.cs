
using ForgeWorks.TauBetaDelta.Collections;

namespace ForgeWorks.TauBetaDelta;

public class TauBetaDelta : IDisposable
{
    private Game Game { get; }

    public TauBetaDelta()
    {
        Game = Registry.Get<Game>();
        Game.Initialize();
    }

    internal void Run()
    {
        Game.Start();
    }
    public void Dispose()
    {
        //  clean up managed resources
    }
}
