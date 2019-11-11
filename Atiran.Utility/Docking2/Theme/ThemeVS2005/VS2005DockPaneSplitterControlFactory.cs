using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static Atiran.Utility.Docking2.DockPane;
using static Atiran.Utility.Docking2.DockPanelExtender;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2005
{
    internal class VS2005DockPaneSplitterControlFactory : IDockPaneSplitterControlFactory
    {
        public SplitterControlBase CreateSplitterControl(DockPane pane)
        {
            return new VS2005SplitterControl(pane);
        }

        [ToolboxItem(false)]
        internal class VS2005SplitterControl : SplitterControlBase
        {
            public VS2005SplitterControl(DockPane pane) : base(pane)
            {
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (DockPane.DockState != DockState.Document)
                    return;

                var g = e.Graphics;
                var rect = ClientRectangle;
                if (Alignment == DockAlignment.Top || Alignment == DockAlignment.Bottom)
                    g.DrawLine(SystemPens.ControlDark, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                else if (Alignment == DockAlignment.Left || Alignment == DockAlignment.Right)
                    g.DrawLine(SystemPens.ControlDarkDark, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom);
            }
        }
    }
}