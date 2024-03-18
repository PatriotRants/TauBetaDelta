using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using ForgeWorks.TauBetaDelta.Logging;

namespace ForgeWorks.TauBetaDelta;

public class StartupView : GameView
{
    public StartupView(StartupState gameState) : base(gameState) { }

    public override void OnLoad()
    {
        base.OnLoad();
    }

    public override void Init() { }
    public override void OnRenderFrame(FrameEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{nameof(StartupView)}.{nameof(OnRenderFrame)}");

        GL.ClearColor(Background);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        SwapBuffers();
    }
    public override void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");

        //  update the opengl viewport
        SetViewport(Location, ViewPort);
    }
}
