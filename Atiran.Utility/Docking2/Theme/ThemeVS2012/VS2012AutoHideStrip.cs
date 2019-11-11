using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    [ToolboxItem(false)]
    internal class VS2012AutoHideStrip : AutoHideStripBase
    {
        private const int TextGapLeft = 0;
        private const int TextGapRight = 0;
        private const int TextGapBottom = 3;
        private const int TabGapTop = 3;
        private const int TabGapBottom = 8;
        private const int TabGapLeft = 0;
        private const int TabGapBetween = 12;

        private static DockState[] _dockStates;

        private static GraphicsPath _graphicsPath;

        private TabVS2012 lastSelectedTab;

        public VS2012AutoHideStrip(DockPanel panel)
            : base(panel)
        {
            SetStyle(ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = DockPanel.Theme.ColorPalette.MainWindowActive.Background;
        }

        private static Matrix MatrixIdentity { get; } = new Matrix();

        private static DockState[] DockStates
        {
            get
            {
                if (_dockStates == null)
                {
                    _dockStates = new DockState[4];
                    _dockStates[0] = DockState.DockLeftAutoHide;
                    _dockStates[1] = DockState.DockRightAutoHide;
                    _dockStates[2] = DockState.DockTopAutoHide;
                    _dockStates[3] = DockState.DockBottomAutoHide;
                }

                return _dockStates;
            }
        }

        internal static GraphicsPath GraphicsPath
        {
            get
            {
                if (_graphicsPath == null)
                    _graphicsPath = new GraphicsPath();

                return _graphicsPath;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            DrawTabStrip(g);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            CalculateTabs();
            base.OnLayout(levent);
        }

        private void DrawTabStrip(Graphics g)
        {
            DrawTabStrip(g, DockState.DockTopAutoHide);
            DrawTabStrip(g, DockState.DockBottomAutoHide);
            DrawTabStrip(g, DockState.DockLeftAutoHide);
            DrawTabStrip(g, DockState.DockRightAutoHide);
        }

        private void DrawTabStrip(Graphics g, DockState dockState)
        {
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            if (rectTabStrip.IsEmpty)
                return;

            var matrixIdentity = g.Transform;
            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
            {
                var matrixRotated = new Matrix();
                matrixRotated.RotateAt(90, new PointF(rectTabStrip.X + (float) rectTabStrip.Height / 2,
                    rectTabStrip.Y + (float) rectTabStrip.Height / 2));
                g.Transform = matrixRotated;
            }

            foreach (var pane in GetPanes(dockState))
            foreach (TabVS2012 tab in pane.AutoHideTabs)
                DrawTab(g, tab);

            g.Transform = matrixIdentity;
        }

        private void CalculateTabs()
        {
            CalculateTabs(DockState.DockTopAutoHide);
            CalculateTabs(DockState.DockBottomAutoHide);
            CalculateTabs(DockState.DockLeftAutoHide);
            CalculateTabs(DockState.DockRightAutoHide);
        }

        private void CalculateTabs(DockState dockState)
        {
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            var x = TabGapLeft + rectTabStrip.X;
            foreach (var pane in GetPanes(dockState))
            foreach (TabVS2012 tab in pane.AutoHideTabs)
            {
                var width = TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width +
                            TextGapLeft + TextGapRight;
                tab.TabX = x;
                tab.TabWidth = width;
                x += width + TabGapBetween;
            }
        }

        private Rectangle RtlTransform(Rectangle rect, DockState dockState)
        {
            Rectangle rectTransformed;
            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                rectTransformed = rect;
            else
                rectTransformed = DrawHelper.RtlTransform(this, rect);

            return rectTransformed;
        }

        private GraphicsPath GetTabOutline(TabVS2012 tab, bool rtlTransform)
        {
            var dockState = tab.Content.DockHandler.DockState;
            var rectTab = GetTabRectangle(tab);
            if (rtlTransform)
                rectTab = RtlTransform(rectTab, dockState);

            if (GraphicsPath != null)
            {
                GraphicsPath.Reset();
                GraphicsPath.AddRectangle(rectTab);
            }

            return GraphicsPath;
        }

        private void DrawTab(Graphics g, TabVS2012 tab)
        {
            var rectTabOrigin = GetTabRectangle(tab);
            if (rectTabOrigin.IsEmpty)
                return;

            var dockState = tab.Content.DockHandler.DockState;
            var content = tab.Content;

            //Set no rotate for drawing icon and text
            var matrixRotate = g.Transform;
            g.Transform = MatrixIdentity;

            Color borderColor;
            Color backgroundColor;
            Color textColor;
            if (tab.IsMouseOver)
            {
                borderColor = DockPanel.Theme.ColorPalette.AutoHideStripHovered.Border;
                backgroundColor = DockPanel.Theme.ColorPalette.AutoHideStripHovered.Background;
                textColor = DockPanel.Theme.ColorPalette.AutoHideStripHovered.Text;
            }
            else
            {
                borderColor = DockPanel.Theme.ColorPalette.AutoHideStripDefault.Border;
                backgroundColor = DockPanel.Theme.ColorPalette.AutoHideStripDefault.Background;
                textColor = DockPanel.Theme.ColorPalette.AutoHideStripDefault.Text;
            }

            g.FillRectangle(DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rectTabOrigin);

            var rectBorder = GetBorderRectangle(rectTabOrigin, dockState,
                TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width);
            g.FillRectangle(DockPanel.Theme.PaintingService.GetBrush(borderColor), rectBorder);

            // Draw the text
            var rectText = GetTextRectangle(rectTabOrigin, dockState);

            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                g.DrawString(content.DockHandler.TabText, TextFont, DockPanel.Theme.PaintingService.GetBrush(textColor),
                    rectText, StringFormatTabVertical);
            else
                g.DrawString(content.DockHandler.TabText, TextFont, DockPanel.Theme.PaintingService.GetBrush(textColor),
                    rectText, StringFormatTabHorizontal);

            // Set rotate back
            g.Transform = matrixRotate;
        }

        private Rectangle GetBorderRectangle(Rectangle tab, DockState state, int width)
        {
            var result = new Rectangle(tab.Location, tab.Size);
            if (state == DockState.DockLeftAutoHide)
            {
                result.Height = width;
                result.Width = DockPanel.Theme.Measures.AutoHideTabLineWidth;
                result.Y += TextGapLeft;
                return result;
            }

            if (state == DockState.DockRightAutoHide)
            {
                result.Height = width;
                result.Width = DockPanel.Theme.Measures.AutoHideTabLineWidth;
                result.X += tab.Width - result.Width;
                result.Y += TextGapLeft;
                return result;
            }

            if (state == DockState.DockBottomAutoHide)
            {
                result.Width = width;
                result.Height = DockPanel.Theme.Measures.AutoHideTabLineWidth;
                result.X += TextGapLeft;
                result.Y += tab.Height - result.Height;
                return result;
            }

            if (state == DockState.DockTopAutoHide)
            {
                result.Width = width;
                result.Height = DockPanel.Theme.Measures.AutoHideTabLineWidth;
                result.X += TextGapLeft;
                return result;
            }

            return Rectangle.Empty;
        }

        public Rectangle GetLogicalTabStripRectangle(DockState state)
        {
            var rectStrip = GetTabStripRectangle(state);
            var location = rectStrip.Location;
            if (state == DockState.DockLeftAutoHide || state == DockState.DockRightAutoHide)
                return new Rectangle(0, 0, rectStrip.Height, rectStrip.Width);

            return new Rectangle(0, 0, rectStrip.Width, rectStrip.Height);
        }

        private Rectangle GetTabRectangle(TabVS2012 tab)
        {
            var state = tab.Content.DockHandler.DockState;
            var rectStrip = GetTabStripRectangle(state);
            var location = rectStrip.Location;
            if (state == DockState.DockLeftAutoHide || state == DockState.DockRightAutoHide)
            {
                location.Y += tab.TabX;
                return new Rectangle(location.X, location.Y, rectStrip.Width, tab.TabWidth);
            }

            location.X += tab.TabX;
            return new Rectangle(location.X, location.Y, tab.TabWidth, rectStrip.Height);
        }

        private Rectangle GetTextRectangle(Rectangle tab, DockState state)
        {
            var result = new Rectangle(tab.Location, tab.Size);
            if (state == DockState.DockLeftAutoHide)
            {
                result.X += TextGapBottom;
                result.Y += TextGapLeft;
                result.Height -= TextGapLeft + TextGapRight;
                result.Width -= TextGapBottom;
                return result;
            }

            if (state == DockState.DockRightAutoHide)
            {
                result.Y += TextGapLeft;
                result.Height -= TextGapLeft + TextGapRight;
                result.Width -= TextGapBottom;
                return result;
            }

            if (state == DockState.DockBottomAutoHide)
            {
                result.X += TextGapLeft;
                result.Width -= TextGapLeft + TextGapRight;
                result.Height -= TextGapBottom;
                return result;
            }

            if (state == DockState.DockTopAutoHide)
            {
                result.X += TextGapLeft;
                result.Y += TextGapBottom;
                result.Width -= TextGapLeft + TextGapRight;
                result.Height -= TextGapBottom;
                return result;
            }

            return Rectangle.Empty;
        }

        protected override IDockContent HitTest(Point point)
        {
            var tab = TabHitTest(point);

            if (tab != null)
                return tab.Content;
            return null;
        }

        protected override Rectangle GetTabBounds(Tab tab)
        {
            var path = GetTabOutline((TabVS2012) tab, true);
            var bounds = path.GetBounds();
            return new Rectangle((int) bounds.Left, (int) bounds.Top, (int) bounds.Width, (int) bounds.Height);
        }

        protected Tab TabHitTest(Point ptMouse)
        {
            foreach (var state in DockStates)
            {
                var rectTabStrip = GetTabStripRectangle(state);
                if (!rectTabStrip.Contains(ptMouse))
                    continue;

                foreach (var pane in GetPanes(state))
                foreach (TabVS2012 tab in pane.AutoHideTabs)
                {
                    var path = GetTabOutline(tab, true);
                    if (path.IsVisible(ptMouse))
                        return tab;
                }
            }

            return null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var tab = (TabVS2012) TabHitTest(PointToClient(MousePosition));
            if (tab != null)
            {
                tab.IsMouseOver = true;
                Invalidate();
            }

            if (lastSelectedTab != tab)
            {
                if (lastSelectedTab != null)
                {
                    lastSelectedTab.IsMouseOver = false;
                    Invalidate();
                }

                lastSelectedTab = tab;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (lastSelectedTab != null)
                lastSelectedTab.IsMouseOver = false;
            Invalidate();
        }

        protected internal override int MeasureHeight()
        {
            return 31;
        }

        protected override void OnRefreshChanges()
        {
            CalculateTabs();
            Invalidate();
        }

        protected override Tab CreateTab(IDockContent content)
        {
            return new TabVS2012(content);
        }

        private class TabVS2012 : Tab
        {
            internal TabVS2012(IDockContent content)
                : base(content)
            {
            }

            /// <summary>
            ///     X for this <see href="TabVS2012" /> inside the logical strip rectangle.
            /// </summary>
            public int TabX { get; set; }

            /// <summary>
            ///     Width of this <see href="TabVS2012" />.
            /// </summary>
            public int TabWidth { get; set; }

            public bool IsMouseOver { get; set; }
        }

        #region Customizable Properties

        public Font TextFont => DockPanel.Theme.Skin.AutoHideStripSkin.TextFont;

        private static StringFormat _stringFormatTabHorizontal;

        private StringFormat StringFormatTabHorizontal
        {
            get
            {
                if (_stringFormatTabHorizontal == null)
                {
                    _stringFormatTabHorizontal = new StringFormat();
                    _stringFormatTabHorizontal.Alignment = StringAlignment.Near;
                    _stringFormatTabHorizontal.LineAlignment = StringAlignment.Center;
                    _stringFormatTabHorizontal.FormatFlags = StringFormatFlags.NoWrap;
                    _stringFormatTabHorizontal.Trimming = StringTrimming.None;
                }

                if (RightToLeft == RightToLeft.Yes)
                    _stringFormatTabHorizontal.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                else
                    _stringFormatTabHorizontal.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

                return _stringFormatTabHorizontal;
            }
        }

        private static StringFormat _stringFormatTabVertical;

        private StringFormat StringFormatTabVertical
        {
            get
            {
                if (_stringFormatTabVertical == null)
                {
                    _stringFormatTabVertical = new StringFormat();
                    _stringFormatTabVertical.Alignment = StringAlignment.Near;
                    _stringFormatTabVertical.LineAlignment = StringAlignment.Center;
                    _stringFormatTabVertical.FormatFlags =
                        StringFormatFlags.NoWrap | StringFormatFlags.DirectionVertical;
                    _stringFormatTabVertical.Trimming = StringTrimming.None;
                }

                if (RightToLeft == RightToLeft.Yes)
                    _stringFormatTabVertical.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                else
                    _stringFormatTabVertical.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

                return _stringFormatTabVertical;
            }
        }

        #endregion
    }
}