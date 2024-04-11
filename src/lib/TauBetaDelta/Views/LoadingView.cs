using System.Collections.ObjectModel;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using ForgeWorks.RailThorn.Controls;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

public class LoadingView : GameView
{
    private readonly Collection<RendererControl> controls = new();
    private readonly Text text;

    public LoadingView(LoadingState gameState) : base(gameState)
    {
        // controls.Add(new Image("Splash", "sample_splash"));
        // controls.Add(new Label(this, "Status_1")
        // {
        //     Color = Color4.PaleGoldenrod,
        //     Text = "AaBb",
        //     Location = (25, 50)
        // });
        text = new(this)
        {
            Content = "Project Omega.Chi"
        };
    }

    public override void Init()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(Init)}");

        foreach (var c in controls)
        {
            //  init controls
            c.Init();
        }
        text.Init();

        OnLoad();
    }
    public override void OnLoad()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnLoad)}");
        GL.ClearColor(Background);

        //  raise event
        base.OnLoad();
    }
    public override void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Debug, $"{Name}View.{nameof(OnFramebufferResize)} [{Location};{ViewPort}]");
        SetViewport(Location, ViewPort);
    }
    public override void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Debug, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");
    }
    public override void OnUpdateFrame(FrameEventArgs args)
    {
        foreach (var c in controls)
        {
            //  update controls
            c.Update();
        }
        text.Update();
    }
    public override void OnRenderFrame(FrameEventArgs args)
    {
        // GL.ClearColor(Background);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var c in controls)
        {
            //  redner controls
            c.Render();
        }
        text.Render();

        //  swap buffers
        SwapBuffers();
    }
}
