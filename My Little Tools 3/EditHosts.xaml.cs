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
        String Hfile_bk = Environment.SystemDirectory + @"\drivers\etc\hosts.bk";

        public EditHosts()
        {
            InitializeComponent();
        }

        private void Checklastline()
        {
            if (HEditor.GetLineText(HEditor.LineCount - 1) != "")
            {
                HEditor.AppendText(Environment.NewLine);
            }
        }

        private void Checknewline()
        {
            if (HEditor.Text.Substring(HEditor.SelectionStart - 2, 2) != Environment.NewLine)
            {
                HEditor.SelectedText = Environment.NewLine;
            }
        }

        private void LoadHost()
        {
            String text = File.ReadAllText(Hfile);
            HEditor.Text = text;
            Checklastline();
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

        private void LoadPreset(String filename)
        {
            String pfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            if (File.Exists(pfile))
            {
                String text = File.ReadAllText(pfile);
                HEditor.Text = text;
            }
            else
            {
                HEditor.Text = "文件载入失败";
            }
        }

        private void HostsEditor_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHost();
        }

        private void SavaHostsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveHost();
        }

        private void BtnHostsSave_Click(object sender, RoutedEventArgs e)
        {
            SaveHost();
        }

        private void BtnHostsReload_Click(object sender, RoutedEventArgs e)
        {
            LoadHost();
        }

        private void BtnHostsBackup_Click(object sender, RoutedEventArgs e)
        {
            LoadHost();
        }

        private void BtnHostsPreset1_Click(object sender, RoutedEventArgs e)
        {
            LoadPreset("hosts.p1");

        }

        private void BtnHostsPreset2_Click(object sender, RoutedEventArgs e)
        {
            LoadPreset("hosts.p2");
        }

    }
}
