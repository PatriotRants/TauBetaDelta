using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using ForgeWorks.RailThorn.Graphics;
using ForgeWorks.RailThorn.Controls;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

public class LoadingView : GameView
{
    private float[] Vertices { get; set; }
    private uint[] Indices { get; set; }
    private Image Splash { get; set; }
    private Label StatusOne { get; set; }

    public LoadingView(LoadingState gameState) : base(gameState)
    {
        //  add Image control
        Splash = new Image("Splash", "splash");
        // add Label control
        StatusOne = new Label(this, "Status_1")
        {
            Color = Color4.PaleGoldenrod,
            Text = "Hello, TauBetaDelta!!!"
        };
    }

    public override void Init()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(Init)}");

        OnLoad();
    }

    public override void OnLoad()
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnLoad)}");

        GL.ClearColor(Background);

        //  initi controls here
        Splash.Init();
        StatusOne.Init();

        //  raise event
        base.OnLoad();
    }
    public override void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");

        //  update the opengl viewport
        SetViewport(Location, ViewPort);
    }
    public override void OnUpdateFrame(FrameEventArgs args)
    {
        //  update controls
        Splash.Update();
        StatusOne.Update();
    }
    public override void OnRenderFrame(FrameEventArgs args)
    {
        // GL.ClearColor(Background);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        //  redner controls
        Splash.Render();
        StatusOne.Render();

        //  swap buffers
        SwapBuffers();
    }
}
