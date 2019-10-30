using System.Drawing;

namespace Atiran.Utility.Docking2
{
    public static class LayoutUtils
    {
        public static bool IsZeroWidthOrHeight(Rectangle rectangle)
        {
            return (rectangle.Width == 0 || rectangle.Height == 0);
        }
    }
}
