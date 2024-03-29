using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using Atiran.Utility.Docking.Win32;

namespace Atiran.Utility.Docking
{
    public abstract class DockPaneStripBase : Control
    {
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

        protected TabCollection Tabs
        {
            get
            {
                if (m_tabs == null)
                    m_tabs = new TabCollection(DockPane);

                return m_tabs;
            }
        }

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

        protected internal abstract GraphicsPath GetOutline(int index);

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
                var content = Tabs[index].Content;
                if (DockPane.ActiveContent != content)
                    DockPane.ActiveContent = content;
            }

            if (e.Button == MouseButtons.Left)
                if (DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop &&
                    DockPane.ActiveContent.DockHandler.AllowEndUserDocking)
                    DockPane.DockPanel.BeginDrag(DockPane.ActiveContent.DockHandler);
        }

        protected void ShowTabPageContextMenu(Point position)
        {
            DockPane.ShowTabPageContextMenu(this, position);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
            {
                ShowTabPageContextMenu(new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Middle && DockPane.Appearance == DockPane.AppearanceStyle.Document)
            {
                // Get the content located under the click (if there is one)
                var index = HitTest();
                if (index != -1)
                {
                    // Close the specified content.
                    var content = Tabs[index].Content;
                    DockPane.CloseContent(content);
                }
            }
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

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected internal class Tab : IDisposable
        {
            public Tab(IDockContent content)
            {
                Content = content;
            }

            public IDockContent Content { get; }

            public Form ContentForm => Content as Form;

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
                        throw new ArgumentOutOfRangeException("index");
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
    }
}