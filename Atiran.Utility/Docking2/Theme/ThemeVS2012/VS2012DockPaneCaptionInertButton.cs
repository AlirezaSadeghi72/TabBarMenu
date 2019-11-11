using System.ComponentModel;
using System.Drawing;

namespace Atiran.Utility.Docking2.Theme.ThemeVS2012
{
    [ToolboxItem(false)]
    public class VS2012DockPaneCaptionInertButton : InertButtonBase
    {
        private Bitmap _active;
        private Bitmap _autoHide;
        private Bitmap _hovered;
        private Bitmap _hoveredActive;
        private Bitmap _hoveredAutoHide;
        private Bitmap _normal;
        private Bitmap _pressed;
        private Bitmap _pressedAutoHide;

        public VS2012DockPaneCaptionInertButton(DockPaneCaptionBase dockPaneCaption, Bitmap hovered, Bitmap normal,
            Bitmap pressed, Bitmap hoveredActive, Bitmap active, Bitmap hoveredAutoHide = null, Bitmap autoHide = null,
            Bitmap pressedAutoHide = null)
        {
            DockPaneCaption = dockPaneCaption;
            _hovered = hovered;
            _normal = normal;
            _pressed = pressed;
            _hoveredActive = hoveredActive;
            _active = active;
            _hoveredAutoHide = hoveredAutoHide ?? hoveredActive;
            _autoHide = autoHide ?? active;
            _pressedAutoHide = pressedAutoHide ?? pressed;
            RefreshChanges();
        }

        private DockPaneCaptionBase DockPaneCaption { get; }

        public bool IsAutoHide => DockPaneCaption.DockPane.IsAutoHide;

        public bool IsActive => DockPaneCaption.DockPane.IsActivePane;

        public override Bitmap Image => IsActive ? IsAutoHide ? _autoHide : _active : _normal;

        public override Bitmap HoverImage => IsActive ? IsAutoHide ? _hoveredAutoHide : _hoveredActive : _hovered;

        public override Bitmap PressImage => IsAutoHide ? _pressedAutoHide : _pressed;
    }
}