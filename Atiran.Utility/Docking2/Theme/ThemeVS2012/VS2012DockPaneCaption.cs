using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    [ToolboxItem(false)]
    internal class VS2012DockPaneCaption : DockPaneCaptionBase
    {
        private static string _toolTipClose;

        private static string _toolTipOptions;

        private static string _toolTipAutoHide;

        private static TextFormatFlags _textFormat =
            TextFormatFlags.SingleLine |
            TextFormatFlags.EndEllipsis |
            TextFormatFlags.VerticalCenter;

        private InertButtonBase m_buttonAutoHide;

        private InertButtonBase m_buttonClose;

        private InertButtonBase m_buttonOptions;

        private ToolTip m_toolTip;

        public VS2012DockPaneCaption(DockPane pane) : base(pane)
        {
            SuspendLayout();

            Components = new Container();
            m_toolTip = new ToolTip(Components);

            ResumeLayout();
        }

        private InertButtonBase ButtonClose
        {
            get
            {
                if (m_buttonClose == null)
                {
                    m_buttonClose = new VS2012DockPaneCaptionInertButton(this,
                        DockPane.DockPanel.Theme.ImageService.DockPaneHover_Close,
                        DockPane.DockPanel.Theme.ImageService.DockPane_Close,
                        DockPane.DockPanel.Theme.ImageService.DockPanePress_Close,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Close,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActive_Close);
                    m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
                    m_buttonClose.Click += Close_Click;
                    Controls.Add(m_buttonClose);
                }

                return m_buttonClose;
            }
        }

        private InertButtonBase ButtonAutoHide
        {
            get
            {
                if (m_buttonAutoHide == null)
                {
                    m_buttonAutoHide = new VS2012DockPaneCaptionInertButton(this,
                        DockPane.DockPanel.Theme.ImageService.DockPaneHover_Dock,
                        DockPane.DockPanel.Theme.ImageService.DockPane_Dock,
                        DockPane.DockPanel.Theme.ImageService.DockPanePress_Dock,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Dock,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActive_Dock,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_AutoHide,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActive_AutoHide,
                        DockPane.DockPanel.Theme.ImageService.DockPanePress_AutoHide);
                    m_toolTip.SetToolTip(m_buttonAutoHide, ToolTipAutoHide);
                    m_buttonAutoHide.Click += AutoHide_Click;
                    Controls.Add(m_buttonAutoHide);
                }

                return m_buttonAutoHide;
            }
        }

        private InertButtonBase ButtonOptions
        {
            get
            {
                if (m_buttonOptions == null)
                {
                    m_buttonOptions = new VS2012DockPaneCaptionInertButton(this,
                        DockPane.DockPanel.Theme.ImageService.DockPaneHover_Option,
                        DockPane.DockPanel.Theme.ImageService.DockPane_Option,
                        DockPane.DockPanel.Theme.ImageService.DockPanePress_Option,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Option,
                        DockPane.DockPanel.Theme.ImageService.DockPaneActive_Option);
                    m_toolTip.SetToolTip(m_buttonOptions, ToolTipOptions);
                    m_buttonOptions.Click += Options_Click;
                    Controls.Add(m_buttonOptions);
                }

                return m_buttonOptions;
            }
        }

        private IContainer Components { get; }

        public Font TextFont => DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont;

        private static string ToolTipClose
        {
            get
            {
                if (_toolTipClose == null)
                    _toolTipClose = Strings.DockPaneCaption_ToolTipClose;
                return _toolTipClose;
            }
        }

        private static string ToolTipOptions
        {
            get
            {
                if (_toolTipOptions == null)
                    _toolTipOptions = Strings.DockPaneCaption_ToolTipOptions;

                return _toolTipOptions;
            }
        }

        private static string ToolTipAutoHide
        {
            get
            {
                if (_toolTipAutoHide == null)
                    _toolTipAutoHide = Strings.DockPaneCaption_ToolTipAutoHide;
                return _toolTipAutoHide;
            }
        }

        private Color TextColor
        {
            get
            {
                if (DockPane.IsActivePane)
                    return DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Text;
                return DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive.Text;
            }
        }

        private TextFormatFlags TextFormat
        {
            get
            {
                if (RightToLeft == RightToLeft.No)
                    return _textFormat;
                return _textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }
        }

        private bool CloseButtonEnabled =>
            DockPane.ActiveContent != null ? DockPane.ActiveContent.DockHandler.CloseButton : false;

        /// <summary>
        ///     Determines whether the close button is visible on the content
        /// </summary>
        private bool CloseButtonVisible => DockPane.ActiveContent != null
            ? DockPane.ActiveContent.DockHandler.CloseButtonVisible
            : false;

        private bool ShouldShowAutoHideButton => !DockPane.IsFloat;

        protected override bool CanDragAutoHide => true;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Components.Dispose();
            base.Dispose(disposing);
        }

        protected internal override int MeasureHeight()
        {
            var height = TextFont.Height + TextGapTop + TextGapBottom;

            if (height < ButtonClose.Image.Height + ButtonGapTop + ButtonGapBottom)
                height = ButtonClose.Image.Height + ButtonGapTop + ButtonGapBottom;

            return height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawCaption(e.Graphics);
        }

        private void DrawCaption(Graphics g)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
                return;

            var rect = ClientRectangle;
            Color captionColor;

            if (DockPane.IsActivePane)
                captionColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Background;
            else
                captionColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive.Background;

            var captionBrush = DockPane.DockPanel.Theme.PaintingService.GetBrush(captionColor);
            g.FillRectangle(captionBrush, rect);

            var rectCaption = rect;

            var rectCaptionText = rectCaption;
            rectCaptionText.X += TextGapLeft;
            rectCaptionText.Width -= TextGapLeft + TextGapRight;
            rectCaptionText.Width -= ButtonGapLeft + ButtonClose.Width + ButtonGapRight;
            if (ShouldShowAutoHideButton)
                rectCaptionText.Width -= ButtonAutoHide.Width + ButtonGapBetween;
            if (HasTabPageContextMenu)
                rectCaptionText.Width -= ButtonOptions.Width + ButtonGapBetween;
            rectCaptionText.Y += TextGapTop;
            rectCaptionText.Height -= TextGapTop + TextGapBottom;

            Color colorText;
            if (DockPane.IsActivated)
                colorText = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Text;
            else
                colorText = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive.Text;

            TextRenderer.DrawText(g, DockPane.CaptionText, TextFont, DrawHelper.RtlTransform(this, rectCaptionText),
                colorText, TextFormat);

            var rectDotsStrip = rectCaptionText;
            var textLength = (int) g.MeasureString(DockPane.CaptionText, TextFont).Width + TextGapLeft;
            rectDotsStrip.X += textLength;
            rectDotsStrip.Width -= textLength;
            rectDotsStrip.Height = ClientRectangle.Height;

            Color dotsColor;
            if (DockPane.IsActivated)
                dotsColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Grip;
            else
                dotsColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive.Grip;

            DrawDotsStrip(g, rectDotsStrip, dotsColor);
        }

        protected void DrawDotsStrip(Graphics g, Rectangle rectStrip, Color colorDots)
        {
            if (rectStrip.Width <= 0 || rectStrip.Height <= 0)
                return;

            var penDots = DockPane.DockPanel.Theme.PaintingService.GetPen(colorDots);
            penDots.DashStyle = DashStyle.Custom;
            penDots.DashPattern = new float[] {1, 3};
            var positionY = rectStrip.Height / 2;

            g.DrawLine(penDots, rectStrip.X + 2, positionY, rectStrip.X + rectStrip.Width - 2, positionY);

            g.DrawLine(penDots, rectStrip.X, positionY - 2, rectStrip.X + rectStrip.Width, positionY - 2);
            g.DrawLine(penDots, rectStrip.X, positionY + 2, rectStrip.X + rectStrip.Width, positionY + 2);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            SetButtonsPosition();
            base.OnLayout(levent);
        }

        protected override void OnRefreshChanges()
        {
            SetButtons();
            Invalidate();
        }

        private void SetButtons()
        {
            ButtonClose.Enabled = CloseButtonEnabled;
            ButtonClose.Visible = CloseButtonVisible;
            ButtonAutoHide.Visible = ShouldShowAutoHideButton;
            ButtonOptions.Visible = HasTabPageContextMenu;
            ButtonClose.RefreshChanges();
            ButtonAutoHide.RefreshChanges();
            ButtonOptions.RefreshChanges();

            SetButtonsPosition();
        }

        private void SetButtonsPosition()
        {
            // set the size and location for close and auto-hide buttons
            var rectCaption = ClientRectangle;
            var buttonWidth = ButtonClose.Image.Width;
            var buttonHeight = ButtonClose.Image.Height;

            var buttonSize = new Size(buttonWidth, buttonHeight);
            var x = rectCaption.X + rectCaption.Width - ButtonGapRight - m_buttonClose.Width;
            var y = rectCaption.Y + ButtonGapTop;
            var point = new Point(x, y);
            ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

            // If the close button is not visible draw the auto hide button overtop.
            // Otherwise it is drawn to the left of the close button.
            if (CloseButtonVisible)
                point.Offset(-(buttonWidth + ButtonGapBetween), 0);

            ButtonAutoHide.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
            if (ShouldShowAutoHideButton)
                point.Offset(-(buttonWidth + ButtonGapBetween), 0);
            ButtonOptions.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
        }

        private void Close_Click(object sender, EventArgs e)
        {
            DockPane.CloseActiveContent();
        }

        private void AutoHide_Click(object sender, EventArgs e)
        {
            DockPane.DockState = DockHelper.ToggleAutoHideState(DockPane.DockState);
            if (DockHelper.IsDockStateAutoHide(DockPane.DockState))
            {
                DockPane.DockPanel.ActiveAutoHideContent = null;
                DockPane.NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(DockPane);
            }
        }

        private void Options_Click(object sender, EventArgs e)
        {
            ShowTabPageContextMenu(PointToClient(MousePosition));
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            PerformLayout();
        }

        #region consts

        private const int TextGapTop = 3;
        private const int TextGapBottom = 2;
        private const int TextGapLeft = 2;
        private const int TextGapRight = 3;
        private const int ButtonGapTop = 3;
        private const int ButtonGapBottom = 3;
        private const int ButtonGapBetween = 1;
        private const int ButtonGapLeft = 1;
        private const int ButtonGapRight = 4;

        #endregion
    }
}