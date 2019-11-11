using System;
using System.Drawing;
using System.Windows.Forms;
using Atiran.Utility.Docking2.Desk;
using Atiran.Utility.MassageBox;

namespace Atiran.Reporting.BankAndChek.ChekPardakhti
{
    public class ReportChekhayePardakhti : UserControl
    {
        private Button btnCancel;
        private Button button1;
        private GroupBox groupBox1;
        public Label label1;
        private string Text1;
        private TextBox textBox1;

        public ReportChekhayePardakhti()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            button1 = new Button();
            btnCancel = new Button();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(btnCancel);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1280, 680);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(751, 19);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(256, 20);
            textBox1.TabIndex = 5;
            // 
            // button1
            // 
            button1.Location = new Point(751, 45);
            button1.Name = "button1";
            button1.Size = new Size(256, 23);
            button1.TabIndex = 4;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Location = new Point(6, 650);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "انصراف";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 16);
            label1.Name = "label1";
            label1.Size = new Size(1274, 661);
            label1.TabIndex = 3;
            label1.Text = "label1";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ReportChekhayePardakhti
            // 
            Controls.Add(groupBox1);
            Name = "ReportChekhayePardakhti";
            Size = new Size(1280, 680);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (((DeskTab) ((Form) TopLevelControl).ActiveMdiChild).ShowQuestionClose)
            {
                if (ShowPersianMessageBox.ShowMessge("پيغام",
                        "آيا تب " + ((Form) TopLevelControl).ActiveMdiChild.Text + " بسته شود",
                        MessageBoxButtons.YesNo, false, false) == DialogResult.Yes)
                    ((Form) TopLevelControl).ActiveMdiChild.Close();
            }
            else
            {
                ((Form) TopLevelControl).ActiveMdiChild.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Text1 = Text1 ?? ((Form) TopLevelControl).ActiveMdiChild.Text;

            ((Form) TopLevelControl).ActiveMdiChild.Text = Text1 + " -> " + textBox1.Text;
        }
    }
}