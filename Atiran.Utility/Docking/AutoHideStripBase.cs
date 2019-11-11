using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking
{
    public abstract class AutoHideStripBase : Control
    {
        private GraphicsPath m_displayingArea;

        protected AutoHideStripBase(DockPanel panel)
        {
            DockPanel = panel;
            PanesTop = new PaneCollection(panel, DockState.DockTopAutoHide);
            PanesBottom = new PaneCollection(panel, DockState.DockBottomAutoHide);
            PanesLeft = new PaneCollection(panel, DockState.DockLeftAutoHide);
            PanesRight = new PaneCollection(panel, DockState.DockRightAutoHide);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
        }

        protected DockPanel DockPanel { get; }

        protected PaneCollection PanesTop { get; }

        protected PaneCollection PanesBottom { get; }

        protected PaneCollection PanesLeft { get; }

        protected PaneCollection PanesRight { get; }

        protected Rectangle RectangleTopLeft
        {
            get
            {
                var height = MeasureHeight();
                return PanesTop.Count > 0 && PanesLeft.Count > 0
                    ? new Rectangle(0, 0, height, height)
                    : Rectangle.Empty;
            }
        }

        protected Rectangle RectangleTopRight
        {
            get
            {
                var height = MeasureHeight();
                return PanesTop.Count > 0 && PanesRight.Count > 0
                    ? new Rectangle(Width - height, 0, height, height)
                    : Rectangle.Empty;
            }
        }

        protected Rectangle RectangleBottomLeft
        {
            get
            {
                var height = MeasureHeight();
                return PanesBottom.Count > 0 && PanesLeft.Count > 0
                    ? new Rectangle(0, Height - height, height, height)
                    : Rectangle.Empty;
            }
        }

        protected Rectangle RectangleBottomRight
        {
            get
            {
                var height = MeasureHeight();
                return PanesBottom.Count > 0 && PanesRight.Count > 0
                    ? new Rectangle(Width - height, Height - height, height, height)
                    : Rectangle.Empty;
            }
        }

        private GraphicsPath DisplayingArea
        {
            get
            {
                if (m_displayingArea == null)
                    m_displayingArea = new GraphicsPath();

                return m_displayingArea;
            }
        }

        protected PaneCollection GetPanes(DockState dockState)
        {
            if (dockState == DockState.DockTopAutoHide)
                return PanesTop;
            if (dockState == DockState.DockBottomAutoHide)
                return PanesBottom;
            if (dockState == DockState.DockLeftAutoHide)
                return PanesLeft;
            if (dockState == DockState.DockRightAutoHide)
                return PanesRight;
            throw new ArgumentOutOfRangeException("dockState");
        }

        internal int GetNumberOfPanes(DockState dockState)
        {
            return GetPanes(dockState).Count;
        }

        protected internal Rectangle GetTabStripRectangle(DockState dockState)
        {
            var height = MeasureHeight();
            if (dockState == DockState.DockTopAutoHide && PanesTop.Count > 0)
                return new Rectangle(RectangleTopLeft.Width, 0,
                    Width - RectangleTopLeft.Width - RectangleTopRight.Width, height);
            if (dockState == DockState.DockBottomAutoHide && PanesBottom.Count > 0)
                return new Rectangle(RectangleBottomLeft.Width, Height - height,
                    Width - RectangleBottomLeft.Width - RectangleBottomRight.Width, height);
            if (dockState == DockState.DockLeftAutoHide && PanesLeft.Count > 0)
                return new Rectangle(0, RectangleTopLeft.Width, height,
                    Height - RectangleTopLeft.Height - RectangleBottomLeft.Height);
            if (dockState == DockState.DockRightAutoHide && PanesRight.Count > 0)
                return new Rectangle(Width - height, RectangleTopRight.Width, height,
                    Height - RectangleTopRight.Height - RectangleBottomRight.Height);
            return Rectangle.Empty;
        }

        private void SetRegion()
        {
            DisplayingArea.Reset();
            DisplayingArea.AddRectangle(RectangleTopLeft);
            DisplayingArea.AddRectangle(RectangleTopRight);
            DisplayingArea.AddRectangle(RectangleBottomLeft);
            DisplayingArea.AddRectangle(RectangleBottomRight);
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockTopAutoHide));
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockBottomAutoHide));
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockLeftAutoHide));
            DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockRightAutoHide));
            Region = new Region(DisplayingArea);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            var content = HitTest();
            if (content == null)
                return;

            content.DockHandler.Activate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            var content = HitTest();
            if (content != null && DockPanel.ActiveAutoHideContent != content)
                DockPanel.ActiveAutoHideContent = content;

            // requires further tracking of mouse hover behavior,
            ResetMouseEventArgs();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            RefreshChanges();
            base.OnLayout(levent);
        }

        internal void RefreshChanges()
        {
            if (IsDisposed)
                return;

            SetRegion();
            OnRefreshChanges();
        }

        protected virtual void OnRefreshChanges()
        {
        }

        protected internal abstract int MeasureHeight();

        private IDockContent HitTest()
        {
            var ptMouse = PointToClient(MousePosition);
            return HitTest(ptMouse);
        }

        protected virtual Tab CreateTab(IDockContent content)
        {
            return new Tab(content);
        }

        protected virtual Pane CreatePane(DockPane dockPane)
        {
            return new Pane(dockPane);
        }

        protected abstract IDockContent HitTest(Point point);

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected class Tab : IDisposable
        {
            protected internal Tab(IDockContent content)
            {
                Content = content;
            }

            public IDockContent Content { get; }

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

            public DockPanel DockPanel => DockPane.DockPanel;

            public int Count => DockPane.DisplayingContents.Count;

            public Tab this[int index]
            {
                get
                {
                    var content = DockPane.DisplayingContents[index];
                    if (content == null)
                        throw new ArgumentOutOfRangeException("index");
                    if (content.DockHandler.AutoHideTab == null)
                        content.DockHandler.AutoHideTab = DockPanel.AutoHideStripControl.CreateTab(content);
                    return content.DockHandler.AutoHideTab as Tab;
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

                return IndexOf(tab.Content);
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

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected class Pane : IDisposable
        {
            protected internal Pane(DockPane dockPane)
            {
                DockPane = dockPane;
            }

            public DockPane DockPane { get; }

            public TabCollection AutoHideTabs
            {
                get
                {
                    if (DockPane.AutoHideTabs == null)
                        DockPane.AutoHideTabs = new TabCollection(DockPane);
                    return DockPane.AutoHideTabs as TabCollection;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~Pane()
            {
                Dispose(false);
            }

            protected virtual void Dispose(bool disposing)
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        protected sealed class PaneCollection : IEnumerable<Pane>
        {
            internal PaneCollection(DockPanel panel, DockState dockState)
            {
                DockPanel = panel;
                States = new AutoHideStateCollection();
                States[DockState.DockTopAutoHide].Selected = dockState == DockState.DockTopAutoHide;
                States[DockState.DockBottomAutoHide].Selected = dockState == DockState.DockBottomAutoHide;
                States[DockState.DockLeftAutoHide].Selected = dockState == DockState.DockLeftAutoHide;
                States[DockState.DockRightAutoHide].Selected = dockState == DockState.DockRightAutoHide;
            }

            public DockPanel DockPanel { get; }

            private AutoHideStateCollection States { get; }

            public int Count
            {
                get
                {
                    var count = 0;
                    foreach (var pane in DockPanel.Panes)
                        if (States.ContainsPane(pane))
                            count++;

                    return count;
                }
            }

            public Pane this[int index]
            {
                get
                {
                    var count = 0;
                    foreach (var pane in DockPanel.Panes)
                    {
                        if (!States.ContainsPane(pane))
                            continue;

                        if (count == index)
                        {
                            if (pane.AutoHidePane == null)
                                pane.AutoHidePane = DockPanel.AutoHideStripControl.CreatePane(pane);
                            return pane.AutoHidePane as Pane;
                        }

                        count++;
                    }

                    throw new ArgumentOutOfRangeException("index");
                }
            }

            public bool Contains(Pane pane)
            {
                return IndexOf(pane) != -1;
            }

            public int IndexOf(Pane pane)
            {
                if (pane == null)
                    return -1;

                var index = 0;
                foreach (var dockPane in DockPanel.Panes)
                {
                    if (!States.ContainsPane(pane.DockPane))
                        continue;

                    if (pane == dockPane.AutoHidePane)
                        return index;

                    index++;
                }

                return -1;
            }

            private class AutoHideState
            {
                public DockState m_dockState;
                public bool m_selected;

                public AutoHideState(DockState dockState)
                {
                    m_dockState = dockState;
                }

                public DockState DockState => m_dockState;

                public bool Selected
                {
                    get => m_selected;
                    set => m_selected = value;
                }
            }

            private class AutoHideStateCollection
            {
                private AutoHideState[] m_states;

                public AutoHideStateCollection()
                {
                    m_states = new[]
                    {
                        new AutoHideState(DockState.DockTopAutoHide),
                        new AutoHideState(DockState.DockBottomAutoHide),
                        new AutoHideState(DockState.DockLeftAutoHide),
                        new AutoHideState(DockState.DockRightAutoHide)
                    };
                }

                public AutoHideState this[DockState dockState]
                {
                    get
                    {
                        for (var i = 0; i < m_states.Length; i++)
                            if (m_states[i].DockState == dockState)
                                return m_states[i];
                        throw new ArgumentOutOfRangeException("dockState");
                    }
                }

                public bool ContainsPane(DockPane pane)
                {
                    if (pane.IsHidden)
                        return false;

                    for (var i = 0; i < m_states.Length; i++)
                        if (m_states[i].DockState == pane.DockState && m_states[i].Selected)
                            return true;
                    return false;
                }
            }

            #region IEnumerable Members

            IEnumerator<Pane> IEnumerable<Pane>.GetEnumerator()
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