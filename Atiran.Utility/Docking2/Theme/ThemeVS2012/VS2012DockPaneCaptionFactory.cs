
namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    internal class VS2012DockPaneCaptionFactory : DockPanelExtender.IDockPaneCaptionFactory
    {
        public DockPaneCaptionBase CreateDockPaneCaption(DockPane pane)
        {
            return new VS2012DockPaneCaption(pane);
        }
    }
}
