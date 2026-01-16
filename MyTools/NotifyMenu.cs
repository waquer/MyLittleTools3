using System;
using System.Windows.Forms;

namespace MyLittleTools3.MyTools
{
    internal class NotifyMenu
    {
        private NotifyForm _notifyForm = new NotifyForm();

        private readonly ToolStripSeparator _menuSp = new ToolStripSeparator();

        private readonly ToolStripMenuItem _menuExit = new ToolStripMenuItem();

        private ContextMenuStrip _contextMenu;

        public ContextMenuStrip GetNotifyMenu()
        {
            if (_contextMenu != null) return _contextMenu;
            _menuExit.Text = @"Exit";
            _menuExit.Click += MenuExit_Click;

            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.AddRange(new ToolStripItem[] {
                _menuSp,
                _menuExit
            });
            return _contextMenu;
        }

        private static void MenuExit_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void AddLog(string log)
        {
            if (_notifyForm == null)
            {
                _notifyForm = new NotifyForm();
            }
            _notifyForm.Show();
            _notifyForm.AddLog(log);
        }

    }
}
