using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
{
    partial class DockPanel
    {
        private AutoHideWindowControl AutoHideWindow { get; set; }

        internal Control AutoHideControl => AutoHideWindow;

        internal Rectangle AutoHideWindowRectangle
        {
            get
            {
                var state = AutoHideWindow.DockState;
                var rectDockArea = DockArea;
                if (ActiveAutoHideContent == null)
                    return Rectangle.Empty;

                if (Parent == null)
                    return Rectangle.Empty;

                var rect = Rectangle.Empty;
                var autoHideSize = ActiveAutoHideContent.DockHandler.AutoHidePortion;
                if (state == DockState.DockLeftAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Width * autoHideSize;
                    if (autoHideSize > rectDockArea.Width - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Width - MeasurePane.MinSize;
                    rect.X = rectDockArea.X - Theme.Measures.DockPadding;
                    rect.Y = rectDockArea.Y;
                    rect.Width = (int) autoHideSize;
                    rect.Height = rectDockArea.Height;
                }
                else if (state == DockState.DockRightAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Width * autoHideSize;
                    if (autoHideSize > rectDockArea.Width - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Width - MeasurePane.MinSize;
                    rect.X = rectDockArea.X + rectDockArea.Width - (int) autoHideSize + Theme.Measures.DockPadding;
                    rect.Y = rectDockArea.Y;
                    rect.Width = (int) autoHideSize;
                    rect.Height = rectDockArea.Height;
                }
                else if (state == DockState.DockTopAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Height * autoHideSize;
                    if (autoHideSize > rectDockArea.Height - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Height - MeasurePane.MinSize;
                    rect.X = rectDockArea.X;
                    rect.Y = rectDockArea.Y - Theme.Measures.DockPadding;
                    rect.Width = rectDockArea.Width;
                    rect.Height = (int) autoHideSize;
                }
                else if (state == DockState.DockBottomAutoHide)
                {
                    if (autoHideSize < 1)
                        autoHideSize = rectDockArea.Height * autoHideSize;
                    if (autoHideSize > rectDockArea.Height - MeasurePane.MinSize)
                        autoHideSize = rectDockArea.Height - MeasurePane.MinSize;
                    rect.X = rectDockArea.X;
                    rect.Y = rectDockArea.Y + rectDockArea.Height - (int) autoHideSize + Theme.Measures.DockPadding;
                    rect.Width = rectDockArea.Width;
                    rect.Height = (int) autoHideSize;
                }

                return rect;
            }
        }

        internal void RefreshActiveAutoHideContent()
        {
            AutoHideWindow.RefreshActiveContent();
        }

        internal Rectangle GetAutoHideWindowBounds(Rectangle rectAutoHideWindow)
        {
            if (DocumentStyle == DocumentStyle.SystemMdi ||
                DocumentStyle == DocumentStyle.DockingMdi)
                return Parent == null
                    ? Rectangle.Empty
                    : Parent.RectangleToClient(RectangleToScreen(rectAutoHideWindow));
            return rectAutoHideWindow;
        }

        internal void RefreshAutoHideStrip()
        {
            AutoHideStripControl.RefreshChanges();
        }

        [ToolboxItem(false)]
        public class AutoHideWindowControl : Panel, ISplitterHost
        {
            #region consts

            private const int ANIMATE_TIME = 100; // in mini-seconds

            #endregion

            private static readonly object AutoHideActiveContentChangedEvent = new object();

            private IDockContent m_activeContent;

            private bool m_flagDragging;

            private Timer m_timerMouseTrack;

            public AutoHideWindowControl(DockPanel dockPanel)
            {
                DockPanel = dockPanel;

                m_timerMouseTrack = new Timer();
                m_timerMouseTrack.Tick += TimerMouseTrack_Tick;

                Visible = false;
                m_splitter = DockPanel.Theme.Extender.WindowSplitterControlFactory.CreateSplitterControl(this);
                Controls.Add(m_splitter);
            }

            protected SplitterBase m_splitter { get; }

            public DockPane ActivePane { get; private set; }

            public IDockContent ActiveContent
            {
                get => m_activeContent;
                set
                {
                    if (value == m_activeContent)
                        return;

                    if (value != null)
                        if (!DockHelper.IsDockStateAutoHide(value.DockHandler.DockState) ||
                            value.DockHandler.DockPanel != DockPanel)
                            throw new InvalidOperationException(Strings.DockPanel_ActiveAutoHideContent_InvalidValue);

                    DockPanel.SuspendLayout();

                    if (m_activeContent != null)
                    {
                        if (m_activeContent.DockHandler.Form.ContainsFocus)
                            if (!Win32Helper.IsRunningOnMono)
                                DockPanel.ContentFocusManager.GiveUpFocus(m_activeContent);

                        AnimateWindow(false);
                    }

                    m_activeContent = value;
                    SetActivePane();
                    if (ActivePane != null)
                        ActivePane.ActiveContent = m_activeContent;

                    if (m_activeContent != null)
                        AnimateWindow(true);

                    DockPanel.ResumeLayout();
                    DockPanel.RefreshAutoHideStrip();

                    SetTimerMouseTrack();

                    OnActiveContentChanged(EventArgs.Empty);
                }
            }

            private bool FlagAnimate { get; set; } = true;

            internal bool FlagDragging
            {
                get => m_flagDragging;
                set
                {
                    if (m_flagDragging == value)
                        return;

                    m_flagDragging = value;
                    SetTimerMouseTrack();
                }
            }

            protected virtual Rectangle DisplayingRectangle
            {
                get
                {
                    var rect = ClientRectangle;

                    // exclude the border and the splitter
                    if (DockState == DockState.DockBottomAutoHide)
                    {
                        rect.Y += 2 + DockPanel.Theme.Measures.AutoHideSplitterSize;
                        rect.Height -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize;
                    }
                    else if (DockState == DockState.DockRightAutoHide)
                    {
                        rect.X += 2 + DockPanel.Theme.Measures.AutoHideSplitterSize;
                        rect.Width -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize;
                    }
                    else if (DockState == DockState.DockTopAutoHide)
                    {
                        rect.Height -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize;
                    }
                    else if (DockState == DockState.DockLeftAutoHide)
                    {
                        rect.Width -= 2 + DockPanel.Theme.Measures.AutoHideSplitterSize;
                    }

                    return rect;
                }
            }

            public bool IsDockWindow => false;

            public DockPanel DockPanel { get; }

            public DockState DockState =>
                ActiveContent == null ? DockState.Unknown : ActiveContent.DockHandler.DockState;

            protected override void Dispose(bool disposing)
            {
                if (disposing) m_timerMouseTrack.Dispose();
                base.Dispose(disposing);
            }

            private void SetActivePane()
            {
                var value = ActiveContent == null ? null : ActiveContent.DockHandler.Pane;

                if (value == ActivePane)
                    return;

                ActivePane = value;
            }

            public event EventHandler ActiveContentChanged
            {
                add => Events.AddHandler(AutoHideActiveContentChangedEvent, value);
                remove => Events.RemoveHandler(AutoHideActiveContentChangedEvent, value);
            }

            protected virtual void OnActiveContentChanged(EventArgs e)
            {
                var handler = (EventHandler) Events[ActiveContentChangedEvent];
                if (handler != null)
                    handler(this, e);
            }

            private void AnimateWindow(bool show)
            {
                if (!FlagAnimate && Visible != show)
                {
                    Visible = show;
                    return;
                }

                Parent.SuspendLayout();

                var rectSource = GetRectangle(!show);
                var rectTarget = GetRectangle(show);
                int dxLoc, dyLoc;
                int dWidth, dHeight;
                dxLoc = dyLoc = dWidth = dHeight = 0;
                if (DockState == DockState.DockTopAutoHide)
                {
                    dHeight = show ? 1 : -1;
                }
                else if (DockState == DockState.DockLeftAutoHide)
                {
                    dWidth = show ? 1 : -1;
                }
                else if (DockState == DockState.DockRightAutoHide)
                {
                    dxLoc = show ? -1 : 1;
                    dWidth = show ? 1 : -1;
                }
                else if (DockState == DockState.DockBottomAutoHide)
                {
                    dyLoc = show ? -1 : 1;
                    dHeight = show ? 1 : -1;
                }

                if (show)
                {
                    Bounds = DockPanel.GetAutoHideWindowBounds(new Rectangle(-rectTarget.Width, -rectTarget.Height,
                        rectTarget.Width, rectTarget.Height));
                    if (Visible == false)
                        Visible = true;
                    PerformLayout();
                }

                SuspendLayout();

                LayoutAnimateWindow(rectSource);
                if (Visible == false)
                    Visible = true;

                var speedFactor = 1;
                var totalPixels = rectSource.Width != rectTarget.Width
                    ? Math.Abs(rectSource.Width - rectTarget.Width)
                    : Math.Abs(rectSource.Height - rectTarget.Height);
                var remainPixels = totalPixels;
                var startingTime = DateTime.Now;
                while (rectSource != rectTarget)
                {
                    var startPerMove = DateTime.Now;

                    rectSource.X += dxLoc * speedFactor;
                    rectSource.Y += dyLoc * speedFactor;
                    rectSource.Width += dWidth * speedFactor;
                    rectSource.Height += dHeight * speedFactor;
                    if (Math.Sign(rectTarget.X - rectSource.X) != Math.Sign(dxLoc))
                        rectSource.X = rectTarget.X;
                    if (Math.Sign(rectTarget.Y - rectSource.Y) != Math.Sign(dyLoc))
                        rectSource.Y = rectTarget.Y;
                    if (Math.Sign(rectTarget.Width - rectSource.Width) != Math.Sign(dWidth))
                        rectSource.Width = rectTarget.Width;
                    if (Math.Sign(rectTarget.Height - rectSource.Height) != Math.Sign(dHeight))
                        rectSource.Height = rectTarget.Height;

                    LayoutAnimateWindow(rectSource);
                    if (Parent != null)
                        Parent.Update();

                    remainPixels -= speedFactor;

                    while (true)
                    {
                        var time = new TimeSpan(0, 0, 0, 0, ANIMATE_TIME);
                        var elapsedPerMove = DateTime.Now - startPerMove;
                        var elapsedTime = DateTime.Now - startingTime;
                        if ((int) (time - elapsedTime).TotalMilliseconds <= 0)
                        {
                            speedFactor = remainPixels;
                            break;
                        }

                        speedFactor = remainPixels * (int) elapsedPerMove.TotalMilliseconds /
                                      (int) (time - elapsedTime).TotalMilliseconds;

                        if (speedFactor >= 1)
                            break;
                    }
                }

                ResumeLayout();
                Parent.ResumeLayout();
            }

            private void LayoutAnimateWindow(Rectangle rect)
            {
                Bounds = DockPanel.GetAutoHideWindowBounds(rect);

                var rectClient = ClientRectangle;

                if (DockState == DockState.DockLeftAutoHide)
                    ActivePane.Location =
                        new Point(
                            rectClient.Right - 2 - DockPanel.Theme.Measures.AutoHideSplitterSize - ActivePane.Width,
                            ActivePane.Location.Y);
                else if (DockState == DockState.DockTopAutoHide)
                    ActivePane.Location = new Point(ActivePane.Location.X,
                        rectClient.Bottom - 2 - DockPanel.Theme.Measures.AutoHideSplitterSize - ActivePane.Height);
            }

            private Rectangle GetRectangle(bool show)
            {
                if (DockState == DockState.Unknown)
                    return Rectangle.Empty;

                var rect = DockPanel.AutoHideWindowRectangle;

                if (show)
                    return rect;

                if (DockState == DockState.DockLeftAutoHide)
                {
                    rect.Width = 0;
                }
                else if (DockState == DockState.DockRightAutoHide)
                {
                    rect.X += rect.Width;
                    rect.Width = 0;
                }
                else if (DockState == DockState.DockTopAutoHide)
                {
                    rect.Height = 0;
                }
                else
                {
                    rect.Y += rect.Height;
                    rect.Height = 0;
                }

                return rect;
            }

            private void SetTimerMouseTrack()
            {
                if (ActivePane == null || ActivePane.IsActivated || FlagDragging)
                {
                    m_timerMouseTrack.Enabled = false;
                    return;
                }

                // start the timer
                var hovertime = SystemInformation.MouseHoverTime;

                // assign a default value 400 in case of setting Timer.Interval invalid value exception
                if (hovertime <= 0)
                    hovertime = 400;

                m_timerMouseTrack.Interval = 2 * hovertime;
                m_timerMouseTrack.Enabled = true;
            }

            public void RefreshActiveContent()
            {
                if (ActiveContent == null)
                    return;

                if (!DockHelper.IsDockStateAutoHide(ActiveContent.DockHandler.DockState))
                {
                    FlagAnimate = false;
                    ActiveContent = null;
                    FlagAnimate = true;
                }
            }

            public void RefreshActivePane()
            {
                SetTimerMouseTrack();
            }

            private void TimerMouseTrack_Tick(object sender, EventArgs e)
            {
                if (IsDisposed)
                    return;

                if (ActivePane == null || ActivePane.IsActivated)
                {
                    m_timerMouseTrack.Enabled = false;
                    return;
                }

                var pane = ActivePane;
                var ptMouseInAutoHideWindow = PointToClient(MousePosition);
                var ptMouseInDockPanel = DockPanel.PointToClient(MousePosition);

                var rectTabStrip = DockPanel.GetTabStripRectangle(pane.DockState);

                if (!ClientRectangle.Contains(ptMouseInAutoHideWindow) && !rectTabStrip.Contains(ptMouseInDockPanel))
                {
                    ActiveContent = null;
                    m_timerMouseTrack.Enabled = false;
                }
            }

            protected class SplitterControl : SplitterBase
            {
                public SplitterControl(AutoHideWindowControl autoHideWindow)
                {
                    AutoHideWindow = autoHideWindow;
                }

                private AutoHideWindowControl AutoHideWindow { get; }

                protected override int SplitterSize => AutoHideWindow.DockPanel.Theme.Measures.AutoHideSplitterSize;

                protected override void StartDrag()
                {
                    AutoHideWindow.DockPanel.BeginDrag(AutoHideWindow, AutoHideWindow.RectangleToScreen(Bounds));
                }
            }

            #region ISplitterDragSource Members

            void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
            {
                FlagDragging = true;
            }

            void ISplitterDragSource.EndDrag()
            {
                FlagDragging = false;
            }

            bool ISplitterDragSource.IsVertical =>
                DockState == DockState.DockLeftAutoHide || DockState == DockState.DockRightAutoHide;

            Rectangle ISplitterDragSource.DragLimitBounds
            {
                get
                {
                    var rectLimit = DockPanel.DockArea;

                    if ((this as ISplitterDragSource).IsVertical)
                    {
                        rectLimit.X += MeasurePane.MinSize;
                        rectLimit.Width -= 2 * MeasurePane.MinSize;
                    }
                    else
                    {
                        rectLimit.Y += MeasurePane.MinSize;
                        rectLimit.Height -= 2 * MeasurePane.MinSize;
                    }

                    return DockPanel.RectangleToScreen(rectLimit);
                }
            }

            void ISplitterDragSource.MoveSplitter(int offset)
            {
                var rectDockArea = DockPanel.DockArea;
                var content = ActiveContent;
                if (DockState == DockState.DockLeftAutoHide && rectDockArea.Width > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion += offset / (double) rectDockArea.Width;
                    else
                        content.DockHandler.AutoHidePortion = Width + offset;
                }
                else if (DockState == DockState.DockRightAutoHide && rectDockArea.Width > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion -= offset / (double) rectDockArea.Width;
                    else
                        content.DockHandler.AutoHidePortion = Width - offset;
                }
                else if (DockState == DockState.DockBottomAutoHide && rectDockArea.Height > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion -= offset / (double) rectDockArea.Height;
                    else
                        content.DockHandler.AutoHidePortion = Height - offset;
                }
                else if (DockState == DockState.DockTopAutoHide && rectDockArea.Height > 0)
                {
                    if (content.DockHandler.AutoHidePortion < 1)
                        content.DockHandler.AutoHidePortion += offset / (double) rectDockArea.Height;
                    else
                        content.DockHandler.AutoHidePortion = Height + offset;
                }
            }

            #region IDragSource Members

            Control IDragSource.DragControl => this;

            #endregion

            #endregion
        }
    }
}