using System;

namespace Atiran.Utility.Docking
{
	public class DockContentEventArgs : EventArgs
	{
		private IDockContent m_content;

		public DockContentEventArgs(IDockContent content)
		{
			m_content = content;
		}

		public IDockContent Content
		{
			get	{	return m_content;	}
		}
	}
}
