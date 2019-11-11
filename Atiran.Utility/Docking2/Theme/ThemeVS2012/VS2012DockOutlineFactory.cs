using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    internal class VS2012DockOutlineFactory : DockPanelExtender.IDockOutlineFactory
    {
        public DockOutlineBase CreateDockOutline()
        {
            return new VS2012LightDockOutline();
        }

        private class VS2012LightDockOutline : DockOutlineBase
        {
            public VS2012LightDockOutline()
            {
                DragForm = new DragForm();
                SetDragForm(Rectangle.Empty);
                // IMPORTANT: this color does not come from palette.
                DragForm.BackColor = ColorTranslator.FromHtml("#FFC2C2C2");
                DragForm.BackgroundColor = ColorTranslator.FromHtml("#FF5BADFF");
                DragForm.Opacity = 0.5;
                DragForm.Show(false);
            }

            private DragForm DragForm { get; }

            protected override void OnShow()
            {
                CalculateRegion();
            }

            protected override void OnClose()
            {
                DragForm.Close();
            }

            private void CalculateRegion()
            {
                if (SameAsOldValue)
                    return;

                if (!FloatWindowBounds.IsEmpty)
                    SetOutline(FloatWindowBounds);
                else if (DockTo is DockPanel)
                    SetOutline(DockTo as DockPanel, Dock, ContentIndex != 0);
                else if (DockTo is DockPane)
                    SetOutline(DockTo as DockPane, Dock, ContentIndex);
                else
                    SetOutline();
            }

            private void SetOutline()
            {
                SetDragForm(Rectangle.Empty);
            }

            private void SetOutline(Rectangle floatWindowBounds)
            {
                SetDragForm(floatWindowBounds);
            }

            private void SetOutline(DockPanel dockPanel, DockStyle dock, bool fullPanelEdge)
            {
                var rect = fullPanelEdge ? dockPanel.DockArea : dockPanel.DocumentWindowBounds;
                rect.Location = dockPanel.PointToScreen(rect.Location);
                if (dock == DockStyle.Top)
                {
                    var height = dockPanel.GetDockWindowSize(DockState.DockTop);
                    rect = new Rectangle(rect.X, rect.Y, rect.Width, height);
                }
                else if (dock == DockStyle.Bottom)
                {
                    var height = dockPanel.GetDockWindowSize(DockState.DockBottom);
                    rect = new Rectangle(rect.X, rect.Bottom - height, rect.Width, height);
                }
                else if (dock == DockStyle.Left)
                {
                    var width = dockPanel.GetDockWindowSize(DockState.DockLeft);
                    rect = new Rectangle(rect.X, rect.Y, width, rect.Height);
                }
                else if (dock == DockStyle.Right)
                {
                    var width = dockPanel.GetDockWindowSize(DockState.DockRight);
                    rect = new Rectangle(rect.Right - width, rect.Y, width, rect.Height);
                }
                else if (dock == DockStyle.Fill)
                {
                    rect = dockPanel.DocumentWindowBounds;
                    rect.Location = dockPanel.PointToScreen(rect.Location);
                }

                SetDragForm(rect);
            }

            private void SetOutline(DockPane pane, DockStyle dock, int contentIndex)
            {
                if (dock != DockStyle.Fill)
                {
                    var rect = pane.DisplayingRectangle;
                    if (dock == DockStyle.Right)
                        rect.X += rect.Width / 2;
                    if (dock == DockStyle.Bottom)
                        rect.Y += rect.Height / 2;
                    if (dock == DockStyle.Left || dock == DockStyle.Right)
                        rect.Width -= rect.Width / 2;
                    if (dock == DockStyle.Top || dock == DockStyle.Bottom)
                        rect.Height -= rect.Height / 2;
                    rect.Location = pane.PointToScreen(rect.Location);

                    SetDragForm(rect);
                }
                else if (contentIndex == -1)
                {
                    var rect = pane.DisplayingRectangle;
                    rect.Location = pane.PointToScreen(rect.Location);
                    SetDragForm(rect);
                }
                else
                {
                    using (var path = pane.TabStripControl.GetOutline(contentIndex))
                    {
                        var rectF = path.GetBounds();
                        var rect = new Rectangle((int) rectF.X, (int) rectF.Y, (int) rectF.Width, (int) rectF.Height);
                        using (var matrix = new Matrix(rect,
                            new[] {new Point(0, 0), new Point(rect.Width, 0), new Point(0, rect.Height)}))
                        {
                            path.Transform(matrix);
                        }

                        var region = new Region(path);
                        SetDragForm(rect, region);
                    }
                }
            }

            private void SetDragForm(Rectangle rect)
            {
                DragForm.Bounds = rect;
                if (rect == Rectangle.Empty)
                {
                    if (DragForm.Region != null) DragForm.Region.Dispose();

                    DragForm.Region = new Region(Rectangle.Empty);
                }
                else if (DragForm.Region != null)
                {
                    DragForm.Region.Dispose();
                    DragForm.Region = null;
                }
            }

            private void SetDragForm(Rectangle rect, Region region)
            {
                DragForm.Bounds = rect;
                if (DragForm.Region != null) DragForm.Region.Dispose();

                DragForm.Region = region;
            }
        }
    }
}