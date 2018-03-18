using System.Windows.Forms;
using System.ServiceProcess;

namespace MyLittleTools3
{
    class NotifyMenu
    {
        private string scname_apache = "wampapache64";
        private string scname_mysqld = "wampmysqld64";

        private ToolStripSeparator menu_sp = new ToolStripSeparator();
        private ToolStripMenuItem menu_apacheStatus = new ToolStripMenuItem();
        private ToolStripMenuItem menu_apacheStart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_apacheRestart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_mysqlStatus = new ToolStripMenuItem();
        private ToolStripMenuItem menu_mysqlStart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_mysqlRestart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_allStart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_allStop = new ToolStripMenuItem();
        private ToolStripMenuItem menu_exit = new ToolStripMenuItem();

        private ContextMenuStrip contextMenu;

        public ContextMenuStrip Instance()
        {
            if (contextMenu == null)
            {
                menu_apacheStatus.Text = "Apache Stoped";
                menu_apacheStart.Text = "Apache Start";
                menu_apacheRestart.Text = "Apache Restart";

                menu_mysqlStatus.Text = "MySQL Stoped";
                menu_mysqlStart.Text = "MySQL Start";
                menu_mysqlRestart.Text = "MySQL Restart";

                menu_allStart.Text = "All Start";
                menu_allStop.Text = "All Stop";

                menu_exit.Text = "Exit";
                menu_exit.Click += MenuExit_Click;

                contextMenu = new ContextMenuStrip();
                contextMenu.Items.AddRange(new ToolStripItem[] {
                    this.menu_apacheStatus,
                    this.menu_mysqlStatus,
                    this.menu_allStart,
                    this.menu_allStop,
                    this.menu_sp,
                    this.menu_exit
                });
                this.menu_apacheStatus.DropDownItems.AddRange(new ToolStripItem[] {
                    this.menu_apacheStart,
                    this.menu_apacheRestart
                });
                this.menu_mysqlStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.menu_mysqlStart,
                    this.menu_mysqlRestart
                });
            }
            return contextMenu;
        }

        private void MenuExit_Click(object sender, System.EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private string CheckServiceStatus(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            return sc.Status == ServiceControllerStatus.Running ? "Running" : "Stoped";
        }

        private void StartService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
            }
        }

        private void StopService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                sc.Stop();
            }
        }


    }
}
