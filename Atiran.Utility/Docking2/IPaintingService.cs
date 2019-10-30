using System.Drawing;

namespace Atiran.Utility.Docking2
{
    public interface IPaintingService
    {
        Pen GetPen(Color color, int thickness = 1);
        SolidBrush GetBrush(Color color);
        void CleanUp();
    }
}
