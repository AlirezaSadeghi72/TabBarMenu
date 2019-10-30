using static Atiran.Utility.Docking2.DockPanel.DockDragHandler;
using static Atiran.Utility.Docking2.DockPanelExtender;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2005
{
    public class VS2005DockIndicatorFactory : IDockIndicatorFactory
    {
        public DockIndicator CreateDockIndicator(DockPanel.DockDragHandler dockDragHandler)
        {
            return new DockIndicator(dockDragHandler);
        }
    }
}
