using System.Drawing;
using System.Windows.Forms;

namespace Atiran.Utility.MassageBox
{
    public static class ShowPersianMessageBox
    {
        public static DialogResult ShowMessge(string Caption, string Message, MessageBoxButtons buttons,
            bool FocuseOnYes = true, bool ShowAllButton = true)
        {
            var Result = DialogResult.None;
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    using (var Temp = new Form())
                    {
                        Temp.Dock = DockStyle.Fill;
                        Temp.WindowState = FormWindowState.Maximized;
                        Temp.BackColor = Color.Silver;
                        Temp.Opacity = 0.7D;
                        Temp.FormBorderStyle = FormBorderStyle.None;
                        Temp.KeyDown += (sender, e) =>
                        {
                            if (e.KeyCode == Keys.Escape) Temp.Close();
                        };
                        Temp.Load += (sender, e) =>
                        {
                            using (var Confirm = new ConfirmMessageBox())
                            {
                                Confirm.Caption = Caption;
                                Confirm.SetMessage = Message;
                                Result = Confirm.ShowDialog();
                                Temp.Close();
                            }
                        };
                        Temp.ShowDialog();
                    }

                    break;
                case MessageBoxButtons.OKCancel:
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    break;
                case MessageBoxButtons.YesNoCancel:
                    break;
                case MessageBoxButtons.YesNo:
                    using (var Temp = new Form())
                    {
                        Temp.Dock = DockStyle.Fill;
                        Temp.WindowState = FormWindowState.Maximized;
                        Temp.BackColor = Color.Silver;
                        Temp.Opacity = 0.7D;
                        Temp.FormBorderStyle = FormBorderStyle.None;
                        Temp.KeyDown += (sender, e) =>
                        {
                            if (e.KeyCode == Keys.Escape) Temp.Close();
                        };
                        Temp.Load += (sender, e) =>
                        {
                            using (var Confirm = new YesNoMessageBox())
                            {
                                Confirm.Caption = Caption;
                                Confirm.SetMessage = Message;
                                Confirm.LoadOnYesButton = FocuseOnYes;
                                Confirm.ShowAllButton = ShowAllButton;
                                Result = Confirm.ShowDialog();
                                Temp.Close();
                            }
                        };
                        Temp.ShowDialog();
                    }

                    break;
                case MessageBoxButtons.RetryCancel:
                    break;
            }

            return Result;
        }
    }
}