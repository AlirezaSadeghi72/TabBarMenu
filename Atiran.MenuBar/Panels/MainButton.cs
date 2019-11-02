using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.MenuBar.Class;
using Atiran.MenuBar.Forms;
using Atiran.MenuBar.Properties;
using Atiran.Utility.Docking2;

namespace Atiran.MenuBar.Panels
{
    public class MainButton : UserControl
    {
        private Label lblMaximis;
        private Label lblMinimis;
        private Timer timer1;
        private System.ComponentModel.IContainer components;
        private Label lblClose;
        private DockPanel mainPane;
        private Label lblDateTime;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label8;
        private Label btnShortcutDesk;
        private Label label10;
        private MenuStrip msUserActivs;
        private ToolStripMenuItem miUserActivs;
        private ToolStripMenuItem sadfsfToolStripMenuItem;
        private ToolStripMenuItem asfdasToolStripMenuItem;
        private ToolStripMenuItem asdfasdfToolStripMenuItem;
        private ShortcutDesk sh1;
        private PersianCalendar pc =new PersianCalendar();

        public MainButton()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;
            timer1.Start();
            
        }
        private void MainButton_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                mainPane = (DockPanel)((Form)(this.TopLevelControl)).Controls.Find("MainTab", true).FirstOrDefault();
                sh1 = new ShortcutDesk(ref mainPane, 1);
                lblMaximis.Image = (((Form)this.TopLevelControl).WindowState == FormWindowState.Normal)
                    ? Resources.Maximis_1
                    : Resources.Maximis_2;
            }
            msUserActivs.Renderer = new ToolStripProfessionalRendererAtiran();
            msUserActivs.BackColor = Color.FromArgb(21, 100, 123);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMinimis = new System.Windows.Forms.Label();
            this.lblMaximis = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDateTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnShortcutDesk = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.msUserActivs = new System.Windows.Forms.MenuStrip();
            this.miUserActivs = new System.Windows.Forms.ToolStripMenuItem();
            this.sadfsfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asfdasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asdfasdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msUserActivs.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMinimis
            // 
            this.lblMinimis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMinimis.BackColor = System.Drawing.Color.Transparent;
            this.lblMinimis.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblMinimis.ForeColor = System.Drawing.Color.Transparent;
            this.lblMinimis.Image = global::Atiran.MenuBar.Properties.Resources.Minimis_1;
            this.lblMinimis.Location = new System.Drawing.Point(1006, 7);
            this.lblMinimis.Name = "lblMinimis";
            this.lblMinimis.Size = new System.Drawing.Size(24, 24);
            this.lblMinimis.TabIndex = 4;
            this.lblMinimis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMinimis.Click += new System.EventHandler(this.lblMinimis_Click);
            this.lblMinimis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_MouseDown);
            this.lblMinimis.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // lblMaximis
            // 
            this.lblMaximis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaximis.BackColor = System.Drawing.Color.Transparent;
            this.lblMaximis.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblMaximis.ForeColor = System.Drawing.Color.Transparent;
            this.lblMaximis.Image = global::Atiran.MenuBar.Properties.Resources.Maximis_1;
            this.lblMaximis.Location = new System.Drawing.Point(1033, 7);
            this.lblMaximis.Name = "lblMaximis";
            this.lblMaximis.Size = new System.Drawing.Size(24, 24);
            this.lblMaximis.TabIndex = 3;
            this.lblMaximis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMaximis.Click += new System.EventHandler(this.lblMaximis_Click);
            this.lblMaximis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_MouseDown);
            this.lblMaximis.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblClose.ForeColor = System.Drawing.Color.Transparent;
            this.lblClose.Image = global::Atiran.MenuBar.Properties.Resources.Exit_1;
            this.lblClose.Location = new System.Drawing.Point(1060, 7);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(24, 24);
            this.lblClose.TabIndex = 2;
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblClose_MouseDown);
            this.lblClose.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblDateTime
            // 
            this.lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTime.Font = new System.Drawing.Font("IRANSans(FaNum)", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblDateTime.Location = new System.Drawing.Point(3, 5);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDateTime.Size = new System.Drawing.Size(160, 31);
            this.lblDateTime.TabIndex = 5;
            this.lblDateTime.Text = "1398/08/08   12/12/12";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(100)))));
            this.label2.Location = new System.Drawing.Point(164, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(3, 38);
            this.label2.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(100)))));
            this.label3.Location = new System.Drawing.Point(287, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(3, 38);
            this.label3.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("IRANSans(FaNum)", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label4.Location = new System.Drawing.Point(167, 5);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(120, 31);
            this.label4.TabIndex = 7;
            this.label4.Text = "سال جاري";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("IRANSans(FaNum)", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label5.Location = new System.Drawing.Point(292, 1);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(200, 36);
            this.label5.TabIndex = 7;
            this.label5.Text = "لاين فروش انتخاب نشده";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.MouseEnter += new System.EventHandler(this.miUserActivs_MouseEnter);
            this.label5.MouseLeave += new System.EventHandler(this.label9_MouseLeave);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(100)))));
            this.label6.Location = new System.Drawing.Point(493, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(3, 38);
            this.label6.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(100)))));
            this.label8.Location = new System.Drawing.Point(624, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(3, 38);
            this.label8.TabIndex = 8;
            // 
            // btnShortcutDesk
            // 
            this.btnShortcutDesk.BackColor = System.Drawing.Color.Transparent;
            this.btnShortcutDesk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShortcutDesk.Font = new System.Drawing.Font("IRANSans(FaNum)", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnShortcutDesk.Location = new System.Drawing.Point(628, 1);
            this.btnShortcutDesk.Name = "btnShortcutDesk";
            this.btnShortcutDesk.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnShortcutDesk.Size = new System.Drawing.Size(150, 36);
            this.btnShortcutDesk.TabIndex = 7;
            this.btnShortcutDesk.Text = "ميزكار";
            this.btnShortcutDesk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnShortcutDesk.Click += new System.EventHandler(this.lblShortcutDesk_Click);
            this.btnShortcutDesk.MouseEnter += new System.EventHandler(this.miUserActivs_MouseEnter);
            this.btnShortcutDesk.MouseLeave += new System.EventHandler(this.label9_MouseLeave);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(100)))));
            this.label10.Location = new System.Drawing.Point(779, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(3, 38);
            this.label10.TabIndex = 8;
            // 
            // msUserActivs
            // 
            this.msUserActivs.AutoSize = false;
            this.msUserActivs.BackColor = System.Drawing.Color.Transparent;
            this.msUserActivs.Dock = System.Windows.Forms.DockStyle.None;
            this.msUserActivs.Font = new System.Drawing.Font("IRANSans(FaNum)", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msUserActivs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUserActivs});
            this.msUserActivs.Location = new System.Drawing.Point(481, 2);
            this.msUserActivs.Name = "msUserActivs";
            this.msUserActivs.Size = new System.Drawing.Size(142, 36);
            this.msUserActivs.TabIndex = 9;
            this.msUserActivs.Text = "menuStrip1";
            // 
            // miUserActivs
            // 
            this.miUserActivs.AutoSize = false;
            this.miUserActivs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sadfsfToolStripMenuItem,
            this.asfdasToolStripMenuItem,
            this.asdfasdfToolStripMenuItem});
            this.miUserActivs.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.miUserActivs.ForeColor = System.Drawing.Color.White;
            this.miUserActivs.Image = global::Atiran.MenuBar.Properties.Resources.Exit_1;
            this.miUserActivs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.miUserActivs.Name = "miUserActivs";
            this.miUserActivs.Size = new System.Drawing.Size(122, 35);
            this.miUserActivs.Text = "كاربر";
            this.miUserActivs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sadfsfToolStripMenuItem
            // 
            this.sadfsfToolStripMenuItem.AutoSize = false;
            this.sadfsfToolStripMenuItem.Name = "sadfsfToolStripMenuItem";
            this.sadfsfToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.sadfsfToolStripMenuItem.Text = "sadfsf";
            // 
            // asfdasToolStripMenuItem
            // 
            this.asfdasToolStripMenuItem.AutoSize = false;
            this.asfdasToolStripMenuItem.Name = "asfdasToolStripMenuItem";
            this.asfdasToolStripMenuItem.Size = new System.Drawing.Size(150, 24);
            this.asfdasToolStripMenuItem.Text = "asfdas";
            // 
            // asdfasdfToolStripMenuItem
            // 
            this.asdfasdfToolStripMenuItem.AutoSize = false;
            this.asdfasdfToolStripMenuItem.Name = "asdfasdfToolStripMenuItem";
            this.asdfasdfToolStripMenuItem.Size = new System.Drawing.Size(150, 24);
            this.asdfasdfToolStripMenuItem.Text = "asdfasdf";
            // 
            // MainButton
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(100)))), ((int)(((byte)(123)))));
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnShortcutDesk);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMinimis);
            this.Controls.Add(this.lblMaximis);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.msUserActivs);
            this.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "MainButton";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1094, 278);
            this.Load += new System.EventHandler(this.MainButton_Load);
            this.msUserActivs.ResumeLayout(false);
            this.msUserActivs.PerformLayout();
            this.ResumeLayout(false);

        }

        //-------
        private void MakeYellow(object sender, EventArgs e)
        {
            //((ToolStripMenuItem)sender).Image = Properties.Resources.selected;
        }




        //-------

        private void label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.SkyBlue;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Transparent;
        }


        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.DeepSkyBlue;
        }

        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Red;
        }

        private void lblClose_MouseDown(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.DarkRed;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            ((Form) this.TopLevelControl).Close();
        }

        private void lblMaximis_Click(object sender, EventArgs e)
        {
            if (((Form) this.TopLevelControl).WindowState == FormWindowState.Normal)
            {
                ((Form) this.TopLevelControl).WindowState = FormWindowState.Maximized;
                lblMaximis.Image = Resources.Maximis_2;
            }
            else
            {
                ((Form)this.TopLevelControl).WindowState = FormWindowState.Normal;
                lblMaximis.Image = Resources.Maximis_1;
            }
        }

        private void lblMinimis_Click(object sender, EventArgs e)
        {
            ((Form) this.TopLevelControl).WindowState = FormWindowState.Minimized;
        }

        private void lblShortcutDesk_Click(object sender, EventArgs e)
        {
            sh1.Text = "ميزكار";
            sh1.Show(mainPane);
        }

        private void miUserActivs_MouseEnter(object sender, EventArgs e)
        {
            ((Control) sender).BackColor = Color.Wheat;
            ((Control) sender).ForeColor = Color.Black;
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.Transparent;
            ((Control) sender).ForeColor = Color.White;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime td = DateTime.Now;
            lblDateTime.Text = String.Format("{0}/{1}/{2}   {3}:{4}:{5}",pc.GetYear(td).ToString("0000"),pc.GetMonth(td).ToString("00"), pc.GetDayOfMonth(td).ToString("00"), td.Hour.ToString("00"), td.Minute.ToString("00"), td.Second.ToString("00"));
        }
    }
}
