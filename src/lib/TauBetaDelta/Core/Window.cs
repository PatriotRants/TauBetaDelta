using System.ComponentModel;
using ForgeWorks.TauBetaDelta.Presentation;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ForgeWorks.TauBetaDelta;

internal sealed partial class Game
{
    private Window _window;

    private class Window : GameWindow
    {
        private IView View { get; set; }

        public Window(IView view) : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            ClientSize = view.ClientSize,
            WindowState = view.WindowState,
            WindowBorder = view.WindowBorder,
            StartVisible = view.IsVisible,
            Title = view.Title
        })
        {
            SetView(view);
        }


        #region View overrides
        protected override void OnClosing(CancelEventArgs args)
        {
            View.OnClosing(args);
        }
        protected override void OnFileDrop(FileDropEventArgs args)
        {
            View.OnFileDrop(args);
        }
        protected override void OnFocusedChanged(FocusedChangedEventArgs args)
        {
            View.OnFocusChanged(args);
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs args)
        {
            View.OnFramebufferResize(args);
        }
        protected override void OnJoystickConnected(JoystickEventArgs args)
        {
            View.OnJoystickConnected(args);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs args)
        {
            View.OnKeyDown(args);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs args)
        {
            View.OnKeyUp(args);
        }
        protected override void OnLoad()
        {
            View.OnLoad();
        }
        protected override void OnMaximized(MaximizedEventArgs args)
        {
            View.OnMaximized(args);
        }
        protected override void OnMinimized(MinimizedEventArgs args)
        {
            View.OnMinimized(args);
        }
        protected override void OnMouseDown(MouseButtonEventArgs args)
        {
            View.OnMouseDown(args);
        }
        protected override void OnMouseEnter()
        {
            View.OnMouseEnter();
        }
        protected override void OnMouseLeave()
        {
            View.OnMouseLeave();
        }
        protected override void OnMouseMove(MouseMoveEventArgs args)
        {
            View.OnMouseMove(args);
        }
        protected override void OnMouseUp(MouseButtonEventArgs args)
        {
            View.OnMouseUp(args);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs args)
        {
            View.OnMouseWheel(args);
        }
        protected override void OnMove(WindowPositionEventArgs args)
        {
            View.OnMove(args);
        }
        protected override void OnRefresh()
        {
            View.OnRefresh();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            View.OnRenderFrame(args);
        }
        protected override void OnResize(ResizeEventArgs args)
        {
            View.OnResize(args);
        }
        protected override void OnTextInput(TextInputEventArgs args)
        {
            View.OnTextInput(args);
        }
        protected override void OnUnload()
        {
            View.OnUnload();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            View.OnUpdateFrame(args);
        }
        #endregion

        internal void SetView(IView view)
        {
            View = view;
        }
        internal void ChangeView(IView view)
        {
            SetView(view);

            ClientSize = view.ClientSize;
            WindowBorder = view.WindowBorder;
            WindowState = view.WindowState;
            IsVisible = view.IsVisible;
        }
    }
}
