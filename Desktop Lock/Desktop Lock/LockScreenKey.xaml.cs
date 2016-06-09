using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics; //相关于结束任务管理器的
using System.Runtime.InteropServices; //调用dll
using System.Windows.Forms; //使用winform计时器
using System.Net;
using System.Security.Cryptography;
using System.Windows.Interop;
using Microsoft.Win32;

namespace Desktop_Lock
{
    /// <summary>
    /// LockScreenKey.xaml 的交互逻辑
    /// </summary>
    public partial class LockScreenKey : Window
    {
        public LockScreenKey()
        {
            InitializeComponent();
        }
        //全局计时器变量
        Timer timer1 = new Timer();
        Timer timer2 = new Timer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获取文本框焦点
            passwordbox.Focus();
            //获得配置文件中的skin
            MainWindow ma = new MainWindow();
            string skin = ma.Obtain("skin");
            //选择图片路径
            if (skin == "1")
            {
                img.Source = new BitmapImage(new Uri("/Image/autumn.png", UriKind.Relative));
            }
            else if (skin == "2")
            {
                img.Source = new BitmapImage(new Uri("/Image/GTGraphics.png", UriKind.Relative));
            }
            else if (skin == "3")
            {
                img.Source = new BitmapImage(new Uri("/Image/Mountainriver.png", UriKind.Relative));
            }
            else if (skin == "4")
            {
                //获得配置文件中的image
                MainWindow ma2 = new MainWindow();
                string image = ma2.Obtain("image");
                BitmapImage imagetemp = new BitmapImage(new Uri(image, UriKind.Absolute));
                img.Source = imagetemp;
            }
            //调整背景图片大小
            rectangle.Width = Width;
            rectangle.Height = Height;
            img.Width = Width;
            img.Height = Height;
            //计时器1
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(timer1_Tick);
            //计时器2
            timer2.Enabled = true;
            timer2.Interval = 10;
            timer2.Tick += new System.EventHandler(timer2_Tick);
            //启动键盘钩子   
            if (hKeyboardHook == 0)
            {
                //实例化委托  
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            }
            //修改配置文件(当前处于锁屏状态)
            MainWindow ma1 = new MainWindow();
            ma1.Modify("lock", "true");
            //关闭主窗口
            ma1.Modify("closemain", "true");
            //如果开启了强力模式则写入注册表
            if(ma1.Obtain("startup") == "on")
            {
                string a = "\"" + System.Windows.Forms.Application.StartupPath + "\\" + System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name) + ".exe" + "\"";
                RegistryKey reg = null;
                reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                {
                    reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                }
                reg.SetValue("DLock", a);
                reg.Close();
            }
            //刷新组件位置
            RefreshControl();
        }

        #region 登录按钮、控制解锁主界面
        string password; //储存解锁密码的变量
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //检测密码输入框的值
            string s = Pass();
            //获得配置文件中的mode
            MainWindow ma3 = new MainWindow();
            string mode = ma3.Obtain("mode");
            //检测密码来源
            if (mode == "1")
            {
                //得到配置文件中存的密码
                MainWindow ma = new MainWindow();
                password = ma.Obtain("password");
                //判断密码是否正确
                if (password == s)
                {
                    //结束计时器
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    //卸载钩子
                    bool retKeyboard = true;
                    if (hKeyboardHook != 0)
                    {
                        retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                        hKeyboardHook = 0;
                    }
                    //如果卸下钩子失败   
                    if (!(retKeyboard)) throw new Exception("卸下钩子失败!");
                    //通知窗口已经关闭
                    MainWindow.sw = true;
                    //修改配置文件(当前处于解锁状态)
                    ma.Modify("lock", "false");
                    //如果正常关闭则删除注册表
                    delregedit();
                    //跳转到主界面
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    passwordbox.Password = "";
                    System.Windows.MessageBox.Show("密码输入错误!");
                }
            }
            else if (mode == "2")
            {
                password = timeKey();
                //判断密码是否正确
                if (password == s)
                {
                    //结束计时器
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    //卸载钩子
                    bool retKeyboard = true;
                    if (hKeyboardHook != 0)
                    {
                        retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                        hKeyboardHook = 0;
                    }
                    //如果卸下钩子失败   
                    if (!(retKeyboard)) throw new Exception("卸下钩子失败!");
                    //通知窗口已经关闭
                    MainWindow.sw = true;
                    //修改配置文件(当前处于解锁状态)
                    MainWindow ma = new MainWindow();
                    ma.Modify("lock", "false");
                    //如果正常关闭则删除注册表
                    delregedit();
                    //跳转到主界面
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    passwordbox.Password = "";
                    System.Windows.MessageBox.Show("密码输入错误!");
                }
            }
        }
        #endregion

        #region 删除注册表
         private void delregedit()
        {
            RegistryKey reg = null;
            reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            reg.DeleteValue("DLock", false);
            reg.Close();
        }
        #endregion

        #region 调整控件适应分辨率
        private void RefreshControl()
         {
             if (Width < 1400)
             {
                 passwordbox.Width = 180;
                 slider.Width = 180;
             }
             Window window = Window.GetWindow(img1);
             Point point = img1.TransformToAncestor(window).Transform(new Point(0, 0));
             textblock1.Margin = new Thickness(point.X + 25, point.Y + 165, 0, 0);
             button.Margin = new Thickness(point.X + 300, point.Y + 163, 0, 0);
             textblock2.Margin = new Thickness(point.X + 25, point.Y + 212, 0, 0);
             slider.Margin = new Thickness((Width - 240) / 2, (Height + 55) / 2 + 50, 0, 0);
             passwordbox.Margin = new Thickness((Width - 230) / 2, (Height - 35) / 2 + 50, 0, 0);
         }
        #endregion

        #region 拖拽控制背景透明度
         private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double d = double.Parse(this.slider.Value.ToString()) / 100;
            img.Opacity = d;
        }
        #endregion

        #region 时间密码
        private String timeKey()
        {
            /*
             * 时间密码
            DateTime.Now.Year.ToString();       获取年
            DateTime.Now.Month.ToString();      获取月
            DateTime.Now.Day.ToString();        获取日 
            DateTime.Now.Hour.ToString();       获取小时
            DateTime.Now.Minute.ToString();     获取分钟
             * 2016513024   2016年5月13日  0:24
            */
            String a = DateTime.Now.Year.ToString();
            String b = DateTime.Now.Month.ToString();
            String c = DateTime.Now.Day.ToString();
            String d = DateTime.Now.Hour.ToString();
            String e = DateTime.Now.Minute.ToString();
            String time = a + b + c + d + e;
            //把时间密码加密MD5
            //获得主机名+9260后进行16位md5加密
            string HostName = time + "0461";
            //完整的文件格式
            string s = UserMd5(HostName);
            string key = s.Substring(0, s.Length - 2);
            return key;
        }
        #endregion

        #region MD5加密32位
        static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        #endregion

        #region 登录密码加密
        private String Pass()
        {
            //获得主机名+9260后进行16位md5加密
            string HostName = passwordbox.Password +"0461";
            //完整的文件格式
            string s = UserMd5(HostName);
            string key = s.Substring(0, s.Length - 2);
            return key;
        }
        #endregion

        #region 监听回车按键
        private void passwordbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //调用按钮的事件处理代码
                Button_Click(sender, e);
            }
        }
        #endregion

        #region 计时器1:锁屏时间、监控鼠标键盘10秒内是否有操作
        //计时器1
        int s = 0;//时
        int f = 0;//分
        int m = 0;//秒
        string ss;
        string ff;
        string mm;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //每秒刷新
            //界面运行时间
            m++;
            if (m >= 60)
            {
                m = 0;
                f += 1;
            }
            else if (f >= 60)
            {
                f = 0;
                s += 1;
            }
            else if (s >= 60)
            {
                s = 0;
            }
            if (m < 10)
            {
                mm = "0" + m.ToString();
            }
            else
            {
                mm = m.ToString();
            }
            if (f < 10)
            {
                ff = "0" + f.ToString();
            }
            else
            {
                ff = f.ToString();
            }
            if (s < 10)
            {
                ss = "0" + s.ToString();
            }
            else
            {
                ss = s.ToString();
            }
            string test = ss + ":" + ff + ":" + mm;
            //textbox2.Text = test;
            //监控鼠标键盘10秒内是否有操作，如果无操作则隐藏密码输入框
            if (GetIdleTick() / 1000 > 10)
            {
                //鼠标键盘超过10秒没有操作，隐藏对话框
                img1.Visibility = System.Windows.Visibility.Collapsed; //隐藏底框
                img2.Visibility = System.Windows.Visibility.Collapsed; //隐藏logo
                textblock1.Visibility = System.Windows.Visibility.Collapsed; //隐藏文字1
                textblock2.Visibility = System.Windows.Visibility.Collapsed; //隐藏文字2
                passwordbox.Visibility = System.Windows.Visibility.Collapsed; //隐藏密码输入框
                button.Visibility = System.Windows.Visibility.Collapsed; //隐藏确认按钮
                slider.Visibility = System.Windows.Visibility.Collapsed; //隐藏透明度
            }
            else
            {
                //鼠标键盘10秒内有操作，显示对话框
                img1.Visibility = System.Windows.Visibility.Visible; //显示底框
                img2.Visibility = System.Windows.Visibility.Visible; //显示logo
                textblock1.Visibility = System.Windows.Visibility.Visible; //显示文字1
                textblock2.Visibility = System.Windows.Visibility.Visible; //显示文字2
                passwordbox.Visibility = System.Windows.Visibility.Visible; //显示密码输入框
                button.Visibility = System.Windows.Visibility.Visible; //显示确认按钮
                slider.Visibility = System.Windows.Visibility.Visible; //显示透明度
                passwordbox.Focus(); //获得文本焦点
            }
        }
        #endregion

        #region 计时器2:关闭任务管理器、窗口置顶、刷新组件位置
        private void timer2_Tick(object sender, EventArgs e)
        {   //每毫秒刷新
            //关闭任务管理器
            Process[] p = Process.GetProcesses();
            foreach (Process p1 in p)
            {
                try
                {
                    if (p1.ProcessName.ToLower().Trim() == "taskmgr")//这里判断是任务管理器    
                    {
                        p1.Kill();
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
            //刷新组件位置
            RefreshControl();
            //刷新窗口置顶
            this.Topmost = true;
        }
        #endregion

        #region 监控鼠标键盘是否有操作
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static long GetIdleTick()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            if (!GetLastInputInfo(ref　lastInputInfo)) return 0;
            return Environment.TickCount - (long)lastInputInfo.dwTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        #endregion

        #region 键盘钩子
        /// 声明回调函数委托  
        /// </summary>  
        /// <param name="nCode"></param>  
        /// <param name="wParam"></param>  
        /// <param name="lParam"></param>  
        /// <returns></returns>  
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>  
        /// 委托实例  
        /// </summary>  
        HookProc KeyboardHookProcedure;

        /// <summary>  
        /// 键盘钩子句柄  
        /// </summary>  
        static int hKeyboardHook = 0;

        //装置钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //卸下钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //获取某个进程的句柄函数  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>  
        /// 普通按键消息  
        /// </summary>  
        private const int WM_KEYDOWN = 0x100;
        /// <summary>  
        /// 系统按键消息  
        /// </summary>  
        private const int WM_SYSKEYDOWN = 0x104;

        //鼠标常量   
        public const int WH_KEYBOARD_LL = 13;

        //声明键盘钩子的封送结构类型   
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode; //表示一个在1到254间的虚似键盘码   
            public int scanCode; //表示硬件扫描码   
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        /// <summary>  
        /// 截取全局按键，发送新按键，返回  
        /// </summary>  
        /// <param name="nCode"></param>  
        /// <param name="wParam"></param>  
        /// <param name="lParam"></param>  
        /// <returns></returns>  
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;
                //屏蔽的按键
                if (keyData == System.Windows.Forms.Keys.LWin)
                {
                    // 截获左win(开始菜单键)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.RWin)
                {
                    // 截获右win(开始菜单键)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Escape && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control)
                {
                    // 截获ctrl+esc(开始菜单)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Escape)
                {
                    // 截获alt+esc(任务栏，暂不好使)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LShiftKey + (int)System.Windows.Forms.Keys.Escape)
                {
                    //截获alt+shift+esc(暂不好使)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.RShiftKey + (int)System.Windows.Forms.Keys.Escape)
                {
                    //截获alt+shift+esc(暂不好使)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Shift + (int)System.Windows.Forms.Keys.Tab)
                {
                    //截获alt+shift+esc(暂不好使)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.F4 && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)
                {
                    //截获alt+F4(关闭)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Tab && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)
                {
                    //截获alt+tab(切换)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.E && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //截获win+e(资源管理器)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Space && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽alt+space(打开快捷方式菜单)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Tab && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽ctrl+alt+tab(可以在打开的项目中切换)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Space && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //屏蔽win+space(预览桌面)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Up && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //屏蔽win+↑(调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Down && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //屏蔽win+↓(调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Left && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //屏蔽win+←(调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Right && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //屏蔽win+→(调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Up && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽ctrl+alt+↑(屏蔽屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Down && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽ctrl+alt+↓(屏蔽屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Left && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽ctrl+alt+←(屏蔽屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Right && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽ctrl+alt+→(屏蔽屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt)
                {
                    //屏蔽alt键(靠刷新率关掉进程)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.F4)
                {
                    //屏蔽F4键(靠刷新率关掉进程)
                    return 1;
                }
                /* 不在屏蔽空格,空格可以作为密码
                if (keyData == System.Windows.Forms.Keys.Space)
                {
                    //屏蔽space(靠刷新率关掉进程)
                    return 1;
                }
                 */
                if (keyData == System.Windows.Forms.Keys.Tab)
                {
                    //屏蔽TAB(靠刷新率关掉进程)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.F1)
                {
                    //截获F1(个别软件帮助文档)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.A && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //截获ctrl+alt+a(qq截图)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Z && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    //截获ctrl+alt+a(qq主窗口)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.L && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    //截获左win+l(锁屏)无效
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.L && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.RWin)
                {
                    //截获右win+l(锁屏)无效
                    return 1;
                }
            }
            return 0;
        }
        #endregion

    }
}
