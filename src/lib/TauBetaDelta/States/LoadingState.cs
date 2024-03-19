using OpenTK.Mathematics;

using ForgeWorks.GlowFork.Graphics;
using ErrorCode = ForgeWorks.GlowFork.ErrorCode;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.GlowFork.Tasking;
using ForgeWorks.ShowBird.Messaging;
using ForgeWorks.GlowFork;

namespace ForgeWorks.TauBetaDelta;

/// <summary>
/// Loading state will show the splash screen and begin loading assets
/// and resources.
/// </summary>
public class LoadingState : GameState
{
    //  TODO: make configurable
    private const int NUM_TASK_AGENTS = 2;

    private readonly List<Texture> textures = new();
    private readonly TaskQueue taskQueue = new(NUM_TASK_AGENTS);

    internal Shader Shader { get; set; }
    internal Texture[] Textures => textures.ToArray();

    public LoadingState() : base()
    {
        Name = nameof(LoadingState);
        Next = string.Empty;

        LoadSplash();

        taskQueue.Enqueue(new GameTask(LoadContent)
        {
            Updater = (s) =>
            {
                LOGGER.Post(LoadStatus.Okay, $"[Updater.1] {s}");
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
            ViewPort = GAME.GetState().View.ViewPort,
            ClientSize = GAME.GetState().View.ClientSize,
            WindowBorder = GAME.GetState().View.WindowBorder,
            WindowState = GAME.GetState().View.WindowState,
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
        gateEvent.WaitOne();

        //  when loading is complete we can call the next state from here.
    }
    public override void Dispose()
    {
        // delete shader programs
        Shader.Dispose();
        //  unsub events
        View.Load -= OnViewLoaded;
        //  dispose view
        View.Dispose();
    }

    private void LoadSplash()
    {
        Shader = SHADERS.LoadShader("splash.shaders");

        if (ASSETS.LoadTexture("splash", out Texture _texture))
        {
            textures.Add(_texture);
        }
        else
        {
            //  log error condition
        }
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
                        case ErrorCode.NoErr:
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
            LOGGER.Post(LogLevel.Error, ex.Message);
            LOGGER.Post(LogLevel.Error, $"Source: '{ex.Source}'");
            LOGGER.Post(LogLevel.Error, $"Target: '{ex.TargetSite}");
            LOGGER.Post(LogLevel.Error, ex.StackTrace);
        }

        return ASSETS.Info;
    }
    private static string StartNetwork(UpdateAgent updateAgent)
    {
        return NETWORK.StartNetwork(updateAgent);
    }
    private void OnViewLoaded()
    {
        //  TODO: install AssetManager
        View.CenterWindow();
    }
}
