using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Atiran.Utility.MassageBox;

namespace Atiran.Utility.Docking2.Desk
{
    public class DeskTab : DockContent
    {
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private List<Form> deskTabs;
        private bool isCanselCLoseAll;
        private bool isCLoseAll;
        private string m_fileName = string.Empty;

        private bool m_resetText = true;
        private ToolStripMenuItem miClose;
        private ToolStripMenuItem miCloseAll;
        private ToolStripMenuItem miCloseAllButThis;

        public bool ShowQuestionClose = false;

        public DeskTab()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document;
        }

        public string FileName
        {
            get => m_fileName;
            set
            {
                if (value != string.Empty)
                {
                    Stream s = new FileStream(value, FileMode.Open);

                    var efInfo = new FileInfo(value);

                    var fext = efInfo.Extension.ToUpper();

                    s.Close();
                }

                m_fileName = value;
                ToolTipText = value;
            }
        }

        private void InitializeComponent()
        {
            components = new Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            miClose = new ToolStripMenuItem();
            miCloseAllButThis = new ToolStripMenuItem();
            miCloseAll = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                miClose,
                miCloseAllButThis,
                miCloseAll
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(161, 70);
            // 
            // miClose
            // 
            miClose.Name = "miClose";
            miClose.Size = new Size(160, 22);
            miClose.Text = "بستن";
            miClose.Click += miClose_Click;
            // 
            // miCloseAllButThis
            // 
            miCloseAllButThis.Name = "miCloseAllButThis";
            miCloseAllButThis.Size = new Size(160, 22);
            miCloseAllButThis.Text = "بستن  ساير تب ها";
            miCloseAllButThis.Click += miCloseAllButThis_Click;
            // 
            // miCloseAll
            // 
            miCloseAll.Name = "miCloseAll";
            miCloseAll.Size = new Size(160, 22);
            miCloseAll.Text = "بستن همه";
            miCloseAll.Click += miCloseAll_Click;
            // 
            // DeskTab
            // 
            AutoScroll = true;
            ClientSize = new Size(519, 280);
            DockAreas = DockAreas.Document;
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular,
                GraphicsUnit.Point, 0);
            Name = "DeskTab";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            TabPageContextMenuStrip = contextMenuStrip1;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

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
            return GetType() + "," + FileName + "," + Text;
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            if (ShowQuestionClose)
            {
                if (ShowPersianMessageBox.ShowMessge("پيغام", "آيا تب " + Text + " بسته شود",
                        MessageBoxButtons.YesNo, false, false) == DialogResult.Yes)
                    Close();
            }
            else
            {
                Close();
            }
        }

        private void miCloseAllButThis_Click(object sender, EventArgs e)
        {
            deskTabs = ((Form) TopLevelControl).MdiChildren.ToList();
            foreach (DeskTab form in ((Form) TopLevelControl).MdiChildren)
                if (form != this && !isCanselCLoseAll)
                    TryClose(form, deskTabs.Where(f => f != form && f != this).ToArray());

            isCLoseAll = false;
            isCanselCLoseAll = false;
        }

        private void miCloseAll_Click(object sender, EventArgs e)
        {
            deskTabs = ((Form) TopLevelControl).MdiChildren.ToList();
            foreach (DeskTab form in ((Form) TopLevelControl).MdiChildren)
                if (!isCanselCLoseAll)
                    TryClose(form, deskTabs.Where(f => f != form).ToArray());
            isCLoseAll = false;
            isCanselCLoseAll = false;
        }

        private void TryClose(DeskTab form, Form[] forms)
        {
            if (form.ShowQuestionClose)
            {
                if (!isCLoseAll)
                {
                    var TextTabs = form.Text;
                    foreach (var tab in forms) TextTabs += "\n" + tab.Text;
                    var result = ShowPersianMessageBox.ShowMessge("آيا تب ها بسته شوند؟", TextTabs,
                        MessageBoxButtons.YesNo, false);
                    if (result == DialogResult.Yes)
                    {
                        form.Close();
                    }
                    else if (result == DialogResult.OK)
                    {
                        isCLoseAll = true;
                        form.Close();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        isCanselCLoseAll = true;
                    }
                }
                else
                {
                    form.Close();
                }
            }
            else
            {
                form.Close();
            }

            deskTabs.Remove(form);
        }
    }
}