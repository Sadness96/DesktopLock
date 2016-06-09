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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2;
            this.Left = (workWidth - this.Width) / 2;
            //默认窗口可以在打开一个
            sw = true;
            //每次启动程序检测是否存在配置文件
            Write();//不存在则创建一个，并重置内容
            //获得配置文件closemain
            string closemain = Obtain("closemain");
            //关闭主窗口(不用关闭)
            Modify("closemain", "false");
        }
        //全局计时器
        DispatcherTimer clock = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //设置计时器属性
            clock.Interval = new TimeSpan(0, 0, 0, 0, 200);
            clock.Tick += new EventHandler(timer_kkick);
            clock.Start();
            //判断是否开了自启功能
            if(Obtain("startup") == "on")
            {
                //每次启动判断程序是否意外关闭,如果意外关闭则直接锁住屏幕
                AccidentalClosure();
            }
            //定时窗口只能打开一次
            if(Timing.one)
            {
                //开机启动定时功能,但不直接使用
                Timing t = new Timing();
                t.Show();
            }

        }

        //全局变量(属性)
        public static Boolean sw { get; set; }  //设置开关控制只能打开一个辅窗口

        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            Point poi = Mouse.GetPosition(this);
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void img1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //关闭程序
            //this.Close();
            //关闭所有的窗口
            Application.Current.Shutdown();
        }

        private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //最小化程序
            this.WindowState = WindowState.Minimized;
        }

        private void img3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //获得配置变量中的mode
            MainWindow ma = new MainWindow();
            string mode = ma.Obtain("mode");
            //获得配置变量中的last
            string last = ma.Obtain("last");
            if (sw == true)
            {
                //判断密码方式
                if (mode == "1")
                {
                    //密码解锁
                    //已经打开一个窗口了
                    sw = false;
                    //如果在设置中选择了使用上一次的密码,则直接跳到锁屏界面
                    if(last == "on")
                    {
                        //打开密码锁屏窗口，关闭当前窗口
                        LockScreenKey screen = new LockScreenKey();
                        screen.Show();
                        this.Close();
                    }
                    else
                    {
                        //打开密码锁屏窗口
                        PassWord pass = new PassWord();
                        if (pass.ShowDialog() == true)
                        {
                            pass.Show();
                        }
                    }
                }
                else if(mode == "2")
                {
                    //时间解锁
                    //已经打开一个窗口了
                    sw = false;
                    //使用系统时间作为锁屏 格式为201652015
                    LockScreenKey screen = new LockScreenKey();
                    screen.Show();
                    this.Close();
                }
                else if(mode == "3")
                {
                    //U盘解锁,没有输入框和按钮
                    //已经打开一个窗口了
                    sw = false;
                    //调用自定义弹窗(从这里进入U盘锁屏)
                    Warning2 warn = new Warning2();
                    if (warn.ShowDialog() == true)
                    {
                        warn.Show();
                    }
                }
            }
        }

        private void img4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sw == true)
            {
                //已经打开一个窗口了
                sw = false;
                //打开换肤窗口
                Skin skin = new Skin();
                if (skin.ShowDialog() == true)
                {
                    skin.Show();
                }
            }
        }

        private void img5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sw == true)
            {
                //已经打开一个窗口了
                sw = false;
                //打开设置窗口
                SetUp setup = new SetUp();
                if (setup.ShowDialog() == true)
                {
                    setup.Show();
                }
            }
        }

        private void img6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sw == true)
            {
                //已经打开一个窗口了
                sw = false;
                //打开U盘设置窗口
                USBdrive drive = new USBdrive();
                if (drive.ShowDialog() == true)
                {
                    drive.Show();
                }
            }
        }

        private void img7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //打开系统时间设置窗口
            Surface surface = new Surface();
            if (surface.ShowDialog() == true)
            {
                surface.Show();
            }
        }

        private void img8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //打开关于窗口
            About about = new About();
            if (about.ShowDialog() == true)
            {
                about.Show();
            }
        }

        #region 计时器1,是否关闭主界面
        private void timer_kkick(object sender, EventArgs e)
        {
            //获得配置文件closemain
            string closemain = Obtain("closemain");
            if (closemain == "true")
            {
                //结束计时器
                clock.Stop();
                //关闭主窗口
                this.Close();
            }
        }
        #endregion

        #region 意外关闭
        private void AccidentalClosure()
        {
            if(Obtain("lock") == "true")
            {
                if(Obtain("mode") == "1")
                {
                    //打开密码锁屏窗口，关闭当前窗口
                    LockScreenKey screen = new LockScreenKey();
                    screen.Show();
                    this.Close();
                }
                else if (Obtain("mode") == "2")
                {
                    //打开密码锁屏窗口,密码为时间
                    LockScreenKey screen = new LockScreenKey();
                    screen.Show();
                    this.Close();
                }
                else if (Obtain("mode") == "3")
                {
                    //打开U盘锁屏窗口，关闭当前窗口
                    LockScreen screen = new LockScreen();
                    screen.Show();
                    this.Close();
                }
            }
        }
        #endregion

        #region 配置文件类:写入
        private void Write()
        {   //写入文件
            //创建文件路径名变量
            string file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "DLock.ini";
            if (!File.Exists(file))
            {   //判断文件是否存在，不存在则创建一个
                IniFileClass.INIWriteValue(file, "DesktopLock", "lock", "false");       //锁屏状态
                IniFileClass.INIWriteValue(file, "DesktopLock", "startup", "off");      //开机自启
                IniFileClass.INIWriteValue(file, "DesktopLock", "last", "off");         //使用上一次密码解锁
                IniFileClass.INIWriteValue(file, "DesktopLock", "mode", "1");           //锁屏方式
                IniFileClass.INIWriteValue(file, "DesktopLock", "password", "");        //自定义密码
                IniFileClass.INIWriteValue(file, "DesktopLock", "skin", "1");           //皮肤选项
                IniFileClass.INIWriteValue(file, "DesktopLock", "image", "");           //皮肤目录
                IniFileClass.INIWriteValue(file, "DesktopLock", "closemain", "flase");  //是否需要关闭主窗口
            }
        }
        #endregion

        #region 配置文件类:修改
        public void Modify(String key, String value)
        {
            string file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "DLock.ini";
            IniFileClass.INIWriteValue(file, "DesktopLock", key, value); //以传过来的值修改配置文件
        }
        #endregion

        #region 配置文件类:读取
        public String Obtain(String key)
        {
            string file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "DLock.ini";
            string value = IniFileClass.INIGetStringValue(file, "DesktopLock", key, null);
            return value;
        }
        #endregion

    }
}
