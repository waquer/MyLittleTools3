using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MyLittleTools3
{
    /// <summary>
    /// EditHosts.xaml 的交互逻辑
    /// </summary>
    public partial class EditHosts : Window
    {
        String Hfile = Environment.SystemDirectory + @"\drivers\etc\hosts";

        public EditHosts()
        {
            InitializeComponent();
        }

        private void checklastline()
        {
            if (HEditor.GetLineText(HEditor.LineCount - 1) != "")
            {
                HEditor.AppendText(Environment.NewLine);
            }
        }

        private void checknewline()
        {
            if (HEditor.Text.Substring(HEditor.SelectionStart - 2, 2) != Environment.NewLine)
            {
                HEditor.SelectedText = Environment.NewLine;
            }
        }

        private void LoadHost()
        {
            StreamReader sr = new StreamReader(Hfile, Encoding.Default);
            HEditor.Text = sr.ReadToEnd();
            sr.Close();
            checklastline();
            HEditor.ScrollToEnd();
            HEditor.Select(HEditor.Text.Length, 0);
        }

        private void SaveHost()
        {
            if (MessageBox.Show("是否保存Hosts文件？", "确认保存", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(Hfile, false, Encoding.Default);
                    sw.Write(HEditor.Text);
                    sw.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void HostsEditor_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHost();
        }

        private void btnHostsSave_Click(object sender, RoutedEventArgs e)
        {
            SaveHost();
        }

        private void btnHostsReload_Click(object sender, RoutedEventArgs e)
        {
            LoadHost();
        }

        private void SavaHostsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveHost();
        }



    }
}
