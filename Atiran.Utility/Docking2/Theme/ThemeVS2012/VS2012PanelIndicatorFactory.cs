using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    internal class VS2012PanelIndicatorFactory : DockPanelExtender.IPanelIndicatorFactory
    {
        public DockPanel.IPanelIndicator CreatePanelIndicator(DockStyle style, ThemeBase theme)
        {
            return new VS2012PanelIndicator(style, theme);
        }

        [ToolboxItem(false)]
        private class VS2012PanelIndicator : PictureBox, DockPanel.IPanelIndicator
        {
            private Image _imagePanelBottom;
            private Image _imagePanelBottomActive;
            private Image _imagePanelFill;
            private Image _imagePanelFillActive;
            private Image _imagePanelLeft;
            private Image _imagePanelLeftActive;
            private Image _imagePanelRight;
            private Image _imagePanelRightActive;
            private Image _imagePanelTop;
            private Image _imagePanelTopActive;

            private bool m_isActivated;

            private DockStyle m_status;

            public VS2012PanelIndicator(DockStyle dockStyle, ThemeBase theme)
            {
                _imagePanelLeft = theme.ImageService.DockIndicator_PanelLeft;
                _imagePanelRight = theme.ImageService.DockIndicator_PanelRight;
                _imagePanelTop = theme.ImageService.DockIndicator_PanelTop;
                _imagePanelBottom = theme.ImageService.DockIndicator_PanelBottom;
                _imagePanelFill = theme.ImageService.DockIndicator_PanelFill;
                _imagePanelLeftActive = theme.ImageService.DockIndicator_PanelLeft;
                _imagePanelRightActive = theme.ImageService.DockIndicator_PanelRight;
                _imagePanelTopActive = theme.ImageService.DockIndicator_PanelTop;
                _imagePanelBottomActive = theme.ImageService.DockIndicator_PanelBottom;
                _imagePanelFillActive = theme.ImageService.DockIndicator_PanelFill;

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
    }
}