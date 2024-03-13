using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using ForgeWorks.GlowFork.Graphics;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Collections;
using ForgeWorks.TauBetaDelta.Presentation;

namespace ForgeWorks.TauBetaDelta;

public abstract class GameView : IView
{
    private static readonly Resources RESOURCES = Registry.Get<Resources>();
    private static readonly Game GAME = Registry.Get<Game>();

    protected static readonly LoggerManager LOGGER = RESOURCES.LoggerManager;
    protected static readonly ShaderManager SHADERS = RESOURCES.ShaderManager;
    protected static readonly AssetManager ASSETS = RESOURCES.AssetManager;

    private IGLFWGraphicsContext _context => GAME.WindowContext;

    protected GameState State { get; }

    public Color4 Background { get; set; }
    public Vector2i ClientSize { get; set; }
    public Vector2i Location { get; set; } = (0, 0);
    public Vector2i ViewPort { get; set; }
    public WindowBorder WindowBorder { get; set; }
    public WindowState WindowState { get; set; }

    public int ShaderProgram { get; set; }
    public string Title => State.Title;
    public string Name => State.Name;

    protected GameView(GameState gameState)
    {
        State = gameState;
    }

    public virtual void OnClosingWindow(CancelEventArgs args)
    {

    }

    public virtual void OnFileDrop(FileDropEventArgs args)
    {

    }

    public virtual void OnFramebufferResize(FramebufferResizeEventArgs args)
    {

    }

    public virtual void OnKeyDown(KeyboardKeyEventArgs args)
    {

    }

    public virtual void OnKeyUp(KeyboardKeyEventArgs args)
    {

    }

    public virtual void OnMouseDown(MouseButtonEventArgs args)
    {

    }

    public virtual void OnMouseEnterWindow()
    {

    }

    public virtual void OnMouseLeaveWindow()
    {

    }

    public virtual void OnMouseWheel(MouseWheelEventArgs args)
    {

    }

    public virtual void OnRefresh()
    {

    }

    public virtual void OnTextInput(TextInputEventArgs args)
    {

    }

    public virtual void OnMaximized(MaximizedEventArgs args)
    {

    }

    public virtual void OnMinimized(MinimizedEventArgs args)
    {

    }

    public virtual void OnMove(WindowPositionEventArgs args)
    {

    }

    public virtual void OnRenderFrame(FrameEventArgs args)
    {

    }

    public virtual void OnUpdateFrame(FrameEventArgs args)
    {

    }

    public virtual void OnClosing(CancelEventArgs args)
    {

    }

    public virtual void OnFocusChanged(FocusedChangedEventArgs args)
    {

    }

    public virtual void OnJoystickConnected(JoystickEventArgs args)
    {

    }

    public virtual void OnLoad()
    {

    }

    public virtual void OnMouseEnter()
    {

    }

    public virtual void OnMouseLeave()
    {

    }

    public virtual void OnMouseMove(MouseMoveEventArgs args)
    {

    }

    public virtual void OnMouseUp(MouseButtonEventArgs args)
    {

    }

    public virtual void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");
    }

    public virtual void OnUnload()
    {

    }

    public virtual void Dispose()
    {

    }

    protected void SetViewport(Vector2i location, Vector2i viewPort)
    {
        GL.Viewport(location.X, location.Y, viewPort.X, viewPort.Y);
    }
    protected void SwapBuffers()
    {
        _context.SwapBuffers();
    }

}
