using System;

namespace Atiran.Utility.Docking2
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