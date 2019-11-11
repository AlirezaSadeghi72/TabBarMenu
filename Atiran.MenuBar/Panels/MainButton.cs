using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Atiran.DataLayer.Context;
using Atiran.MenuBar.Class;
using Atiran.MenuBar.Forms;
using Atiran.MenuBar.Properties;
using Atiran.Utility.Docking2;
using Atiran.Utility.Docking2.Desk;
using Atiran.Utility.MassageBox;

namespace Atiran.MenuBar.Panels
{
    public class MainButton : UserControl
    {
        private Label btnLine;
        private Label btnShortcutDesk;
        private IContainer components;
        private List<Form> deskTabs;
        private bool isCanselCLoseAll;
        private bool isCLoseAll;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label label6;
        private Label label8;
        private PictureBox lblClose;
        private Label lblDateTime;
        private Label lblMaximis;
        private Label lblMinimis;
        private Label lblSalMali;
        private DockPanel mainPane;
        private ToolStripMenuItem miRestartApplication;
        private ToolStripMenuItem miUserActivs;
        private MenuStrip msUserActivs;
        private PersianCalendar pc = new PersianCalendar();
        private ToolStripMenuItem sadfsfToolStripMenuItem;
        private ShortcutDesk sh1;
        private Timer timer1;
        public int UserID, SalMaliID;

        public MainButton()
        {
            InitializeComponent();
            BackColor = Color.Transparent;
            timer1.Start();
        }

        private void MainButton_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                mainPane = (DockPanel) ((Form) TopLevelControl).Controls.Find("MainTab", true).FirstOrDefault();
                sh1 = new ShortcutDesk(ref mainPane);
                lblMaximis.Image = ((Form) TopLevelControl).WindowState == FormWindowState.Normal
                    ? Resources.Maximis_1
                    : Resources.Maximis_2;
                var users = Connection.GetActiveUsers();

                miUserActivs.DropDownItems.Clear();
                var list = new List<ToolStripMenuItem>();
                foreach (var u in users)
                    list.Add(new ToolStripMenuItem
                    {
                        Text = u.user_name,
                        RightToLeft = RightToLeft.Yes,
                        ForeColor = SystemColors.ButtonFace,
                        BackColor = Color.FromArgb(40, 130, 150),
                        Font = new Font("IRANSans(FaNum)", 11)
                    });

                miUserActivs.DropDownItems.AddRange(list.ToArray());

                miUserActivs.Text = users.FirstOrDefault(u => u.user_id == UserID)?.user_name;
                lblSalMali.Text = Connection.GetNameSalMali(SalMaliID);
            }

            msUserActivs.Renderer = new ToolStripProfessionalRendererAtiran();
        }

        private void InitializeComponent()
        {
            components = new Container();
            timer1 = new Timer(components);
            lblDateTime = new Label();
            label2 = new Label();
            label3 = new Label();
            lblSalMali = new Label();
            btnLine = new Label();
            label6 = new Label();
            label8 = new Label();
            btnShortcutDesk = new Label();
            label10 = new Label();
            msUserActivs = new MenuStrip();
            miRestartApplication = new ToolStripMenuItem();
            miUserActivs = new ToolStripMenuItem();
            sadfsfToolStripMenuItem = new ToolStripMenuItem();
            lblClose = new PictureBox();
            lblMinimis = new Label();
            lblMaximis = new Label();
            msUserActivs.SuspendLayout();
            ((ISupportInitialize) lblClose).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // lblDateTime
            // 
            lblDateTime.BackColor = Color.Transparent;
            lblDateTime.Cursor = Cursors.Arrow;
            lblDateTime.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDateTime.Location = new Point(3, 5);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.RightToLeft = RightToLeft.No;
            lblDateTime.Size = new Size(160, 31);
            lblDateTime.TabIndex = 5;
            lblDateTime.Text = "1398/08/08   12/12/12";
            lblDateTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BackColor = Color.FromArgb(17, 81, 100);
            label2.Cursor = Cursors.Arrow;
            label2.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(164, 2);
            label2.Name = "label2";
            label2.Size = new Size(3, 38);
            label2.TabIndex = 6;
            // 
            // label3
            // 
            label3.BackColor = Color.FromArgb(17, 81, 100);
            label3.Cursor = Cursors.Arrow;
            label3.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(287, 1);
            label3.Name = "label3";
            label3.Size = new Size(3, 38);
            label3.TabIndex = 8;
            // 
            // lblSalMali
            // 
            lblSalMali.BackColor = Color.Transparent;
            lblSalMali.Cursor = Cursors.Arrow;
            lblSalMali.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSalMali.Location = new Point(167, 5);
            lblSalMali.Name = "lblSalMali";
            lblSalMali.RightToLeft = RightToLeft.No;
            lblSalMali.Size = new Size(120, 31);
            lblSalMali.TabIndex = 7;
            lblSalMali.Text = "سال جاري";
            lblSalMali.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLine
            // 
            btnLine.BackColor = Color.Transparent;
            btnLine.Cursor = Cursors.Hand;
            btnLine.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLine.Location = new Point(292, 1);
            btnLine.Name = "btnLine";
            btnLine.RightToLeft = RightToLeft.No;
            btnLine.Size = new Size(200, 36);
            btnLine.TabIndex = 7;
            btnLine.Text = "لاين فروش انتخاب نشده";
            btnLine.TextAlign = ContentAlignment.MiddleCenter;
            btnLine.Click += btnLine_Click;
            btnLine.MouseEnter += label_MouseEnter;
            btnLine.MouseLeave += label_MouseLeave;
            // 
            // label6
            // 
            label6.BackColor = Color.FromArgb(17, 81, 100);
            label6.Cursor = Cursors.Arrow;
            label6.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(493, 1);
            label6.Name = "label6";
            label6.Size = new Size(3, 38);
            label6.TabIndex = 8;
            // 
            // label8
            // 
            label8.BackColor = Color.FromArgb(17, 81, 100);
            label8.Cursor = Cursors.Arrow;
            label8.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(621, 0);
            label8.Name = "label8";
            label8.Size = new Size(3, 38);
            label8.TabIndex = 8;
            // 
            // btnShortcutDesk
            // 
            btnShortcutDesk.BackColor = Color.Transparent;
            btnShortcutDesk.Cursor = Cursors.Hand;
            btnShortcutDesk.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnShortcutDesk.Location = new Point(625, 1);
            btnShortcutDesk.Name = "btnShortcutDesk";
            btnShortcutDesk.RightToLeft = RightToLeft.No;
            btnShortcutDesk.Size = new Size(150, 36);
            btnShortcutDesk.TabIndex = 7;
            btnShortcutDesk.Text = "ميزكار";
            btnShortcutDesk.TextAlign = ContentAlignment.MiddleCenter;
            btnShortcutDesk.Click += lblShortcutDesk_Click;
            btnShortcutDesk.MouseEnter += label_MouseEnter;
            btnShortcutDesk.MouseLeave += label_MouseLeave;
            // 
            // label10
            // 
            label10.BackColor = Color.FromArgb(17, 81, 100);
            label10.Cursor = Cursors.Arrow;
            label10.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(776, 0);
            label10.Name = "label10";
            label10.Size = new Size(3, 38);
            label10.TabIndex = 8;
            // 
            // msUserActivs
            // 
            msUserActivs.AutoSize = false;
            msUserActivs.BackColor = Color.Transparent;
            msUserActivs.Dock = DockStyle.None;
            msUserActivs.Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            msUserActivs.Items.AddRange(new ToolStripItem[]
            {
                miRestartApplication,
                miUserActivs
            });
            msUserActivs.Location = new Point(439, 1);
            msUserActivs.Name = "msUserActivs";
            msUserActivs.Size = new Size(180, 36);
            msUserActivs.TabIndex = 9;
            msUserActivs.Text = "menuStrip1";
            // 
            // miRestartApplication
            // 
            miRestartApplication.AutoSize = false;
            miRestartApplication.DisplayStyle = ToolStripItemDisplayStyle.Image;
            miRestartApplication.Image = Resources.icoUser;
            miRestartApplication.Name = "miRestartApplication";
            miRestartApplication.Size = new Size(28, 37);
            miRestartApplication.Text = " ";
            miRestartApplication.TextImageRelation = TextImageRelation.Overlay;
            miRestartApplication.Click += miRestartApplication_Click;
            // 
            // miUserActivs
            // 
            miUserActivs.AutoSize = false;
            miUserActivs.DisplayStyle = ToolStripItemDisplayStyle.Text;
            miUserActivs.DropDownItems.AddRange(new ToolStripItem[]
            {
                sadfsfToolStripMenuItem
            });
            miUserActivs.Font = new Font("Segoe UI", 10F);
            miUserActivs.ForeColor = Color.White;
            miUserActivs.Name = "miUserActivs";
            miUserActivs.RightToLeft = RightToLeft.Yes;
            miUserActivs.Size = new Size(94, 37);
            miUserActivs.Text = "كاربر";
            miUserActivs.TextAlign = ContentAlignment.MiddleLeft;
            miUserActivs.TextImageRelation = TextImageRelation.Overlay;
            // 
            // sadfsfToolStripMenuItem
            // 
            sadfsfToolStripMenuItem.AutoSize = false;
            sadfsfToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            sadfsfToolStripMenuItem.Image = Resources.expandleft;
            sadfsfToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            sadfsfToolStripMenuItem.Name = "sadfsfToolStripMenuItem";
            sadfsfToolStripMenuItem.Size = new Size(180, 24);
            sadfsfToolStripMenuItem.Text = "sadfsf";
            // 
            // lblClose
            // 
            lblClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblClose.BackColor = Color.Transparent;
            lblClose.BackgroundImage = Resources.Exit_1;
            lblClose.BackgroundImageLayout = ImageLayout.Zoom;
            lblClose.Location = new Point(1060, 1);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(34, 38);
            lblClose.TabIndex = 10;
            lblClose.TabStop = false;
            lblClose.Click += lblClose_Click;
            lblClose.MouseDown += lblClose_MouseDown;
            lblClose.MouseEnter += label_MouseEnter;
            lblClose.MouseLeave += label_MouseLeave;
            // 
            // lblMinimis
            // 
            lblMinimis.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMinimis.BackColor = Color.Transparent;
            lblMinimis.Cursor = Cursors.Arrow;
            lblMinimis.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 178);
            lblMinimis.ForeColor = Color.Transparent;
            lblMinimis.Image = Resources.Minimis_1;
            lblMinimis.Location = new Point(996, 1);
            lblMinimis.Name = "lblMinimis";
            lblMinimis.Size = new Size(32, 38);
            lblMinimis.TabIndex = 4;
            lblMinimis.TextAlign = ContentAlignment.MiddleCenter;
            lblMinimis.Click += lblMinimis_Click;
            lblMinimis.MouseDown += label_MouseDown;
            lblMinimis.MouseEnter += label_MouseEnter;
            lblMinimis.MouseLeave += label_MouseLeave;
            // 
            // lblMaximis
            // 
            lblMaximis.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMaximis.BackColor = Color.Transparent;
            lblMaximis.Cursor = Cursors.Arrow;
            lblMaximis.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 178);
            lblMaximis.ForeColor = Color.Transparent;
            lblMaximis.Image = Resources.Maximis_1;
            lblMaximis.Location = new Point(1028, 1);
            lblMaximis.Name = "lblMaximis";
            lblMaximis.Size = new Size(32, 38);
            lblMaximis.TabIndex = 3;
            lblMaximis.TextAlign = ContentAlignment.MiddleCenter;
            lblMaximis.Click += lblMaximis_Click;
            lblMaximis.MouseDown += label_MouseDown;
            lblMaximis.MouseEnter += label_MouseEnter;
            lblMaximis.MouseLeave += label_MouseLeave;
            // 
            // MainButton
            // 
            BackColor = Color.FromArgb(21, 100, 123);
            Controls.Add(lblClose);
            Controls.Add(lblDateTime);
            Controls.Add(lblSalMali);
            Controls.Add(btnLine);
            Controls.Add(label10);
            Controls.Add(btnShortcutDesk);
            Controls.Add(label6);
            Controls.Add(label8);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblMinimis);
            Controls.Add(lblMaximis);
            Controls.Add(msUserActivs);
            Font = new Font("IRANSans(FaNum)", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, 178);
            ForeColor = Color.White;
            Name = "MainButton";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1094, 278);
            Load += MainButton_Load;
            MouseDown += MainButton_MouseDown;
            msUserActivs.ResumeLayout(false);
            msUserActivs.PerformLayout();
            ((ISupportInitialize) lblClose).EndInit();
            ResumeLayout(false);
        }

        private void lblClose_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control) sender).BackColor = Color.DarkRed;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            ((Form) TopLevelControl).Close();
        }

        private void lblMaximis_Click(object sender, EventArgs e)
        {
            if (((Form) TopLevelControl).WindowState == FormWindowState.Normal)
            {
                ((Form) TopLevelControl).WindowState = FormWindowState.Maximized;
                lblMaximis.Image = Resources.Maximis_2;
            }
            else
            {
                ((Form) TopLevelControl).WindowState = FormWindowState.Normal;
                lblMaximis.Image = Resources.Maximis_1;
            }
        }

        private void lblMinimis_Click(object sender, EventArgs e)
        {
            ((Form) TopLevelControl).WindowState = FormWindowState.Minimized;
        }

        private void lblShortcutDesk_Click(object sender, EventArgs e)
        {
            sh1.Text = "ميزكار";
            sh1.Show(mainPane);
        }

        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            ((Label) sender).BackColor = Color.DeepSkyBlue;
        }

        private void label_MouseEnter(object sender, EventArgs e)
        {
            //msUserActivs.BackColor = Color.FromArgb(21, 100, 123);
            ((Control) sender).BackColor = Color.FromArgb(128, Color.FromArgb(20, 130, 150)); //Color.Wheat;
            //((Control)sender).ForeColor = Color.Black;
            ((Control) sender).Focus();
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            ((Control) sender).BackColor = Color.Transparent;
            //((Control)sender).ForeColor = Color.White;
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            var deskTab = new DeskTab
            {
                Text = "انتخاب لاين فروش",
                StartPosition = FormStartPosition.CenterParent
            };
            deskTab.ShowDialog();
        }

        private void miRestartApplication_Click(object sender, EventArgs e)
        {
            deskTabs = ((Form) TopLevelControl).MdiChildren.ToList();

            foreach (DeskTab form in ((Form) TopLevelControl).MdiChildren)
                if (!isCanselCLoseAll)
                    TryClose(form, deskTabs.Where(f => f != form).ToArray());

            isCLoseAll = false;
            isCanselCLoseAll = false;

            if (((Form) TopLevelControl).MdiChildren.Length < 1) Application.Restart();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            var td = DateTime.Now;
            lblDateTime.Text = string.Format("{0}/{1}/{2}   {3}:{4}:{5}", pc.GetYear(td).ToString("0000"),
                pc.GetMonth(td).ToString("00"), pc.GetDayOfMonth(td).ToString("00"), td.Hour.ToString("00"),
                td.Minute.ToString("00"), td.Second.ToString("00"));
        }

        #region moving form by muse in header

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
            int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        private void MainButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(((Form) TopLevelControl).Handle, 0x00A1, 0x2, 0);
            }
        }

        #endregion
    }
}