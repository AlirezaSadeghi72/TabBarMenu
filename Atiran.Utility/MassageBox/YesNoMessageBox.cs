using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Atiran.Utility.MassageBox
{
    public class YesNoMessageBox : Form
    {
        private Button btnNo;
        private Button btnNoAll;
        private Button btnYes;
        private Button btnYesAll;
        private PictureBox ClosePictureBox;
        private Label lblCaption;
        public bool LoadOnYesButton = true;
        private PictureBox pictureBox1;
        private Panel pnlButtons;
        private Panel pnlMain;
        private Panel pnlTop;
        public bool ShowAllButton = true;
        private Thread threadLoad;
        private ThreadStart threadStartLoad;
        private ListBox txtMessage;

        public YesNoMessageBox()
        {
            InitializeComponent();
        }

        public string Caption
        {
            set => lblCaption.Text = value;
        }

        public string SetMessage
        {
            set
            {
                txtMessage.Items.AddRange(value.Split('\n'));
                if (ShowAllButton) txtMessage.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblCaption = new Label();
            ClosePictureBox = new PictureBox();
            pnlMain = new Panel();
            txtMessage = new ListBox();
            pnlButtons = new Panel();
            btnNoAll = new Button();
            btnNo = new Button();
            btnYesAll = new Button();
            btnYes = new Button();
            pictureBox1 = new PictureBox();
            pnlTop.SuspendLayout();
            ((ISupportInitialize) ClosePictureBox).BeginInit();
            pnlMain.SuspendLayout();
            pnlButtons.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(21, 100, 123);
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
            lblCaption.BackColor = Color.FromArgb(21, 100, 123);
            lblCaption.Dock = DockStyle.Fill;
            lblCaption.ForeColor = SystemColors.ActiveCaptionText;
            lblCaption.Location = new Point(0, 0);
            lblCaption.Name = "lblCaption";
            lblCaption.RightToLeft = RightToLeft.Yes;
            lblCaption.Size = new Size(436, 32);
            lblCaption.TabIndex = 1;
            lblCaption.Text = "توضيحات";
            lblCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ClosePictureBox
            // 
            ClosePictureBox.BackColor = Color.Transparent;
            ClosePictureBox.Dock = DockStyle.Right;
            ClosePictureBox.Image = Properties.Resources.Exit_1;
            ClosePictureBox.Location = new Point(436, 0);
            ClosePictureBox.Name = "ClosePictureBox";
            ClosePictureBox.Size = new Size(34, 32);
            ClosePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ClosePictureBox.TabIndex = 0;
            ClosePictureBox.TabStop = false;
            ClosePictureBox.Click += ClosePictureBox_Click;
            ClosePictureBox.MouseEnter += ClosePictureBox_MouseEnter;
            ClosePictureBox.MouseLeave += ClosePictureBox_MouseLeave;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(20, 130, 150);
            pnlMain.Controls.Add(txtMessage);
            pnlMain.Controls.Add(pnlButtons);
            pnlMain.Controls.Add(pictureBox1);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 32);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(470, 151);
            pnlMain.TabIndex = 1;
            // 
            // txtMessage
            // 
            txtMessage.FormattingEnabled = true;
            txtMessage.ItemHeight = 22;
            txtMessage.Location = new Point(12, 15);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(373, 70);
            txtMessage.TabIndex = 5;
            txtMessage.TabStop = false;
            // 
            // pnlButtons
            // 
            pnlButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlButtons.Controls.Add(btnNoAll);
            pnlButtons.Controls.Add(btnNo);
            pnlButtons.Controls.Add(btnYesAll);
            pnlButtons.Controls.Add(btnYes);
            pnlButtons.Location = new Point(52, 99);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(333, 36);
            pnlButtons.TabIndex = 3;
            // 
            // btnNoAll
            // 
            btnNoAll.BackColor = Color.FromArgb(64, 0, 0);
            btnNoAll.DialogResult = DialogResult.Cancel;
            btnNoAll.FlatStyle = FlatStyle.Flat;
            btnNoAll.ForeColor = SystemColors.ButtonFace;
            btnNoAll.Location = new Point(3, 3);
            btnNoAll.Name = "btnNoAll";
            btnNoAll.Size = new Size(75, 30);
            btnNoAll.TabIndex = 3;
            btnNoAll.Text = "انصراف";
            btnNoAll.UseVisualStyleBackColor = false;
            btnNoAll.Enter += Button_Enter;
            btnNoAll.Leave += Button_Leave;
            // 
            // btnNo
            // 
            btnNo.BackColor = Color.FromArgb(192, 0, 0);
            btnNo.DialogResult = DialogResult.No;
            btnNo.FlatStyle = FlatStyle.Flat;
            btnNo.ForeColor = Color.White;
            btnNo.Location = new Point(169, 3);
            btnNo.Name = "btnNo";
            btnNo.Size = new Size(75, 30);
            btnNo.TabIndex = 1;
            btnNo.Text = "خير";
            btnNo.UseVisualStyleBackColor = false;
            btnNo.Enter += Button_Enter;
            btnNo.Leave += Button_Leave;
            // 
            // btnYesAll
            // 
            btnYesAll.BackColor = Color.FromArgb(0, 64, 0);
            btnYesAll.DialogResult = DialogResult.OK;
            btnYesAll.FlatStyle = FlatStyle.Flat;
            btnYesAll.ForeColor = Color.White;
            btnYesAll.Location = new Point(81, 3);
            btnYesAll.Name = "btnYesAll";
            btnYesAll.Size = new Size(75, 30);
            btnYesAll.TabIndex = 2;
            btnYesAll.Text = "بلي(همه)";
            btnYesAll.UseVisualStyleBackColor = false;
            btnYesAll.Enter += Button_Enter;
            btnYesAll.Leave += Button_Leave;
            // 
            // btnYes
            // 
            btnYes.BackColor = Color.FromArgb(0, 192, 0);
            btnYes.DialogResult = DialogResult.Yes;
            btnYes.FlatStyle = FlatStyle.Flat;
            btnYes.ForeColor = Color.White;
            btnYes.Location = new Point(248, 3);
            btnYes.Name = "btnYes";
            btnYes.Size = new Size(75, 30);
            btnYes.TabIndex = 0;
            btnYes.Text = "بلي";
            btnYes.UseVisualStyleBackColor = false;
            btnYes.Enter += Button_Enter;
            btnYes.Leave += Button_Leave;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.Image = Resources.Question;
            pictureBox1.Location = new Point(391, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(72, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // YesNoMessageBox
            // 
            BackColor = Color.FromArgb(181, 172, 9);
            ClientSize = new Size(470, 183);
            Controls.Add(pnlMain);
            Controls.Add(pnlTop);
            Font = new Font("IRANSans(FaNum)", 10F, FontStyle.Regular, GraphicsUnit.Point, 178);
            FormBorderStyle = FormBorderStyle.None;
            Name = "YesNoMessageBox";
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterParent;
            Load += ConfirmMessageBox_Load;
            pnlTop.ResumeLayout(false);
            ((ISupportInitialize) ClosePictureBox).EndInit();
            pnlMain.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            ((ISupportInitialize) pictureBox1).EndInit();
            ResumeLayout(false);
        }

        private void ClosePictureBox_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == (Keys.Alt | Keys.F4))
            {
                DialogResult = DialogResult.No;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClosePictureBox_MouseEnter(object sender, EventArgs e)
        {
            ClosePictureBox.BackColor = Color.DarkRed;
        }

        private void ClosePictureBox_MouseLeave(object sender, EventArgs e)
        {
            ClosePictureBox.BackColor = Color.Transparent;
        }

        private void Button_Leave(object sender, EventArgs e)
        {
            ((Button) sender).FlatAppearance.BorderColor = Color.White;
            ((Button) sender).FlatAppearance.BorderSize = 1;
        }

        private void Button_Enter(object sender, EventArgs e)
        {
            ((Button) sender).FlatAppearance.BorderColor = Color.Blue;
            ((Button) sender).FlatAppearance.BorderSize = 1;
        }

        //================================================================
        /// <summary>
        ///     Load User Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmMessageBox_Load(object sender, EventArgs e)
        {
            try
            {
                MTF_UserControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        ///     Starting Thread
        /// </summary>
        private void MTF_UserControl()
        {
            try
            {
                threadStartLoad = MTF_UserControlLoad_Load;
                threadLoad = new Thread(threadStartLoad)
                {
                    Priority = ThreadPriority.AboveNormal,
                    IsBackground = true //<— Set the thread to work in background
                };
                //
                threadLoad.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        ///     Act Thread
        /// </summary>
        private void MTF_UserControlLoad_Load()
        {
            Invoke(new MethodInvoker(delegate
            {
                if (!ShowAllButton)
                {
                    btnNoAll.Visible = false;
                    btnYesAll.Visible = false;
                }

                if (LoadOnYesButton)
                    btnYes.Focus();
                else
                    btnNo.Focus();
            }));
        }
    }
}