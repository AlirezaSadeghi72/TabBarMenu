using System.Drawing;

namespace Atiran.Utility.Docking2
{
    partial class DockPanel
    {
        private SplitterDragHandler m_splitterDragHandler;

        private SplitterDragHandler GetSplitterDragHandler()
        {
            if (m_splitterDragHandler == null)
                m_splitterDragHandler = new SplitterDragHandler(this);
            return m_splitterDragHandler;
        }

        public void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
        {
            GetSplitterDragHandler().BeginDrag(dragSource, rectSplitter);
        }

        private sealed class SplitterDragHandler : DragHandler
        {
            public SplitterDragHandler(DockPanel dockPanel)
                : base(dockPanel)
            {
            }

            public new ISplitterDragSource DragSource
            {
                get => base.DragSource as ISplitterDragSource;
                private set => base.DragSource = value;
            }

            private SplitterOutline Outline { get; set; }

            private Rectangle RectSplitter { get; set; }

            public void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
            {
                DragSource = dragSource;
                RectSplitter = rectSplitter;

                if (!BeginDrag())
                {
                    DragSource = null;
                    return;
                }

                Outline = new SplitterOutline();
                Outline.Show(rectSplitter);
                DragSource.BeginDrag(rectSplitter);
            }

            protected override void OnDragging()
            {
                Outline.Show(GetSplitterOutlineBounds(MousePosition));
            }

            protected override void OnEndDrag(bool abort)
            {
                DockPanel.SuspendLayout(true);

                Outline.Close();

                if (!abort)
                    DragSource.MoveSplitter(GetMovingOffset(MousePosition));

                DragSource.EndDrag();
                DockPanel.ResumeLayout(true, true);
            }

            private int GetMovingOffset(Point ptMouse)
            {
                var rect = GetSplitterOutlineBounds(ptMouse);
                if (DragSource.IsVertical)
                    return rect.X - RectSplitter.X;
                return rect.Y - RectSplitter.Y;
            }

            private Rectangle GetSplitterOutlineBounds(Point ptMouse)
            {
                var rectLimit = DragSource.DragLimitBounds;

                var rect = RectSplitter;
                if (rectLimit.Width <= 0 || rectLimit.Height <= 0)
                    return rect;

                if (DragSource.IsVertical)
                {
                    rect.X += ptMouse.X - StartMousePosition.X;
                    rect.Height = rectLimit.Height;
                }
                else
                {
                    rect.Y += ptMouse.Y - StartMousePosition.Y;
                    rect.Width = rectLimit.Width;
                }

                if (rect.Left < rectLimit.Left)
                    rect.X = rectLimit.X;
                if (rect.Top < rectLimit.Top)
                    rect.Y = rectLimit.Y;
                if (rect.Right > rectLimit.Right)
                    rect.X -= rect.Right - rectLimit.Right;
                if (rect.Bottom > rectLimit.Bottom)
                    rect.Y -= rect.Bottom - rectLimit.Bottom;

                return rect;
            }

            private class SplitterOutline
            {
                public SplitterOutline()
                {
                    DragForm = new DragForm();
                    SetDragForm(Rectangle.Empty);
                    DragForm.BackColor = Color.Black;
                    DragForm.Opacity = 0.7;
                    DragForm.Show(false);
                }

                private DragForm DragForm { get; }

                public void Show(Rectangle rect)
                {
                    SetDragForm(rect);
                }

                public void Close()
                {
                    DragForm.Bounds = Rectangle.Empty;
                    DragForm.Close();
                }

                private void SetDragForm(Rectangle rect)
                {
                    DragForm.Bounds = rect;
                    if (rect == Rectangle.Empty)
                        DragForm.Region = new Region(Rectangle.Empty);
                    else if (DragForm.Region != null)
                        DragForm.Region = null;
                }
            }
        }
    }
}