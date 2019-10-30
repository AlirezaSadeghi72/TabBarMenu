using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Atiran.DataLayer.Context;
using Atiran.DataLayer.Model;
using Atiran.MenuBar.Class;
using Atiran.Utility.Docking2;
using Atiran.Utility.Docking2.Theme.ThemeVS2017;
using Menu = Atiran.DataLayer.Model.Menu;

namespace Atiran.MenuBar.Forms
{
    public class TabBarMenu : System.Windows.Forms.Form
    {
        //private DeskTab sh = new DeskTab();
        private Panel pnlMainButtons;
        List<Menu> menus = new List<Menu>();
        private Panel pnlFooter;
        private Utility.Panels.MainButton mainButton1;
        private ShortcutDesk sh1 = new ShortcutDesk();
        private VS2017LightTheme vS2017LightTheme1;
        private List<SubSystem> subSystems = new List<SubSystem>();
        private DockPanel MainTab;
        private MenuStrip MyMnSt;
        //private CustomTabControl MainTab;
        //private Image CloseImage = Resources.close_button;

        public TabBarMenu()
        {
            InitializeComponent();
            //Connection.GetVersion();
            Connection.SetMenu();
            this.menus = Connection.ResultAllMenu;
            this.subSystems = Connection.ResultAllSubSystem;
            FirstTurn();

            ToolStripMenuItem tmp = new ToolStripMenuItem();
            {
                tmp.Text = "ميزكار";
                tmp.Tag = new MyTag()
                    { MenuId = -2, FormId = 0, ParentId = -1 };
                tmp.RightToLeft = RightToLeft.Yes;
                tmp.ForeColor = System.Drawing.SystemColors.ButtonFace;
                tmp.BackColor = System.Drawing.Color.FromArgb(20, 130, 150);
                tmp.AutoSize = false;
                tmp.Size = new System.Drawing.Size(97, tmp.Height + 20);
                tmp.Font = new Font("IRANSans(FaNum)", 11);
                tmp.Click += Shortcut_Click;
            }
            MyMnSt.Items.Add(tmp);


            //MainTab = new CustomTabControl();
            //MainTab.Dock = DockStyle.Fill;
            //MainTab.DisplayStyle = TabStyle.Chrome;
            //MainTab.AllowDrop = true;
            //MainTab.RightToLeft = RightToLeft;
            //MainTab.RightToLeftLayout = true;

            

        }

        private void FirstTurn()
        {
            //ToolStripItem next = MyMnSt.Items.Add(Properties.Resources.next);
            //next.Click += Next_Click;
            ToolStripMenuItem tmp = new ToolStripMenuItem()
            {
                Tag = new MyTag() { MenuId = 0, FormId = 0, ParentId = -2 }
            };
            CreateMenus(tmp);
        }

        //private void Next_Click(object sender, EventArgs e)
        //{
        //    ToolStripItem[] tsc = new ToolStripItem[MyMnSt.Items.Count];
        //    MyMnSt.Items.CopyTo(tsc, 0);
        //    MyMnSt.Items.Clear();
        //    for (int i = 0; i < tsc.Length; i++)
        //    {
        //        if (i == 0)
        //            MyMnSt.Items.Add(tsc[0]);
        //        else
        //        {
        //            if (i == 1)
        //                MyMnSt.Items.Add(tsc[tsc.Length - 1]);
        //            else
        //                MyMnSt.Items.Add(tsc[i - 1]);
        //        }
        //    }
        //}

        private void InitializeComponent()
        {
            this.pnlMainButtons = new System.Windows.Forms.Panel();
            this.mainButton1 = new Atiran.Utility.Panels.MainButton();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.vS2017LightTheme1 = new Atiran.Utility.Docking2.Theme.ThemeVS2017.VS2017LightTheme();
            this.MainTab = new Atiran.Utility.Docking2.DockPanel();
            this.MyMnSt = new System.Windows.Forms.MenuStrip();
            this.pnlMainButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainTab)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMainButtons
            // 
            this.pnlMainButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(100)))), ((int)(((byte)(123)))));
            this.pnlMainButtons.Controls.Add(this.mainButton1);
            this.pnlMainButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMainButtons.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.pnlMainButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlMainButtons.Name = "pnlMainButtons";
            this.pnlMainButtons.Size = new System.Drawing.Size(1200, 38);
            this.pnlMainButtons.TabIndex = 0;
            this.pnlMainButtons.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlMainButtons_MouseDoubleClick);
            // 
            // mainButton1
            // 
            this.mainButton1.BackColor = System.Drawing.Color.Transparent;
            this.mainButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainButton1.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.mainButton1.ForeColor = System.Drawing.Color.White;
            this.mainButton1.Location = new System.Drawing.Point(0, 0);
            this.mainButton1.Name = "mainButton1";
            this.mainButton1.Size = new System.Drawing.Size(1200, 38);
            this.mainButton1.TabIndex = 0;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.pnlFooter.Location = new System.Drawing.Point(0, 544);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1200, 56);
            this.pnlFooter.TabIndex = 2;
            // 
            // MainTab
            // 
            this.MainTab.AllowDrop = true;
            this.MainTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.MainTab.Location = new System.Drawing.Point(0, 42);
            this.MainTab.Name = "MainTab";
            this.MainTab.Padding = new System.Windows.Forms.Padding(6);
            this.MainTab.RightToLeftLayout = true;
            this.MainTab.ShowAutoHideContentOnHover = false;
            this.MainTab.ShowDocumentIcon = true;
            this.MainTab.Size = new System.Drawing.Size(1200, 502);
            this.MainTab.TabIndex = 7;
            this.MainTab.Theme = this.vS2017LightTheme1;
            // 
            // MyMnSt
            // 
            this.MyMnSt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.MyMnSt.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.MyMnSt.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.MyMnSt.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.MyMnSt.Location = new System.Drawing.Point(0, 38);
            this.MyMnSt.Name = "MyMnSt";
            this.MyMnSt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MyMnSt.Size = new System.Drawing.Size(1200, 4);
            this.MyMnSt.TabIndex = 8;
            // 
            // TabBarMenu
            // 
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.MainTab);
            this.Controls.Add(this.MyMnSt);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlMainButtons);
            this.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Name = "TabBarMenu";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MyMenuItem_Load);
            this.pnlMainButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainTab)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void CreateMenus(ToolStripItem TStrip)
        {
            MyTag tag = ((MyTag)TStrip.Tag);
            ToolStripMenuItem tmp;
            if (tag.ParentId == -2)
            {
                foreach (SubSystem sub in subSystems)
                {
                    tmp = new ToolStripMenuItem();
                    {
                        tmp.Text = sub.Name;
                        tmp.Tag = new MyTag()
                        { MenuId = sub.SubSystemId, FormId = 0, ParentId = -1 };
                        tmp.RightToLeft = RightToLeft.Yes;
                        tmp.ForeColor = System.Drawing.SystemColors.ButtonFace;
                        tmp.BackColor = System.Drawing.Color.FromArgb(20, 130, 150);
                        tmp.AutoSize = false;
                        tmp.Size = new System.Drawing.Size(97, tmp.Height + 20);
                        tmp.Font = new Font("IRANSans(FaNum)", 11);
                    }
                    MyMnSt.Items.Add(tmp);
                    CreateMenus(tmp);
                }
            }
            else
            {
                IEnumerable<Menu> items;
                if (tag.ParentId == -1)
                    items = menus.Where(a => (a.ParentMenuID ?? 0) == 0 && (a.SubSystemID == tag.MenuId)).OrderBy(s => s.MenuID).OrderBy(s => s.order);
                else
                    items = menus.Where(a => (a.ParentMenuID ?? 0) == tag.MenuId).OrderBy(s => s.MenuID).OrderBy(s => s.order);
                foreach (var item in items)
                {
                    tmp = new ToolStripMenuItem();
                    {
                        tmp.Text = item.Text;
                        tmp.Tag = new MyTag()
                        { MenuId = item.MenuID, FormId = item.FormID ?? 0, ParentId = item.ParentMenuID ?? 0 };
                        tmp.RightToLeft = RightToLeft.Yes;
                        tmp.ForeColor = System.Drawing.SystemColors.ButtonFace;
                        tmp.BackColor = System.Drawing.Color.FromArgb(40, 130, 150);
                        tmp.Font = new Font("IRANSans(FaNum)", 11);
                    }
                    ((ToolStripMenuItem)TStrip).DropDownItems.Add(tmp);
                    if ((item.FormID ?? 0) > 0)
                        tmp.Click += Form_Click;
                    CreateMenus(tmp);
                }

                if (((ToolStripMenuItem)TStrip).DropDownItems.Count > 0)
                {
                    //((ToolStripMenuItem)TStrip).Image =
                    //    Properties.Resources.Expand;


                    ((ToolStripMenuItem)TStrip).MouseHover +=
                        MyMenuItem_MouseHover;
                    ((ToolStripMenuItem)TStrip).MouseLeave +=
                        MyMenuItem_MouseLeave;
                    if (tag.ParentId == -1)
                    {
                        ((ToolStripMenuItem)TStrip).MouseEnter +=
                            RootMenuItem_MouseEnter;
                    }
                    else
                    {
                        ((ToolStripMenuItem)TStrip).Alignment =
                            ToolStripItemAlignment.Left;
                        ((ToolStripMenuItem)TStrip).MouseEnter +=
                            MyMenuItem_MouseEnter;

                    }
                }
                else
                {
                    ((ToolStripMenuItem)TStrip).MouseEnter += MakeYellow;
                    ((ToolStripMenuItem)TStrip).MouseLeave += MakeBack;
                }
            }
        }

        private void Form_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("The Form whit Id " +
            //                ((MyTag)((ToolStripItem)sender).Tag).FormId +
            //                " has been hitted!");

            //((MyTag)((ToolStripItem)sender).Tag).
            //string typeName = mb.self.Form.NameSpace + "." + mb.self.Form.Class;
            string Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
            string Class = "ReportChekhayePardakhti";
            string typeName = Namespace + "." + Class;
            AddTab(((ToolStripItem)sender).Text, "tab" + ((MyTag)((ToolStripItem)sender).Tag).FormId, typeName);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                case (Keys.Alt | Keys.F4):
                    {
                        try
                        {
                            //MainTab.TabPages.Remove(MainTab.TabPages[MainTab.SelectedIndex]);
                            ActiveMdiChild.Close();
                        }
                        catch (Exception)
                        {
                            //CloseProgramm();
                            this.Close();
                        }
                    }
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //private void CloseProgramm()
        //{
        //    UI.WindowsForms.MessageBoxes.MessageBoxWarning.state = 0;
        //    DialogResult close =
        //        Atiran.UI.WindowsForms.MessageBoxes.CustomMessageForm.CustomMessageBox.Show("پيام سيستم",
        //            "آيا مي خواهيد از سيستم خارج شويد؟", "w");
        //    UI.WindowsForms.MessageBoxes.MessageBoxWarning.state = 1;
        //    if (close == DialogResult.Yes)
        //    {
        //        if (CheckBackupPermission.HavePermission())
        //        {
        //            DialogResult res =
        //                Atiran.UI.WindowsForms.MessageBoxes.CustomMessageForm.CustomMessageBox.Show("پيام سيستم",
        //                    "آيا مي خواهيد از اطلاعات پشتيبان بگيريد؟", "w");
        //            if (res == DialogResult.Yes)
        //            {
        //                // backup
        //                Atiran.BackupAndRestore.AtiranBackup c = new Atiran.BackupAndRestore.AtiranBackup(true);
        //                new Atiran.UI.WindowsForms.Shortcuts.UserControlLoader(c, true, false, true);
        //            }
        //        }

        //        // user Logouted
        //        Connections.UserService.UserLogouted(Connections.GetCurrentSysUser.Instance.user_id);
        //        Application.Exit();
        //    }
        //}

        private void MakeYellow(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).Image = Properties.Resources.selected;
        }

        private void MakeBack(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Image = null;
        }

        private void MyMenuItem_MouseHover(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ShowDropDown();
        }

        private void RootMenuItem_MouseEnter(object sender, EventArgs e)
        {

            // ((ToolStripMenuItem)sender).Image = Properties.Resources.expandDown;
        }

        private void MyMenuItem_MouseLeave(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).Image = Properties.Resources.Expand;
        }

        private void MyMenuItem_MouseEnter(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).Image = Properties.Resources.expandleft;
        }

        private void MyMenuItem_Load(object sender, EventArgs e)
        {
            MyMnSt.Renderer = new ToolStripProfessionalRendererAtiran();
            MyMnSt.BackColor = Color.FromArgb(20, 130, 150);
        }

        private void pnlMainButtons_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
        }



        #region me

        #region Method

        private void AddTab(string text, string name, string typeName)
        {
            var control = (Control)GetObjectFromString(typeName);
            control.Dock = DockStyle.Fill;

            //Label lbl = new Label()
            //{
            //    Text = name,
            //    Dock = DockStyle.Bottom
            //};

            DeskTab sh = new DeskTab();
            sh.Text = text;
            sh.Controls.Add(control);
            sh.Show(MainTab);

            //MainTab.TabPages.Add(name, text);
            //int intextTab = MainTab.TabPages.Count - 1;
            //MainTab.SelectTab(intextTab);

            //var tabpage = MainTab.TabPages[intextTab];
            //tabpage.BackColor = Color.White;
            //tabpage.Controls.Add(control);
        }

        private object GetObjectFromString(string str)
        {
            //Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.CreateInstance(str) != null)
            //    .FirstOrDefault();
            var arrStr = str.Split('.');
            Assembly assembly = Assembly.Load(arrStr[0] + "." + arrStr[1]);
            if (assembly != null)
            {
                return assembly.CreateInstance(str);
            }

            return new Control();
        }

        #endregion

        #region Event
        
        private void Shortcut_Click(object sender, EventArgs e)
        {
            sh1.Text = "ميزكار";
            sh1.Show(MainTab);
            //DeskTab sh = new DeskTab();
            //sh.Text = "ميزكار";
            //sh.Show(dockPanel2,DockState.DockRight);
        }

        #endregion

        #endregion
    }
}