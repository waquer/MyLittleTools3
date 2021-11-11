using System;
using System.Windows;
using System.Windows.Forms;

namespace MyLittleTools3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        MyJumpList myJumpList = new MyJumpList();
        MyFileTool myFileTool = new MyFileTool();
        NotifyIcon notifyIcon = new NotifyIcon();
        NotifyMenu notifyMenu = new NotifyMenu();

        public MainWindow(int tabidx = 0)
        {
            InitializeComponent();
            btnUpdateSelf.Content = App.ResourceAssembly.GetName(false).Version;
            lsvJumpList.ItemsSource = myJumpList.JTData;
            ListFiles.ItemsSource = myFileTool.fileList;
            tabMain.SelectedIndex = tabidx;
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            notifyIcon.ContextMenuStrip = notifyMenu.GetNotifyMenu();
            notifyIcon.MouseClick += NotifyIcon_Click;
            notifyIcon.Visible = true;
        }

        /* 更新程序 */
        private void UpdateSelf(object sender, RoutedEventArgs e)
        {
            string filepath = MyIniTool.ReadString("Appconfig", "filepath", "");
            if (filepath.Length > 0) {
                myFileTool.DoUpdate(filepath);
            } else {
                OpenFileDialog OpenFileD = new OpenFileDialog();
                if (OpenFileD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    filepath = OpenFileD.FileName;
                    MyIniTool.WriteString("Appconfig", "filepath", filepath);
                    myFileTool.DoUpdate(filepath);
                }
            }
        }

        /* 设置窗体总在最上状态 */
        private void CbOnTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = (cbOnTop.IsChecked == true) ? true : false;
        }

        /* 最小化时显示隐藏任务栏图标 */
        private void WinMain_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) {
                ShowInTaskbar = false;
            } else {
                ShowInTaskbar = true;
            }
        }

        private void NotifyIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                notifyIcon.ContextMenuStrip.Show();
            } else {
                WindowState = WindowState.Normal;
            }
        }

    }
}
