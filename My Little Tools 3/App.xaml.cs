using System;
using System.Windows;

namespace MyLittleTools3
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            String arg = e.Args.Length > 0 ? e.Args[0].ToLower() : "";
            if (arg == "-edithosts") {
                EditHosts editHosts = new EditHosts();
                editHosts.Show();
            } else {
                int tabidx = 0;
                if (arg == "-rename") {
                    tabidx = 2;
                }
                MainWindow mainWindow = new MainWindow(tabidx);
                mainWindow.Show();
            }
        }
    }
}
