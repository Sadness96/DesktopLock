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
    /// LockScreen.xaml 的交互逻辑
    /// </summary>
    public partial class LockScreen : Window
    {
        public LockScreen()
        {
            InitializeComponent();
        }
        //全局计时器变量
        Timer timer1 = new Timer();
        Timer timer2 = new Timer();
        Timer timer3 = new Timer();
        //全局变量 系统用户名
        String WinName = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            //计时器3(U盘解锁)
            timer3.Enabled = true;
            timer3.Interval = 2000;
            timer3.Tick += new System.EventHandler(timer3_Tick);
            //修改配置文件(当前处于锁定状态)
            MainWindow ma = new MainWindow();
            ma.Modify("lock", "true");
            //关闭主窗口
            ma.Modify("closemain", "true");
            //如果开启了强力模式则写入注册表
            if (ma.Obtain("startup") == "on")
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
            //启动键盘钩子   
            if (hKeyboardHook == 0)
            {
                //实例化委托  
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            }
            //给读取U盘盘符使用的方法
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;//窗口过程
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(WndProc));//挂钩
            }
            //读取系统名称
            try
            {
                WinName = System.Environment.UserName;
            }
            catch
            {
                WinName = "Michael XU";
            }
            //调整三行文字的位置
            RefreshControl();
        }

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
            label1.Margin = new Thickness(Width / 2 - 45, Height - 370, 0, 0);
            label2.Margin = new Thickness(Width / 2 - 45, Height - 320, 0, 0);
            label3.Margin = new Thickness(Width / 2 - 45, Height - 270, 0, 0);
        }
        #endregion

        #region U盘密码
        private String UsbKey()
        {
            //获得主机名+9260后进行16位md5加密
            string HostName = Dns.GetHostName() + "9260";
            //完整的文件格式
            string key = panfu + GetMd5Str(HostName) + ".log";
            return key;
        }
        #endregion

        #region 计时器1:锁屏时间

        private void timer1_Tick(object sender, EventArgs e)
        {
            //显示内容 DEPARTMENT
            label1.Content = "DEPARTMENT:Strategic Homeland Intervention,Enforcenment and Logistics Division";
            //显示内容 LOGIN USER
            label2.Content = "LOGIN USER   :" + WinName + ",Level 8 Agent";
            //显示内容 LOGIN DATE
            label3.Content = "LOGIN DATE   :" + DateTime.Now.ToString();
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

        #region 计时器3:U盘解锁
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (File.Exists(UsbKey()))
            {
                //结束计时器
                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = false;
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
        }
        #endregion

        #region MD5加密(16位)
        //md5加密
        public static string GetMd5Str(string ConvertString)
        {
            //MD5加密16位
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        #endregion

        #region 读取U盘盘符
        string panfu = "";
        DriveInfo[] drive = null;
        public const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息
        public const int DBT_DEVICEARRIVAL = 0x8000;  //就是用来表示U盘可用的。一个设备或媒体已被插入一块，现在可用。
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;  //要求更改当前的配置（或取消停靠码头）已被取消。
        public const int DBT_CONFIGCHANGED = 0x0018;  //当前的配置发生了变化，由于码头或取消固定。
        public const int DBT_CUSTOMEVENT = 0x8006; //自定义的事件发生。 的Windows NT 4.0和Windows 95：此值不支持。
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;  //审批要求删除一个设备或媒体作品。任何应用程序也不能否认这一要求，并取消删除。
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;  //请求删除一个设备或媒体片已被取消。
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;  //一个设备或媒体一块即将被删除。不能否认的。
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;  //一个设备特定事件发生。
        public const int DBT_DEVNODES_CHANGED = 0x0007;  //一种设备已被添加到或从系统中删除。
        public const int DBT_QUERYCHANGECONFIG = 0x0017;  //许可是要求改变目前的配置（码头或取消固定）。
        public const int DBT_USERDEFINED = 0xFFFF;  //此消息的含义是用户定义的
        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DEVICECHANGE)
            {
                switch (wParam.ToInt32())
                {
                    case DBT_DEVICEARRIVAL:
                        DriveInfo[] s = DriveInfo.GetDrives();
                        drive = s;
                        s.Any(t =>
                        {
                            if (t.DriveType == DriveType.Removable)
                            {
                                panfu = t.Name;
                                if (!File.Exists(UsbKey()))
                                {
                                    //如果插入U盘时检测不到秘钥则报错
                                    System.Windows.MessageBox.Show("U盘插入,无秘钥!");
                                }
                                //libel1.Content = "U盘插入,盘符为:" + t.Name;
                                DirSearch(t.Name);
                                return true;
                            }
                            return false;
                        });
                        break;
                    case DBT_DEVICEREMOVECOMPLETE:
                        //MessageBox.Show("U盘卸载");
                        DriveInfo[] ss = DriveInfo.GetDrives();
                        //MessageBox.Show("U盘拔出,盘符为：" + getusb(drive, ss));
                        //libel1.Content = "U盘已卸载";
                        break;
                    default:
                        break;
                }
            }
            return IntPtr.Zero;
        }
        private string getusb(DriveInfo[] s, DriveInfo[] ss)
        {
            string usbstr = "未知盘符";
            foreach (DriveInfo d in s)
            {
                foreach (DriveInfo dd in ss)
                {
                    if (d != dd)
                    {
                        usbstr = d.Name;
                    }
                }
            }

            return usbstr;
        }

        private void DirSearch(string path)
        {
            //try
            //{
            //    foreach (string f in Directory.GetFiles(path))
            //    {
            //listview.Items.Add("可移磁盘为：" + path);
            //usb.Add(path);
            //    }
            //    foreach (string d in Directory.GetDirectories(path))
            //    {
            //        DirSearch(d);
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
         string lpFileName,
         uint dwDesireAccess,
         uint dwShareMode,
         IntPtr SecurityAttributes,
         uint dwCreationDisposition,
         uint dwFlagsAndAttributes,
         IntPtr hTemplateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped
        );
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
