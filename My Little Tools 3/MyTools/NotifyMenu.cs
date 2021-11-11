using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace MyLittleTools3
{
    class NotifyMenu
    {
        public NotifyForm notifyForm = new NotifyForm();

        private ToolStripSeparator menu_sp = new ToolStripSeparator();

        private ToolStripMenuItem menu_exit = new ToolStripMenuItem();

        private ContextMenuStrip contextMenu;

        public ContextMenuStrip GetNotifyMenu()
        {
            if (contextMenu == null)
            {

                menu_exit.Text = "Exit";
                menu_exit.Click += MenuExit_Click;

                contextMenu = new ContextMenuStrip();
                contextMenu.Items.AddRange(new ToolStripItem[] {
                    this.menu_sp,
                    this.menu_exit
                });
            }
            return contextMenu;
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void AddLog(string log)
        {
            if (notifyForm == null)
            {
                notifyForm = new NotifyForm();
            }
            notifyForm.Show();
            notifyForm.AddLog(log);
        }

    }
}
