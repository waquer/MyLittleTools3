﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            if (e.Args.Length > 0)
            {
                switch (e.Args[0].ToLower())
                {
                    case "-edithosts":
                        EditHosts editHosts = new EditHosts();
                        editHosts.Show();
                        break;
                    default:
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        break;
                }
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
