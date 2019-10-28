using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atiran.Utility.Docking;

namespace Atiran.MenuBar.Forms
{
    public class DeskTab: DockContent
    {
        public DeskTab()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DeskTab
            // 
            this.ClientSize = new System.Drawing.Size(519, 280);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DeskTab";
            this.ResumeLayout(false);

        }
    }
}
