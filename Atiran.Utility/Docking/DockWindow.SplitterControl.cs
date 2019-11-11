namespace Atiran.Utility.Docking
{
    public partial class DockWindow
    {
        private class SplitterControl : SplitterBase
        {
            protected override int SplitterSize => Measures.SplitterSize;

            protected override void StartDrag()
            {
                var window = Parent as DockWindow;
                if (window == null)
                    return;

                window.DockPanel.BeginDrag(window, window.RectangleToScreen(Bounds));
            }
        }
    }
}