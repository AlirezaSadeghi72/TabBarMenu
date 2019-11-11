using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using static Atiran.Utility.Docking2.DockPanel;
using static Atiran.Utility.Docking2.DockPanel.DockDragHandler;

namespace Atiran.Utility.Docking2
{
    public sealed class DockPanelExtender
    {
        private IAutoHideStripFactory m_autoHideStripFactory;

        private IAutoHideWindowFactory m_autoHideWindowFactory;

        private IDockPaneFactory m_dockPaneFactory;

        private IFloatWindowFactory m_floatWindowFactory;

        public IDockPaneFactory DockPaneFactory
        {
            get
            {
                if (m_dockPaneFactory == null) m_dockPaneFactory = new DefaultDockPaneFactory();

                return m_dockPaneFactory;
            }
            set => m_dockPaneFactory = value;
        }

        public IDockPaneSplitterControlFactory DockPaneSplitterControlFactory { get; set; }

        public IWindowSplitterControlFactory WindowSplitterControlFactory { get; set; }

        public IFloatWindowFactory FloatWindowFactory
        {
            get
            {
                if (m_floatWindowFactory == null) m_floatWindowFactory = new DefaultFloatWindowFactory();

                return m_floatWindowFactory;
            }
            set => m_floatWindowFactory = value;
        }

        public IDockWindowFactory DockWindowFactory { get; set; }

        public IDockPaneCaptionFactory DockPaneCaptionFactory { get; set; }

        public IDockPaneStripFactory DockPaneStripFactory { get; set; }

        public IAutoHideStripFactory AutoHideStripFactory
        {
            get => m_autoHideStripFactory;
            set
            {
                if (m_autoHideStripFactory == value) return;

                m_autoHideStripFactory = value;
            }
        }

        public IAutoHideWindowFactory AutoHideWindowFactory
        {
            get => m_autoHideWindowFactory;
            set
            {
                if (m_autoHideWindowFactory == value) return;

                m_autoHideWindowFactory = value;
            }
        }

        public IPaneIndicatorFactory PaneIndicatorFactory { get; set; }

        public IPanelIndicatorFactory PanelIndicatorFactory { get; set; }

        public IDockOutlineFactory DockOutlineFactory { get; set; }

        public IDockIndicatorFactory DockIndicatorFactory { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IDockPaneFactory
        {
            DockPane CreateDockPane(IDockContent content, DockState visibleState, bool show);

            [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
            DockPane CreateDockPane(IDockContent content, FloatWindow floatWindow, bool show);

            DockPane CreateDockPane(IDockContent content, DockPane previousPane, DockAlignment alignment,
                double proportion, bool show);

            [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
            DockPane CreateDockPane(IDockContent content, Rectangle floatWindowBounds, bool show);
        }

        public interface IDockPaneSplitterControlFactory
        {
            DockPane.SplitterControlBase CreateSplitterControl(DockPane pane);
        }

        public interface IWindowSplitterControlFactory
        {
            SplitterBase CreateSplitterControl(ISplitterHost host);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IFloatWindowFactory
        {
            FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane);
            FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds);
        }

        public interface IDockWindowFactory
        {
            DockWindow CreateDockWindow(DockPanel dockPanel, DockState dockState);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IDockPaneCaptionFactory
        {
            DockPaneCaptionBase CreateDockPaneCaption(DockPane pane);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IDockPaneStripFactory
        {
            DockPaneStripBase CreateDockPaneStrip(DockPane pane);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IAutoHideStripFactory
        {
            AutoHideStripBase CreateAutoHideStrip(DockPanel panel);
        }

        public interface IAutoHideWindowFactory
        {
            AutoHideWindowControl CreateAutoHideWindow(DockPanel panel);
        }

        public interface IPaneIndicatorFactory
        {
            IPaneIndicator CreatePaneIndicator(ThemeBase theme);
        }

        public interface IPanelIndicatorFactory
        {
            IPanelIndicator CreatePanelIndicator(DockStyle style, ThemeBase theme);
        }

        public interface IDockOutlineFactory
        {
            DockOutlineBase CreateDockOutline();
        }

        public interface IDockIndicatorFactory
        {
            DockIndicator CreateDockIndicator(DockDragHandler dockDragHandler);
        }

        #region DefaultDockPaneFactory

        private class DefaultDockPaneFactory : IDockPaneFactory
        {
            public DockPane CreateDockPane(IDockContent content, DockState visibleState, bool show)
            {
                return new DockPane(content, visibleState, show);
            }

            public DockPane CreateDockPane(IDockContent content, FloatWindow floatWindow, bool show)
            {
                return new DockPane(content, floatWindow, show);
            }

            public DockPane CreateDockPane(IDockContent content, DockPane prevPane, DockAlignment alignment,
                double proportion, bool show)
            {
                return new DockPane(content, prevPane, alignment, proportion, show);
            }

            public DockPane CreateDockPane(IDockContent content, Rectangle floatWindowBounds, bool show)
            {
                return new DockPane(content, floatWindowBounds, show);
            }
        }

        #endregion

        #region DefaultFloatWindowFactory

        private class DefaultFloatWindowFactory : IFloatWindowFactory
        {
            public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane)
            {
                return new FloatWindow(dockPanel, pane);
            }

            public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
            {
                return new FloatWindow(dockPanel, pane, bounds);
            }
        }

        #endregion
    }
}