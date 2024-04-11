using ForgeWorks.GlowFork.Tasking;

using ForgeWorks.RailThorn;
using ForgeWorks.ShowBird.Messaging;
using ForgeWorks.TauBetaDelta.Collections;

namespace ForgeWorks.TauBetaDelta;

/* **
    Shut down vs shutdown:

    This is a ShutDown state indicating 2 words which is a verb phrase. One word - Shutdown is a noun.
    Since this is an action state, we treat it as a verb - Shut Down ...
    
    Sounds weird for a coding discussion, but my brain had to be settled with the correct use.
** */
public class ShutDownState : GameState
{
    //  ShutDownState will not have it's own view. It will need to inherit the view from the previous state.
    private const int NUM_TASK_AGENTS = 1;

    private static readonly Resources RESOURCES = Registry.Get<Resources>();

    private readonly TaskQueue taskQueue = new(NUM_TASK_AGENTS);
    private readonly AutoResetEvent taskEvent = new(false);

    public ShutDownState() : base()
    {
        Name = nameof(ShutDownState);
        Next = string.Empty;

        taskQueue.Enqueue(new GameTask(SaveGame)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Update] {s}");
            }
        });
        taskQueue.Enqueue(new GameTask(ShutDownNetwork)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Update] {s}");
            }
        });
        taskQueue.Enqueue(new GameTask(UnloadResources)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Update] {s}");
            }
        });
    }

    public override void Init()
    {
        ManualResetEvent gateEvent = new(false);

        LOGGER.Post(LoadStatus.Okay, $"[{DateTime.UtcNow}] Begin Shut Down...");
        //  run task queue as synchronous
        taskQueue.Start(gateEvent, true);

        gateEvent.WaitOne();
        gateEvent.Dispose();

        LOGGER.Post(LoadStatus.Okay, $"[{DateTime.UtcNow}] Shut Down Complete!");
    }

    private string SaveGame(UpdateAgent updateAgent)
    {
        updateAgent.Invoke($"[{nameof(SaveGame)}] Begin Save Game");

        //  nothing to do here yet

        //taskEvent.WaitOne();

        return $"[{nameof(SaveGame)}] Complete";
    }
    private string ShutDownNetwork(UpdateAgent updateAgent)
    {
        string status = "Skip Network Shutdown (Client Not Available)";

        if (NETWORK.IsAvailable)
        {
            updateAgent.Invoke($"[{nameof(ShutDownNetwork)}] Begin Network Shut Down");

            NETWORK.Unload(taskEvent);
            taskEvent.WaitOne();

            status = "Complete";
        }

        return $"[{nameof(ShutDownNetwork)}] {status}";
    }
    private string UnloadResources(UpdateAgent updateAgent)
    {
        updateAgent.Invoke($"[{nameof(UnloadResources)}] Begin Resource Unload");

        RESOURCES.Unload(taskEvent);
        taskEvent.WaitOne();

        return $"[{nameof(UnloadResources)}] Complete";
    }

    public override void Dispose()
    {

        //  last to be disposed to keep logging alive
        LOGGER.Dispose();

        taskEvent.Dispose();
    }
}