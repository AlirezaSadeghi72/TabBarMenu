namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    internal class VS2012DockPaneStripFactory : DockPanelExtender.IDockPaneStripFactory
    {
        public DockPaneStripBase CreateDockPaneStrip(DockPane pane)
        {
            return new VS2012DockPaneStrip(pane);
        }
    }
}