using OpenTK.Mathematics;

using ForgeWorks.ShowBird.Messaging;

using ForgeWorks.GlowFork.Tasking;

using ForgeWorks.RailThorn;
using ErrorCode = ForgeWorks.RailThorn.ErrorCode;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

/// <summary>
/// Loading state will show the splash screen and begin loading assets
/// and resources.
/// </summary>
public class LoadingState : GameState
{
    //  TODO: make configurable
    private const int NUM_TASK_AGENTS = 2;

    private readonly TaskQueue taskQueue = new(NUM_TASK_AGENTS);

    public LoadingState() : base()
    {
        Name = nameof(LoadingState);
        Next = string.Empty;

        taskQueue.Enqueue(new GameTask(LoadContent)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Updater.1] {s}");
            }
        });
        taskQueue.Enqueue(new GameTask(LoadFonts)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Updater.2] {s}");
            }
        });
        taskQueue.Enqueue(new GameTask(StartNetwork)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Updater.2] {s}");
            }
        });

        View = new LoadingView(this)
        {
            //  change the background color
            Background = new Color4(27, 27, 27, 128),
            //  inherit previous state window settings
            ViewPort = GAME.GetState().View.ViewPort,
            ClientSize = GAME.GetState().View.ClientSize,
            WindowBorder = GAME.GetState().View.WindowBorder,
            WindowState = GAME.GetState().View.WindowState,
            //  set visible
            IsVisible = true,
        };
        View.Load += OnViewLoaded;

        LOGGER.Post(LogLevel.Default, $"{Name}.View [{((LoadingView)View).ClientSize};{((LoadingView)View).Location};{((LoadingView)View).ViewPort}]");
    }

    public override void Init()
    {
        ManualResetEvent gateEvent = new(false);

        View.Init();

        //  start game task execution
        taskQueue.Start(gateEvent);
        //  wait for taskQueue to finish
        gateEvent.WaitOne();

        //  when loading is complete we can call the next state from here.
    }
    public override void Dispose()
    {
        //  unsub events
        View.Load -= OnViewLoaded;
        //  dispose view
        View.Dispose();
    }

    private static string LoadFonts(UpdateAgent updateAgent)
    {
        updateAgent($"[{LogLevel.Debug}] {FONTS.Info}");

        return $"Loaded {FONTS.LoadFonts(updateAgent)} fonts";
    }
    private static string LoadContent(UpdateAgent updateAgent)
    {
        //  TODO: Load assets & resources
        try
        {
            updateAgent($"[{LogLevel.Debug}] {ASSETS.Info}");
            if (!ASSETS.Load(updateAgent))
            {


                if (ASSETS.GetError(out ErrorCode errCode))
                {
                    switch (errCode)
                    {
                        case ErrorCode.DirNotFound:
                            throw new DirectoryNotFoundException(ASSETS.Info);

                        //  break;
                        case ErrorCode.Ok:
                        default:
                            //  nothing to do ... ???

                            break;
                    }
                }
                //  throw exception and halt loading
            }
        }
        catch (DirectoryNotFoundException ex)
        {
            LOGGER.Post(ex, ex.Message);
        }

        return ASSETS.Info;
    }
    private static string StartNetwork(UpdateAgent updateAgent)
    {
        updateAgent(NETWORK.StartNetwork(updateAgent));

        return "Network Start Up Complete";
    }
    private void OnViewLoaded()
    {
        //  TODO: install AssetManager
        View.CenterWindow();
    }
}
