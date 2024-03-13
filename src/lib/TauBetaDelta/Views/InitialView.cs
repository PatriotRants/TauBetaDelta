using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

namespace ForgeWorks.TauBetaDelta;

public class InitialView : GameView
{
    public Action ViewRefresh { get; set; }

    public InitialView(GameState gameState) : base(gameState) { }

    public override void OnRefresh()
    {
        ViewRefresh?.Invoke();
    }
    public override void OnRenderFrame(FrameEventArgs args)
    {
        LOGGER.Post(Logging.LogLevel.Default, $"{nameof(InitialView)}.{nameof(OnRenderFrame)}");

        GL.ClearColor(Background);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        SwapBuffers();
    }
    public override void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(Logging.LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");

        //  update the opengl viewport
        SetViewport(Location, ViewPort);
    }
}
