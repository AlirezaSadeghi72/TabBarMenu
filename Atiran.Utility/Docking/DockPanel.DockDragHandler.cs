using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking
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

        private sealed class DockDragHandler : DragHandler
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

                Outline = new DockOutline();
                Indicator = new DockIndicator(this);
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

                DragSource = null;
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

            private class DockIndicator : DragForm
            {
                #region consts

                private int _PanelIndicatorMargin = 10;

                #endregion

                private DockPane m_dockPane;

                private bool m_fullPanelEdge;

                private IHitTest m_hitTest;

                private PaneIndicator m_paneDiamond;

                private PanelIndicator m_panelBottom;

                private PanelIndicator m_panelFill;

                private PanelIndicator m_panelLeft;

                private PanelIndicator m_panelRight;

                private PanelIndicator m_panelTop;

                public DockIndicator(DockDragHandler dragHandler)
                {
                    DragHandler = dragHandler;
                    Controls.AddRange(new Control[]
                    {
                        PaneDiamond,
                        PanelLeft,
                        PanelRight,
                        PanelTop,
                        PanelBottom,
                        PanelFill
                    });
                    Region = new Region(Rectangle.Empty);
                }

                private PaneIndicator PaneDiamond
                {
                    get
                    {
                        if (m_paneDiamond == null)
                            m_paneDiamond = new PaneIndicator();

                        return m_paneDiamond;
                    }
                }

                private PanelIndicator PanelLeft
                {
                    get
                    {
                        if (m_panelLeft == null)
                            m_panelLeft = new PanelIndicator(DockStyle.Left);

                        return m_panelLeft;
                    }
                }

                private PanelIndicator PanelRight
                {
                    get
                    {
                        if (m_panelRight == null)
                            m_panelRight = new PanelIndicator(DockStyle.Right);

                        return m_panelRight;
                    }
                }

                private PanelIndicator PanelTop
                {
                    get
                    {
                        if (m_panelTop == null)
                            m_panelTop = new PanelIndicator(DockStyle.Top);

                        return m_panelTop;
                    }
                }

                private PanelIndicator PanelBottom
                {
                    get
                    {
                        if (m_panelBottom == null)
                            m_panelBottom = new PanelIndicator(DockStyle.Bottom);

                        return m_panelBottom;
                    }
                }

                private PanelIndicator PanelFill
                {
                    get
                    {
                        if (m_panelFill == null)
                            m_panelFill = new PanelIndicator(DockStyle.Fill);

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
                        using (var graphicsPath = PaneIndicator.DisplayingGraphicsPath.Clone() as GraphicsPath)
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
                        if (HitTestResult is PaneIndicator)
                            DragHandler.Outline.Show(DockPane, HitTestResult.Status);
                        else
                            DragHandler.Outline.Show(DockPanel, HitTestResult.Status, FullPanelEdge);
                    }
                }

                private static DockStyle TestDrop(IHitTest hitTest, Point pt)
                {
                    return hitTest.Status = hitTest.HitTest(pt);
                }

                #region IHitTest

                private interface IHitTest
                {
                    DockStyle Status { get; set; }
                    DockStyle HitTest(Point pt);
                }

                #endregion

                #region PanelIndicator

                private class PanelIndicator : PictureBox, IHitTest
                {
                    private static Image _imagePanelLeft = Resources.DockIndicator_PanelLeft;
                    private static Image _imagePanelRight = Resources.DockIndicator_PanelRight;
                    private static Image _imagePanelTop = Resources.DockIndicator_PanelTop;
                    private static Image _imagePanelBottom = Resources.DockIndicator_PanelBottom;
                    private static Image _imagePanelFill = Resources.DockIndicator_PanelFill;
                    private static Image _imagePanelLeftActive = Resources.DockIndicator_PanelLeft_Active;
                    private static Image _imagePanelRightActive = Resources.DockIndicator_PanelRight_Active;
                    private static Image _imagePanelTopActive = Resources.DockIndicator_PanelTop_Active;
                    private static Image _imagePanelBottomActive = Resources.DockIndicator_PanelBottom_Active;
                    private static Image _imagePanelFillActive = Resources.DockIndicator_PanelFill_Active;

                    private bool m_isActivated;

                    private DockStyle m_status;

                    public PanelIndicator(DockStyle dockStyle)
                    {
                        DockStyle = dockStyle;
                        SizeMode = PictureBoxSizeMode.AutoSize;
                        Image = ImageInactive;
                    }

                    private DockStyle DockStyle { get; }

                    private Image ImageInactive
                    {
                        get
                        {
                            if (DockStyle == DockStyle.Left)
                                return _imagePanelLeft;
                            if (DockStyle == DockStyle.Right)
                                return _imagePanelRight;
                            if (DockStyle == DockStyle.Top)
                                return _imagePanelTop;
                            if (DockStyle == DockStyle.Bottom)
                                return _imagePanelBottom;
                            if (DockStyle == DockStyle.Fill)
                                return _imagePanelFill;
                            return null;
                        }
                    }

                    private Image ImageActive
                    {
                        get
                        {
                            if (DockStyle == DockStyle.Left)
                                return _imagePanelLeftActive;
                            if (DockStyle == DockStyle.Right)
                                return _imagePanelRightActive;
                            if (DockStyle == DockStyle.Top)
                                return _imagePanelTopActive;
                            if (DockStyle == DockStyle.Bottom)
                                return _imagePanelBottomActive;
                            if (DockStyle == DockStyle.Fill)
                                return _imagePanelFillActive;
                            return null;
                        }
                    }

                    private bool IsActivated
                    {
                        get => m_isActivated;
                        set
                        {
                            m_isActivated = value;
                            Image = IsActivated ? ImageActive : ImageInactive;
                        }
                    }

                    public DockStyle Status
                    {
                        get => m_status;
                        set
                        {
                            if (value != DockStyle && value != DockStyle.None)
                                throw new InvalidEnumArgumentException();

                            if (m_status == value)
                                return;

                            m_status = value;
                            IsActivated = m_status != DockStyle.None;
                        }
                    }

                    public DockStyle HitTest(Point pt)
                    {
                        return Visible && ClientRectangle.Contains(PointToClient(pt)) ? DockStyle : DockStyle.None;
                    }
                }

                #endregion PanelIndicator

                #region PaneIndicator

                private class PaneIndicator : PictureBox, IHitTest
                {
                    private static Bitmap _bitmapPaneDiamond = Resources.DockIndicator_PaneDiamond;
                    private static Bitmap _bitmapPaneDiamondLeft = Resources.DockIndicator_PaneDiamond_Left;
                    private static Bitmap _bitmapPaneDiamondRight = Resources.DockIndicator_PaneDiamond_Right;
                    private static Bitmap _bitmapPaneDiamondTop = Resources.DockIndicator_PaneDiamond_Top;
                    private static Bitmap _bitmapPaneDiamondBottom = Resources.DockIndicator_PaneDiamond_Bottom;
                    private static Bitmap _bitmapPaneDiamondFill = Resources.DockIndicator_PaneDiamond_Fill;
                    private static Bitmap _bitmapPaneDiamondHotSpot = Resources.DockIndicator_PaneDiamond_HotSpot;

                    private static Bitmap _bitmapPaneDiamondHotSpotIndex =
                        Resources.DockIndicator_PaneDiamond_HotSpotIndex;

                    private static HotSpotIndex[] _hotSpots =
                    {
                        new HotSpotIndex(1, 0, DockStyle.Top),
                        new HotSpotIndex(0, 1, DockStyle.Left),
                        new HotSpotIndex(1, 1, DockStyle.Fill),
                        new HotSpotIndex(2, 1, DockStyle.Right),
                        new HotSpotIndex(1, 2, DockStyle.Bottom)
                    };

                    private DockStyle m_status = DockStyle.None;

                    public PaneIndicator()
                    {
                        SizeMode = PictureBoxSizeMode.AutoSize;
                        Image = _bitmapPaneDiamond;
                        Region = new Region(DisplayingGraphicsPath);
                    }

                    public static GraphicsPath DisplayingGraphicsPath { get; } =
                        DrawHelper.CalculateGraphicsPathFromBitmap(_bitmapPaneDiamond);

                    public DockStyle HitTest(Point pt)
                    {
                        if (!Visible)
                            return DockStyle.None;

                        pt = PointToClient(pt);
                        if (!ClientRectangle.Contains(pt))
                            return DockStyle.None;

                        for (var i = _hotSpots.GetLowerBound(0); i <= _hotSpots.GetUpperBound(0); i++)
                            if (_bitmapPaneDiamondHotSpot.GetPixel(pt.X, pt.Y) ==
                                _bitmapPaneDiamondHotSpotIndex.GetPixel(_hotSpots[i].X, _hotSpots[i].Y))
                                return _hotSpots[i].DockStyle;

                        return DockStyle.None;
                    }

                    public DockStyle Status
                    {
                        get => m_status;
                        set
                        {
                            m_status = value;
                            if (m_status == DockStyle.None)
                                Image = _bitmapPaneDiamond;
                            else if (m_status == DockStyle.Left)
                                Image = _bitmapPaneDiamondLeft;
                            else if (m_status == DockStyle.Right)
                                Image = _bitmapPaneDiamondRight;
                            else if (m_status == DockStyle.Top)
                                Image = _bitmapPaneDiamondTop;
                            else if (m_status == DockStyle.Bottom)
                                Image = _bitmapPaneDiamondBottom;
                            else if (m_status == DockStyle.Fill)
                                Image = _bitmapPaneDiamondFill;
                        }
                    }

                    private struct HotSpotIndex
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
                }

                #endregion PaneIndicator
            }

            private class DockOutline : DockOutlineBase
            {
                public DockOutline()
                {
                    DragForm = new DragForm();
                    SetDragForm(Rectangle.Empty);
                    DragForm.BackColor = SystemColors.ActiveCaption;
                    DragForm.Opacity = 0.5;
                    DragForm.Show(false);
                }

                private DragForm DragForm { get; }

                protected override void OnShow()
                {
                    CalculateRegion();
                }

                protected override void OnClose()
                {
                    DragForm.Close();
                }

                private void CalculateRegion()
                {
                    if (SameAsOldValue)
                        return;

                    if (!FloatWindowBounds.IsEmpty)
                        SetOutline(FloatWindowBounds);
                    else if (DockTo is DockPanel)
                        SetOutline(DockTo as DockPanel, Dock, ContentIndex != 0);
                    else if (DockTo is DockPane)
                        SetOutline(DockTo as DockPane, Dock, ContentIndex);
                    else
                        SetOutline();
                }

                private void SetOutline()
                {
                    SetDragForm(Rectangle.Empty);
                }

                private void SetOutline(Rectangle floatWindowBounds)
                {
                    SetDragForm(floatWindowBounds);
                }

                private void SetOutline(DockPanel dockPanel, DockStyle dock, bool fullPanelEdge)
                {
                    var rect = fullPanelEdge ? dockPanel.DockArea : dockPanel.DocumentWindowBounds;
                    rect.Location = dockPanel.PointToScreen(rect.Location);
                    if (dock == DockStyle.Top)
                    {
                        var height = dockPanel.GetDockWindowSize(DockState.DockTop);
                        rect = new Rectangle(rect.X, rect.Y, rect.Width, height);
                    }
                    else if (dock == DockStyle.Bottom)
                    {
                        var height = dockPanel.GetDockWindowSize(DockState.DockBottom);
                        rect = new Rectangle(rect.X, rect.Bottom - height, rect.Width, height);
                    }
                    else if (dock == DockStyle.Left)
                    {
                        var width = dockPanel.GetDockWindowSize(DockState.DockLeft);
                        rect = new Rectangle(rect.X, rect.Y, width, rect.Height);
                    }
                    else if (dock == DockStyle.Right)
                    {
                        var width = dockPanel.GetDockWindowSize(DockState.DockRight);
                        rect = new Rectangle(rect.Right - width, rect.Y, width, rect.Height);
                    }
                    else if (dock == DockStyle.Fill)
                    {
                        rect = dockPanel.DocumentWindowBounds;
                        rect.Location = dockPanel.PointToScreen(rect.Location);
                    }

                    SetDragForm(rect);
                }

                private void SetOutline(DockPane pane, DockStyle dock, int contentIndex)
                {
                    if (dock != DockStyle.Fill)
                    {
                        var rect = pane.DisplayingRectangle;
                        if (dock == DockStyle.Right)
                            rect.X += rect.Width / 2;
                        if (dock == DockStyle.Bottom)
                            rect.Y += rect.Height / 2;
                        if (dock == DockStyle.Left || dock == DockStyle.Right)
                            rect.Width -= rect.Width / 2;
                        if (dock == DockStyle.Top || dock == DockStyle.Bottom)
                            rect.Height -= rect.Height / 2;
                        rect.Location = pane.PointToScreen(rect.Location);

                        SetDragForm(rect);
                    }
                    else if (contentIndex == -1)
                    {
                        var rect = pane.DisplayingRectangle;
                        rect.Location = pane.PointToScreen(rect.Location);
                        SetDragForm(rect);
                    }
                    else
                    {
                        using (var path = pane.TabStripControl.GetOutline(contentIndex))
                        {
                            var rectF = path.GetBounds();
                            var rect = new Rectangle((int) rectF.X, (int) rectF.Y, (int) rectF.Width,
                                (int) rectF.Height);
                            using (var matrix = new Matrix(rect,
                                new[] {new Point(0, 0), new Point(rect.Width, 0), new Point(0, rect.Height)}))
                            {
                                path.Transform(matrix);
                            }

                            var region = new Region(path);
                            SetDragForm(rect, region);
                        }
                    }
                }

                private void SetDragForm(Rectangle rect)
                {
                    DragForm.Bounds = rect;
                    if (rect == Rectangle.Empty)
                        DragForm.Region = new Region(Rectangle.Empty);
                    else if (DragForm.Region != null)
                        DragForm.Region = null;
                }

                private void SetDragForm(Rectangle rect, Region region)
                {
                    DragForm.Bounds = rect;
                    DragForm.Region = region;
                }
            }
        }
    }
}