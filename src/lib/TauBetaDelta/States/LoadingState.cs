using OpenTK.Mathematics;

using ForgeWorks.GlowFork.Graphics;
using ErrorCode = ForgeWorks.GlowFork.ErrorCode;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

/// <summary>
/// Loading state will show the splash screen and begin loading assets
/// and resources.
/// </summary>
public class LoadingState : GameState
{
    private readonly List<Texture> textures = new();

    internal Shader Shader { get; set; }
    internal Texture[] Textures => textures.ToArray();

    public LoadingState() : base()
    {
        Name = nameof(LoadingState);
        Next = string.Empty;

        LoadSplash();

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
        View.Init();

        //  load content pipeline
        LoadContent();

        //  start network - local server peer by default
        StartNetwork();

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
    private static void LoadContent()
    {
        //  TODO: Load assets & resources
        try
        {

            LOGGER.Post(LogLevel.Debug, ASSETS.Info);
            if (!ASSETS.Load())
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
    }
    private static void StartNetwork()
    {
        NETWORK.StartNetwork();
    }
    private void OnViewLoaded()
    {
        //  TODO: install AssetManager
        View.CenterWindow();
    }
}
