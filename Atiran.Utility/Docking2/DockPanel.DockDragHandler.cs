using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
{
    partial class DockPanel
    {
        private DockDragHandler m_dockDragHandler;

        private DockDragHandler GetDockDragHandler()
        {
            if (m_dockDragHandler == null)
                m_dockDragHandler = new DockDragHandler(this);
            return m_dockDragHandler;
        }

        internal void BeginDrag(IDockDragSource dragSource)
        {
            GetDockDragHandler().BeginDrag(dragSource);
        }

        public sealed class DockDragHandler : DragHandler
        {
            public DockDragHandler(DockPanel panel)
                : base(panel)
            {
            }

            public new IDockDragSource DragSource
            {
                get => base.DragSource as IDockDragSource;
                set => base.DragSource = value;
            }

            public DockOutlineBase Outline { get; private set; }

            private DockIndicator Indicator { get; set; }

            private Rectangle FloatOutlineBounds { get; set; }

            public void BeginDrag(IDockDragSource dragSource)
            {
                DragSource = dragSource;

                if (!BeginDrag())
                {
                    DragSource = null;
                    return;
                }

                Outline = DockPanel.Theme.Extender.DockOutlineFactory.CreateDockOutline();
                Indicator = DockPanel.Theme.Extender.DockIndicatorFactory.CreateDockIndicator(this);
                Indicator.Show(false);

                FloatOutlineBounds = DragSource.BeginDrag(StartMousePosition);
            }

            protected override void OnDragging()
            {
                TestDrop();
            }

            protected override void OnEndDrag(bool abort)
            {
                DockPanel.SuspendLayout(true);

                Outline.Close();
                Indicator.Close();

                EndDrag(abort);

                // Queue a request to layout all children controls
                DockPanel.PerformMdiClientLayout();

                DockPanel.ResumeLayout(true, true);

                DragSource.EndDrag();

                DragSource = null;

                // Fire notification
                DockPanel.OnDocumentDragged();
            }

            private void TestDrop()
            {
                Outline.FlagTestDrop = false;

                Indicator.FullPanelEdge = (ModifierKeys & Keys.Shift) != 0;

                if ((ModifierKeys & Keys.Control) == 0)
                {
                    Indicator.TestDrop();

                    if (!Outline.FlagTestDrop)
                    {
                        var pane = DockHelper.PaneAtPoint(MousePosition, DockPanel);
                        if (pane != null && DragSource.IsDockStateValid(pane.DockState))
                            pane.TestDrop(DragSource, Outline);
                    }

                    if (!Outline.FlagTestDrop && DragSource.IsDockStateValid(DockState.Float))
                    {
                        var floatWindow = DockHelper.FloatWindowAtPoint(MousePosition, DockPanel);
                        if (floatWindow != null)
                            floatWindow.TestDrop(DragSource, Outline);
                    }
                }
                else
                {
                    Indicator.DockPane = DockHelper.PaneAtPoint(MousePosition, DockPanel);
                }

                if (!Outline.FlagTestDrop)
                    if (DragSource.IsDockStateValid(DockState.Float))
                    {
                        var rect = FloatOutlineBounds;
                        rect.Offset(MousePosition.X - StartMousePosition.X, MousePosition.Y - StartMousePosition.Y);
                        Outline.Show(rect);
                    }

                if (!Outline.FlagTestDrop)
                {
                    Cursor.Current = Cursors.No;
                    Outline.Show();
                }
                else
                {
                    Cursor.Current = DragControl.Cursor;
                }
            }

            private void EndDrag(bool abort)
            {
                if (abort)
                    return;

                if (!Outline.FloatWindowBounds.IsEmpty)
                {
                    DragSource.FloatAt(Outline.FloatWindowBounds);
                }
                else if (Outline.DockTo is DockPane)
                {
                    var pane = Outline.DockTo as DockPane;
                    DragSource.DockTo(pane, Outline.Dock, Outline.ContentIndex);
                }
                else if (Outline.DockTo is DockPanel)
                {
                    var panel = Outline.DockTo as DockPanel;
                    panel.UpdateDockWindowZOrder(Outline.Dock, Outline.FlagFullEdge);
                    DragSource.DockTo(panel, Outline.Dock);
                }
            }

            public class DockIndicator : DragForm
            {
                #region consts

                private int _PanelIndicatorMargin = 10;

                #endregion

                private DockPane m_dockPane;

                private bool m_fullPanelEdge;

                private IHitTest m_hitTest;

                private IPaneIndicator m_paneDiamond;

                private IPanelIndicator m_panelBottom;

                private IPanelIndicator m_panelFill;

                private IPanelIndicator m_panelLeft;

                private IPanelIndicator m_panelRight;

                private IPanelIndicator m_panelTop;

                public DockIndicator(DockDragHandler dragHandler)
                {
                    DragHandler = dragHandler;
                    Controls.AddRange(new[]
                    {
                        (Control) PaneDiamond,
                        (Control) PanelLeft,
                        (Control) PanelRight,
                        (Control) PanelTop,
                        (Control) PanelBottom,
                        (Control) PanelFill
                    });
                    Region = new Region(Rectangle.Empty);
                }

                private IPaneIndicator PaneDiamond
                {
                    get
                    {
                        if (m_paneDiamond == null)
                            m_paneDiamond =
                                DragHandler.DockPanel.Theme.Extender.PaneIndicatorFactory.CreatePaneIndicator(
                                    DragHandler.DockPanel.Theme);

                        return m_paneDiamond;
                    }
                }

                private IPanelIndicator PanelLeft
                {
                    get
                    {
                        if (m_panelLeft == null)
                            m_panelLeft =
                                DragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(
                                    DockStyle.Left, DragHandler.DockPanel.Theme);

                        return m_panelLeft;
                    }
                }

                private IPanelIndicator PanelRight
                {
                    get
                    {
                        if (m_panelRight == null)
                            m_panelRight =
                                DragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(
                                    DockStyle.Right, DragHandler.DockPanel.Theme);

                        return m_panelRight;
                    }
                }

                private IPanelIndicator PanelTop
                {
                    get
                    {
                        if (m_panelTop == null)
                            m_panelTop =
                                DragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(
                                    DockStyle.Top, DragHandler.DockPanel.Theme);

                        return m_panelTop;
                    }
                }

                private IPanelIndicator PanelBottom
                {
                    get
                    {
                        if (m_panelBottom == null)
                            m_panelBottom =
                                DragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(
                                    DockStyle.Bottom, DragHandler.DockPanel.Theme);

                        return m_panelBottom;
                    }
                }

                private IPanelIndicator PanelFill
                {
                    get
                    {
                        if (m_panelFill == null)
                            m_panelFill =
                                DragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator(
                                    DockStyle.Fill, DragHandler.DockPanel.Theme);

                        return m_panelFill;
                    }
                }

                public bool FullPanelEdge
                {
                    get => m_fullPanelEdge;
                    set
                    {
                        if (m_fullPanelEdge == value)
                            return;

                        m_fullPanelEdge = value;
                        RefreshChanges();
                    }
                }

                public DockDragHandler DragHandler { get; }

                public DockPanel DockPanel => DragHandler.DockPanel;

                public DockPane DockPane
                {
                    get => m_dockPane;
                    internal set
                    {
                        if (m_dockPane == value)
                            return;

                        var oldDisplayingPane = DisplayingPane;
                        m_dockPane = value;
                        if (oldDisplayingPane != DisplayingPane)
                            RefreshChanges();
                    }
                }

                private IHitTest HitTestResult
                {
                    get => m_hitTest;
                    set
                    {
                        if (m_hitTest == value)
                            return;

                        if (m_hitTest != null)
                            m_hitTest.Status = DockStyle.None;

                        m_hitTest = value;
                    }
                }

                private DockPane DisplayingPane => ShouldPaneDiamondVisible() ? DockPane : null;

                private void RefreshChanges()
                {
                    var region = new Region(Rectangle.Empty);
                    var rectDockArea = FullPanelEdge ? DockPanel.DockArea : DockPanel.DocumentWindowBounds;

                    rectDockArea = RectangleToClient(DockPanel.RectangleToScreen(rectDockArea));
                    if (ShouldPanelIndicatorVisible(DockState.DockLeft))
                    {
                        PanelLeft.Location = new Point(rectDockArea.X + _PanelIndicatorMargin,
                            rectDockArea.Y + (rectDockArea.Height - PanelRight.Height) / 2);
                        PanelLeft.Visible = true;
                        region.Union(PanelLeft.Bounds);
                    }
                    else
                    {
                        PanelLeft.Visible = false;
                    }

                    if (ShouldPanelIndicatorVisible(DockState.DockRight))
                    {
                        PanelRight.Location =
                            new Point(rectDockArea.X + rectDockArea.Width - PanelRight.Width - _PanelIndicatorMargin,
                                rectDockArea.Y + (rectDockArea.Height - PanelRight.Height) / 2);
                        PanelRight.Visible = true;
                        region.Union(PanelRight.Bounds);
                    }
                    else
                    {
                        PanelRight.Visible = false;
                    }

                    if (ShouldPanelIndicatorVisible(DockState.DockTop))
                    {
                        PanelTop.Location = new Point(rectDockArea.X + (rectDockArea.Width - PanelTop.Width) / 2,
                            rectDockArea.Y + _PanelIndicatorMargin);
                        PanelTop.Visible = true;
                        region.Union(PanelTop.Bounds);
                    }
                    else
                    {
                        PanelTop.Visible = false;
                    }

                    if (ShouldPanelIndicatorVisible(DockState.DockBottom))
                    {
                        PanelBottom.Location = new Point(rectDockArea.X + (rectDockArea.Width - PanelBottom.Width) / 2,
                            rectDockArea.Y + rectDockArea.Height - PanelBottom.Height - _PanelIndicatorMargin);
                        PanelBottom.Visible = true;
                        region.Union(PanelBottom.Bounds);
                    }
                    else
                    {
                        PanelBottom.Visible = false;
                    }

                    if (ShouldPanelIndicatorVisible(DockState.Document))
                    {
                        var rectDocumentWindow =
                            RectangleToClient(DockPanel.RectangleToScreen(DockPanel.DocumentWindowBounds));
                        PanelFill.Location =
                            new Point(rectDocumentWindow.X + (rectDocumentWindow.Width - PanelFill.Width) / 2,
                                rectDocumentWindow.Y + (rectDocumentWindow.Height - PanelFill.Height) / 2);
                        PanelFill.Visible = true;
                        region.Union(PanelFill.Bounds);
                    }
                    else
                    {
                        PanelFill.Visible = false;
                    }

                    if (ShouldPaneDiamondVisible())
                    {
                        var rect = RectangleToClient(DockPane.RectangleToScreen(DockPane.ClientRectangle));
                        PaneDiamond.Location = new Point(rect.Left + (rect.Width - PaneDiamond.Width) / 2,
                            rect.Top + (rect.Height - PaneDiamond.Height) / 2);
                        PaneDiamond.Visible = true;
                        using (var graphicsPath = PaneDiamond.DisplayingGraphicsPath.Clone() as GraphicsPath)
                        {
                            Point[] pts =
                            {
                                new Point(PaneDiamond.Left, PaneDiamond.Top),
                                new Point(PaneDiamond.Right, PaneDiamond.Top),
                                new Point(PaneDiamond.Left, PaneDiamond.Bottom)
                            };
                            using (var matrix = new Matrix(PaneDiamond.ClientRectangle, pts))
                            {
                                graphicsPath.Transform(matrix);
                            }

                            region.Union(graphicsPath);
                        }
                    }
                    else
                    {
                        PaneDiamond.Visible = false;
                    }

                    Region = region;
                }

                private bool ShouldPanelIndicatorVisible(DockState dockState)
                {
                    if (!Visible)
                        return false;

                    if (DockPanel.DockWindows[dockState].Visible)
                        return false;

                    return DragHandler.DragSource.IsDockStateValid(dockState);
                }

                private bool ShouldPaneDiamondVisible()
                {
                    if (DockPane == null)
                        return false;

                    if (!DockPanel.AllowEndUserNestedDocking)
                        return false;

                    return DragHandler.DragSource.CanDockTo(DockPane);
                }

                public override void Show(bool bActivate)
                {
                    base.Show(bActivate);
                    Bounds = SystemInformation.VirtualScreen;
                    RefreshChanges();
                }

                public void TestDrop()
                {
                    var pt = MousePosition;
                    DockPane = DockHelper.PaneAtPoint(pt, DockPanel);

                    if (TestDrop(PanelLeft, pt) != DockStyle.None)
                        HitTestResult = PanelLeft;
                    else if (TestDrop(PanelRight, pt) != DockStyle.None)
                        HitTestResult = PanelRight;
                    else if (TestDrop(PanelTop, pt) != DockStyle.None)
                        HitTestResult = PanelTop;
                    else if (TestDrop(PanelBottom, pt) != DockStyle.None)
                        HitTestResult = PanelBottom;
                    else if (TestDrop(PanelFill, pt) != DockStyle.None)
                        HitTestResult = PanelFill;
                    else if (TestDrop(PaneDiamond, pt) != DockStyle.None)
                        HitTestResult = PaneDiamond;
                    else
                        HitTestResult = null;

                    if (HitTestResult != null)
                    {
                        if (HitTestResult is IPaneIndicator)
                            DragHandler.Outline.Show(DockPane, HitTestResult.Status);
                        else
                            DragHandler.Outline.Show(DockPanel, HitTestResult.Status, FullPanelEdge);
                    }
                }

                private static DockStyle TestDrop(IHitTest hitTest, Point pt)
                {
                    return hitTest.Status = hitTest.HitTest(pt);
                }
            }
        }

        #region IHitTest

        public interface IHitTest
        {
            DockStyle Status { get; set; }
            DockStyle HitTest(Point pt);
        }

        public interface IPaneIndicator : IHitTest
        {
            Point Location { get; set; }
            bool Visible { get; set; }
            int Left { get; }
            int Top { get; }
            int Right { get; }
            int Bottom { get; }
            Rectangle ClientRectangle { get; }
            int Width { get; }
            int Height { get; }
            GraphicsPath DisplayingGraphicsPath { get; }
        }

        public interface IPanelIndicator : IHitTest
        {
            Point Location { get; set; }
            bool Visible { get; set; }
            Rectangle Bounds { get; }
            int Width { get; }
            int Height { get; }
        }

        public struct HotSpotIndex
        {
            public HotSpotIndex(int x, int y, DockStyle dockStyle)
            {
                X = x;
                Y = y;
                DockStyle = dockStyle;
            }

            public int X { get; }

            public int Y { get; }

            public DockStyle DockStyle { get; }
        }

        #endregion
    }
}