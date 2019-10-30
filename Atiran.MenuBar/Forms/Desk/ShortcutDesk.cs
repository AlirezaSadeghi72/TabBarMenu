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
            this.DockAreas = ((Atiran.Utility.Docking2.DockAreas)((Atiran.Utility.Docking2.DockAreas.DockLeft | Atiran.Utility.Docking2.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "ShortcutDesk";
            this.ShowHint = Atiran.Utility.Docking2.DockState.DockRight;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}
