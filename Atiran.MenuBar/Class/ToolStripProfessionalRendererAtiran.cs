using System.Drawing;
using System.Windows.Forms;
using Atiran.MenuBar.Properties;

namespace Atiran.MenuBar.Class
{
    public class ToolStripProfessionalRendererAtiran : ToolStripProfessionalRenderer
    {
        public ToolStripProfessionalRendererAtiran() : base(new MyColors())
        {
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowRectangle = Rectangle.Empty;
            if (e.Item.Selected)
                e.Graphics.DrawImage(new Bitmap(Resources.expandleft, new Size(16, 16)), new Point(5, 5));
            else
                e.Graphics.DrawImage(new Bitmap(Resources.expandDown, new Size(16, 16)), new Point(5, 5));
            //base.OnRenderArrow(e);
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemImage(e);
            if (e.Item.Tag is MyTag && ((MyTag) e.Item.Tag).FormId > 0)
            {
                if (!e.Item.Selected)
                    e.Graphics.DrawImage(new Bitmap(Resources.LemonChiffon, new Size(16, 16)),
                        new Point(e.Item.ContentRectangle.Right - 19, e.Item.ContentRectangle.Top + 6));
                else
                    e.Graphics.DrawImage(new Bitmap(Resources.Yellow, new Size(16, 16)),
                        new Point(e.Item.ContentRectangle.Right - 19, e.Item.ContentRectangle.Top + 6));
            }
        }
    }

    internal class MyColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(80, 104, 125);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(80, 104, 125);

        public override Color MenuItemPressedGradientEnd => Color.FromArgb(80, 104, 125);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(80, 104, 125);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(20, 130, 150);

        public override Color ImageMarginGradientBegin => Color.FromArgb(20, 130, 150);

        public override Color ImageMarginGradientMiddle => Color.FromArgb(20, 130, 150);

        public override Color ImageMarginGradientEnd => Color.FromArgb(20, 130, 150);

        public override Color MenuItemBorder => Color.Transparent;

        public override Color ButtonSelectedHighlight => Color.Teal; //زماني كه موس روي زير منوها ميرود
        public override Color ButtonCheckedGradientBegin => Color.FromArgb(20, 130, 150);
        public override Color ButtonCheckedGradientEnd => Color.FromArgb(20, 130, 150);
        public override Color ButtonCheckedGradientMiddle => Color.FromArgb(20, 130, 150);
        public override Color ButtonCheckedHighlight => Color.FromArgb(20, 130, 150);
        public override Color ButtonCheckedHighlightBorder => Color.FromArgb(20, 130, 150);
        public override Color ButtonPressedBorder => Color.Red;
        public override Color ButtonPressedGradientBegin => Color.Red;
        public override Color ButtonPressedGradientEnd => Color.Red;
        public override Color ButtonPressedGradientMiddle => Color.Red;
        public override Color ButtonPressedHighlight => Color.Red;
        public override Color ButtonPressedHighlightBorder => Color.Red;
        public override Color ButtonSelectedBorder => Color.Red;
        public override Color ToolStripPanelGradientEnd => Color.Red;
        public override Color ToolStripPanelGradientBegin => Color.Red;
        public override Color ToolStripDropDownBackground => Color.FromArgb(20, 130, 150); // رنگ دور زير منوها
    }
}