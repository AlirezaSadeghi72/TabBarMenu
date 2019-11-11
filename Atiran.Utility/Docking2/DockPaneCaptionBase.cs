using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using Atiran.Utility.Docking2.Win32;

namespace Atiran.Utility.Docking2
{
    public abstract class DockPaneCaptionBase : Control
    {
        protected internal DockPaneCaptionBase(DockPane pane)
        {
            DockPane = pane;

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, false);
        }

        public DockPane DockPane { get; }

        protected DockPane.AppearanceStyle Appearance => DockPane.Appearance;

        protected bool HasTabPageContextMenu => DockPane.HasTabPageContextMenu;

        /// <summary>
        ///     Gets a value indicating whether dock panel can be dragged when in auto hide mode.
        ///     Default is false.
        /// </summary>
        protected virtual bool CanDragAutoHide => false;

        protected void ShowTabPageContextMenu(Point position)
        {
            DockPane.ShowTabPageContextMenu(this, position);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
                ShowTabPageContextMenu(new Point(e.X, e.Y));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left &&
                DockPane.DockPanel.AllowEndUserDocking &&
                DockPane.AllowDockDragAndDrop &&
                DockPane.ActiveContent != null &&
                (!DockHelper.IsDockStateAutoHide(DockPane.DockState) || CanDragAutoHide))
                DockPane.DockPanel.BeginDrag(DockPane);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int) Msgs.WM_LBUTTONDBLCLK)
            {
                if (DockHelper.IsDockStateAutoHide(DockPane.DockState))
                {
                    DockPane.DockPanel.ActiveAutoHideContent = null;
                    return;
                }

                if (DockPane.IsFloat)
                    DockPane.RestoreToPanel();
                else
                    DockPane.Float();
            }

            base.WndProc(ref m);
        }

        internal void RefreshChanges()
        {
            if (IsDisposed)
                return;

            OnRefreshChanges();
        }

        protected virtual void OnRightToLeftLayoutChanged()
        {
        }

        protected virtual void OnRefreshChanges()
        {
        }

        protected internal abstract int MeasureHeight();
    }
}