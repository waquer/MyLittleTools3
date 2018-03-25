using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyLittleTools3
{
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
            int x = Screen.PrimaryScreen.WorkingArea.Size.Width - 300;
            int y = Screen.PrimaryScreen.WorkingArea.Size.Height - 200;
            Point p = new Point(x, y);
            this.PointToScreen(p);
            this.Location = p;
        }

        public void AddLog(string log)
        {
            NotifyText.AppendText(log + Environment.NewLine);
        }

        private void NotifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason) {
                case CloseReason.FormOwnerClosing:      // 自身窗口上的关闭按钮
                case CloseReason.MdiFormClosing:        // MDI窗体关闭事件
                case CloseReason.UserClosing:           // 用户通过UI关闭窗口或者通过Alt+F4关闭窗口
                    this.Hide();
                    e.Cancel = true;                    // 拦截，不响应操作
                    break;
                case CloseReason.ApplicationExitCall:   // 应用程序要求关闭窗口
                case CloseReason.TaskManagerClosing:    // 任务管理器关闭进程
                case CloseReason.WindowsShutDown:       // 操作系统准备关机
                case CloseReason.None:                  // 不明原因的关闭
                default:
                    e.Cancel = false;                   // 不拦截，响应操作
                    break;
            }
        }
    }
}
