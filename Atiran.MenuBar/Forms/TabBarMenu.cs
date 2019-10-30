using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Atiran.DataLayer.Context;
using Atiran.DataLayer.Model;
using Atiran.MenuBar.Class;
using Atiran.Utility.Docking;
using Menu = Atiran.DataLayer.Model.Menu;

namespace Atiran.MenuBar.Forms
{
    public class TabBarMenu : System.Windows.Forms.Form
    {
        //private DeskTab sh = new DeskTab();
        private Panel pnlMainButtons;
        List<Menu> menus = new List<Menu>();
        private Panel pnlFooter;
        private Panel pnlMain;
        private MenuStrip MyMnSt;
        private CustomTabControl MainTab;
        private TabPage tabPage1;
        private Utility.Panels.MainButton mainButton1;
        private Button button1;
        private DockPanel dockPanel2;
        List<SubSystem> subSystems = new List<SubSystem>();
        //private Image CloseImage = Resources.close_button;

        public TabBarMenu()
        {
            InitializeComponent();
            //Connection.GetVersion();
            Connection.SetMenu();
            this.menus = Connection.ResultAllMenu;
            this.subSystems = Connection.ResultAllSubSystem;
            FirstTurn();

            MainTab.Dock = DockStyle.Fill;
            MainTab.TabPages.Clear();



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
           DockPanelSkin dockPanelSkin1 = new DockPanelSkin();
            AutoHideStripSkin autoHideStripSkin1 = new AutoHideStripSkin();
            DockPanelGradient dockPanelGradient1 = new Atiran.Utility.Docking.DockPanelGradient();
            Atiran.Utility.Docking.TabGradient tabGradient1 = new Atiran.Utility.Docking.TabGradient();
            Atiran.Utility.Docking.DockPaneStripSkin dockPaneStripSkin1 = new Atiran.Utility.Docking.DockPaneStripSkin();
            Atiran.Utility.Docking.DockPaneStripGradient dockPaneStripGradient1 = new Atiran.Utility.Docking.DockPaneStripGradient();
            Atiran.Utility.Docking.TabGradient tabGradient2 = new Atiran.Utility.Docking.TabGradient();
            Atiran.Utility.Docking.DockPanelGradient dockPanelGradient2 = new Atiran.Utility.Docking.DockPanelGradient();
            Atiran.Utility.Docking.TabGradient tabGradient3 = new Atiran.Utility.Docking.TabGradient();
            Atiran.Utility.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new Atiran.Utility.Docking.DockPaneStripToolWindowGradient();
            Atiran.Utility.Docking.TabGradient tabGradient4 = new Atiran.Utility.Docking.TabGradient();
            Atiran.Utility.Docking.TabGradient tabGradient5 = new Atiran.Utility.Docking.TabGradient();
            Atiran.Utility.Docking.DockPanelGradient dockPanelGradient3 = new Atiran.Utility.Docking.DockPanelGradient();
            Atiran.Utility.Docking.TabGradient tabGradient6 = new Atiran.Utility.Docking.TabGradient();
            Atiran.Utility.Docking.TabGradient tabGradient7 = new Atiran.Utility.Docking.TabGradient();
            this.pnlMainButtons = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.mainButton1 = new Atiran.Utility.Panels.MainButton();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.dockPanel2 = new DockPanel();
            this.MainTab = new System.Windows.Forms.CustomTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MyMnSt = new System.Windows.Forms.MenuStrip();
            this.pnlMainButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMainButtons
            // 
            this.pnlMainButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(100)))), ((int)(((byte)(123)))));
            this.pnlMainButtons.Controls.Add(this.button1);
            this.pnlMainButtons.Controls.Add(this.mainButton1);
            this.pnlMainButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMainButtons.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.pnlMainButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlMainButtons.Name = "pnlMainButtons";
            this.pnlMainButtons.Size = new System.Drawing.Size(1200, 38);
            this.pnlMainButtons.TabIndex = 0;
            this.pnlMainButtons.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlMainButtons_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "ميزكار";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.dockPanel2);
            this.pnlMain.Controls.Add(this.MainTab);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.pnlMain.Location = new System.Drawing.Point(0, 42);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1200, 502);
            this.pnlMain.TabIndex = 3;
            // 
            // dockPanel2
            // 
            this.dockPanel2.ActiveAutoHideContent = null;
            this.dockPanel2.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel2.Location = new System.Drawing.Point(52, 32);
            this.dockPanel2.Name = "dockPanel2";
            this.dockPanel2.Size = new System.Drawing.Size(454, 331);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel2.TabIndex = 1;
            // 
            // MainTab
            // 
            this.MainTab.AllowDrop = true;
            this.MainTab.Controls.Add(this.tabPage1);
            this.MainTab.DisplayStyle = System.Windows.Forms.TabStyle.VisualStudio;
            // 
            // 
            // 
            this.MainTab.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.MainTab.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.MainTab.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.MainTab.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.MainTab.DisplayStyleProvider.FocusTrack = false;
            this.MainTab.DisplayStyleProvider.HotTrack = true;
            this.MainTab.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MainTab.DisplayStyleProvider.Opacity = 1F;
            this.MainTab.DisplayStyleProvider.Overlap = 7;
            this.MainTab.DisplayStyleProvider.Padding = new System.Drawing.Point(25, 1);
            this.MainTab.DisplayStyleProvider.ShowTabCloser = true;
            this.MainTab.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.MainTab.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.MainTab.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.MainTab.HotTrack = true;
            this.MainTab.Location = new System.Drawing.Point(700, 42);
            this.MainTab.Name = "MainTab";
            this.MainTab.RightToLeftLayout = true;
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(452, 171);
            this.MainTab.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(444, 140);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MyMnSt
            // 
            this.MyMnSt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(130)))), ((int)(((byte)(150)))));
            this.MyMnSt.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.MyMnSt.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.MyMnSt.Location = new System.Drawing.Point(0, 38);
            this.MyMnSt.Name = "MyMnSt";
            this.MyMnSt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MyMnSt.Size = new System.Drawing.Size(1200, 4);
            this.MyMnSt.TabIndex = 4;
            // 
            // TabBarMenu
            // 
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.pnlMain);
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
            this.pnlMain.ResumeLayout(false);
            this.MainTab.ResumeLayout(false);
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

        protected  override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                case (Keys.Alt | Keys.F4):
                    {
                        try
                        {
                            MainTab.TabPages.Remove(MainTab.TabPages[MainTab.SelectedIndex]);
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

            MainTab.TabPages.Add(name, text);
            int intextTab = MainTab.TabPages.Count - 1;
            MainTab.SelectTab(intextTab);

            var tabpage = MainTab.TabPages[intextTab];
            tabpage.BackColor = Color.White;
            tabpage.Controls.Add(control);
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

        #region handler Close Butten In Header TabPage

        //private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    for (var i = 0; i < this.MainTab.TabPages.Count; i++)
        //    {
        //        var tabRect = this.MainTab.GetTabRect(i);
        //        tabRect.Inflate(-2, -2);
        //        var CloseRect = new Rectangle(
        //            tabRect.Left,
        //            tabRect.Top + (tabRect.Height - 20) / 2,
        //            16,
        //            16);
        //        if (CloseRect.Contains(e.Location))
        //        {
        //            this.MainTab.TabPages.RemoveAt(i);
        //            break;
        //        }
        //    }
        //}
        //private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    var tabPage = this.MainTab.TabPages[e.Index];
        //    var tabRect = this.MainTab.GetTabRect(e.Index);
        //    tabRect.Inflate(0, 0);
        //    tabRect.Width += 20;
        //    var RTL = new Rectangle(
        //        MainTab.ClientRectangle.Width - tabRect.Width - tabRect.X,
        //        tabRect.Y,
        //        tabRect.Width,
        //        tabRect.Height);
        //    e.Graphics.DrawString("X", new Font(tabPage.Font.FontFamily,12), Brushes.Red, 
        //       (RTL.Right - 28),
        //        (RTL.Top + (RTL.Height - 30) / 2));
        //    TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
        //        tabRect, tabPage.ForeColor, TextFormatFlags.Right);
        //}

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //DeskTab sh = new DeskTab();
            //sh.Text = "ميزكار";
            //sh.Show(dockPanel2,DockState.DockRight);
        }
    }
}