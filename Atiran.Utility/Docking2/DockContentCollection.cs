using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atiran.Utility.Docking2
{
    public class DockContentCollection : ReadOnlyCollection<IDockContent>
    {
        private static List<IDockContent> _emptyList = new List<IDockContent>(0);

        internal DockContentCollection()
            : base(new List<IDockContent>())
        {
        }

        internal DockContentCollection(DockPane pane)
            : base(_emptyList)
        {
            DockPane = pane;
        }

        private DockPane DockPane { get; }

        public new IDockContent this[int index]
        {
            get
            {
                if (DockPane == null)
                    return Items[index];
                return GetVisibleContent(index);
            }
        }

        public new int Count
        {
            get
            {
                if (DockPane == null)
                    return base.Count;
                return CountOfVisibleContents;
            }
        }

        private int CountOfVisibleContents
        {
            get
            {
#if DEBUG
                if (DockPane == null)
                    throw new InvalidOperationException();
#endif

                var count = 0;
                foreach (var content in DockPane.Contents)
                    if (content.DockHandler.DockState == DockPane.DockState)
                        count++;
                return count;
            }
        }

        internal int Add(IDockContent content)
        {
#if DEBUG
            if (DockPane != null)
                throw new InvalidOperationException();
#endif

            if (Contains(content))
                return IndexOf(content);

            Items.Add(content);
            return Count - 1;
        }

        internal void AddAt(IDockContent content, int index)
        {
#if DEBUG
            if (DockPane != null)
                throw new InvalidOperationException();
#endif

            if (index < 0 || index > Items.Count - 1)
                return;

            if (Contains(content))
                return;

            Items.Insert(index, content);
        }

        public new bool Contains(IDockContent content)
        {
            if (DockPane == null)
                return Items.Contains(content);
            return GetIndexOfVisibleContents(content) != -1;
        }

        public new int IndexOf(IDockContent content)
        {
            if (DockPane == null)
            {
                if (!Contains(content))
                    return -1;
                return Items.IndexOf(content);
            }

            return GetIndexOfVisibleContents(content);
        }

        internal void Remove(IDockContent content)
        {
            if (DockPane != null)
                throw new InvalidOperationException();

            if (!Contains(content))
                return;

            Items.Remove(content);
        }

        private IDockContent GetVisibleContent(int index)
        {
#if DEBUG
            if (DockPane == null)
                throw new InvalidOperationException();
#endif

            var currentIndex = -1;
            foreach (var content in DockPane.Contents)
            {
                if (content.DockHandler.DockState == DockPane.DockState)
                    currentIndex++;

                if (currentIndex == index)
                    return content;
            }

            throw new ArgumentOutOfRangeException();
        }

        private int GetIndexOfVisibleContents(IDockContent content)
        {
#if DEBUG
            if (DockPane == null)
                throw new InvalidOperationException();
#endif

            if (content == null)
                return -1;

            var index = -1;
            foreach (var c in DockPane.Contents)
                if (c.DockHandler.DockState == DockPane.DockState)
                {
                    index++;

                    if (c == content)
                        return index;
                }

            return -1;
        }
    }
}