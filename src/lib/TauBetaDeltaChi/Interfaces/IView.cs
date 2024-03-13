using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace ForgeWorks.TauBetaDelta.Presentation;

public interface IView : IDisposable
{
    Color4 Background { get; set; }
    Vector2i ClientSize { get; set; }
    Vector2i Location { get; set; }
    string Title { get; }
    Vector2i ViewPort { get; set; }
    WindowBorder WindowBorder { get; set; }
    WindowState WindowState { get; set; }

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