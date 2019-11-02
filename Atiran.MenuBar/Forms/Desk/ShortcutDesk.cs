using Atiran.Utility.Docking2;
using System;
using System.Collections.Generic;
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private List<UserShortcut> shortcuts;
        private List<ToolStripItem> AllItem;
        private DockPanel MainTab1;
        private int _userID;

        public ShortcutDesk(ref DockPanel ali, int UserID = 1)
        {
            InitializeComponent();
            MainTab1 = ali;
            _userID = UserID;
            shortcuts = Connection.GetUserShortcuts(UserID);
            CreateMenus();

        }

        private void InitializeComponent()
        {
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(603, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 20);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(603, 252);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ShortcutDesk
            // 
            this.ClientSize = new System.Drawing.Size(603, 272);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtSearch);
            this.DockAreas = ((Atiran.Utility.Docking2.DockAreas)((Atiran.Utility.Docking2.DockAreas.DockLeft | Atiran.Utility.Docking2.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ShortcutDesk";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowHint = Atiran.Utility.Docking2.DockState.DockRight;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CreateMenus()
        {
            foreach (var item in shortcuts)
            {
                ToolStripMenuItem tmp = new ToolStripMenuItem();
                {
                    tmp.Text = item.Text;
                    tmp.Tag = new MyTag()
                    {
                        MenuId = item.MenuID,
                        FormId = item.FormID
                    };
                    tmp.RightToLeft = RightToLeft.Yes;
                    tmp.ForeColor = System.Drawing.SystemColors.ButtonFace;
                    tmp.Size = new System.Drawing.Size(97, tmp.Height + 20);
                    tmp.Font = new Font("IRANSans(FaNum)", 11);
                    tmp.Click += Form_Click;

                }
                menuStrip1.Items.Add(tmp);
            }
            AllItem = menuStrip1.Items.Cast<ToolStripItem>().ToList();
        }

        private void Form_Click(object sender, EventArgs e)
        {
            //((MyTag)((ToolStripItem)sender).Tag).FormId
            string Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
            string Class = "ReportChekhayePardakhti";
            string typeName = Namespace + "." + Class;
            AddTab(((ToolStripItem) sender).Text, typeName);
        }

        #region Method

        private void AddTab(string text, string typeName)
        {
            var control = (Control) GetObjectFromString(typeName);
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
            menuStrip1.Items.Clear();
            shortcuts = Connection.GetUserShortcuts(_userID);
            CreateMenus();
        }

        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            menuStrip1.Items.Clear();
            menuStrip1.Items.AddRange(AllItem.Where(a=>a.Text.Contains(txtSearch.Text)).ToArray());
        }
    }
}
