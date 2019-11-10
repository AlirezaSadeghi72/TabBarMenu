using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.Utility.Docking2;

namespace Atiran.Utility.Docking2.Desk
{
    public class DeskTab : DockContent
    {
        public DeskTab()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document;
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miClose,
            this.miCloseAllButThis,
            this.miCloseAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 70);
            // 
            // miClose
            // 
            this.miClose.Name = "miClose";
            this.miClose.Size = new System.Drawing.Size(160, 22);
            this.miClose.Text = "بستن";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // miCloseAllButThis
            // 
            this.miCloseAllButThis.Name = "miCloseAllButThis";
            this.miCloseAllButThis.Size = new System.Drawing.Size(160, 22);
            this.miCloseAllButThis.Text = "بستن  ساير تب ها";
            this.miCloseAllButThis.Click += new System.EventHandler(this.miCloseAllButThis_Click);
            // 
            // miCloseAll
            // 
            this.miCloseAll.Name = "miCloseAll";
            this.miCloseAll.Size = new System.Drawing.Size(160, 22);
            this.miCloseAll.Text = "بستن همه";
            this.miCloseAll.Click += new System.EventHandler(this.miCloseAll_Click);
            // 
            // DeskTab
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(519, 280);
            this.DockAreas = Atiran.Utility.Docking2.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DeskTab";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.TabPageContextMenuStrip = this.contextMenuStrip1;
            //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeskTab_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public bool isQuestionClose = false;
        private string m_fileName = string.Empty;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem miClose;
        private ToolStripMenuItem miCloseAllButThis;
        private ToolStripMenuItem miCloseAll;

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
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_resetText)
            {
                m_resetText = false;
                FileName = FileName;
            }
        }

        protected override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return GetType().ToString() + "," + FileName + "," + Text;
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            TryClose(this);
        }

        private void miCloseAllButThis_Click(object sender, EventArgs e)
        {
            foreach (DeskTab form in ((Form)TopLevelControl).MdiChildren)
            {
                if (form != this)
                    TryClose(form);
            }
        }

        private void miCloseAll_Click(object sender, EventArgs e)
        {
            foreach (DeskTab form in ((Form)TopLevelControl).MdiChildren)
            {
                TryClose(form);
            }
        }

        private void TryClose(DeskTab form)
        {
            if (form.isQuestionClose)
            {
                if (MessageBox.Show("آيا تب " + Text + " بسته شود",
                        "هشدار",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    form.Close();
                }
            }
            else
            {
                form.Close();
            }
        }

        //private void DeskTab_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (isQuestionClose)
        //    {
        //        e.Cancel = (MessageBox.Show("آيا تب " + Text + " بسته شود",
        //                        "هشدار",
        //                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
        //    }

        //}
    }
}
