using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
{
    partial class DockPane
    {
        private SplitterControlBase Splitter { get; set; }

        internal Rectangle SplitterBounds
        {
            set => Splitter.Bounds = value;
        }

        internal DockAlignment SplitterAlignment
        {
            set => Splitter.Alignment = value;
        }

        [ToolboxItem(false)]
        public class SplitterControlBase : Control, ISplitterDragSource
        {
            private DockAlignment m_alignment;

            public SplitterControlBase(DockPane pane)
            {
                SetStyle(ControlStyles.Selectable, false);
                DockPane = pane;
            }

            public DockPane DockPane { get; }

            public DockAlignment Alignment
            {
                get => m_alignment;
                set
                {
                    m_alignment = value;
                    if (m_alignment == DockAlignment.Left || m_alignment == DockAlignment.Right)
                        Cursor = Cursors.VSplit;
                    else if (m_alignment == DockAlignment.Top || m_alignment == DockAlignment.Bottom)
                        Cursor = Cursors.HSplit;
                    else
                        Cursor = Cursors.Default;

                    if (DockPane.DockState == DockState.Document)
                        Invalidate();
                }
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button != MouseButtons.Left)
                    return;

                DockPane.DockPanel.BeginDrag(this, Parent.RectangleToScreen(Bounds));
            }

            #region ISplitterDragSource Members

            void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
            {
            }

            void ISplitterDragSource.EndDrag()
            {
            }

            bool ISplitterDragSource.IsVertical
            {
                get
                {
                    var status = DockPane.NestedDockingStatus;
                    return status.DisplayingAlignment == DockAlignment.Left ||
                           status.DisplayingAlignment == DockAlignment.Right;
                }
            }

            Rectangle ISplitterDragSource.DragLimitBounds
            {
                get
                {
                    var status = DockPane.NestedDockingStatus;
                    var rectLimit = Parent.RectangleToScreen(status.LogicalBounds);
                    if (((ISplitterDragSource) this).IsVertical)
                    {
                        rectLimit.X += MeasurePane.MinSize;
                        rectLimit.Width -= 2 * MeasurePane.MinSize;
                    }
                    else
                    {
                        rectLimit.Y += MeasurePane.MinSize;
                        rectLimit.Height -= 2 * MeasurePane.MinSize;
                    }

                    return rectLimit;
                }
            }

            void ISplitterDragSource.MoveSplitter(int offset)
            {
                var status = DockPane.NestedDockingStatus;
                var proportion = status.Proportion;
                if (status.LogicalBounds.Width <= 0 || status.LogicalBounds.Height <= 0)
                    return;
                if (status.DisplayingAlignment == DockAlignment.Left)
                    proportion += offset / (double) status.LogicalBounds.Width;
                else if (status.DisplayingAlignment == DockAlignment.Right)
                    proportion -= offset / (double) status.LogicalBounds.Width;
                else if (status.DisplayingAlignment == DockAlignment.Top)
                    proportion += offset / (double) status.LogicalBounds.Height;
                else
                    proportion -= offset / (double) status.LogicalBounds.Height;

                DockPane.SetNestedDockingProportion(proportion);
            }

            #region IDragSource Members

            Control IDragSource.DragControl => this;

            #endregion

            #endregion
        }
    }
}