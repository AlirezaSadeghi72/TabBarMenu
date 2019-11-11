using System;
using System.ComponentModel;

namespace Atiran.Utility.Docking2
{
    public partial class DockPanel
    {
        private ThemeBase m_dockPanelTheme;

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelSkin_Description")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Obsolete("Use Theme.Skin instead.")]
        public DockPanelSkin Skin => null;

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelTheme")]
        public ThemeBase Theme
        {
            get => m_dockPanelTheme;
            set
            {
                var old = m_dockPanelTheme;
                if (value == null)
                {
                    m_dockPanelTheme = null;
                    return;
                }

                if (m_dockPanelTheme?.GetType() == value.GetType()) return;

                m_dockPanelTheme?.CleanUp(this);
                m_dockPanelTheme = value;
                m_dockPanelTheme.ApplyTo(this);
                m_dockPanelTheme.PostApply(this);
                if (old == null)
                {
                    AutoHideWindow = m_dockPanelTheme?.Extender.AutoHideWindowFactory.CreateAutoHideWindow(this);
                    AutoHideWindow.Visible = false;
                    AutoHideWindow.ActiveContentChanged += m_autoHideWindow_ActiveContentChanged;
                    SetAutoHideWindowParent();
                    LoadDockWindows();
                }
            }
        }
    }
}