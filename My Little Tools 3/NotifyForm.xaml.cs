using System.Windows;

namespace MyLittleTools3
{
    /// <summary>
    /// NotifyForm.xaml 的交互逻辑
    /// </summary>
    public partial class NotifyForm : Window
    {
        public NotifyForm()
        {
            InitializeComponent();
            this.Left = SystemParameters.WorkArea.Width - this.Width;
            this.Top = SystemParameters.WorkArea.Height - this.Height;
        }

        public void AddLog(string log)
        {
            NotifyText.AppendText(log + System.Environment.NewLine);
        }

        private void FormNotify_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
