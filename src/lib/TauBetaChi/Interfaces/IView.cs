using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace ForgeWorks.TauBetaDelta.Presentation;

public interface IView : IDisposable
{
    //
    // Summary:
    //     Occurs before the window is displayed for the first time.
    public event Action Load;
    //
    // Summary:
    //     Occurs before the window is destroyed.
    public event Action Unload;
    //
    // Summary:
    //     Occurs when it is time to update a frame. This is invoked before OpenTK.Windowing.Desktop.GameWindow.RenderFrame.
    public event Action<FrameEventArgs> UpdateFrame;
    //
    // Summary:
    //     Occurs when it is time to render a frame. This is invoked after OpenTK.Windowing.Desktop.GameWindow.UpdateFrequency.
    public event Action<FrameEventArgs> RenderFrame;
    /// <summary>
    /// Occurs whenever the window is moved.
    /// </summary>
    public event Action<WindowPositionEventArgs> Move;
    /// <summary>
    /// Occurs whenever the window is resized.
    /// </summary>
    public event Action<ResizeEventArgs> Resize;
    /// <summary>
    /// Occurs whenever the framebuffer is resized.
    /// </summary>
    public event Action<FramebufferResizeEventArgs> FramebufferResize;
    /// <summary>
    /// Occurs whenever the window is refreshed.
    /// </summary>
    public event Action Refresh;
    /// <summary>
    /// Occurs when the window is about to close.
    /// </summary>
    public event Action<CancelEventArgs> Closing;
    /// <summary>
    /// Occurs when the window is minimized.
    /// </summary>
    public event Action<MinimizedEventArgs> Minimized;
    /// <summary>
    /// Occurs when the window is maximized.
    /// </summary>
    public event Action<MaximizedEventArgs> Maximized;
    /// <summary>
    /// Occurs when a joystick is connected or disconnected.
    /// </summary>
    public event Action<JoystickEventArgs> JoystickConnected;
    /// <summary>
    /// Occurs when the <see cref="NativeWindow.IsFocused" /> property of the window changes.
    /// </summary>
    public event Action<FocusedChangedEventArgs> FocusedChanged;
    /// <summary>
    /// Occurs whenever a keyboard key is pressed.
    /// </summary>
    public event Action<KeyboardKeyEventArgs> KeyDown;
    /// <summary>
    /// Occurs whenever a Unicode code point is typed.
    /// </summary>
    public event Action<TextInputEventArgs> TextInput;
    /// <summary>
    /// Occurs whenever a keyboard key is released.
    /// </summary>
    public event Action<KeyboardKeyEventArgs> KeyUp;
    /// <summary>
    /// Occurs whenever the mouse cursor leaves the window <see cref="NativeWindow.Bounds" />.
    /// </summary>
    // FIXME: This this when we leave the client rectangle or the window bounds?
    public event Action MouseLeave;
    /// <summary>
    /// Occurs whenever the mouse cursor enters the window <see cref="NativeWindow.Bounds" />.
    /// </summary>
    // FIXME: This this when we enter the client rectangle or the window bounds?
    public event Action MouseEnter;
    /// <summary>
    /// Occurs whenever a <see cref="MouseButton" /> is clicked.
    /// </summary>
    public event Action<MouseButtonEventArgs> MouseDown;
    /// <summary>
    /// Occurs whenever a <see cref="MouseButton" /> is released.
    /// </summary>
    public event Action<MouseButtonEventArgs> MouseUp;
    /// <summary>
    /// Occurs whenever the mouse cursor is moved;
    /// </summary>
    public event Action<MouseMoveEventArgs> MouseMove;
    /// <summary>
    /// Occurs whenever a mouse wheel is moved;
    /// </summary>
    public event Action<MouseWheelEventArgs> MouseWheel;
    /// <summary>
    /// Occurs whenever one or more files are dropped on the window.
    /// </summary>
    public event Action<FileDropEventArgs> FileDrop;


    Color4 Background { get; set; }
    Vector2i ClientSize { get; set; }
    Vector2i Location { get; set; }
    string Title { get; }
    Vector2i ViewPort { get; set; }
    WindowBorder WindowBorder { get; set; }
    WindowState WindowState { get; set; }
    bool IsVisible { get; set; }

    void OnRenderFrame(FrameEventArgs args);
    void OnUpdateFrame(FrameEventArgs args);
    void OnClosing(CancelEventArgs args);
    void OnFileDrop(FileDropEventArgs args);
    void OnFocusChanged(FocusedChangedEventArgs args);
    void OnJoystickConnected(JoystickEventArgs args);
    void OnFramebufferResize(FramebufferResizeEventArgs args);
    void OnKeyDown(KeyboardKeyEventArgs args);
    void OnKeyUp(KeyboardKeyEventArgs args);
    void OnLoad();
    void OnMaximized(MaximizedEventArgs args);
    void OnMinimized(MinimizedEventArgs args);
    void OnMouseDown(MouseButtonEventArgs args);
    void OnMouseEnter();
    void OnMouseLeave();
    void OnMouseMove(MouseMoveEventArgs args);
    void OnMouseUp(MouseButtonEventArgs args);
    void OnMouseWheel(MouseWheelEventArgs args);
    void OnMove(WindowPositionEventArgs args);
    void OnRefresh();
    void OnResize(ResizeEventArgs args);
    void OnTextInput(TextInputEventArgs args);
    void OnUnload();
}