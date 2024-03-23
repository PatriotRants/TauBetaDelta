using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.RailThorn.Controls;
using ForgeWorks.RailThorn.Graphics;

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

    protected List<Control> Container { get; } = new();

    private IGLFWGraphicsContext _context => GAME.WindowContext;

    #region Window Events
    /// <summary>
    /// <inheritdoc cref="GameWindow.Load"/>
    /// </summary>
    public event Action Load;
    /// <summary>
    /// <inheritdoc cref="GameWindow.Unload"/>
    /// </summary>
    public event Action Unload;
    /// <summary>
    /// <inheritdoc cref="GameWindow.UpdateFrame"/>
    /// </summary>
    public event Action<FrameEventArgs> UpdateFrame;
    /// <summary>
    /// <inheritdoc cref="GameWindow.RenderFrame"/>
    /// </summary>
    public event Action<FrameEventArgs> RenderFrame;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Move"/>
    /// </summary>
    public event Action<WindowPositionEventArgs> Move;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Resize"/>
    /// </summary>
    public event Action<ResizeEventArgs> Resize;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.FramebufferResize"/>
    /// </summary>
    public event Action<FramebufferResizeEventArgs> FramebufferResize;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Refresh"/>
    /// </summary>
    public event Action Refresh;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Closing"/>
    /// </summary>
    public event Action<CancelEventArgs> Closing;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Minimized"/>
    /// </summary>
    public event Action<MinimizedEventArgs> Minimized;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Maximized"/>
    /// </summary>
    public event Action<MaximizedEventArgs> Maximized;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.JoystickConnected"/>
    /// </summary>
    public event Action<JoystickEventArgs> JoystickConnected;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.FocusedChanged"/>
    /// </summary>
    public event Action<FocusedChangedEventArgs> FocusedChanged;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.KeyDown"/>
    /// </summary>
    public event Action<KeyboardKeyEventArgs> KeyDown;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.TextInput"/>
    /// </summary>
    public event Action<TextInputEventArgs> TextInput;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.KeyUp"/>
    /// </summary>
    public event Action<KeyboardKeyEventArgs> KeyUp;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.MouseLeave"/>
    /// </summary>
    public event Action MouseLeave;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.MouseEnter"/>
    /// </summary>
    public event Action MouseEnter;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.MouseDown"/>
    /// </summary>
    public event Action<MouseButtonEventArgs> MouseDown;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.MouseUp"/>
    /// </summary>
    public event Action<MouseButtonEventArgs> MouseUp;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.MouseMove"/>
    /// </summary>
    public event Action<MouseMoveEventArgs> MouseMove;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.MouseWheel"/>
    /// </summary>
    public event Action<MouseWheelEventArgs> MouseWheel;
    /// <summary>
    /// <inheritdoc cref="NativeWindow.FileDrop"/>
    /// </summary>
    public event Action<FileDropEventArgs> FileDrop;
    #endregion

    public KeyboardState KeyboardState => GAME.GetKeyboardState();
    public MouseState MouseState => GAME.GetMouseState();

    #region Window Properties
    protected GameState State { get; }

    /// <summary>
    /// Get or set the window background color
    /// </summary>
    public Color4 Background { get; set; }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.ClientSize"/>
    /// </summary>
    public Vector2i ClientSize { get; set; }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.Location"/>
    /// </summary>
    public Vector2i Location { get; set; } = (0, 0);
    /// <summary>
    /// Get or set the current View ViewPort
    /// </summary>
    public Vector2i ViewPort { get; set; }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.WindowBorder"/>
    /// </summary>
    public WindowBorder WindowBorder { get; set; }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.WindowState"/>
    /// </summary>
    public WindowState WindowState { get; set; }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.IsVisible"/>
    /// </summary>
    public bool IsVisible { get; set; }
    /// <summary>
    /// Get or set the View's Shader
    /// </summary>
    public Shader Shader { get; set; }
    /// <summary>
    /// Get the current View's Title
    /// </summary>
    public string Title => State.Title;
    /// <summary>
    /// Get the current View's Name
    /// </summary>
    public string Name => State.Name;
    #endregion

    protected GameView(GameState gameState)
    {
        State = gameState;
    }

    #region Window Overrides
    /// <summary>
    /// Initialize the current View's resources
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnClosing(CancelEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnClosing(CancelEventArgs args)
    {
        Closing?.Invoke(args);
        GAME.Dispose();
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnFileDrop(FileDropEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnFileDrop(FileDropEventArgs args)
    {
        FileDrop?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnFileDrop(FileDropEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        FramebufferResize?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnKeyDown(KeyboardKeyEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnKeyDown(KeyboardKeyEventArgs args)
    {
        KeyDown?.Invoke(args);
        if (args.Key == Keys.Escape)
        {
            //  close window
            GAME.Stop();
        }
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnKeyUp(KeyboardKeyEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnKeyUp(KeyboardKeyEventArgs args)
    {
        KeyUp?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMouseDown(MouseButtonEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMouseDown(MouseButtonEventArgs args)
    {
        MouseDown?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMouseEnter"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMouseEnter()
    {
        MouseEnter?.Invoke();
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMouseLeave"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMouseLeave()
    {
        MouseLeave?.Invoke();
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMouseWheel(MouseWheelEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMouseWheel(MouseWheelEventArgs args)
    {
        MouseWheel?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnRefresh"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnRefresh()
    {
        Refresh?.Invoke();
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnTextInput(TextInputEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnTextInput(TextInputEventArgs args)
    {
        TextInput?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMaximized(MaximizedEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMaximized(MaximizedEventArgs args)
    {
        Maximized?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMinimized(MinimizedEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMinimized(MinimizedEventArgs args)
    {
        Minimized?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMove(WindowPositionEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMove(WindowPositionEventArgs args)
    {
        Move?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="GameWindow.OnRenderFrame(FrameEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnRenderFrame(FrameEventArgs args)
    {
        RenderFrame?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="GameWindow.OnUpdateFrame(FrameEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnUpdateFrame(FrameEventArgs args)
    {
        UpdateFrame?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnFocusedChanged(FocusedChangedEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnFocusChanged(FocusedChangedEventArgs args)
    {
        FocusedChanged?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnJoystickConnected(JoystickEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnJoystickConnected(JoystickEventArgs args)
    {
        JoystickConnected?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="GameWindow.OnLoad"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnLoad()
    {
        Load?.Invoke();
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMouseMove(MouseMoveEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMouseMove(MouseMoveEventArgs args)
    {
        MouseMove?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnMouseUp(MouseButtonEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnMouseUp(MouseButtonEventArgs args)
    {
        MouseUp?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="NativeWindow.OnResize(ResizeEventArgs)"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnResize(ResizeEventArgs args)
    {
        LOGGER.Post(LogLevel.Default, $"{Name}View.{nameof(OnResize)} [{Location};{ViewPort}]");
        Resize?.Invoke(args);
    }
    /// <summary>
    /// <inheritdoc cref="GameWindow.OnUnload"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnUnload()
    {
        Unload?.Invoke();
    }
    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="args"></param>
    public virtual void Dispose()
    {

    }
    #endregion

    /// <summary>
    /// <inheritdoc cref="NativeWindow.CenterWindow()"/>
    /// </summary>
    /// <param name="args"></param>
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
