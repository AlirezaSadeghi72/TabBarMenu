using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Atiran.DataLayer.Context;
using Atiran.DataLayer.Model;
using Atiran.MenuBar.Class;
using Atiran.MenuBar.Panels;
using Atiran.MenuBar.Properties;
using Atiran.Reporting.BankAndChek.ChekPardakhti;
using Atiran.Utility.Docking2;
using Atiran.Utility.Docking2.Desk;
using Atiran.Utility.Docking2.Theme.ThemeVS2012;
using Atiran.Utility.Docking2.Theme.ThemeVS2017;
using Atiran.Utility.MassageBox;
using Form = System.Windows.Forms.Form;
using Menu = Atiran.DataLayer.Model.Menu;

namespace Atiran.MenuBar.Forms
{
    public class TabBarMenu : Form
    {
        private int _salMaliID = 1;
        private int _userID = 1005;
        private List<Form> deskTabs;
        private bool isCanselCLoseAll;
        private bool isCLoseAll;
        private MainButton mainButton1;
        private DockPanel MainTab;
        private List<Menu> menus = new List<Menu>();
        private MenuStrip MyMnSt;
        private Panel pnlFooter;
        private Panel pnlMainButtons;
        private ShortcutDesk sh1;
        private List<SubSystem> subSystems = new List<SubSystem>();
        private VS2012LightTheme vS2012LightTheme1;
        private VS2017LightTheme vS2017LightTheme1;


        public TabBarMenu()
        {
            InitializeComponent();
            sh1 = new ShortcutDesk(ref MainTab);
            //Connection.GetVersion();
            Connection.SetMenu();
            menus = Connection.ResultAllMenu;
            subSystems = Connection.ResultAllSubSystem;
            FirstTurn();

            mainButton1.UserID = _userID;
            mainButton1.SalMaliID = _salMaliID;

        }

        private void InitializeComponent()
        {
            pnlMainButtons = new Panel();
            mainButton1 = new MainButton();
            pnlFooter = new Panel();
            MyMnSt = new MenuStrip();
            MainTab = new DockPanel();
            vS2017LightTheme1 = new VS2017LightTheme();
            vS2012LightTheme1 = new VS2012LightTheme();
            pnlMainButtons.SuspendLayout();
            ((ISupportInitialize) MainTab).BeginInit();
            SuspendLayout();
            // 
            // pnlMainButtons
            // 
            pnlMainButtons.BackColor = Color.FromArgb(21, 100, 123);
            pnlMainButtons.Controls.Add(mainButton1);
            pnlMainButtons.Dock = DockStyle.Top;
            pnlMainButtons.Font = new Font("IRANSans(FaNum)", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, 178);
            pnlMainButtons.Location = new Point(0, 0);
            pnlMainButtons.Name = "pnlMainButtons";
            pnlMainButtons.Size = new Size(1200, 38);
            pnlMainButtons.TabIndex = 0;
            pnlMainButtons.MouseDoubleClick += pnlMainButtons_MouseDoubleClick;
            // 
            // mainButton1
            // 
            mainButton1.BackColor = Color.Transparent;
            mainButton1.Cursor = Cursors.Default;
            mainButton1.Dock = DockStyle.Fill;
            mainButton1.Font = new Font("IRANSans(FaNum)", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, 178);
            mainButton1.ForeColor = Color.White;
            mainButton1.Location = new Point(0, 0);
            mainButton1.Name = "mainButton1";
            mainButton1.RightToLeft = RightToLeft.Yes;
            mainButton1.Size = new Size(1200, 38);
            mainButton1.TabIndex = 0;
            // 
            // pnlFooter
            // 
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Font = new Font("IRANSans(FaNum)", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, 178);
            pnlFooter.Location = new Point(0, 515);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1200, 56);
            pnlFooter.TabIndex = 2;
            // 
            // MyMnSt
            // 
            MyMnSt.AutoSize = false;
            MyMnSt.BackColor = Color.FromArgb(20, 130, 150);
            MyMnSt.Font = new Font("IRANSans(FaNum)", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, 178);
            MyMnSt.LayoutStyle = ToolStripLayoutStyle.Flow;
            MyMnSt.Location = new Point(0, 38);
            MyMnSt.Name = "MyMnSt";
            MyMnSt.RightToLeft = RightToLeft.Yes;
            MyMnSt.Size = new Size(1200, 36);
            MyMnSt.TabIndex = 8;
            // 
            // MainTab
            // 
            MainTab.AllowDrop = true;
            MainTab.Dock = DockStyle.Fill;
            MainTab.DockBackColor = Color.FromArgb(238, 238, 242);
            MainTab.DockLeftPortion = 0.15D;
            MainTab.DockRightPortion = 0.15D;
            MainTab.Font = new Font("IRANSans", 11F, FontStyle.Bold, GraphicsUnit.Point, 178);
            MainTab.Location = new Point(0, 74);
            MainTab.Name = "MainTab";
            MainTab.Padding = new Padding(6);
            MainTab.RightToLeftLayout = true;
            MainTab.ShowAutoHideContentOnHover = false;
            MainTab.ShowDocumentIcon = true;
            MainTab.Size = new Size(1200, 441);
            MainTab.TabIndex = 7;
            MainTab.TabStop = true;
            MainTab.Theme = vS2017LightTheme1;
            // 
            // TabBarMenu
            // 
            ClientSize = new Size(1200, 571);
            Controls.Add(MainTab);
            Controls.Add(MyMnSt);
            Controls.Add(pnlFooter);
            Controls.Add(pnlMainButtons);
            Font = new Font("IRANSans", 9F, FontStyle.Regular, GraphicsUnit.Point, 178);
            FormBorderStyle = FormBorderStyle.None;
            IsMdiContainer = true;
            Name = "TabBarMenu";
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormClosing += TabBarMenu_FormClosing;
            FormClosed += TabBarMenu_FormClosed;
            Load += MyMenuItem_Load;
            pnlMainButtons.ResumeLayout(false);
            ((ISupportInitialize) MainTab).EndInit();
            ResumeLayout(false);
        }

        #region Event

        private void Form_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("The Form whit Id " +
            //                ((MyTag)((ToolStripItem)sender).Tag).FormId +
            //                " has been hitted!");

            //((MyTag)((ToolStripItem)sender).Tag).
            //string typeName = mb.self.Form.NameSpace + "." + mb.self.Form.Class;
            var Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
            var Class = "ReportChekhayePardakhti";
            var typeName = Namespace + "." + Class;
            var ali = ((ToolStripMenuItem) MyMnSt.Items[7]).DropDown.Items.Cast<ToolStripMenuItem>().ToList();
            if (ali.Any(a => a.Text == ((ToolStripItem) sender).Text))
                AddTab(((ToolStripItem) sender).Text, typeName, false);
            else
                AddTab(((ToolStripItem) sender).Text, typeName, true);
            //AddTab(((ToolStripItem)sender).Text, typeName, false);
        }

        private void MakeYellow(object sender, EventArgs e)
        {
            ((ToolStripMenuItem) sender).Image = Resources.Yellow;
        }

        private void MakeBack(object sender, EventArgs e)
        {
            ((ToolStripMenuItem) sender).Image = Resources.LemonChiffon;
        }

        private void ItemMenuStrip_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var DropDownItems = ((ToolStripDropDownMenu) sender).Items.Cast<ToolStripMenuItem>().ToList();
            if (DropDownItems.Any(a => a.Selected && a.DropDown.Items.Count == 0) && e.KeyCode == Keys.Left)
            {
                var RootItems = MyMnSt.Items.Cast<ToolStripMenuItem>().Where(t => ((MyTag) t.Tag).ParentId < 0)
                    .ToList();
                var RootPressItem = RootItems
                    .Where(t => t.Pressed && t.DropDown.Visible).ToList();
                if (RootPressItem.Count() > 0)
                {
                    var index = RootItems.IndexOf(RootPressItem[0]);

                    if (RootItems[(index + 1) % RootItems.Count].DropDownItems.Count > 0)
                    {
                        RootItems[(index + 1) % RootItems.Count].DropDown.Show();
                        RootItems[(index + 1) % RootItems.Count].DropDown.Focus();
                    }
                    else
                    {
                        var index1 = MyMnSt.Items.Cast<ToolStripMenuItem>().ToList()
                            .IndexOf(RootItems[(index + 1) % RootItems.Count]);
                        MyMnSt.Focus();
                        MyMnSt.Items[index1].Select();
                    }
                }
            }
        }

        private void ItemMenuStrip_MouseHover(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).ShowDropDown();
        }

        private void RootMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ((ToolStripMenuItem) sender).Image = Resources.expandDown;
        }

        private void RootMenuItem_MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripMenuItem) sender).Image = Resources.expandleft;
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                case Keys.Alt | Keys.F4:
                {
                    try
                    {
                        if (((DeskTab) ActiveMdiChild).ShowQuestionClose)
                        {
                            if (ShowPersianMessageBox.ShowMessge("پيغام",
                                    "آيا تب " + ((DeskTab) ActiveMdiChild).Text + " بسته شود",
                                    MessageBoxButtons.YesNo, false, false) == DialogResult.Yes)
                                ActiveMdiChild.Close();
                        }
                        else
                        {
                            ActiveMdiChild.Close();
                        }
                    }
                    catch (Exception)
                    {
                        Close();
                    }

                    return true;
                }
                case Keys.Home:
                {
                    ((ToolStripMenuItem) MyMnSt.Items[0]).ShowDropDown();
                    return true;
                }
                case Keys.Scroll:
                {
                    try
                    {
                        MainTab.NextTabFocus();
                    }
                    catch (Exception)
                    {
                        break;
                    }

                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TabBarMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void TabBarMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            deskTabs = MdiChildren.ToList();
            foreach (DeskTab form in MdiChildren)
                if (!isCanselCLoseAll)
                    TryClose(form, deskTabs.Where(f => f != form).ToArray());

            isCLoseAll = false;
            isCanselCLoseAll = false;

            if (MdiChildren.Length > 0)
            {
                e.Cancel = true;
            }
            else
            {
                if (!CloseProgramm()) e.Cancel = true;
            }
        }

        #endregion

        #region Method

        private void FirstTurn()
        {
            //ToolStripItem next = MyMnSt.Items.Add(Properties.Resources.next);
            //next.Click += Next_Click;
            var tmp = new ToolStripMenuItem
            {
                Tag = new MyTag {MenuId = 0, FormId = 0, ParentId = -2}
            };
            CreateMenus(tmp);
        }

        public void CreateMenus(ToolStripItem TStrip)
        {
            var tag = (MyTag) TStrip.Tag;
            ToolStripMenuItem tmp;
            if (tag.ParentId == -2)
            {
                foreach (var sub in subSystems)
                {
                    tmp = new ToolStripMenuItem();
                    {
                        tmp.Text = sub.Name;
                        tmp.Tag = new MyTag {MenuId = sub.SubSystemId, FormId = 0, ParentId = -1};
                        tmp.ForeColor = SystemColors.ButtonFace;
                        tmp.BackColor = Color.FromArgb(20, 130, 150);
                        tmp.Font = new Font("IRANSans(FaNum)", 11);
                        tmp.Image = Resources.LemonChiffon;
                        tmp.ImageScaling = ToolStripItemImageScaling.None;
                        tmp.TextImageRelation = TextImageRelation.ImageBeforeText;
                        //tmp.RightToLeftAutoMirrorImage = true;
                        tmp.Height = MyMnSt.Height;
                        tmp.CheckedChanged +=
                            ItemMenuStrip_MouseHover;
                    }

                    MyMnSt.Items.Add(tmp);
                    CreateMenus(tmp);
                }
            }
            else
            {
                IEnumerable<Menu> items;
                if (tag.ParentId == -1)
                    items = menus.Where(a => (a.ParentMenuID ?? 0) == 0 && a.SubSystemID == tag.MenuId)
                        .OrderBy(s => s.MenuID).OrderBy(s => s.order);
                else
                    items = menus.Where(a => (a.ParentMenuID ?? 0) == tag.MenuId).OrderBy(s => s.MenuID)
                        .OrderBy(s => s.order);
                foreach (var item in items)
                {
                    tmp = new ToolStripMenuItem();
                    {
                        tmp.Text = item.Text;
                        tmp.TextAlign = ContentAlignment.BottomLeft;
                        tmp.Tag = new MyTag
                            {MenuId = item.MenuID, FormId = item.FormID ?? 0, ParentId = item.ParentMenuID ?? 0};
                        tmp.ForeColor = SystemColors.ButtonFace;
                        tmp.BackColor = Color.FromArgb(40, 130, 150);
                        tmp.Font = new Font("IRANSans(FaNum)", 11);
                        tmp.Image = Resources.LemonChiffon;
                        tmp.ImageAlign = ContentAlignment.MiddleCenter;
                        tmp.ImageScaling = ToolStripItemImageScaling.None;
                        tmp.Height = MyMnSt.Height;
                    }
                    ((ToolStripMenuItem) TStrip).DropDownItems.Add(tmp);
                    if ((item.FormID ?? 0) > 0)
                        tmp.Click += Form_Click;
                    CreateMenus(tmp);
                }

                if (((ToolStripMenuItem) TStrip).DropDownItems.Count > 0)
                {
                    //((ToolStripMenuItem)TStrip).MouseHover +=
                    //    ItemMenuStrip_MouseHover;
                    if (tag.ParentId == -1)
                    {
                        ((ToolStripMenuItem) TStrip).Image =
                            Resources.expandleft;
                        ((ToolStripMenuItem) TStrip).DropDownOpened +=
                            RootMenuItem_MouseEnter;
                        ((ToolStripMenuItem) TStrip).DropDownClosed +=
                            RootMenuItem_MouseLeave;
                    }
                    else
                    {
                        ((ToolStripMenuItem) TStrip).Image = null;

                        ((ToolStripMenuItem) TStrip).DropDown.PreviewKeyDown += ItemMenuStrip_PreviewKeyDown;
                    }
                }
                else
                {
                    ((ToolStripMenuItem) TStrip).MouseEnter += MakeYellow;
                    ((ToolStripMenuItem) TStrip).MouseLeave += MakeBack;
                }
            }
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
            sh.Show(MainTab);

        }

        private object GetObjectFromString(string str)
        {
            var arrStr = str.Split('.');
            var assembly = Assembly.Load(arrStr[0] + "." + arrStr[1]);
            if (assembly != null) return assembly.CreateInstance(str);

            return new Control();
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

        private bool CloseProgramm()
        {
            var close =
                ShowPersianMessageBox.ShowMessge("پيام سيستم",
                    "آيا مي خواهيد از سيستم خارج شويد؟", MessageBoxButtons.YesNo, false, false);
            if (close == DialogResult.Yes)
            {
                //if (CheckBackupPermission.HavePermission())
                //{
                var res =
                    ShowPersianMessageBox.ShowMessge("پيام سيستم",
                        "آيا مي خواهيد از اطلاعات پشتيبان بگيريد؟", MessageBoxButtons.YesNo, false, false);
                if (res == DialogResult.Yes)
                {
                    // backup
                    //Atiran.BackupAndRestore.AtiranBackup c = new Atiran.BackupAndRestore.AtiranBackup(true);
                    //new Atiran.UI.WindowsForms.Shortcuts.UserControlLoader(c, true, false, true);
                }
                //}

                // user Logouted
                //Connections.UserService.UserLogouted(Connections.GetCurrentSysUser.Instance.user_id);
                return true;
            }

            return false;
        }

        #endregion
    }
}