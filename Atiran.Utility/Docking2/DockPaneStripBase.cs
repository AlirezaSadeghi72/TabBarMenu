using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using Atiran.Utility.Docking2.Desk;
using Atiran.Utility.Docking2.Win32;
using Atiran.Utility.MassageBox;

namespace Atiran.Utility.Docking2
{
    public abstract class DockPaneStripBase : Control
    {
        private Rectangle _dragBox = Rectangle.Empty;

        private TabCollection m_tabs;

        protected DockPaneStripBase(DockPane pane)
        {
            DockPane = pane;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
            AllowDrop = true;
        }

        protected DockPane DockPane { get; }

        protected DockPane.AppearanceStyle Appearance => DockPane.Appearance;

        protected TabCollection Tabs => m_tabs ?? (m_tabs = new TabCollection(DockPane));

        protected bool HasTabPageContextMenu => DockPane.HasTabPageContextMenu;

        internal void RefreshChanges()
        {
            if (IsDisposed)
                return;

            OnRefreshChanges();
        }

        protected virtual void OnRefreshChanges()
        {
        }

        protected internal abstract int MeasureHeight();

        protected internal abstract void EnsureTabVisible(IDockContent content);

        protected int HitTest()
        {
            return HitTest(PointToClient(MousePosition));
        }

        protected internal abstract int HitTest(Point point);

        protected virtual bool MouseDownActivateTest(MouseEventArgs e)
        {
            return true;
        }

        public abstract GraphicsPath GetOutline(int index);

        protected internal virtual Tab CreateTab(IDockContent content)
        {
            return new Tab(content);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var index = HitTest();
            if (index != -1)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    // Close the specified content.
                    TryCloseTab(index);
                }
                else
                {
                    var content = Tabs[index].Content;
                    if (DockPane.ActiveContent != content)
                        // Test if the content should be active
                        if (MouseDownActivateTest(e))
                            DockPane.ActiveContent = content;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                var dragSize = SystemInformation.DragSize;
                _dragBox = new Rectangle(new Point(e.X - dragSize.Width / 2,
                    e.Y - dragSize.Height / 2), dragSize);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button != MouseButtons.Left || _dragBox.Contains(e.Location))
                return;

            if (DockPane.ActiveContent == null)
                return;

            if (DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop &&
                DockPane.ActiveContent.DockHandler.AllowEndUserDocking)
                DockPane.DockPanel.BeginDrag(DockPane.ActiveContent.DockHandler);
        }

        protected void ShowTabPageContextMenu(Point position)
        {
            DockPane.ShowTabPageContextMenu(this, position);
        }

        protected bool TryCloseTab(int index)
        {
            if (index >= 0 || index < Tabs.Count)
            {
                var content = Tabs[index].Content;

                if ((Tabs[index].Content.DockHandler.Form as DeskTab).ShowQuestionClose)
                {
                    if (ShowPersianMessageBox.ShowMessge("پيغام",
                            "آيا تب " + Tabs[index].Content.DockHandler.TabText + " بسته شود",
                            MessageBoxButtons.YesNo, false, false) == DialogResult.Yes)
                    {
                        // Close the specified content.
                        DockPane.CloseContent(content);
                        if (PatchController.EnableSelectClosestOnClose == true)
                            SelectClosestPane(index);
                        return true;
                    }
                }
                else
                {
                    // Close the specified content.
                    DockPane.CloseContent(content);
                    if (PatchController.EnableSelectClosestOnClose == true)
                        SelectClosestPane(index);
                    return true;
                }
            }

            return false;
        }

        private void SelectClosestPane(int index)
        {
            if (index > 0 && DockPane.DockPanel.DocumentStyle == DocumentStyle.DockingWindow)
            {
                index = index - 1;

                if (index >= 0 || index < Tabs.Count)
                    DockPane.ActiveContent = Tabs[index].Content;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
                ShowTabPageContextMenu(new Point(e.X, e.Y));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int) Msgs.WM_LBUTTONDBLCLK)
            {
                base.WndProc(ref m);

                var index = HitTest();
                if (DockPane.DockPanel.AllowEndUserDocking && index != -1)
                {
                    var content = Tabs[index].Content;
                    if (content.DockHandler.CheckDockState(!content.DockHandler.IsFloat) != DockState.Unknown)
                        content.DockHandler.IsFloat = !content.DockHandler.IsFloat;
                }

                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);

            var index = HitTest();
            if (index != -1)
            {
                var content = Tabs[index].Content;
                if (DockPane.ActiveContent != content)
                    DockPane.ActiveContent = content;
            }
        }

        protected void ContentClosed()
        {
            if (m_tabs.Count == 0) DockPane.ClearLastActiveContent();
        }

        protected abstract Rectangle GetTabBounds(Tab tab);

        internal static Rectangle ToScreen(Rectangle rectangle, Control parent)
        {
            if (parent == null)
                return rectangle;

            return new Rectangle(parent.PointToScreen(new Point(rectangle.Left, rectangle.Top)),
                new Size(rectangle.Width, rectangle.Height));
        }

        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new DockPaneStripAccessibleObject(this);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected internal class Tab : IDisposable
        {
            private Rectangle? _rect;

            public Tab(IDockContent content)
            {
                Content = content;
            }

            public IDockContent Content { get; }

            public Form ContentForm => Content as Form;

            public Rectangle? Rectangle
            {
                get
                {
                    if (_rect != null) return _rect;

                    return _rect = System.Drawing.Rectangle.Empty;
                }

                set => _rect = value;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~Tab()
            {
                Dispose(false);
            }

            protected virtual void Dispose(bool disposing)
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected sealed class TabCollection : IEnumerable<Tab>
        {
            internal TabCollection(DockPane pane)
            {
                DockPane = pane;
            }

            public DockPane DockPane { get; }

            public int Count => DockPane.DisplayingContents.Count;

            public Tab this[int index]
            {
                get
                {
                    var content = DockPane.DisplayingContents[index];
                    if (content == null)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    return content.DockHandler.GetTab(DockPane.TabStripControl);
                }
            }

            public bool Contains(Tab tab)
            {
                return IndexOf(tab) != -1;
            }

            public bool Contains(IDockContent content)
            {
                return IndexOf(content) != -1;
            }

            public int IndexOf(Tab tab)
            {
                if (tab == null)
                    return -1;

                return DockPane.DisplayingContents.IndexOf(tab.Content);
            }

            public int IndexOf(IDockContent content)
            {
                return DockPane.DisplayingContents.IndexOf(content);
            }

            #region IEnumerable Members

            IEnumerator<Tab> IEnumerable<Tab>.GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return this[i];
            }

            #endregion
        }

        public class DockPaneStripAccessibleObject : ControlAccessibleObject
        {
            private DockPaneStripBase _strip;

            public DockPaneStripAccessibleObject(DockPaneStripBase strip)
                : base(strip)
            {
                _strip = strip;
            }

            public override AccessibleRole Role => AccessibleRole.PageTabList;

            public override int GetChildCount()
            {
                return _strip.Tabs.Count;
            }

            public override AccessibleObject GetChild(int index)
            {
                return new DockPaneStripTabAccessibleObject(_strip, _strip.Tabs[index], this);
            }

            public override AccessibleObject HitTest(int x, int y)
            {
                var point = new Point(x, y);
                foreach (var tab in _strip.Tabs)
                {
                    var rectangle = _strip.GetTabBounds(tab);
                    if (ToScreen(rectangle, _strip).Contains(point))
                        return new DockPaneStripTabAccessibleObject(_strip, tab, this);
                }

                return null;
            }
        }

        protected class DockPaneStripTabAccessibleObject : AccessibleObject
        {
            private DockPaneStripBase _strip;
            private Tab _tab;

            internal DockPaneStripTabAccessibleObject(DockPaneStripBase strip, Tab tab, AccessibleObject parent)
            {
                _strip = strip;
                _tab = tab;

                Parent = parent;
            }

            public override AccessibleObject Parent { get; }

            public override AccessibleRole Role => AccessibleRole.PageTab;

            public override Rectangle Bounds
            {
                get
                {
                    var rectangle = _strip.GetTabBounds(_tab);
                    return ToScreen(rectangle, _strip);
                }
            }

            public override string Name
            {
                get => _tab.Content.DockHandler.TabText;
                set
                {
                    //base.Name = value;
                }
            }
        }
    }
}