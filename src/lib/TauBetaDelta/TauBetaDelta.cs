
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
        //  unload game & clean up managed resources
        AutoResetEvent taskEvent = new(false);
        Task.Factory.StartNew(() =>
        {
            Game.Unload(taskEvent);
        });

        taskEvent.WaitOne();
        taskEvent.Dispose();
    }
}
