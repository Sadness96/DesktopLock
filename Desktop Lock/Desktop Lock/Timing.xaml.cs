using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Threading;

namespace Desktop_Lock
{
    /// <summary>
    /// Timing.xaml 的交互逻辑
    /// </summary>
    public partial class Timing : Window
    {
        public Timing()
        {
            InitializeComponent();
            //设置定时锁屏和定时关机默认为off
            timing_lock = false;
            timing_shutdown = false;
            //设置定时锁屏和定时关机时间为0
            lock_time = 0;
            shutdown_time = 0;
        }
        //全局计时器
        DispatcherTimer clock = new DispatcherTimer();
        public static Boolean timing_lock { get; set; } //设置定时锁屏开关
        public static int lock_time { get; set; } //设置定时锁屏时间
        public static Boolean timing_shutdown { get; set; } //设置定时关机开关
        public static int shutdown_time { get; set; } //设置定时关机计时
        public static Boolean one = true;//设置定时窗口只能打开一个
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //创建一个0*0不显示的窗体,用于控制定时器中的内容
            //设置计时器属性
            clock.Interval = new TimeSpan(0, 0, 0, 1);
            clock.Tick += new EventHandler(timer_kkick);
            clock.Start();
            //运行后不再打开
            one = false;
        }

        private void timer_kkick(object sender, EventArgs e)
        {
            //如果定时锁屏开了并且剩余时间低于0,则锁定屏幕
            if (timing_lock == true)
            {
                if((lock_time--)==0)
                {
                    //关闭定时功能
                    timing_lock = false;
                    //如果当前已经锁屏则不做任何操作
                    MainWindow ma = new MainWindow();
                    if (ma.Obtain("lock") == "false")
                    {
                        //锁住屏幕
                        lockdown();
                    }
                    //关闭计时器窗口
                    Surface.sd = true;
                }
            }
            //如果定时关机开了并且剩余时间低于0,则关闭计算机
            if (timing_shutdown == true)
            {
                if ((shutdown_time--) == 0)
                {
                    //关闭定时功能
                    timing_shutdown = false;
                    //锁住屏幕
                    shutdown();
                    //关闭计时器窗口
                    Surface.sd = true;
                }
            }
        }

        #region 锁屏函数
        public void lockdown()
        {
            //调用配置文件中的mode
            MainWindow ma = new MainWindow();
            string mode = ma.Obtain("mode");
            //锁屏
            if (mode == "1")
            {
                //打开密码锁屏窗口，关闭当前窗口
                LockScreenKey screen = new LockScreenKey();
                screen.Show();
                this.Close();
            }
            else if (mode == "2")
            {
                //打开密码锁屏窗口,密码为时间
                LockScreenKey screen = new LockScreenKey();
                screen.Show();
                this.Close();
            }
            else if (mode == "3")
            {
                //打开U盘锁屏窗口，关闭当前窗口
                LockScreen screen = new LockScreen();
                screen.Show();
                this.Close();
            }
        }
        #endregion

        #region 关机函数
        public void shutdown()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardInput = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.RedirectStandardError = true;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
            myProcess.StandardInput.WriteLine("shutdown -s -f -t 0");
        }
        #endregion
    }
}
