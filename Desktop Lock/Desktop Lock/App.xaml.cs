using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop_Lock
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //进程互斥(只能打开一个进程)
        System.Threading.Mutex mutex;
        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }
        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "WpfMuerterrrterterttex", out ret);
            if (!ret)
                Environment.Exit(0);
        }
    }
}
