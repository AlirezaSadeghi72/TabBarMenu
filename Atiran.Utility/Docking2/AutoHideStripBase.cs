using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
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

        /// <summary>
        ///     The top left rectangle in auto hide strip area.
        /// </summary>
        protected Rectangle RectangleTopLeft
        {
            get
            {
                var standard = MeasureHeight();
                var padding = DockPanel.Theme.Measures.DockPadding;
                var width = PanesLeft.Count > 0 ? standard : padding;
                var height = PanesTop.Count > 0 ? standard : padding;
                return new Rectangle(0, 0, width, height);
            }
        }

        /// <summary>
        ///     The top right rectangle in auto hide strip area.
        /// </summary>
        protected Rectangle RectangleTopRight
        {
            get
            {
                var standard = MeasureHeight();
                var padding = DockPanel.Theme.Measures.DockPadding;
                var width = PanesRight.Count > 0 ? standard : padding;
                var height = PanesTop.Count > 0 ? standard : padding;
                return new Rectangle(Width - width, 0, width, height);
            }
        }

        /// <summary>
        ///     The bottom left rectangle in auto hide strip area.
        /// </summary>
        protected Rectangle RectangleBottomLeft
        {
            get
            {
                var standard = MeasureHeight();
                var padding = DockPanel.Theme.Measures.DockPadding;
                var width = PanesLeft.Count > 0 ? standard : padding;
                var height = PanesBottom.Count > 0 ? standard : padding;
                return new Rectangle(0, Height - height, width, height);
            }
        }

        /// <summary>
        ///     The bottom right rectangle in auto hide strip area.
        /// </summary>
        protected Rectangle RectangleBottomRight
        {
            get
            {
                var standard = MeasureHeight();
                var padding = DockPanel.Theme.Measures.DockPadding;
                var width = PanesRight.Count > 0 ? standard : padding;
                var height = PanesBottom.Count > 0 ? standard : padding;
                return new Rectangle(Width - width, Height - height, width, height);
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
            throw new ArgumentOutOfRangeException(nameof(dockState));
        }

        internal int GetNumberOfPanes(DockState dockState)
        {
            return GetPanes(dockState).Count;
        }

        /// <summary>
        ///     Gets one of the four auto hide strip rectangles.
        /// </summary>
        /// <param name="dockState">Dock state.</param>
        /// <returns>The desired rectangle.</returns>
        /// <remarks>
        ///     As the corners are represented by <see cref="RectangleTopLeft" />, <see cref="RectangleTopRight" />,
        ///     <see cref="RectangleBottomLeft" />, and <see cref="RectangleBottomRight" />,
        ///     the four strips can be easily calculated out as the borders.
        /// </remarks>
        protected internal Rectangle GetTabStripRectangle(DockState dockState)
        {
            if (dockState == DockState.DockTopAutoHide)
                return new Rectangle(RectangleTopLeft.Width, 0,
                    Width - RectangleTopLeft.Width - RectangleTopRight.Width, RectangleTopLeft.Height);

            if (dockState == DockState.DockBottomAutoHide)
                return new Rectangle(RectangleBottomLeft.Width, Height - RectangleBottomLeft.Height,
                    Width - RectangleBottomLeft.Width - RectangleBottomRight.Width, RectangleBottomLeft.Height);

            if (dockState == DockState.DockLeftAutoHide)
                return new Rectangle(0, RectangleTopLeft.Height, RectangleTopLeft.Width,
                    Height - RectangleTopLeft.Height - RectangleBottomLeft.Height);

            if (dockState == DockState.DockRightAutoHide)
                return new Rectangle(Width - RectangleTopRight.Width, RectangleTopRight.Height, RectangleTopRight.Width,
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

            SetActiveAutoHideContent(content);

            content.DockHandler.Activate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            if (!DockPanel.ShowAutoHideContentOnHover)
                return;

            // IMPORTANT: VS2003/2005 themes only.
            var content = HitTest();
            SetActiveAutoHideContent(content);

            // requires further tracking of mouse hover behavior,
            ResetMouseEventArgs();
        }

        private void SetActiveAutoHideContent(IDockContent content)
        {
            if (content != null)
                if (DockPanel.ActiveAutoHideContent != content)
                    DockPanel.ActiveAutoHideContent = content;
                else if (!DockPanel.ShowAutoHideContentOnHover)
                    DockPanel.ActiveAutoHideContent = null; // IMPORTANT: Not needed for VS2003/2005 themes.
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

        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new AutoHideStripsAccessibleObject(this);
        }

        protected abstract Rectangle GetTabBounds(Tab tab);

        internal static Rectangle ToScreen(Rectangle rectangle, Control parent)
        {
            if (parent == null)
                return rectangle;

            return new Rectangle(parent.PointToScreen(new Point(rectangle.Left, rectangle.Top)),
                new Size(rectangle.Width, rectangle.Height));
        }

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
                        throw new ArgumentOutOfRangeException(nameof(index));
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

                    throw new ArgumentOutOfRangeException(nameof(index));
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
                        throw new ArgumentOutOfRangeException(nameof(dockState));
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

        public class AutoHideStripsAccessibleObject : ControlAccessibleObject
        {
            private AutoHideStripBase _strip;

            public AutoHideStripsAccessibleObject(AutoHideStripBase strip)
                : base(strip)
            {
                _strip = strip;
            }

            public override AccessibleRole Role => AccessibleRole.Window;

            public override int GetChildCount()
            {
                // Top, Bottom, Left, Right
                return 4;
            }

            public override AccessibleObject GetChild(int index)
            {
                switch (index)
                {
                    case 0:
                        return new AutoHideStripAccessibleObject(_strip, DockState.DockTopAutoHide, this);
                    case 1:
                        return new AutoHideStripAccessibleObject(_strip, DockState.DockBottomAutoHide, this);
                    case 2:
                        return new AutoHideStripAccessibleObject(_strip, DockState.DockLeftAutoHide, this);
                    case 3:
                    default:
                        return new AutoHideStripAccessibleObject(_strip, DockState.DockRightAutoHide, this);
                }
            }

            public override AccessibleObject HitTest(int x, int y)
            {
                var rectangles = new Dictionary<DockState, Rectangle>
                {
                    {DockState.DockTopAutoHide, _strip.GetTabStripRectangle(DockState.DockTopAutoHide)},
                    {DockState.DockBottomAutoHide, _strip.GetTabStripRectangle(DockState.DockBottomAutoHide)},
                    {DockState.DockLeftAutoHide, _strip.GetTabStripRectangle(DockState.DockLeftAutoHide)},
                    {DockState.DockRightAutoHide, _strip.GetTabStripRectangle(DockState.DockRightAutoHide)}
                };

                var point = _strip.PointToClient(new Point(x, y));
                foreach (var rectangle in rectangles)
                    if (rectangle.Value.Contains(point))
                        return new AutoHideStripAccessibleObject(_strip, rectangle.Key, this);

                return null;
            }
        }

        public class AutoHideStripAccessibleObject : AccessibleObject
        {
            private DockState _state;
            private AutoHideStripBase _strip;

            public AutoHideStripAccessibleObject(AutoHideStripBase strip, DockState state, AccessibleObject parent)
            {
                _strip = strip;
                _state = state;

                Parent = parent;
            }

            public override AccessibleObject Parent { get; }

            public override AccessibleRole Role => AccessibleRole.PageTabList;

            public override Rectangle Bounds
            {
                get
                {
                    var rectangle = _strip.GetTabStripRectangle(_state);
                    return ToScreen(rectangle, _strip);
                }
            }

            public override int GetChildCount()
            {
                var count = 0;
                foreach (var pane in _strip.GetPanes(_state)) count += pane.AutoHideTabs.Count;
                return count;
            }

            public override AccessibleObject GetChild(int index)
            {
                var tabs = new List<Tab>();
                foreach (var pane in _strip.GetPanes(_state)) tabs.AddRange(pane.AutoHideTabs);

                return new AutoHideStripTabAccessibleObject(_strip, tabs[index], this);
            }
        }

        protected class AutoHideStripTabAccessibleObject : AccessibleObject
        {
            private AutoHideStripBase _strip;
            private Tab _tab;

            internal AutoHideStripTabAccessibleObject(AutoHideStripBase strip, Tab tab, AccessibleObject parent)
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