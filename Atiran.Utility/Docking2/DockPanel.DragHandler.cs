using System.Drawing;
using System.Windows.Forms;
using Atiran.Utility.Docking2.Win32;

namespace Atiran.Utility.Docking2
{
    partial class DockPanel
    {
        /// <summary>
        ///     DragHandlerBase is the base class for drag handlers. The derived class should:
        ///     1. Define its public method BeginDrag. From within this public BeginDrag method,
        ///     DragHandlerBase.BeginDrag should be called to initialize the mouse capture
        ///     and message filtering.
        ///     2. Override the OnDragging and OnEndDrag methods.
        /// </summary>
        public abstract class DragHandlerBase : NativeWindow, IMessageFilter
        {
            protected abstract Control DragControl { get; }

            protected Point StartMousePosition { get; private set; } = Point.Empty;

            bool IMessageFilter.PreFilterMessage(ref Message m)
            {
                if (PatchController.EnableActiveXFix == false)
                {
                    if (m.Msg == (int) Msgs.WM_MOUSEMOVE)
                        OnDragging();
                    else if (m.Msg == (int) Msgs.WM_LBUTTONUP)
                        EndDrag(false);
                    else if (m.Msg == (int) Msgs.WM_CAPTURECHANGED)
                        EndDrag(!Win32Helper.IsRunningOnMono);
                    else if (m.Msg == (int) Msgs.WM_KEYDOWN && (int) m.WParam == (int) Keys.Escape)
                        EndDrag(true);
                }

                return OnPreFilterMessage(ref m);
            }

            protected bool BeginDrag()
            {
                if (DragControl == null)
                    return false;

                StartMousePosition = MousePosition;

                if (!Win32Helper.IsRunningOnMono)
                    if (!NativeMethods.DragDetect(DragControl.Handle, StartMousePosition))
                        return false;

                DragControl.FindForm().Capture = true;
                AssignHandle(DragControl.FindForm().Handle);
                if (PatchController.EnableActiveXFix == false) Application.AddMessageFilter(this);

                return true;
            }

            protected abstract void OnDragging();

            protected abstract void OnEndDrag(bool abort);

            private void EndDrag(bool abort)
            {
                ReleaseHandle();

                if (PatchController.EnableActiveXFix == false) Application.RemoveMessageFilter(this);

                DragControl.FindForm().Capture = false;

                OnEndDrag(abort);
            }

            protected virtual bool OnPreFilterMessage(ref Message m)
            {
                if (PatchController.EnableActiveXFix == true)
                {
                    if (m.Msg == (int) Msgs.WM_MOUSEMOVE)
                        OnDragging();
                    else if (m.Msg == (int) Msgs.WM_LBUTTONUP)
                        EndDrag(false);
                    else if (m.Msg == (int) Msgs.WM_CAPTURECHANGED)
                        EndDrag(!Win32Helper.IsRunningOnMono);
                    else if (m.Msg == (int) Msgs.WM_KEYDOWN && (int) m.WParam == (int) Keys.Escape)
                        EndDrag(true);
                }

                return false;
            }

            protected sealed override void WndProc(ref Message m)
            {
                if (PatchController.EnableActiveXFix == true)
                    //Manually pre-filter message, rather than using
                    //Application.AddMessageFilter(this).  This fixes
                    //the docker control for ActiveX objects
                    OnPreFilterMessage(ref m);

                if (m.Msg == (int) Msgs.WM_CANCELMODE || m.Msg == (int) Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);

                base.WndProc(ref m);
            }
        }

        public abstract class DragHandler : DragHandlerBase
        {
            protected DragHandler(DockPanel dockPanel)
            {
                DockPanel = dockPanel;
            }

            public DockPanel DockPanel { get; }

            protected IDragSource DragSource { get; set; }

            protected sealed override Control DragControl => DragSource == null ? null : DragSource.DragControl;

            protected sealed override bool OnPreFilterMessage(ref Message m)
            {
                if ((m.Msg == (int) Msgs.WM_KEYDOWN || m.Msg == (int) Msgs.WM_KEYUP) &&
                    ((int) m.WParam == (int) Keys.ControlKey || (int) m.WParam == (int) Keys.ShiftKey))
                    OnDragging();

                return base.OnPreFilterMessage(ref m);
            }
        }
    }
}