using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    [ToolboxItem(false)]
    internal class VS2012DockPaneStrip : DockPaneStripBase
    {
        private const int TAB_CLOSE_BUTTON_WIDTH = 30;

        private bool m_isMouseDown;

        public VS2012DockPaneStrip(DockPane pane)
            : base(pane)
        {
            SetStyle(ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            SuspendLayout();

            Components = new Container();
            m_toolTip = new ToolTip(Components);
            SelectMenu = new ContextMenuStrip(Components);
            pane.DockPanel.Theme.ApplyTo(SelectMenu);

            ResumeLayout();
        }

        protected bool IsMouseDown
        {
            get => m_isMouseDown;
            private set
            {
                if (m_isMouseDown == value)
                    return;

                m_isMouseDown = value;
                Invalidate();
            }
        }

        private Rectangle ActiveClose { get; set; }

        protected internal override Tab CreateTab(IDockContent content)
        {
            return new TabVS2012(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Components.Dispose();
                if (m_boldFont != null)
                {
                    m_boldFont.Dispose();
                    m_boldFont = null;
                }
            }

            base.Dispose(disposing);
        }

        protected internal override int MeasureHeight()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return MeasureHeight_ToolWindow();
            return MeasureHeight_Document();
        }

        private int MeasureHeight_ToolWindow()
        {
            if (DockPane.IsAutoHide || Tabs.Count <= 1)
                return 0;

            var height = Math.Max(TextFont.Height + (PatchController.EnableHighDpi == true ? DocumentIconGapBottom : 0),
                             ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom)
                         + ToolWindowStripGapTop + ToolWindowStripGapBottom;

            return height;
        }

        private int MeasureHeight_Document()
        {
            var height =
                Math.Max(
                    TextFont.Height + DocumentTabGapTop +
                    (PatchController.EnableHighDpi == true ? DocumentIconGapBottom : 0),
                    ButtonOverflow.Height + DocumentButtonGapTop + DocumentButtonGapBottom)
                + DocumentStripGapBottom + DocumentStripGapTop;

            return height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            CalculateTabs();
            if (Appearance == DockPane.AppearanceStyle.Document && DockPane.ActiveContent != null)
                if (EnsureDocumentTabVisible(DockPane.ActiveContent, false))
                    CalculateTabs();

            DrawTabStrip(e.Graphics);
        }

        protected override void OnRefreshChanges()
        {
            SetInertButtons();
            Invalidate();
        }

        public override GraphicsPath GetOutline(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.Document)
                return GetOutline_Document(index);
            return GetOutline_ToolWindow(index);
        }

        private GraphicsPath GetOutline_Document(int index)
        {
            var rectTab = Tabs[index].Rectangle.Value;
            rectTab.X -= rectTab.Height / 2;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            var path = new GraphicsPath();
            var pathTab = GetTabOutline_Document(Tabs[index], true, true, true);
            path.AddPath(pathTab, true);

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
            {
                path.AddLine(rectTab.Right, rectTab.Top, rectPaneClient.Right, rectTab.Top);
                path.AddLine(rectPaneClient.Right, rectTab.Top, rectPaneClient.Right, rectPaneClient.Top);
                path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Left, rectPaneClient.Top);
                path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Left, rectTab.Top);
                path.AddLine(rectPaneClient.Left, rectTab.Top, rectTab.Right, rectTab.Top);
            }
            else
            {
                path.AddLine(rectTab.Right, rectTab.Bottom, rectPaneClient.Right, rectTab.Bottom);
                path.AddLine(rectPaneClient.Right, rectTab.Bottom, rectPaneClient.Right, rectPaneClient.Bottom);
                path.AddLine(rectPaneClient.Right, rectPaneClient.Bottom, rectPaneClient.Left, rectPaneClient.Bottom);
                path.AddLine(rectPaneClient.Left, rectPaneClient.Bottom, rectPaneClient.Left, rectTab.Bottom);
                path.AddLine(rectPaneClient.Left, rectTab.Bottom, rectTab.Right, rectTab.Bottom);
            }

            return path;
        }

        private GraphicsPath GetOutline_ToolWindow(int index)
        {
            var rectTab = Tabs[index].Rectangle.Value;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            var path = new GraphicsPath();
            var pathTab = GetTabOutline(Tabs[index], true, true);
            path.AddPath(pathTab, true);
            path.AddLine(rectTab.Left, rectTab.Top, rectPaneClient.Left, rectTab.Top);
            path.AddLine(rectPaneClient.Left, rectTab.Top, rectPaneClient.Left, rectPaneClient.Top);
            path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Right, rectPaneClient.Top);
            path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Right, rectTab.Top);
            path.AddLine(rectPaneClient.Right, rectTab.Top, rectTab.Right, rectTab.Top);
            return path;
        }

        private void CalculateTabs()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                CalculateTabs_ToolWindow();
            else
                CalculateTabs_Document();
        }

        private void CalculateTabs_ToolWindow()
        {
            if (Tabs.Count <= 1 || DockPane.IsAutoHide)
                return;

            var rectTabStrip = TabStripRectangle;

            // Calculate tab widths
            var countTabs = Tabs.Count;
            foreach (TabVS2012 tab in Tabs)
            {
                tab.MaxWidth = GetMaxTabWidth(Tabs.IndexOf(tab));
                tab.Flag = false;
            }

            // Set tab whose max width less than average width
            var anyWidthWithinAverage = true;
            var totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight;
            var totalAllocatedWidth = 0;
            var averageWidth = totalWidth / countTabs;
            var remainedTabs = countTabs;
            for (anyWidthWithinAverage = true; anyWidthWithinAverage && remainedTabs > 0;)
            {
                anyWidthWithinAverage = false;
                foreach (TabVS2012 tab in Tabs)
                {
                    if (tab.Flag)
                        continue;

                    if (tab.MaxWidth <= averageWidth)
                    {
                        tab.Flag = true;
                        tab.TabWidth = tab.MaxWidth;
                        totalAllocatedWidth += tab.TabWidth;
                        anyWidthWithinAverage = true;
                        remainedTabs--;
                    }
                }

                if (remainedTabs != 0)
                    averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs;
            }

            // If any tab width not set yet, set it to the average width
            if (remainedTabs > 0)
            {
                var roundUpWidth = totalWidth - totalAllocatedWidth - averageWidth * remainedTabs;
                foreach (TabVS2012 tab in Tabs)
                {
                    if (tab.Flag)
                        continue;

                    tab.Flag = true;
                    if (roundUpWidth > 0)
                    {
                        tab.TabWidth = averageWidth + 1;
                        roundUpWidth--;
                    }
                    else
                    {
                        tab.TabWidth = averageWidth;
                    }
                }
            }

            // Set the X position of the tabs
            var x = rectTabStrip.X + ToolWindowStripGapLeft;
            foreach (TabVS2012 tab in Tabs)
            {
                tab.TabX = x;
                x += tab.TabWidth;
            }
        }

        private bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
        {
            var overflow = false;

            var tab = Tabs[index] as TabVS2012;
            tab.MaxWidth = GetMaxTabWidth(index);
            var width = Math.Min(tab.MaxWidth, DocumentTabMaxWidth);
            if (x + width < rectTabStrip.Right || index == StartDisplayingTab)
            {
                tab.TabX = x;
                tab.TabWidth = width;
                EndDisplayingTab = index;
            }
            else
            {
                tab.TabX = 0;
                tab.TabWidth = 0;
                overflow = true;
            }

            x += width;

            return overflow;
        }

        /// <summary>
        ///     Calculate which tabs are displayed and in what order.
        /// </summary>
        private void CalculateTabs_Document()
        {
            if (m_startDisplayingTab >= Tabs.Count)
                m_startDisplayingTab = 0;

            var rectTabStrip = TabsRectangle;

            var x = rectTabStrip.X; //+ rectTabStrip.Height / 2;
            var overflow = false;

            // Originally all new documents that were considered overflow
            // (not enough pane strip space to show all tabs) were added to
            // the far left (assuming not right to left) and the tabs on the
            // right were dropped from view. If StartDisplayingTab is not 0
            // then we are dealing with making sure a specific tab is kept in focus.
            if (m_startDisplayingTab > 0)
            {
                var tempX = x;
                var tab = Tabs[m_startDisplayingTab] as TabVS2012;
                tab.MaxWidth = GetMaxTabWidth(m_startDisplayingTab);

                // Add the active tab and tabs to the left
                for (var i = StartDisplayingTab; i >= 0; i--)
                    CalculateDocumentTab(rectTabStrip, ref tempX, i);

                // Store which tab is the first one displayed so that it
                // will be drawn correctly (without part of the tab cut off)
                FirstDisplayingTab = EndDisplayingTab;

                tempX = x; // Reset X location because we are starting over

                // Start with the first tab displayed - name is a little misleading.
                // Loop through each tab and set its location. If there is not enough
                // room for all of them overflow will be returned.
                for (var i = EndDisplayingTab; i < Tabs.Count; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref tempX, i);

                // If not all tabs are shown then we have an overflow.
                if (FirstDisplayingTab != 0)
                    overflow = true;
            }
            else
            {
                for (var i = StartDisplayingTab; i < Tabs.Count; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);
                for (var i = 0; i < StartDisplayingTab; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);

                FirstDisplayingTab = StartDisplayingTab;
            }

            if (!overflow)
            {
                m_startDisplayingTab = 0;
                FirstDisplayingTab = 0;
                x = rectTabStrip.X; // +rectTabStrip.Height / 2;
                foreach (TabVS2012 tab in Tabs)
                {
                    tab.TabX = x;
                    x += tab.TabWidth;
                }
            }

            DocumentTabsOverflow = overflow;
        }

        protected internal override void EnsureTabVisible(IDockContent content)
        {
            if (Appearance != DockPane.AppearanceStyle.Document || !Tabs.Contains(content))
                return;

            CalculateTabs();
            EnsureDocumentTabVisible(content, true);
        }

        private bool EnsureDocumentTabVisible(IDockContent content, bool repaint)
        {
            var index = Tabs.IndexOf(content);
            if (index == -1) // TODO: should prevent it from being -1;
                return false;

            var tab = Tabs[index] as TabVS2012;
            if (tab.TabWidth != 0)
                return false;

            StartDisplayingTab = index;
            if (repaint)
                Invalidate();

            return true;
        }

        private int GetMaxTabWidth(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetMaxTabWidth_ToolWindow(index);
            return GetMaxTabWidth_Document(index);
        }

        private int GetMaxTabWidth_ToolWindow(int index)
        {
            var content = Tabs[index].Content;
            var sizeString = TextRenderer.MeasureText(content.DockHandler.TabText, TextFont);
            return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft
                   + ToolWindowImageGapRight + ToolWindowTextGapRight;
        }

        private int GetMaxTabWidth_Document(int index)
        {
            var content = Tabs[index].Content;
            var height = GetTabRectangle_Document(index).Height;
            var sizeText = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont,
                new Size(DocumentTabMaxWidth, height), DocumentTextFormat);

            int width;
            if (DockPane.DockPanel.ShowDocumentIcon)
                width = sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight +
                        DocumentTextGapRight;
            else
                width = sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight;

            width += TAB_CLOSE_BUTTON_WIDTH;
            return width;
        }

        private void DrawTabStrip(Graphics g)
        {
            // IMPORTANT: fill background.
            var rectTabStrip = TabStripRectangle;
            g.FillRectangle(
                DockPane.DockPanel.Theme.PaintingService.GetBrush(DockPane.DockPanel.Theme.ColorPalette.MainWindowActive
                    .Background), rectTabStrip);

            if (Appearance == DockPane.AppearanceStyle.Document)
                DrawTabStrip_Document(g);
            else
                DrawTabStrip_ToolWindow(g);
        }

        private void DrawTabStrip_Document(Graphics g)
        {
            var count = Tabs.Count;
            if (count == 0)
                return;

            var rectTabStrip = new Rectangle(TabStripRectangle.Location, TabStripRectangle.Size);
            rectTabStrip.Height += 1;

            // Draw the tabs
            var rectTabOnly = TabsRectangle;
            var rectTab = Rectangle.Empty;
            TabVS2012 tabActive = null;
            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            for (var i = 0; i < count; i++)
            {
                rectTab = GetTabRectangle(i);
                if (Tabs[i].Content == DockPane.ActiveContent)
                {
                    tabActive = Tabs[i] as TabVS2012;
                    tabActive.Rectangle = rectTab;
                    continue;
                }

                if (rectTab.IntersectsWith(rectTabOnly))
                {
                    var tab = Tabs[i] as TabVS2012;
                    tab.Rectangle = rectTab;
                    DrawTab(g, tab, false);
                }
            }

            g.SetClip(rectTabStrip);

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
            {
            }
            else
            {
                Color tabUnderLineColor;
                if (tabActive != null && DockPane.IsActiveDocumentPane)
                    tabUnderLineColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Background;
                else
                    tabUnderLineColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background;

                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(tabUnderLineColor, 4), rectTabStrip.Left,
                    rectTabStrip.Bottom, rectTabStrip.Right, rectTabStrip.Bottom);
            }

            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            if (tabActive != null)
            {
                rectTab = tabActive.Rectangle.Value;
                if (rectTab.IntersectsWith(rectTabOnly))
                {
                    rectTab.Intersect(rectTabOnly);
                    tabActive.Rectangle = rectTab;
                    DrawTab(g, tabActive, false);
                }
            }
        }

        private void DrawTabStrip_ToolWindow(Graphics g)
        {
            for (var i = 0; i < Tabs.Count; i++)
            {
                var tab = Tabs[i] as TabVS2012;
                tab.Rectangle = GetTabRectangle(i);
                DrawTab(g, tab, i == Tabs.Count - 1);
            }
        }

        private Rectangle GetTabRectangle(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetTabRectangle_ToolWindow(index);
            return GetTabRectangle_Document(index);
        }

        private Rectangle GetTabRectangle_ToolWindow(int index)
        {
            var rectTabStrip = TabStripRectangle;

            var tab = (TabVS2012) Tabs[index];
            return new Rectangle(tab.TabX, rectTabStrip.Y, tab.TabWidth, rectTabStrip.Height);
        }

        private Rectangle GetTabRectangle_Document(int index)
        {
            var rectTabStrip = TabStripRectangle;
            var tab = (TabVS2012) Tabs[index];

            var rect = new Rectangle();
            rect.X = tab.TabX;
            rect.Width = tab.TabWidth;
            rect.Height = rectTabStrip.Height - DocumentTabGapTop;

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                rect.Y = rectTabStrip.Y + DocumentStripGapBottom;
            else
                rect.Y = rectTabStrip.Y + DocumentTabGapTop;

            return rect;
        }

        private void DrawTab(Graphics g, TabVS2012 tab, bool last)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                DrawTab_ToolWindow(g, tab, last);
            else
                DrawTab_Document(g, tab);
        }

        private GraphicsPath GetTabOutline(Tab tab, bool rtlTransform, bool toScreen)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetTabOutline_ToolWindow(tab, rtlTransform, toScreen);
            return GetTabOutline_Document(tab, rtlTransform, toScreen, false);
        }

        private GraphicsPath GetTabOutline_ToolWindow(Tab tab, bool rtlTransform, bool toScreen)
        {
            var rect = GetTabRectangle(Tabs.IndexOf(tab));
            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            DrawHelper.GetRoundedCornerTab(GraphicsPath, rect, false);
            return GraphicsPath;
        }

        private GraphicsPath GetTabOutline_Document(Tab tab, bool rtlTransform, bool toScreen, bool full)
        {
            GraphicsPath.Reset();
            var rect = GetTabRectangle(Tabs.IndexOf(tab));

            // Shorten TabOutline so it doesn't get overdrawn by icons next to it
            rect.Intersect(TabsRectangle);
            rect.Width--;

            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            GraphicsPath.AddRectangle(rect);
            return GraphicsPath;
        }

        private void DrawTab_ToolWindow(Graphics g, TabVS2012 tab, bool last)
        {
            var rect = tab.Rectangle.Value;
            rect.Y += 1;
            var rectIcon = new Rectangle(
                rect.X + ToolWindowImageGapLeft,
                rect.Y - 1 + rect.Height - ToolWindowImageGapBottom - ToolWindowImageHeight,
                ToolWindowImageWidth, ToolWindowImageHeight);
            var rectText = PatchController.EnableHighDpi == true
                ? new Rectangle(
                    rect.X + ToolWindowImageGapLeft,
                    rect.Y - 1 + rect.Height - ToolWindowImageGapBottom - TextFont.Height,
                    ToolWindowImageWidth, TextFont.Height)
                : rectIcon;
            rectText.X += rectIcon.Width + ToolWindowImageGapRight;
            rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft -
                             ToolWindowImageGapRight - ToolWindowTextGapRight;

            var rectTab = DrawHelper.RtlTransform(this, rect);
            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);
            var separatorColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowSeparator;
            if (DockPane.ActiveContent == tab.Content)
            {
                Color textColor;
                Color backgroundColor;
                if (DockPane.IsActiveDocumentPane)
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedActive.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedActive.Background;
                }
                else
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedInactive.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedInactive.Background;
                }

                g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rect);
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor,
                    ToolWindowTextFormat);
                // TODO: how to cache Pen?
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(separatorColor), rect.X + rect.Width - 1,
                    rect.Y, rect.X + rect.Width - 1, rect.Height);
            }
            else
            {
                Color textColor;
                Color backgroundColor;
                if (tab.Content == DockPane.MouseOverTab)
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Background;
                }
                else
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselected.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background;
                }

                g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rect);
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor,
                    ToolWindowTextFormat);
                if (!last)
                    g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(separatorColor), rect.X + rect.Width - 1,
                        rect.Y, rect.X + rect.Width - 1, rect.Height);
            }

            if (rectTab.Contains(rectIcon))
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        private void DrawTab_Document(Graphics g, TabVS2012 tab)
        {
            var rect = tab.Rectangle.Value;
            rect = DrawHelper.RtlTransform(this, rect);

            if (tab.TabWidth == 0)
                return;

            var rectCloseButton = GetCloseButtonRect(rect);

            var rectIcon = new Rectangle(
                rect.X + DocumentIconGapLeft - 6,
                rect.Y + rect.Height - DocumentIconGapBottom - DocumentIconHeight,
                DocumentIconWidth, DocumentIconHeight);


            var rectText = PatchController.EnableHighDpi == true
                ? new Rectangle(
                    rect.X + DocumentIconGapLeft,
                    rect.Y + rect.Height - DocumentIconGapBottom - TextFont.Height,
                    DocumentIconWidth, TextFont.Height)
                : rectIcon;
            if (DockPane.DockPanel.ShowDocumentIcon)
            {
                rectText.X += rectIcon.Width + DocumentIconGapRight;
                rectText.Y = rect.Y;
                rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft - DocumentIconGapRight -
                                 DocumentTextGapRight - rectCloseButton.Width;
                rectText.Height = rect.Height;
            }
            else
            {
                rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight - rectCloseButton.Width;
            }

            var rectTab = rect; // DrawHelper.RtlTransform(this, rect);
            var rectBack = rect; // DrawHelper.RtlTransform(this, rect);
            rectBack.Width += DocumentIconGapLeft;
            rectBack.X -= DocumentIconGapLeft;


            var activeColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Background;
            var lostFocusColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background;
            var inactiveColor = DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background;
            var mouseHoverColor = DockPane.DockPanel.Theme.ColorPalette.TabUnselectedHovered.Background;

            var activeText = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Text;
            var lostFocusText = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Text;
            var inactiveText = DockPane.DockPanel.Theme.ColorPalette.TabUnselected.Text;
            var mouseHoverText = DockPane.DockPanel.Theme.ColorPalette.TabUnselectedHovered.Text;

            Color text;
            Image image = null;
            Color paint;
            var imageService = DockPane.DockPanel.Theme.ImageService;
            if (DockPane.ActiveContent == tab.Content)
            {
                if (DockPane.IsActiveDocumentPane)
                {
                    paint = activeColor;
                    text = activeText;
                    image = IsMouseDown
                        ? imageService.TabPressActive_Close
                        : rectCloseButton == ActiveClose
                            ? imageService.TabHoverActive_Close
                            : imageService.TabActive_Close;
                }
                else
                {
                    paint = lostFocusColor;
                    text = lostFocusText;
                    image = IsMouseDown
                        ? imageService.TabPressLostFocus_Close
                        : rectCloseButton == ActiveClose
                            ? imageService.TabHoverLostFocus_Close
                            : imageService.TabLostFocus_Close;
                }
            }
            else
            {
                if (tab.Content == DockPane.MouseOverTab)
                {
                    paint = mouseHoverColor;
                    text = mouseHoverText;
                    image = IsMouseDown
                        ? imageService.TabPressInactive_Close
                        : rectCloseButton == ActiveClose
                            ? imageService.TabHoverInactive_Close
                            : imageService.TabInactive_Close;
                }
                else
                {
                    paint = inactiveColor;
                    text = inactiveText;
                    image = imageService.TabHoverLostFocus_Close;
                }
            }

            g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(paint), rect);
            TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, text, DocumentTextFormat);
            if (image != null)
                g.DrawImage(image, rectCloseButton);

            if (rectTab.Contains(rectIcon) && DockPane.DockPanel.ShowDocumentIcon)
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseDown)
                IsMouseDown = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            // suspend drag if mouse is down on active close button.
            m_suspendDrag = ActiveCloseHitTest(e.Location);
            if (!IsMouseDown)
                IsMouseDown = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!m_suspendDrag)
                base.OnMouseMove(e);

            var index = HitTest(PointToClient(MousePosition));
            var toolTip = string.Empty;

            var tabUpdate = false;
            var buttonUpdate = false;
            if (index != -1)
            {
                var tab = Tabs[index] as TabVS2012;
                if (Appearance == DockPane.AppearanceStyle.ToolWindow || Appearance == DockPane.AppearanceStyle.Document
                ) tabUpdate = SetMouseOverTab(tab.Content == DockPane.ActiveContent ? null : tab.Content);

                if (!string.IsNullOrEmpty(tab.Content.DockHandler.ToolTipText))
                    toolTip = tab.Content.DockHandler.ToolTipText;
                else if (tab.MaxWidth > tab.TabWidth)
                    toolTip = tab.Content.DockHandler.TabText;

                var mousePos = PointToClient(MousePosition);
                var tabRect = DrawHelper.RtlTransform(this, tab.Rectangle.Value);
                var closeButtonRect = GetCloseButtonRect(tabRect);
                var mouseRect = new Rectangle(mousePos, new Size(1, 1));
                buttonUpdate =
                    SetActiveClose(closeButtonRect.IntersectsWith(mouseRect) ? closeButtonRect : Rectangle.Empty);
            }
            else
            {
                tabUpdate = SetMouseOverTab(null);
                buttonUpdate = SetActiveClose(Rectangle.Empty);
            }

            if (tabUpdate || buttonUpdate)
                Invalidate();

            if (m_toolTip.GetToolTip(this) != toolTip)
            {
                m_toolTip.Active = false;
                m_toolTip.SetToolTip(this, toolTip);
                m_toolTip.Active = true;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button != MouseButtons.Left || Appearance != DockPane.AppearanceStyle.Document)
                return;

            var indexHit = HitTest();
            if (indexHit > -1)
                TabCloseButtonHit(indexHit);
        }

        private void TabCloseButtonHit(int index)
        {
            var mousePos = PointToClient(MousePosition);
            var tabRect = GetTabBounds(Tabs[index]);
            if (tabRect.Contains(ActiveClose) && ActiveCloseHitTest(mousePos))
                TryCloseTab(index);
        }

        private Rectangle GetCloseButtonRect(Rectangle rectTab)
        {
            if (Appearance != DockPane.AppearanceStyle.Document) return Rectangle.Empty;

            const int gap = 3;
            var imageSize = PatchController.EnableHighDpi == true ? rectTab.Height - gap * 2 : 15;
            return new Rectangle(rectTab.X + rectTab.Width - imageSize - gap - 1, rectTab.Y + gap, imageSize,
                imageSize);
            
        }

        private void WindowList_Click(object sender, EventArgs e)
        {
            SelectMenu.ShowCheckMargin = true;
            SelectMenu.Items.Clear();
            foreach (TabVS2012 tab in Tabs)
            {
                var content = tab.Content;

                var item = SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap());
                item.Tag = tab.Content;

                item.Click += ContextMenuItem_Click;
                item.MouseMove += ContextMenuItem_MuseMove;
                if (DockPane.ActiveContent == content) item.BackColor = Color.SkyBlue;
            }

            ShowDropDown();
        }

        private void ShowDropDown()
        {
            try
            {
                var workingArea =
                    Screen.GetWorkingArea(ButtonWindowList.PointToScreen(new Point(ButtonWindowList.Width / 2,
                        ButtonWindowList.Height / 2)));
                var menu = new Rectangle(
                    ButtonWindowList.PointToScreen(new Point(0, ButtonWindowList.Location.Y + ButtonWindowList.Height)),
                    SelectMenu.Size);
                var menuMargined = new Rectangle(menu.X - SelectMenuMargin, menu.Y - SelectMenuMargin,
                    menu.Width + SelectMenuMargin, menu.Height + SelectMenuMargin);
                if (workingArea.Contains(menuMargined))
                {
                    SelectMenu.Show(menu.Location);
                }
                else
                {
                    var newPoint = menu.Location;
                    newPoint.X = DrawHelper.Balance(SelectMenu.Width, SelectMenuMargin, newPoint.X, workingArea.Left,
                        workingArea.Right);
                    newPoint.Y = DrawHelper.Balance(SelectMenu.Size.Height, SelectMenuMargin, newPoint.Y,
                        workingArea.Top, workingArea.Bottom);
                    var button = ButtonWindowList.PointToScreen(new Point(0, ButtonWindowList.Height));
                    if (newPoint.Y < button.Y)
                    {
                        // flip the menu up to be above the button.
                        newPoint.Y = button.Y - ButtonWindowList.Height;
                        SelectMenu.Show(newPoint, ToolStripDropDownDirection.AboveRight);
                    }
                    else
                    {
                        SelectMenu.Show(newPoint);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private bool IsCloseButtonMenu(ToolStripMenuItem item)
        {
            var contentRect = item.Bounds;

            var ButtonRec =
                new Rectangle(contentRect.X + contentRect.Width - 25, contentRect.Y, 14, 14);
            var MuoseRec =
                new Rectangle(contentRect.X + contentRect.Width - 25, contentRect.Y + contentRect.Height, 14, 14);
            if (item.Bounds.Contains(ButtonRec) &&
                MuoseRec.Contains(new Rectangle(PointToClient(MousePosition), new Size(1, 1)))) return true;

            return false;
        }

        private void ContextMenuItem_MuseMove(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                var imageService = DockPane.DockPanel.Theme.ImageService;

                VisualStudioToolStripRenderer.CloseButtom = IsCloseButtonMenu(item)
                    ? imageService.TabLostFocus_Close
                    : imageService.TabHoverLostFocus_Close;
            }
        }

        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                var content = (IDockContent) item.Tag;

                if (IsCloseButtonMenu(item))
                {
                    var Tab = Tabs.FirstOrDefault(u => u.Content == content);
                    var index = Tabs.IndexOf(Tab);

                    if (TryCloseTab(index))
                        SelectMenu.Items.Remove(item);
                    ShowDropDown();
                }
                else
                {
                    DockPane.ActiveContent = content;
                    SelectMenu.Close();
                }
            }
        }

        private void SetInertButtons()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                if (m_buttonOverflow != null)
                    m_buttonOverflow.Left = -m_buttonOverflow.Width;

                if (m_buttonWindowList != null)
                    m_buttonWindowList.Left = -m_buttonWindowList.Width;
            }
            else
            {
                ButtonOverflow.Visible = m_documentTabsOverflow;
                ButtonOverflow.RefreshChanges();

                ButtonWindowList.Visible = !m_documentTabsOverflow;
                ButtonWindowList.RefreshChanges();
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (Appearance == DockPane.AppearanceStyle.Document)
            {
                LayoutButtons();
                OnRefreshChanges();
            }

            base.OnLayout(levent);
        }

        private void LayoutButtons()
        {
            var rectTabStrip = TabStripRectangle;

            // Set position and size of the buttons
            var buttonWidth = ButtonOverflow.Image.Width;
            var buttonHeight = ButtonOverflow.Image.Height;
            var height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom;
            if (buttonHeight < height)
            {
                buttonWidth = buttonWidth * height / buttonHeight;
                buttonHeight = height;
            }

            var buttonSize = new Size(buttonWidth, buttonHeight);

            var x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft
                                                        - DocumentButtonGapRight - buttonWidth;
            var y = rectTabStrip.Y + DocumentButtonGapTop;
            var point = new Point(x, y);
            ButtonOverflow.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

            // If the close button is not visible draw the window list button overtop.
            // Otherwise it is drawn to the left of the close button.
            ButtonWindowList.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
        }

        private void Close_Click(object sender, EventArgs e)
        {
            DockPane.CloseActiveContent();
            if (PatchController.EnableMemoryLeakFix == true) ContentClosed();
        }

        protected internal override int HitTest(Point point)
        {
            //if (!TabsRectangle.Contains(point))
            //    return -1;

            foreach (var tab in Tabs)
            {
                var path = GetTabOutline(tab, true, false);
                if (path.IsVisible(point))
                    return Tabs.IndexOf(tab);
            }

            return -1;
        }

        protected override bool MouseDownActivateTest(MouseEventArgs e)
        {
            var result = base.MouseDownActivateTest(e);
            if (result && e.Button == MouseButtons.Left && Appearance == DockPane.AppearanceStyle.Document)
                // don't activate if mouse is down on active close button
                result = !ActiveCloseHitTest(e.Location);
            return result;
        }

        private bool ActiveCloseHitTest(Point ptMouse)
        {
            var result = false;
            if (!ActiveClose.IsEmpty)
            {
                var mouseRect = new Rectangle(ptMouse, new Size(1, 1));
                result = ActiveClose.IntersectsWith(mouseRect);
            }

            return result;
        }

        protected override Rectangle GetTabBounds(Tab tab)
        {
            var path = GetTabOutline(tab, true, false);
            var rectangle = path.GetBounds();
            return new Rectangle((int) rectangle.Left, (int) rectangle.Top, (int) rectangle.Width,
                (int) rectangle.Height);
        }

        private bool SetActiveClose(Rectangle rectangle)
        {
            if (ActiveClose == rectangle)
                return false;

            ActiveClose = rectangle;
            return true;
        }

        private bool SetMouseOverTab(IDockContent content)
        {
            if (DockPane.MouseOverTab == content)
                return false;

            DockPane.MouseOverTab = content;
            return true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            var tabUpdate = SetMouseOverTab(null);
            var buttonUpdate = SetActiveClose(Rectangle.Empty);
            if (tabUpdate || buttonUpdate)
                Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            PerformLayout();
        }

        private class TabVS2012 : Tab
        {
            public TabVS2012(IDockContent content)
                : base(content)
            {
            }

            public int TabX { get; set; }

            public int TabWidth { get; set; }

            public int MaxWidth { get; set; }

            protected internal bool Flag { get; set; }
        }

        [ToolboxItem(false)]
        private sealed class InertButton : InertButtonBase
        {
            public InertButton(Bitmap hovered, Bitmap normal, Bitmap pressed)
            {
                HoverImage = hovered;
                Image = normal;
                PressImage = pressed;
            }

            public override Bitmap Image { get; }

            public override Bitmap HoverImage { get; }

            public override Bitmap PressImage { get; }
        }

        #region Constants

        private const int _ToolWindowStripGapTop = 0;
        private const int _ToolWindowStripGapBottom = 0;
        private const int _ToolWindowStripGapLeft = 0;
        private const int _ToolWindowStripGapRight = 0;
        private const int _ToolWindowImageHeight = 16;
        private const int _ToolWindowImageWidth = 0; //16;
        private const int _ToolWindowImageGapTop = 3;
        private const int _ToolWindowImageGapBottom = 1;
        private const int _ToolWindowImageGapLeft = 2;
        private const int _ToolWindowImageGapRight = 0;
        private const int _ToolWindowTextGapRight = 3;
        private const int _ToolWindowTabSeperatorGapTop = 3;
        private const int _ToolWindowTabSeperatorGapBottom = 3;

        private const int _DocumentStripGapTop = 0;
        private const int _DocumentStripGapBottom = 1;
        private const int _DocumentTabMaxWidth = 400;
        private const int _DocumentButtonGapTop = 3;
        private const int _DocumentButtonGapBottom = 3;
        private const int _DocumentButtonGapBetween = 0;
        private const int _DocumentButtonGapRight = 3;
        private const int _DocumentTabGapTop = 0; //3;
        private const int _DocumentTabGapLeft = 0; //3;
        private const int _DocumentTabGapRight = 0; //3;
        private const int _DocumentIconGapBottom = 2; //2;
        private const int _DocumentIconGapLeft = 13;
        private const int _DocumentIconGapRight = 0;
        private const int _DocumentIconHeight = 16;
        private const int _DocumentIconWidth = 16;
        private const int _DocumentTextGapRight = 6;

        #endregion

        #region Members

        private InertButton m_buttonOverflow;
        private InertButton m_buttonWindowList;
        private ToolTip m_toolTip;
        private Font m_font;
        private Font m_boldFont;
        private int m_startDisplayingTab;
        private bool m_documentTabsOverflow;
        private static string m_toolTipSelect;
        private bool m_suspendDrag;

        #endregion

        #region Properties

        private Rectangle TabStripRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.Document)
                    return TabStripRectangle_Document;
                return TabStripRectangle_ToolWindow;
            }
        }

        private Rectangle TabStripRectangle_ToolWindow
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + ToolWindowStripGapTop, rect.Width,
                    rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom);
            }
        }

        private Rectangle TabStripRectangle_Document
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + DocumentStripGapTop, rect.Width,
                    rect.Height + DocumentStripGapTop - DocumentStripGapBottom);
            }
        }

        private Rectangle TabsRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                    return TabStripRectangle;

                var rectWindow = TabStripRectangle;
                var x = rectWindow.X;
                var y = rectWindow.Y;
                var width = rectWindow.Width;
                var height = rectWindow.Height;

                x += DocumentTabGapLeft;
                width -= DocumentTabGapLeft +
                         DocumentTabGapRight +
                         DocumentButtonGapRight +
                         ButtonOverflow.Width +
                         ButtonWindowList.Width +
                         2 * DocumentButtonGapBetween;

                return new Rectangle(x, y, width, height);
            }
        }

        private ContextMenuStrip SelectMenu { get; }

        public int SelectMenuMargin { get; set; } = 5;

        private InertButton ButtonOverflow
        {
            get
            {
                if (m_buttonOverflow == null)
                {
                    m_buttonOverflow = new InertButton(
                        DockPane.DockPanel.Theme.ImageService.DockPaneHover_OptionOverflow,
                        DockPane.DockPanel.Theme.ImageService.DockPane_OptionOverflow,
                        DockPane.DockPanel.Theme.ImageService.DockPanePress_OptionOverflow);
                    m_buttonOverflow.Click += WindowList_Click;
                    Controls.Add(m_buttonOverflow);
                }

                return m_buttonOverflow;
            }
        }

        private InertButton ButtonWindowList
        {
            get
            {
                if (m_buttonWindowList == null)
                {
                    m_buttonWindowList = new InertButton(
                        DockPane.DockPanel.Theme.ImageService.DockPaneHover_List,
                        DockPane.DockPanel.Theme.ImageService.DockPane_List,
                        DockPane.DockPanel.Theme.ImageService.DockPanePress_List);
                    m_buttonWindowList.Click += WindowList_Click;
                    Controls.Add(m_buttonWindowList);
                }

                return m_buttonWindowList;
            }
        }

        private static GraphicsPath GraphicsPath => VS2012AutoHideStrip.GraphicsPath;

        private IContainer Components { get; }

        public Font TextFont => DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont;

        private Font BoldFont
        {
            get
            {
                if (IsDisposed)
                    return null;

                if (m_boldFont == null)
                {
                    m_font = TextFont;
                    m_boldFont = new Font(TextFont, FontStyle.Bold);
                }
                else if (m_font != TextFont)
                {
                    m_boldFont.Dispose();
                    m_font = TextFont;
                    m_boldFont = new Font(TextFont, FontStyle.Bold);
                }

                return m_boldFont;
            }
        }

        private int StartDisplayingTab
        {
            get => m_startDisplayingTab;
            set
            {
                m_startDisplayingTab = value;
                Invalidate();
            }
        }

        private int EndDisplayingTab { get; set; }

        private int FirstDisplayingTab { get; set; }

        private bool DocumentTabsOverflow
        {
            set
            {
                if (m_documentTabsOverflow == value)
                    return;

                m_documentTabsOverflow = value;
                SetInertButtons();
            }
        }

        #region Customizable Properties

        private static int ToolWindowStripGapTop => _ToolWindowStripGapTop;

        private static int ToolWindowStripGapBottom => _ToolWindowStripGapBottom;

        private static int ToolWindowStripGapLeft => _ToolWindowStripGapLeft;

        private static int ToolWindowStripGapRight => _ToolWindowStripGapRight;

        private static int ToolWindowImageHeight => _ToolWindowImageHeight;

        private static int ToolWindowImageWidth => _ToolWindowImageWidth;

        private static int ToolWindowImageGapTop => _ToolWindowImageGapTop;

        private static int ToolWindowImageGapBottom => _ToolWindowImageGapBottom;

        private static int ToolWindowImageGapLeft => _ToolWindowImageGapLeft;

        private static int ToolWindowImageGapRight => _ToolWindowImageGapRight;

        private static int ToolWindowTextGapRight => _ToolWindowTextGapRight;

        private static int ToolWindowTabSeperatorGapTop => _ToolWindowTabSeperatorGapTop;

        private static int ToolWindowTabSeperatorGapBottom => _ToolWindowTabSeperatorGapBottom;

        private static string ToolTipSelect
        {
            get
            {
                if (m_toolTipSelect == null)
                    m_toolTipSelect = Strings.DockPaneStrip_ToolTipWindowList;
                return m_toolTipSelect;
            }
        }

        private TextFormatFlags ToolWindowTextFormat
        {
            get
            {
                var textFormat = TextFormatFlags.EndEllipsis |
                                 TextFormatFlags.HorizontalCenter |
                                 TextFormatFlags.SingleLine |
                                 TextFormatFlags.VerticalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                return textFormat;
            }
        }

        private static int DocumentStripGapTop => _DocumentStripGapTop;

        private static int DocumentStripGapBottom => _DocumentStripGapBottom;

        private TextFormatFlags DocumentTextFormat
        {
            get
            {
                var textFormat = TextFormatFlags.EndEllipsis |
                                 TextFormatFlags.SingleLine |
                                 TextFormatFlags.VerticalCenter |
                                 TextFormatFlags.HorizontalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft;
                return textFormat;
            }
        }

        private static int DocumentTabMaxWidth => _DocumentTabMaxWidth;

        private static int DocumentButtonGapTop => _DocumentButtonGapTop;

        private static int DocumentButtonGapBottom => _DocumentButtonGapBottom;

        private static int DocumentButtonGapBetween => _DocumentButtonGapBetween;

        private static int DocumentButtonGapRight => _DocumentButtonGapRight;

        private static int DocumentTabGapTop => _DocumentTabGapTop;

        private static int DocumentTabGapLeft => _DocumentTabGapLeft;

        private static int DocumentTabGapRight => _DocumentTabGapRight;

        private static int DocumentIconGapBottom => _DocumentIconGapBottom;

        private static int DocumentIconGapLeft => _DocumentIconGapLeft;

        private static int DocumentIconGapRight => _DocumentIconGapRight;

        private static int DocumentIconWidth => _DocumentIconWidth;

        private static int DocumentIconHeight => _DocumentIconHeight;

        private static int DocumentTextGapRight => _DocumentTextGapRight;

        #endregion

        #endregion
    }
}