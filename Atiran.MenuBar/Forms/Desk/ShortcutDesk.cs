using Atiran.Utility.Docking2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atiran.MenuBar.Forms
{
    public class ShortcutDesk : DockContent
    {
        public ShortcutDesk()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ShortcutDesk
            // 
            this.ClientSize = new System.Drawing.Size(626, 272);
            this.DockAreas = ((DockAreas)((DockAreas.DockLeft | DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "ShortcutDesk";
            this.ShowHint = DockState.DockRight;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}
