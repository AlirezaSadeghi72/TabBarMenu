using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static Atiran.Utility.Docking2.DockPanel;
using static Atiran.Utility.Docking2.DockPanelExtender;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2005
{
    public class VS2005PanelIndicatorFactory : IPanelIndicatorFactory
    {
        public IPanelIndicator CreatePanelIndicator(DockStyle style, ThemeBase theme)
        {
            return new VS2005PanelIndicator(style);
        }

        [ToolboxItem(false)]
        private class VS2005PanelIndicator : PictureBox, IPanelIndicator
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

            public VS2005PanelIndicator(DockStyle dockStyle)
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
    }
}