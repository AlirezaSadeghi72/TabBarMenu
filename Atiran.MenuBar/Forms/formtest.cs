using Atiran.Utility.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atiran.MenuBar.Forms
{
    class formtest : Form
    {
        private Button button1;
        private DockPanel dockPanel1;
        DeskTab sh1 = new DeskTab();

        public formtest()
        {
            InitializeComponent();
            DeskTab sh = new DeskTab();
            sh.Text = "Alireza";
            
            if (dockPanel1.DocumentStyle == Utility.Docking.DocumentStyle.SystemMdi)
            {
                sh.MdiParent = this;
                sh.Show();
            }
            else
                sh.Show(dockPanel1);
        }

        private void InitializeComponent()
        {
            Atiran.Utility.Docking.DockPanelSkin dockPanelSkin1 = new Atiran.Utility.Docking.DockPanelSkin();
            Atiran.Utility.Docking.AutoHideStripSkin autoHideStripSkin1 = new Atiran.Utility.Docking.AutoHideStripSkin();
            Atiran.Utility.Docking.DockPanelGradient dockPanelGradient1 = new Atiran.Utility.Docking.DockPanelGradient();
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
            this.dockPanel1 = new Atiran.Utility.Docking.DockPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel1.Location = new System.Drawing.Point(0, 23);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(909, 411);
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
            this.dockPanel1.Skin = dockPanelSkin1;
            this.dockPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(909, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // formtest
            // 
            this.ClientSize = new System.Drawing.Size(909, 434);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.button1);
            this.IsMdiContainer = true;
            this.Name = "formtest";
            this.ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
                sh1.Text = "ميزكار";
                sh1.Show(dockPanel1, DockState.DockRight);
        }
    }
}
