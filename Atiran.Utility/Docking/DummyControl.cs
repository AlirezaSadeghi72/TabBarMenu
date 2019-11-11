using System.Windows.Forms;

namespace Atiran.Utility.Docking
{
    internal class DummyControl : Control
    {
        public DummyControl()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}