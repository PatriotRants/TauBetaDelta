using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using ErrorCode = ForgeWorks.GlowFork.ErrorCode;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

/// <summary>
/// Loading state will show the splash screen and begin loading assets
/// and resources.
/// </summary>
public class LoadingState : GameState
{
    public LoadingState() : base()
    {
        Name = nameof(LoadingState);
        Next = string.Empty;

        View = new SplashScreenView(this)
        {
            //  change the background color
            Background = new Color4(27, 27, 27, 128),
            ViewPort = (1200, 900),
            ClientSize = GAME.GetState().View.ClientSize,
        };

        LOGGER.Post(LogLevel.Default, $"{Name}.View [{((SplashScreenView)View).ClientSize};{((SplashScreenView)View).Location};{((SplashScreenView)View).ViewPort}]");
    }

    public override void Load()
    {
        //  TODO: Load assets & resources
        try
        {
            ShowSplash();

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

        //  start server

        //  start client

        //  when loading is complete we can call the next state from here.
    }

    public override void Unload()
    {
        // delete shader programs
        SHADERS.DeleteProgram("splash");
        // View.ShaderProgram = -1;
    }

    private void ShowSplash()
    {
        //  load shaders
        var frag = SHADERS.Load(ShaderType.FragmentShader, "splash.fragment");
        var vert = SHADERS.Load(ShaderType.VertexShader, "splash.vertex");
        SHADERS.CreateProgram("splash", frag, vert);
        View.OnLoad();
    }
}
