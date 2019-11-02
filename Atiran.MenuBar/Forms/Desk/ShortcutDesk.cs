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

namespace Atiran.MenuBar.Forms
{
    public class ShortcutDesk : DockContent
    {
        private System.Windows.Forms.TextBox txtSearch;
        private List<UserShortcut> shortcuts;
        private List<ListViewItem> AllItem;
        private DockPanel MainTab1;
        private DataGridView dataGridView1;
        private int _userID;

        public ShortcutDesk(ref DockPanel ali, int UserID = 1)
        {
            InitializeComponent();
            MainTab1 = ali;
            _userID = UserID;
            shortcuts = Connection.GetUserShortcuts(UserID);
            dataGridView1.DataSource = shortcuts;
            SetGrid();
            //CreateMenus();

        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.Size = new System.Drawing.Size(603, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 20);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(603, 252);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // ShortcutDesk
            // 
            this.ClientSize = new System.Drawing.Size(603, 272);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtSearch);
            this.DockAreas = ((Atiran.Utility.Docking2.DockAreas)((Atiran.Utility.Docking2.DockAreas.DockLeft | Atiran.Utility.Docking2.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "ShortcutDesk";
            this.ShowHint = Atiran.Utility.Docking2.DockState.DockRight;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private void Form_Click(object sender, EventArgs e)
        //{
        //    //((MyTag)((ToolStripItem)sender).Tag).FormId
        //    string Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
        //    string Class = "ReportChekhayePardakhti";
        //    string typeName = Namespace + "." + Class;
        //    AddTab(((ToolStripItem)sender).Text, typeName);
        //}

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //menuStrip1.Items.Clear();
            //menuStrip1.Items.AddRange(AllItem.Where(a=>a.Text.Contains(txtSearch.Text)).ToArray());
            //listView1.Items.Clear();
            //listView1.Items.AddRange(AllItem.Where(a => a.Text.Contains(txtSearch.Text)).ToArray());
            dataGridView1.DataSource = shortcuts.Where(s => s.Text.Contains(txtSearch.Text)).ToList();
            //try
            //{
            //    listView1.Items[0].Selected = true;
            //}
            //catch (Exception)
            //{
            //}
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
                            //if (dataGridView1.SelectedItems.Count > 0)
                            //{
                            //    try
                            //    {
                            //        listView1.Items[listView1.SelectedItems[0].Index - 1].Selected = true;
                            //    }
                            //    catch (Exception)
                            //    {
                            //        if (listView1.Items.Count > 0)
                            //            listView1.Items[listView1.Items.Count - 1].Selected = true;
                            //    }
                            //}
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
                            //if (listView1.SelectedItems.Count > 0)
                            //{
                            //    try
                            //    {
                            //        listView1.Items[listView1.SelectedItems[0].Index + 1].Selected = true;
                            //    }
                            //    catch (Exception)
                            //    {
                            //        if (listView1.Items.Count > 0)
                            //            listView1.Items[0].Selected = true;
                            //    }
                            //}
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
                    case Keys.Enter:
                        {
                            OpenTab();
                            break;
                        }
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenTab();
            }
        }

        #region Method

        private void CreateMenus()
        {
            //foreach (var item in shortcuts)
            //{
            //    ToolStripMenuItem tmp = new ToolStripMenuItem();
            //    {
            //        tmp.Text = item.Text;
            //        tmp.Tag = new MyTag()
            //        {
            //            MenuId = item.MenuID,
            //            FormId = item.FormID
            //        };
            //        tmp.RightToLeft = RightToLeft.Yes;
            //        tmp.ForeColor = System.Drawing.SystemColors.ButtonFace;
            //        tmp.Size = new System.Drawing.Size(97, tmp.Height + 20);
            //        tmp.Font = new Font("IRANSans(FaNum)", 11);
            //        tmp.Click += Form_Click;

            //    }
            //    menuStrip1.Items.Add(tmp);
            //}
            //AllItem = menuStrip1.Items.Cast<ToolStripItem>().ToList();


        }

        private void SetGrid()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns) col.Visible = false;

            dataGridView1.Columns["Text"].Visible = true;
            dataGridView1.Columns["Text"].HeaderText = "نام";
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
            var control = (Control)GetObjectFromString(typeName);
            control.Dock = DockStyle.Fill;


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
            //listView1.Items.Clear();
            shortcuts = Connection.GetUserShortcuts(_userID);
            dataGridView1.DataSource = shortcuts;
            SetGrid();

            //CreateMenus();
        }


        #endregion
    }
}
