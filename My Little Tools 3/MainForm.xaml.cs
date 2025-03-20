using System;
using System.Windows;
using System.Windows.Forms;
using MyLittleTools3.MyTools;
using Application = System.Windows.Application;

namespace MyLittleTools3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly MyJumpList _myJumpList = new MyJumpList();
        private readonly MyFileTool _myFileTool = new MyFileTool();
        private readonly NotifyIcon _notifyIcon = new NotifyIcon();
        private readonly NotifyMenu _notifyMenu = new NotifyMenu();

        public MainWindow(int tabidx = 0)
        {
            InitializeComponent();
            BtnUpdateSelf.Content = Application.ResourceAssembly.GetName(false).Version;
            LsvJumpList.ItemsSource = _myJumpList.JtData;
            ListFiles.ItemsSource = _myFileTool.FileList;
            TabMain.SelectedIndex = tabidx;
            _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            _notifyIcon.ContextMenuStrip = _notifyMenu.GetNotifyMenu();
            _notifyIcon.MouseClick += NotifyIcon_Click;
            _notifyIcon.Visible = true;
        }

        /* 更新程序 */
        private void UpdateSelf(object sender, RoutedEventArgs e)
        {
            var filepath = MyIniTool.ReadString("Appconfig", "filepath", "");
            if (filepath.Length > 0) {
            } else {
                var openFileD = new OpenFileDialog();
                if (openFileD.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                filepath = openFileD.FileName;
                MyIniTool.WriteString("Appconfig", "filepath", filepath);
            }

            _myFileTool.DoUpdate(filepath);
        }

        /* 设置窗体总在最上状态 */
        private void CbOnTop_Click(object sender, RoutedEventArgs e)
        {
            Topmost = CbOnTop.IsChecked == true;
        }

        /* 最小化时显示隐藏任务栏图标 */
        private void WinMain_StateChanged(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != WindowState.Minimized;
        }

        private void NotifyIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                _notifyIcon.ContextMenuStrip.Show();
            } else {
                WindowState = WindowState.Normal;
            }
        }

    }
}
