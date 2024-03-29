using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Atiran.Utility.Docking
{
    public sealed class NestedPaneCollection : ReadOnlyCollection<DockPane>
    {
        internal NestedPaneCollection(INestedPanesContainer container)
            : base(new List<DockPane>())
        {
            Container = container;
            VisibleNestedPanes = new VisibleNestedPaneCollection(this);
        }

        public INestedPanesContainer Container { get; }

        public VisibleNestedPaneCollection VisibleNestedPanes { get; }

        public DockState DockState => Container.DockState;

        public bool IsFloat => DockState == DockState.Float;

        internal void Add(DockPane pane)
        {
            if (pane == null)
                return;

            var oldNestedPanes = pane.NestedPanesContainer == null ? null : pane.NestedPanesContainer.NestedPanes;
            if (oldNestedPanes != null)
                oldNestedPanes.InternalRemove(pane);
            Items.Add(pane);
            if (oldNestedPanes != null)
                oldNestedPanes.CheckFloatWindowDispose();
        }

        private void CheckFloatWindowDispose()
        {
            if (Count == 0 && Container.DockState == DockState.Float)
            {
                var floatWindow = (FloatWindow) Container;
                if (!floatWindow.Disposing && !floatWindow.IsDisposed)
                    NativeMethods.PostMessage(((FloatWindow) Container).Handle, FloatWindow.WM_CHECKDISPOSE, 0, 0);
            }
        }

        /// <summary>
        ///     Switches a pane with its first child in the pane hierarchy. (The actual hiding happens elsewhere.)
        /// </summary>
        /// <param name="pane">Pane to switch</param>
        internal void SwitchPaneWithFirstChild(DockPane pane)
        {
            if (!Contains(pane))
                return;

            var statusPane = pane.NestedDockingStatus;
            DockPane lastNestedPane = null;
            for (var i = Count - 1; i > IndexOf(pane); i--)
                if (this[i].NestedDockingStatus.PreviousPane == pane)
                {
                    lastNestedPane = this[i];
                    break;
                }

            if (lastNestedPane != null)
            {
                var indexLastNestedPane = IndexOf(lastNestedPane);
                Items[IndexOf(pane)] = lastNestedPane;
                Items[indexLastNestedPane] = pane;
                var lastNestedDock = lastNestedPane.NestedDockingStatus;

                DockAlignment newAlignment;
                if (lastNestedDock.Alignment == DockAlignment.Left)
                    newAlignment = DockAlignment.Right;
                else if (lastNestedDock.Alignment == DockAlignment.Right)
                    newAlignment = DockAlignment.Left;
                else if (lastNestedDock.Alignment == DockAlignment.Top)
                    newAlignment = DockAlignment.Bottom;
                else
                    newAlignment = DockAlignment.Top;
                var newProportion = 1 - lastNestedDock.Proportion;

                lastNestedDock.SetStatus(this, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion);
                for (var i = indexLastNestedPane - 1; i > IndexOf(lastNestedPane); i--)
                {
                    var status = this[i].NestedDockingStatus;
                    if (status.PreviousPane == pane)
                        status.SetStatus(this, lastNestedPane, status.Alignment, status.Proportion);
                }

                statusPane.SetStatus(this, lastNestedPane, newAlignment, newProportion);
            }
        }

        internal void Remove(DockPane pane)
        {
            InternalRemove(pane);
            CheckFloatWindowDispose();
        }

        private void InternalRemove(DockPane pane)
        {
            if (!Contains(pane))
                return;

            var statusPane = pane.NestedDockingStatus;
            DockPane lastNestedPane = null;
            for (var i = Count - 1; i > IndexOf(pane); i--)
                if (this[i].NestedDockingStatus.PreviousPane == pane)
                {
                    lastNestedPane = this[i];
                    break;
                }

            if (lastNestedPane != null)
            {
                var indexLastNestedPane = IndexOf(lastNestedPane);
                Items.Remove(lastNestedPane);
                Items[IndexOf(pane)] = lastNestedPane;
                var lastNestedDock = lastNestedPane.NestedDockingStatus;
                lastNestedDock.SetStatus(this, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion);
                for (var i = indexLastNestedPane - 1; i > IndexOf(lastNestedPane); i--)
                {
                    var status = this[i].NestedDockingStatus;
                    if (status.PreviousPane == pane)
                        status.SetStatus(this, lastNestedPane, status.Alignment, status.Proportion);
                }
            }
            else
            {
                Items.Remove(pane);
            }

            statusPane.SetStatus(null, null, DockAlignment.Left, 0.5);
            statusPane.SetDisplayingStatus(false, null, DockAlignment.Left, 0.5);
            statusPane.SetDisplayingBounds(Rectangle.Empty, Rectangle.Empty, Rectangle.Empty);
        }

        public DockPane GetDefaultPreviousPane(DockPane pane)
        {
            for (var i = Count - 1; i >= 0; i--)
                if (this[i] != pane)
                    return this[i];

            return null;
        }
    }
}