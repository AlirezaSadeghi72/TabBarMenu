using System;

namespace Atiran.Utility.Docking
{
    public class DockContentEventArgs : EventArgs
    {
        public DockContentEventArgs(IDockContent content)
        {
            Content = content;
        }

        public IDockContent Content { get; }
    }
}