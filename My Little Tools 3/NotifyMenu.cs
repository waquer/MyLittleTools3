using System.Drawing;
using System.Windows.Forms;
using System.ServiceProcess;

namespace MyLittleTools3
{
    class Service
    {
        public string serviceName;
        public string serviceTitle;
        public string serviceStatus;

        public bool IsRunning = false;
        public ToolStripMenuItem menuItem;

        public Service(string serviceTitle, string serviceName)
        {
            this.serviceTitle = serviceTitle;
            this.serviceName = serviceName;
        }

        public string ChangeStatus(bool isRunning)
        {
            this.IsRunning = isRunning;
            this.serviceStatus = this.serviceTitle + " is " + (isRunning ? "Running" : "Stopped");
            this.menuItem.Text = this.serviceStatus;
            return serviceStatus;
        }

    }


    class NotifyMenu
    {

        private Service sc_apache = new Service("Apache", "wampapache64");
        private Service sc_mysqld = new Service("MySQL", "wampmysqld64");

        private ToolStripSeparator menu_sp = new ToolStripSeparator();
        private ToolStripMenuItem menu_apacheStatus = new ToolStripMenuItem();
        private ToolStripMenuItem menu_apacheStart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_apacheRestart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_mysqldStatus = new ToolStripMenuItem();
        private ToolStripMenuItem menu_mysqldStart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_mysqldRestart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_allStart = new ToolStripMenuItem();
        private ToolStripMenuItem menu_allStop = new ToolStripMenuItem();
        private ToolStripMenuItem menu_exit = new ToolStripMenuItem();

        private ContextMenuStrip contextMenu;

        private NotifyIcon notifyIcon;

        public void SetNotifyIcon(NotifyIcon notifyIcon)
        {
            this.notifyIcon = notifyIcon;
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.ContextMenuStrip = GetNotifyMenu();
            notifyIcon.Visible = true;
        }

        public ContextMenuStrip GetNotifyMenu()
        {
            if (contextMenu == null) {

                sc_apache.menuItem = menu_apacheStatus;
                sc_apache.ChangeStatus(false);
                sc_mysqld.menuItem = menu_mysqldStatus;
                sc_mysqld.ChangeStatus(false);

                menu_apacheStart.Text = "Apache Start/Stop";
                menu_apacheStart.Click += MenuApacheStart_Click;
                menu_mysqldStart.Text = "MySQL Start/Stop";
                menu_mysqldStart.Click += MenuMysqlStart_Click;

                menu_apacheRestart.Text = "Apache Restart";
                menu_apacheRestart.Click += MenuApacheRestart_Click;
                menu_mysqldRestart.Text = "MySQL Restart";
                menu_mysqldRestart.Click += MenuMysqlRestart_Click;

                menu_allStart.Text = "All Start";
                menu_allStart.Click += MenuAllStart_Click;
                menu_allStop.Text = "All Stop";
                menu_allStop.Click += MenuAllStop_Click;

                menu_exit.Text = "Exit";
                menu_exit.Click += MenuExit_Click;

                contextMenu = new ContextMenuStrip();
                contextMenu.Items.AddRange(new ToolStripItem[] {
                    this.menu_apacheStatus,
                    this.menu_mysqldStatus,
                    this.menu_allStart,
                    this.menu_allStop,
                    this.menu_sp,
                    this.menu_exit
                });
                this.menu_apacheStatus.DropDownItems.AddRange(new ToolStripItem[] {
                    this.menu_apacheStart,
                    this.menu_apacheRestart
                });
                this.menu_mysqldStatus.DropDownItems.AddRange(new ToolStripItem[] {
                    this.menu_mysqldStart,
                    this.menu_mysqldRestart
                });

            }
            return contextMenu;
        }

        private void MenuExit_Click(object sender, System.EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuAllStop_Click(object sender, System.EventArgs e)
        {
            StopService(sc_apache);
            StopService(sc_mysqld);
        }

        private void MenuAllStart_Click(object sender, System.EventArgs e)
        {
            StartService(sc_mysqld);
            StartService(sc_apache);
        }

        private void MenuApacheRestart_Click(object sender, System.EventArgs e)
        {
            StopService(sc_apache);
            StartService(sc_apache);
        }

        private void MenuMysqlRestart_Click(object sender, System.EventArgs e)
        {
            StopService(sc_mysqld);
            StartService(sc_mysqld);
        }

        private void MenuApacheStart_Click(object sender, System.EventArgs e)
        {
            if (sc_apache.IsRunning) {
                StopService(sc_apache);
            } else {
                StartService(sc_apache);
            }
        }

        private void MenuMysqlStart_Click(object sender, System.EventArgs e)
        {
            if (sc_mysqld.IsRunning) {
                StopService(sc_mysqld);
            } else {
                StartService(sc_mysqld);
            }
        }

        private void StartService(Service service)
        {
            ServiceController sc = new ServiceController(service.serviceName);
            if (sc.Status == ServiceControllerStatus.Stopped) {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                service.ChangeStatus(true);
                notifyIcon.BalloonTipText = service.serviceStatus;
                notifyIcon.ShowBalloonTip(2000);
            }
        }

        private void StopService(Service service)
        {
            ServiceController sc = new ServiceController(service.serviceName);
            if (sc.Status != ServiceControllerStatus.Stopped) {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                service.ChangeStatus(false);
                notifyIcon.BalloonTipText = service.serviceStatus;
                notifyIcon.ShowBalloonTip(2000);
            }
        }


    }
}
