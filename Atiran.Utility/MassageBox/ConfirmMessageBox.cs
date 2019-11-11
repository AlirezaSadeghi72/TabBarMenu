using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Atiran.Utility.MassageBox
{
    public class ConfirmMessageBox : Form
    {
        private Button btnConfirm;
        private PictureBox ClosePictureBox;
        private Label lblCaption;
        private PictureBox pictureBox1;
        private Panel pnlMain;
        private Panel pnlTop;
        private RichTextBox txtMessage;

        public ConfirmMessageBox()
        {
            InitializeComponent();
        }

        public string Caption
        {
            set => lblCaption.Text = value;
        }

        public string SetMessage
        {
            set => txtMessage.Text = value;
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblCaption = new Label();
            btnConfirm = new Button();
            pnlMain = new Panel();
            txtMessage = new RichTextBox();
            pictureBox1 = new PictureBox();
            ClosePictureBox = new PictureBox();
            pnlTop.SuspendLayout();
            pnlMain.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            ((ISupportInitialize) ClosePictureBox).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblCaption);
            pnlTop.Controls.Add(ClosePictureBox);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(470, 32);
            pnlTop.TabIndex = 0;
            // 
            // lblCaption
            // 
            lblCaption.BackColor = Color.FromArgb(19, 48, 21);
            lblCaption.Dock = DockStyle.Fill;
            lblCaption.ForeColor = SystemColors.ButtonFace;
            lblCaption.Location = new Point(0, 0);
            lblCaption.Name = "lblCaption";
            lblCaption.RightToLeft = RightToLeft.Yes;
            lblCaption.Size = new Size(436, 32);
            lblCaption.TabIndex = 1;
            lblCaption.Text = "توضيحات";
            lblCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnConfirm
            // 
            btnConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfirm.BackColor = Color.FromArgb(19, 48, 21);
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.ForeColor = SystemColors.ButtonFace;
            btnConfirm.Location = new Point(310, 81);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(75, 31);
            btnConfirm.TabIndex = 0;
            btnConfirm.Text = "تاييد";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Enter += Button_Enter;
            btnConfirm.Leave += Button_Leave;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(61, 61, 61);
            pnlMain.Controls.Add(txtMessage);
            pnlMain.Controls.Add(pictureBox1);
            pnlMain.Controls.Add(btnConfirm);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 32);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(470, 119);
            pnlMain.TabIndex = 1;
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtMessage.BackColor = Color.FromArgb(61, 61, 61);
            txtMessage.ForeColor = SystemColors.ButtonFace;
            txtMessage.Location = new Point(3, 15);
            txtMessage.Name = "txtMessage";
            txtMessage.ReadOnly = true;
            txtMessage.ScrollBars = RichTextBoxScrollBars.Horizontal;
            txtMessage.Size = new Size(382, 60);
            txtMessage.TabIndex = 2;
            txtMessage.Text = "توضيحات";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.Image = Resources.confirm;
            pictureBox1.Location = new Point(391, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(72, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // ClosePictureBox
            // 
            ClosePictureBox.BackColor = Color.Transparent;
            ClosePictureBox.Dock = DockStyle.Right;
            ClosePictureBox.Image = Resources.Close;
            ClosePictureBox.Location = new Point(436, 0);
            ClosePictureBox.Name = "ClosePictureBox";
            ClosePictureBox.Size = new Size(34, 32);
            ClosePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ClosePictureBox.TabIndex = 0;
            ClosePictureBox.TabStop = false;
            ClosePictureBox.Click += ClosePictureBox_Click;
            ClosePictureBox.MouseEnter += ClosePictureBox_MouseHover;
            ClosePictureBox.MouseLeave += ClosePictureBox_MouseLeave;
            // 
            // ConfirmMessageBox
            // 
            AcceptButton = btnConfirm;
            BackColor = Color.FromArgb(19, 48, 21);
            ClientSize = new Size(470, 151);
            Controls.Add(pnlMain);
            Controls.Add(pnlTop);
            Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 178);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ConfirmMessageBox";
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterParent;
            Load += ConfirmMessageBox_Load;
            pnlTop.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            ((ISupportInitialize) pictureBox1).EndInit();
            ((ISupportInitialize) ClosePictureBox).EndInit();
            ResumeLayout(false);
        }

        private void ClosePictureBox_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ConfirmMessageBox_Load(object sender, EventArgs e)
        {
            btnConfirm.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Escape || keyData == (Keys.Alt | Keys.F4))
            {
                DialogResult = DialogResult.OK;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClosePictureBox_MouseHover(object sender, EventArgs e)
        {
            ClosePictureBox.Image = Resources.CloseMouseHover;
        }

        private void ClosePictureBox_MouseLeave(object sender, EventArgs e)
        {
            ClosePictureBox.Image = Resources.Close;
        }

        private void Button_Leave(object sender, EventArgs e)
        {
            btnConfirm.FlatAppearance.BorderColor = Color.White;
            btnConfirm.FlatAppearance.BorderSize = 1;
        }

        private void Button_Enter(object sender, EventArgs e)
        {
            btnConfirm.FlatAppearance.BorderColor = Color.DarkOrange;
            btnConfirm.FlatAppearance.BorderSize = 1;
        }
    }
}