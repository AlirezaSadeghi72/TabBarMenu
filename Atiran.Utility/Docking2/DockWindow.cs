using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
{
    /// <summary>
    ///     Dock window base class.
    /// </summary>
    [ToolboxItem(false)]
    public class DockWindow : Panel, INestedPanesContainer, ISplitterHost
    {
        private SplitterBase m_splitter;

        protected internal DockWindow(DockPanel dockPanel, DockState dockState)
        {
            NestedPanes = new NestedPaneCollection(this);
            DockPanel = dockPanel;
            DockState = dockState;
            Visible = false;

            SuspendLayout();

            if (DockState == DockState.DockLeft || DockState == DockState.DockRight ||
                DockState == DockState.DockTop || DockState == DockState.DockBottom)
            {
                m_splitter = DockPanel.Theme.Extender.WindowSplitterControlFactory.CreateSplitterControl(this);
                Controls.Add(m_splitter);
            }

            if (DockState == DockState.DockLeft)
            {
                Dock = DockStyle.Left;
                m_splitter.Dock = DockStyle.Right;
            }
            else if (DockState == DockState.DockRight)
            {
                Dock = DockStyle.Right;
                m_splitter.Dock = DockStyle.Left;
            }
            else if (DockState == DockState.DockTop)
            {
                Dock = DockStyle.Top;
                m_splitter.Dock = DockStyle.Bottom;
            }
            else if (DockState == DockState.DockBottom)
            {
                Dock = DockStyle.Bottom;
                m_splitter.Dock = DockStyle.Top;
            }
            else if (DockState == DockState.Document)
            {
                Dock = DockStyle.Fill;
            }

            ResumeLayout();
        }

        internal DockPane DefaultPane => VisibleNestedPanes.Count == 0 ? null : VisibleNestedPanes[0];

        public VisibleNestedPaneCollection VisibleNestedPanes => NestedPanes.VisibleNestedPanes;

        public NestedPaneCollection NestedPanes { get; }

        public DockState DockState { get; }

        public bool IsFloat => DockState == DockState.Float;

        public virtual Rectangle DisplayingRectangle
        {
            get
            {
                var rect = ClientRectangle;
                // if DockWindow is document, exclude the border
                if (DockState == DockState.Document)
                {
                    rect.X += 1;
                    rect.Y += 1;
                    rect.Width -= 2;
                    rect.Height -= 2;
                }
                // exclude the splitter
                else if (DockState == DockState.DockLeft)
                {
                    rect.Width -= DockPanel.Theme.Measures.SplitterSize;
                }
                else if (DockState == DockState.DockRight)
                {
                    rect.X += DockPanel.Theme.Measures.SplitterSize;
                    rect.Width -= DockPanel.Theme.Measures.SplitterSize;
                }
                else if (DockState == DockState.DockTop)
                {
                    rect.Height -= DockPanel.Theme.Measures.SplitterSize;
                }
                else if (DockState == DockState.DockBottom)
                {
                    rect.Y += DockPanel.Theme.Measures.SplitterSize;
                    rect.Height -= DockPanel.Theme.Measures.SplitterSize;
                }

                return rect;
            }
        }

        public bool IsDockWindow => true;

        public DockPanel DockPanel { get; }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            VisibleNestedPanes.Refresh();
            if (VisibleNestedPanes.Count == 0)
            {
                if (Visible)
                    Visible = false;
            }
            else if (!Visible)
            {
                Visible = true;
                VisibleNestedPanes.Refresh();
            }

            base.OnLayout(levent);
        }

        #region ISplitterDragSource Members

        void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
        {
        }

        void ISplitterDragSource.EndDrag()
        {
        }

        bool ISplitterDragSource.IsVertical => DockState == DockState.DockLeft || DockState == DockState.DockRight;

        Rectangle ISplitterDragSource.DragLimitBounds
        {
            get
            {
                var rectLimit = DockPanel.DockArea;
                Point location;
                if ((ModifierKeys & Keys.Shift) == 0)
                    location = Location;
                else
                    location = DockPanel.DockArea.Location;

                if (((ISplitterDragSource) this).IsVertical)
                {
                    rectLimit.X += MeasurePane.MinSize;
                    rectLimit.Width -= 2 * MeasurePane.MinSize;
                    rectLimit.Y = location.Y;
                    if ((ModifierKeys & Keys.Shift) == 0)
                        rectLimit.Height = Height;
                }
                else
                {
                    rectLimit.Y += MeasurePane.MinSize;
                    rectLimit.Height -= 2 * MeasurePane.MinSize;
                    rectLimit.X = location.X;
                    if ((ModifierKeys & Keys.Shift) == 0)
                        rectLimit.Width = Width;
                }

                return DockPanel.RectangleToScreen(rectLimit);
            }
        }

        void ISplitterDragSource.MoveSplitter(int offset)
        {
            if ((ModifierKeys & Keys.Shift) != 0)
                SendToBack();

            var rectDockArea = DockPanel.DockArea;
            if (DockState == DockState.DockLeft && rectDockArea.Width > 0)
            {
                if (DockPanel.DockLeftPortion > 1)
                    DockPanel.DockLeftPortion = Width + offset;
                else
                    DockPanel.DockLeftPortion += offset / (double) rectDockArea.Width;
            }
            else if (DockState == DockState.DockRight && rectDockArea.Width > 0)
            {
                if (DockPanel.DockRightPortion > 1)
                    DockPanel.DockRightPortion = Width - offset;
                else
                    DockPanel.DockRightPortion -= offset / (double) rectDockArea.Width;
            }
            else if (DockState == DockState.DockBottom && rectDockArea.Height > 0)
            {
                if (DockPanel.DockBottomPortion > 1)
                    DockPanel.DockBottomPortion = Height - offset;
                else
                    DockPanel.DockBottomPortion -= offset / (double) rectDockArea.Height;
            }
            else if (DockState == DockState.DockTop && rectDockArea.Height > 0)
            {
                if (DockPanel.DockTopPortion > 1)
                    DockPanel.DockTopPortion = Height + offset;
                else
                    DockPanel.DockTopPortion += offset / (double) rectDockArea.Height;
            }
        }

        #region IDragSource Members

        Control IDragSource.DragControl => this;

        #endregion

        #endregion
    }
}