using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2005
{
    [ToolboxItem(false)]
    internal class VS2005DockPaneCaption : DockPaneCaptionBase
    {
        private static Bitmap _imageButtonClose;

        private static Bitmap _imageButtonAutoHide;

        private static Bitmap _imageButtonDock;

        private static Bitmap _imageButtonOptions;

        private static string _toolTipClose;

        private static string _toolTipOptions;

        private static string _toolTipAutoHide;

        private static Blend _activeBackColorGradientBlend;

        private static TextFormatFlags _textFormat =
            TextFormatFlags.SingleLine |
            TextFormatFlags.EndEllipsis |
            TextFormatFlags.VerticalCenter;

        private InertButton m_buttonAutoHide;

        private InertButton m_buttonClose;

        private InertButton m_buttonOptions;

        private ToolTip m_toolTip;

        public VS2005DockPaneCaption(DockPane pane) : base(pane)
        {
            SuspendLayout();

            Components = new Container();
            m_toolTip = new ToolTip(Components);

            ResumeLayout();
        }

        private static Bitmap ImageButtonClose
        {
            get
            {
                if (_imageButtonClose == null)
                    _imageButtonClose = Resources.DockPane_Close;

                return _imageButtonClose;
            }
        }

        private InertButton ButtonClose
        {
            get
            {
                if (m_buttonClose == null)
                {
                    m_buttonClose = new InertButton(this, ImageButtonClose, ImageButtonClose);
                    m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
                    m_buttonClose.Click += Close_Click;
                    Controls.Add(m_buttonClose);
                }

                return m_buttonClose;
            }
        }

        private static Bitmap ImageButtonAutoHide
        {
            get
            {
                if (_imageButtonAutoHide == null)
                    _imageButtonAutoHide = Resources.DockPane_AutoHide;

                return _imageButtonAutoHide;
            }
        }

        private static Bitmap ImageButtonDock
        {
            get
            {
                if (_imageButtonDock == null)
                    _imageButtonDock = Resources.DockPane_Dock;

                return _imageButtonDock;
            }
        }

        private InertButton ButtonAutoHide
        {
            get
            {
                if (m_buttonAutoHide == null)
                {
                    m_buttonAutoHide = new InertButton(this, ImageButtonDock, ImageButtonAutoHide);
                    m_toolTip.SetToolTip(m_buttonAutoHide, ToolTipAutoHide);
                    m_buttonAutoHide.Click += AutoHide_Click;
                    Controls.Add(m_buttonAutoHide);
                }

                return m_buttonAutoHide;
            }
        }

        private static Bitmap ImageButtonOptions
        {
            get
            {
                if (_imageButtonOptions == null)
                    _imageButtonOptions = Resources.DockPane_Option;

                return _imageButtonOptions;
            }
        }

        private InertButton ButtonOptions
        {
            get
            {
                if (m_buttonOptions == null)
                {
                    m_buttonOptions = new InertButton(this, ImageButtonOptions, ImageButtonOptions);
                    m_toolTip.SetToolTip(m_buttonOptions, ToolTipOptions);
                    m_buttonOptions.Click += Options_Click;
                    Controls.Add(m_buttonOptions);
                }

                return m_buttonOptions;
            }
        }

        private IContainer Components { get; }

        private static int TextGapTop => _TextGapTop;

        public Font TextFont => DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont;

        private static int TextGapBottom => _TextGapBottom;

        private static int TextGapLeft => _TextGapLeft;

        private static int TextGapRight => _TextGapRight;

        private static int ButtonGapTop => _ButtonGapTop;

        private static int ButtonGapBottom => _ButtonGapBottom;

        private static int ButtonGapLeft => _ButtonGapLeft;

        private static int ButtonGapRight => _ButtonGapRight;

        private static int ButtonGapBetween => _ButtonGapBetween;

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

        private static Blend ActiveBackColorGradientBlend
        {
            get
            {
                if (_activeBackColorGradientBlend == null)
                {
                    var blend = new Blend(2);

                    blend.Factors = new[] {0.5F, 1.0F};
                    blend.Positions = new[] {0.0F, 1.0F};
                    _activeBackColorGradientBlend = blend;
                }

                return _activeBackColorGradientBlend;
            }
        }

        private Color TextColor
        {
            get
            {
                if (DockPane.IsActivated)
                    return DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient
                        .TextColor;
                return DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient
                    .TextColor;
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

            if (DockPane.IsActivated)
            {
                var startColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient
                    .ActiveCaptionGradient.StartColor;
                var endColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient
                    .EndColor;
                var gradientMode = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient
                    .ActiveCaptionGradient.LinearGradientMode;
                ClientRectangle.SafelyDrawLinearGradient(startColor, endColor, gradientMode, g,
                    ActiveBackColorGradientBlend);
            }
            else
            {
                var startColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient
                    .InactiveCaptionGradient.StartColor;
                var endColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient
                    .InactiveCaptionGradient.EndColor;
                var gradientMode = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient
                    .InactiveCaptionGradient.LinearGradientMode;
                ClientRectangle.SafelyDrawLinearGradient(startColor, endColor, gradientMode, g);
            }

            var rectCaption = ClientRectangle;

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
                colorText = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient
                    .TextColor;
            else
                colorText = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient
                    .TextColor;

            TextRenderer.DrawText(g, DockPane.CaptionText, TextFont, DrawHelper.RtlTransform(this, rectCaptionText),
                colorText, TextFormat);
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
            var height = rectCaption.Height - ButtonGapTop - ButtonGapBottom;
            if (buttonHeight < height)
            {
                buttonWidth = buttonWidth * height / buttonHeight;
                buttonHeight = height;
            }

            var buttonSize = new Size(buttonWidth, buttonHeight);
            var x = rectCaption.X + rectCaption.Width - 1 - ButtonGapRight - m_buttonClose.Width;
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

        [ToolboxItem(false)]
        private sealed class InertButton : InertButtonBase
        {
            private Bitmap m_image, m_imageAutoHide;

            public InertButton(VS2005DockPaneCaption dockPaneCaption, Bitmap image, Bitmap imageAutoHide)
            {
                DockPaneCaption = dockPaneCaption;
                m_image = image;
                m_imageAutoHide = imageAutoHide;
                RefreshChanges();
            }

            private VS2005DockPaneCaption DockPaneCaption { get; }

            public bool IsAutoHide => DockPaneCaption.DockPane.IsAutoHide;

            public override Bitmap Image => IsAutoHide ? m_imageAutoHide : m_image;

            public override Bitmap HoverImage => null;

            public override Bitmap PressImage => null;

            protected override void OnRefreshChanges()
            {
                if (DockPaneCaption.DockPane.DockPanel != null)
                    if (DockPaneCaption.TextColor != ForeColor)
                    {
                        ForeColor = DockPaneCaption.TextColor;
                        Invalidate();
                    }
            }
        }

        #region consts

        private const int _TextGapTop = 2;
        private const int _TextGapBottom = 0;
        private const int _TextGapLeft = 3;
        private const int _TextGapRight = 3;
        private const int _ButtonGapTop = 2;
        private const int _ButtonGapBottom = 1;
        private const int _ButtonGapBetween = 1;
        private const int _ButtonGapLeft = 1;
        private const int _ButtonGapRight = 2;

        #endregion
    }
}