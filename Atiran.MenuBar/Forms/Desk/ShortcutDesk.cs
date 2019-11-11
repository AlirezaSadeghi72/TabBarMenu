using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Atiran.DataLayer.Context;
using Atiran.DataLayer.Services;
using Atiran.Reporting.BankAndChek.ChekPardakhti;
using Atiran.Utility.Docking2;
using Atiran.Utility.Docking2.Desk;

namespace Atiran.MenuBar.Forms
{
    public class ShortcutDesk : DockContent
    {
        private int _userID;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridView dataGridView1;
        private DockPanel MainTab1;
        private ToolStripMenuItem miAddEdit;
        private List<UserShortcut> shortcuts;
        private TextBox txtSearch;

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
            components = new Container();
            var dataGridViewCellStyle4 = new DataGridViewCellStyle();
            var dataGridViewCellStyle5 = new DataGridViewCellStyle();
            var dataGridViewCellStyle6 = new DataGridViewCellStyle();
            txtSearch = new TextBox();
            dataGridView1 = new DataGridView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            miAddEdit = new ToolStripMenuItem();
            ((ISupportInitialize) dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.Size = new Size(603, 28);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(224, 224, 224);
            dataGridViewCellStyle4.Font = new Font("IRANSans(FaNum)", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.HotTrack;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("IRANSans(FaNum)", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = Color.DodgerBlue;
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(0, 28);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RightToLeft = RightToLeft.Yes;
            dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle6;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(603, 244);
            dataGridView1.TabIndex = 6;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                miAddEdit
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(155, 26);
            // 
            // miAddEdit
            // 
            miAddEdit.Name = "miAddEdit";
            miAddEdit.Size = new Size(154, 22);
            miAddEdit.Text = "ويرايش / افزودن";
            miAddEdit.Click += miAddEdit_Click;
            // 
            // ShortcutDesk
            // 
            ClientSize = new Size(603, 272);
            Controls.Add(dataGridView1);
            Controls.Add(txtSearch);
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight;
            Font = new Font("IRANSans(FaNum)", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HideOnClose = true;
            Name = "ShortcutDesk";
            ShowHint = DockState.DockLeft;
            ShowInTaskbar = false;
            TabPageContextMenuStrip = contextMenuStrip1;
            ((ISupportInitialize) dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = shortcuts.Where(s => s.Text.Contains(txtSearch.Text)).ToList();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            var countRowGrid = dataGridView1.Rows.Count;
            if (countRowGrid > 0)
            {
                var rowIndexSelected = dataGridView1.SelectedRows[0].Index;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    {
                        if (rowIndexSelected - 1 >= 0)
                            dataGridView1.Rows[rowIndexSelected - 1].Cells["Text"].Selected = true;
                        else
                            dataGridView1.Rows[countRowGrid - 1].Cells["Text"].Selected = true;

                        break;
                    }
                    case Keys.Down:
                    {
                        if (rowIndexSelected + 1 == countRowGrid)
                            dataGridView1.Rows[0].Cells["Text"].Selected = true;
                        else
                            dataGridView1.Rows[rowIndexSelected + 1].Cells["Text"].Selected = true;

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
            var Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
            var Class = "ReportChekhayePardakhti";
            var typeName = Namespace + "." + Class;
            AddTab(dataGridView1.SelectedRows[0].Cells["Text"].Value.ToString(), typeName, true);
            //AddTab(dataGridView1.SelectedRows[0].Cells["Text"].Value.ToString(), typeName,false);
        }

        private void AddTab(string text, string typeName, bool ShowQuestionClose)
        {
            var control = (ReportChekhayePardakhti) GetObjectFromString(typeName);
            control.Dock = DockStyle.Fill;
            control.label1.Text = "لود كردن usercontrol مربوط به " + text;

            var sh = new DeskTab();
            sh.Text = text;
            sh.Controls.Add(control);
            sh.ShowQuestionClose = ShowQuestionClose;
            sh.Show(MainTab1);
        }

        private object GetObjectFromString(string str)
        {
            var arrStr = str.Split('.');
            var assembly = Assembly.Load(arrStr[0] + "." + arrStr[1]);
            if (assembly != null) return assembly.CreateInstance(str);

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