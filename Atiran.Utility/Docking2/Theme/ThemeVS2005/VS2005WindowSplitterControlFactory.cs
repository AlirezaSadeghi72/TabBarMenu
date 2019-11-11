using System.ComponentModel;
using System.Windows.Forms;
using static Atiran.Utility.Docking2.DockPanelExtender;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2005
{
    internal class VS2005WindowSplitterControlFactory : IWindowSplitterControlFactory
    {
        public SplitterBase CreateSplitterControl(ISplitterHost host)
        {
            return new VS2005WindowSplitterControl(host);
        }

        [ToolboxItem(false)]
        private class VS2005WindowSplitterControl : SplitterBase
        {
            private ISplitterHost _host;

            public VS2005WindowSplitterControl(ISplitterHost host)
            {
                _host = host;
            }

            protected override int SplitterSize => _host.DockPanel.Theme.Measures.SplitterSize;

            protected override void StartDrag()
            {
                _host.DockPanel.BeginDrag(_host, ((Control) _host).RectangleToScreen(Bounds));
            }
        }
    }
}