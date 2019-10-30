using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.Utility.Docking2;

namespace Atiran.MenuBar.Forms
{
    public class DeskTab: DockContent
    {
        public DeskTab()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document;
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DeskTab
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(519, 280);
            this.DockAreas = Atiran.Utility.Docking2.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DeskTab";
            this.ResumeLayout(false);

        }

        private string m_fileName = string.Empty;
        public string FileName
        {
            get { return m_fileName; }
            set
            {
                if (value != string.Empty)
                {
                    Stream s = new FileStream(value, FileMode.Open);

                    FileInfo efInfo = new FileInfo(value);

                    string fext = efInfo.Extension.ToUpper();
                    
                    s.Close();
                }

                m_fileName = value;
                this.ToolTipText = value;
            }
        }

        private bool m_resetText = true;
        protected  override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_resetText)
            {
                m_resetText = false;
                FileName = FileName;
            }
        }

        protected  override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return GetType().ToString() + "," + FileName + "," + Text;
        }

        internal void Show(Utility.Docking.DockPanel dockPanel2, Utility.Docking.DockState dockRight)
        {
            throw new NotImplementedException();
        }
    }
}
