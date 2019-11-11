using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
{
    public abstract class InertButtonBase : Control
    {
        private bool m_isMouseDown;

        private bool m_isMouseOver;

        protected InertButtonBase()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        public abstract Bitmap HoverImage { get; }

        public abstract Bitmap PressImage { get; }

        public abstract Bitmap Image { get; }

        protected bool IsMouseOver
        {
            get => m_isMouseOver;
            private set
            {
                if (m_isMouseOver == value)
                    return;

                m_isMouseOver = value;
                Invalidate();
            }
        }

        protected bool IsMouseDown
        {
            get => m_isMouseDown;
            private set
            {
                if (m_isMouseDown == value)
                    return;

                m_isMouseDown = value;
                Invalidate();
            }
        }

        protected override Size DefaultSize => new Size(16, 15);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var over = ClientRectangle.Contains(e.X, e.Y);
            if (IsMouseOver != over)
                IsMouseOver = over;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!IsMouseOver)
                IsMouseOver = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsMouseOver)
                IsMouseOver = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsMouseDown)
                IsMouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsMouseDown)
                IsMouseDown = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (HoverImage != null)
            {
                if (IsMouseOver && Enabled)
                    e.Graphics.DrawImage(
                        IsMouseDown ? PressImage : HoverImage,
                        PatchController.EnableHighDpi == true
                            ? ClientRectangle
                            : new Rectangle(0, 0, Image.Width, Image.Height));
                else
                    e.Graphics.DrawImage(
                        Image,
                        PatchController.EnableHighDpi == true
                            ? ClientRectangle
                            : new Rectangle(0, 0, Image.Width, Image.Height));

                base.OnPaint(e);
                return;
            }

            if (IsMouseOver && Enabled)
                using (var pen = new Pen(ForeColor))
                {
                    e.Graphics.DrawRectangle(pen, Rectangle.Inflate(ClientRectangle, -1, -1));
                }

            using (var imageAttributes = new ImageAttributes())
            {
                var colorMap = new ColorMap[2];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.FromArgb(0, 0, 0);
                colorMap[0].NewColor = ForeColor;
                colorMap[1] = new ColorMap();
                colorMap[1].OldColor = Image.GetPixel(0, 0);
                colorMap[1].NewColor = Color.Transparent;

                imageAttributes.SetRemapTable(colorMap);

                e.Graphics.DrawImage(
                    Image,
                    new Rectangle(0, 0, Image.Width, Image.Height),
                    0, 0,
                    Image.Width,
                    Image.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }

            base.OnPaint(e);
        }

        public void RefreshChanges()
        {
            if (IsDisposed)
                return;

            var mouseOver = ClientRectangle.Contains(PointToClient(MousePosition));
            if (mouseOver != IsMouseOver)
                IsMouseOver = mouseOver;

            OnRefreshChanges();
        }

        protected virtual void OnRefreshChanges()
        {
        }
    }
}