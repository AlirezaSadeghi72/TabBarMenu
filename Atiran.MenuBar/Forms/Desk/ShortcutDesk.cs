using Atiran.Utility.Docking2;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.DataLayer.Context;
using Atiran.DataLayer.Services;
using Atiran.MenuBar.Class;
using Atiran.Reporting.BankAndChek.ChekPardakhti;

namespace Atiran.MenuBar.Forms
{
    public class ShortcutDesk : DockContent
    {
        private TextBox txtSearch;
        private List<UserShortcut> shortcuts;
        private DockPanel MainTab1;
        private DataGridView dataGridView1;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem miAddEdit;
        private int _userID;

        public ShortcutDesk(ref DockPanel dockPanel, int UserID = 1)
        {
            InitializeComponent();
            MainTab1 = dockPanel;
            _userID = UserID;

            shortcuts = Connection.GetUserShortcuts(UserID);
            dataGridView1.DataSource = shortcuts;
            SetGrid();

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddEdit = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.Size = new System.Drawing.Size(603, 28);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("IRANSans(FaNum)", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("IRANSans(FaNum)", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(603, 244);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddEdit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 26);
            // 
            // miAddEdit
            // 
            this.miAddEdit.Name = "miAddEdit";
            this.miAddEdit.Size = new System.Drawing.Size(154, 22);
            this.miAddEdit.Text = "ويرايش / افزودن";
            this.miAddEdit.Click += new System.EventHandler(this.miAddEdit_Click);
            // 
            // ShortcutDesk
            // 
            this.ClientSize = new System.Drawing.Size(603, 272);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtSearch);
            this.DockAreas = ((Atiran.Utility.Docking2.DockAreas)((Atiran.Utility.Docking2.DockAreas.DockLeft | Atiran.Utility.Docking2.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("IRANSans(FaNum)", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "ShortcutDesk";
            this.ShowHint = Atiran.Utility.Docking2.DockState.DockLeft;
            this.ShowInTaskbar = false;
            this.TabPageContextMenuStrip = this.contextMenuStrip1;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = shortcuts.Where(s => s.Text.Contains(txtSearch.Text)).ToList();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            int countRowGrid = dataGridView1.Rows.Count;
            if (countRowGrid > 0)
            {
                int rowIndexSelected = dataGridView1.SelectedRows[0].Index;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        {

                            if (rowIndexSelected - 1 >= 0)
                            {
                                dataGridView1.Rows[rowIndexSelected - 1].Cells["Text"].Selected = true;
                            }
                            else
                            {
                                dataGridView1.Rows[countRowGrid - 1].Cells["Text"].Selected = true;
                            }

                            break;
                        }
                    case Keys.Down:
                        {

                            if (rowIndexSelected + 1 == countRowGrid)
                            {
                                dataGridView1.Rows[0].Cells["Text"].Selected = true;
                            }
                            else
                            {
                                dataGridView1.Rows[rowIndexSelected + 1].Cells["Text"].Selected = true;
                            }

                            break;
                        }
                }
            }
        }
        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenTab();
        }

        private void miAddEdit_Click(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    {
                        if (dataGridView1.Rows.Count > 0)
                            OpenTab();
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Method

        private void SetGrid()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.Visible = false;

            dataGridView1.Columns["Text"].Visible = true;
            dataGridView1.Columns["Text"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Text"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns["Text"].HeaderText = "نام";

            dataGridView1.Columns["Ico"].Visible = true;
            dataGridView1.Columns["Ico"].Width = 30;
            dataGridView1.Columns["Ico"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["Ico"].HeaderText = "ايكون";

        }

        private void OpenTab()
        {
            //(int)dataGridView1.SelectedRows[0].Cells["FormId"].Value
            string Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
            string Class = "ReportChekhayePardakhti";
            string typeName = Namespace + "." + Class;
            AddTab(dataGridView1.SelectedRows[0].Cells["Text"].Value.ToString(), typeName);
        }

        private void AddTab(string text, string typeName)
        {
            var control = (ReportChekhayePardakhti)GetObjectFromString(typeName);
            control.Dock = DockStyle.Fill;
            control.label1.Text = "لود كردن usercontrol مربوط به " + text;

            DeskTab sh = new DeskTab();
            sh.Text = text;
            sh.Controls.Add(control);
            sh.Show(MainTab1);
        }

        private object GetObjectFromString(string str)
        {
            var arrStr = str.Split('.');
            Assembly assembly = Assembly.Load(arrStr[0] + "." + arrStr[1]);
            if (assembly != null)
            {
                return assembly.CreateInstance(str);
            }

            return new Control();
        }

        public void RelodeForm()
        {
            shortcuts = Connection.GetUserShortcuts(_userID);
            dataGridView1.DataSource = shortcuts;
            SetGrid();
        }


        #endregion

    }
}
