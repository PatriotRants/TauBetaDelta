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

    public event Action Load;
    public event Action Unload;
    public event Action<FrameEventArgs> UpdateFrame;
    public event Action<FrameEventArgs> RenderFrame;
    public event Action<WindowPositionEventArgs> Move;
    public event Action<ResizeEventArgs> Resize;
    public event Action<FramebufferResizeEventArgs> FramebufferResize;
    public event Action Refresh;
    public event Action<CancelEventArgs> Closing;
    public event Action<MinimizedEventArgs> Minimized;
    public event Action<MaximizedEventArgs> Maximized;
    public event Action<JoystickEventArgs> JoystickConnected;
    public event Action<FocusedChangedEventArgs> FocusedChanged;
    public event Action<KeyboardKeyEventArgs> KeyDown;
    public event Action<TextInputEventArgs> TextInput;
    public event Action<KeyboardKeyEventArgs> KeyUp;
    public event Action MouseLeave;
    public event Action MouseEnter;
    public event Action<MouseButtonEventArgs> MouseDown;
    public event Action<MouseButtonEventArgs> MouseUp;
    public event Action<MouseMoveEventArgs> MouseMove;
    public event Action<MouseWheelEventArgs> MouseWheel;
    public event Action<FileDropEventArgs> FileDrop;

    private IGLFWGraphicsContext _context => GAME.WindowContext;

    protected GameState State { get; }

    public Color4 Background { get; set; }
    public Vector2i ClientSize { get; set; }
    public Vector2i Location { get; set; } = (0, 0);
    public Vector2i ViewPort { get; set; }
    public WindowBorder WindowBorder { get; set; }
    public WindowState WindowState { get; set; }
    public bool IsVisible { get; set; }

    public Shader Shader { get; set; }
    public string Title => State.Title;
    public string Name => State.Name;

    protected GameView(GameState gameState)
    {
        State = gameState;
    }

    #region Window Overrides
    public abstract void Init();

    public virtual void OnClosingWindow(CancelEventArgs args)
    {
        Closing?.Invoke(args);
    }

    public virtual void OnFileDrop(FileDropEventArgs args)
    {
        FileDrop?.Invoke(args);
    }

    public virtual void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        FramebufferResize?.Invoke(args);
    }

    public virtual void OnKeyDown(KeyboardKeyEventArgs args)
    {
        KeyDown?.Invoke(args);
    }

    public virtual void OnKeyUp(KeyboardKeyEventArgs args)
    {
        KeyUp?.Invoke(args);
    }

    public virtual void OnMouseDown(MouseButtonEventArgs args)
    {
        MouseDown?.Invoke(args);
    }

    public virtual void OnMouseEnterWindow()
    {
        MouseEnter?.Invoke();
    }

    public virtual void OnMouseLeaveWindow()
    {
        MouseLeave?.Invoke();
    }

    public virtual void OnMouseWheel(MouseWheelEventArgs args)
    {
        MouseWheel?.Invoke(args);
    }

    public virtual void OnRefresh()
    {
        Refresh?.Invoke();
    }

    public virtual void OnTextInput(TextInputEventArgs args)
    {
        TextInput?.Invoke(args);
    }

    public virtual void OnMaximized(MaximizedEventArgs args)
    {
        Maximized?.Invoke(args);
    }

    public virtual void OnMinimized(MinimizedEventArgs args)
    {
        Minimized?.Invoke(args);
    }

    public virtual void OnMove(WindowPositionEventArgs args)
    {
        Move?.Invoke(args);
    }

    public virtual void OnRenderFrame(FrameEventArgs args)
    {
        RenderFrame?.Invoke(args);
    }

    public virtual void OnUpdateFrame(FrameEventArgs args)
    {
        UpdateFrame?.Invoke(args);
    }

    public virtual void OnClosing(CancelEventArgs args)
    {
        Closing?.Invoke(args);
    }

    public virtual void OnFocusChanged(FocusedChangedEventArgs args)
    {
        FocusedChanged?.Invoke(args);
    }

    public virtual void OnJoystickConnected(JoystickEventArgs args)
    {
        JoystickConnected?.Invoke(args);
    }

    public virtual void OnLoad()
    {
        Load?.Invoke();
    }

    public virtual void OnMouseEnter()
    {
        MouseEnter?.Invoke();
    }

    public virtual void OnMouseLeave()
    {
        MouseLeave?.Invoke();
    }

    public virtual void OnMouseMove(MouseMoveEventArgs args)
    {
        MouseMove?.Invoke(args);
    }

    public virtual void OnMouseUp(MouseButtonEventArgs args)
    {
        MouseUp?.Invoke(args);
    }

    public virtual void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");
        Resize?.Invoke(args);
    }

    public virtual void OnUnload()
    {
        Unload?.Invoke();
    }

    public virtual void Dispose()
    {

    }
    #endregion

    internal void CenterWindow()
    {
        GAME.CenterWindow();
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
