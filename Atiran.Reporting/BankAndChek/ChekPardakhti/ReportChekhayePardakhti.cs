using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.Utility.Docking2;
using Atiran.Utility.Docking2.Desk;
using Atiran.Utility.MassageBox;

namespace Atiran.Reporting.BankAndChek.ChekPardakhti
{
    public class ReportChekhayePardakhti : UserControl
    {
        private GroupBox groupBox1;
        public Label label1;
        private TextBox textBox1;
        private Button button1;
        private Button btnCancel;
        private string Text1;

        public ReportChekhayePardakhti()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1280, 680);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(751, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(256, 20);
            this.textBox1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(751, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(256, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(6, 650);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1274, 661);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReportChekhayePardakhti
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "ReportChekhayePardakhti";
            this.Size = new System.Drawing.Size(1280, 680);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (((DeskTab)((Form)this.TopLevelControl).ActiveMdiChild).ShowQuestionClose)
            {
                if (ShowPersianMessageBox.ShowMessge("پيغام", "آيا تب " + ((Form)this.TopLevelControl).ActiveMdiChild.Text + " بسته شود",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ((Form)this.TopLevelControl).ActiveMdiChild.Close();
                }
            }
            else
            {
                ((Form)this.TopLevelControl).ActiveMdiChild.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Text1 = Text1 ?? ((Form)this.TopLevelControl).ActiveMdiChild.Text;

            ((Form)this.TopLevelControl).ActiveMdiChild.Text = Text1 + " -> " + textBox1.Text;
        }

    }
}
