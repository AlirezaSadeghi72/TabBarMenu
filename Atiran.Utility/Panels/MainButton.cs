using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.Utility.Properties;

namespace Atiran.Utility.Panels
{
    public class MainButton : UserControl
    {
        private Label lblMaximis;
        private Label lblMinimis;
        private Label lblClose;

        public MainButton()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;
            
        }
        private void MainButton_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                lblMaximis.Image = (((Form)this.TopLevelControl).WindowState == FormWindowState.Normal)
                    ? Resources.Maximis_1
                    : Resources.Maximis_2;
            }
        }
        private void InitializeComponent()
        {
            this.lblMinimis = new System.Windows.Forms.Label();
            this.lblMaximis = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMinimis
            // 
            this.lblMinimis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMinimis.BackColor = System.Drawing.Color.Transparent;
            this.lblMinimis.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblMinimis.ForeColor = System.Drawing.Color.Transparent;
            this.lblMinimis.Image = global::Atiran.Utility.Properties.Resources.Minimis_1;
            this.lblMinimis.Location = new System.Drawing.Point(596, 7);
            this.lblMinimis.Name = "lblMinimis";
            this.lblMinimis.Size = new System.Drawing.Size(24, 24);
            this.lblMinimis.TabIndex = 0;
            this.lblMinimis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMinimis.Click += new System.EventHandler(this.lblMinimis_Click);
            this.lblMinimis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_MouseDown);
            this.lblMinimis.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.lblMinimis.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // lblMaximis
            // 
            this.lblMaximis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaximis.BackColor = System.Drawing.Color.Transparent;
            this.lblMaximis.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblMaximis.ForeColor = System.Drawing.Color.Transparent;
            this.lblMaximis.Image = global::Atiran.Utility.Properties.Resources.Maximis_1;
            this.lblMaximis.Location = new System.Drawing.Point(623, 7);
            this.lblMaximis.Name = "lblMaximis";
            this.lblMaximis.Size = new System.Drawing.Size(24, 24);
            this.lblMaximis.TabIndex = 0;
            this.lblMaximis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMaximis.Click += new System.EventHandler(this.lblMaximis_Click);
            this.lblMaximis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_MouseDown);
            this.lblMaximis.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.lblMaximis.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblClose.ForeColor = System.Drawing.Color.Transparent;
            this.lblClose.Image = global::Atiran.Utility.Properties.Resources.Exit_1;
            this.lblClose.Location = new System.Drawing.Point(650, 7);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(24, 24);
            this.lblClose.TabIndex = 0;
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblClose_MouseDown);
            this.lblClose.MouseEnter += new System.EventHandler(this.lblClose_MouseEnter);
            this.lblClose.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // MainButton
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(100)))), ((int)(((byte)(123)))));
            this.Controls.Add(this.lblMinimis);
            this.Controls.Add(this.lblMaximis);
            this.Controls.Add(this.lblClose);
            this.Font = new System.Drawing.Font("IRANSans(FaNum)", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "MainButton";
            this.Size = new System.Drawing.Size(684, 38);
            this.Load += new System.EventHandler(this.MainButton_Load);
            this.ResumeLayout(false);

        }

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

        
    }
}
