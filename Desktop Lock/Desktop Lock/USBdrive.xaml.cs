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


namespace Desktop_Lock
{
    /// <summary>
    /// USBdrive.xaml 的交互逻辑
    /// </summary>
    public partial class USBdrive : Window
    {
        //全局变量 密码存放
        string password;
        public USBdrive()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2;
            this.Left = (workWidth - this.Width) / 2;
            //获得主机名+9260后进行16位md5加密
            string HostName = Dns.GetHostName() + "9260";
            password = GetMd5Str(HostName);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*WPF中处理消息首先要获取窗口句柄，创建HwndSource对象 通过HwndSource对象添加
             * 消息处理回调函数.HwndSource类: 实现其自己的窗口过程。 创建窗口之后使用 AddHook 
             * 和 RemoveHook 来添加和移除挂钩，接收所有窗口消息。
            */

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;//窗口过程
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(WndProc));//挂钩
            }
        }
        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            //窗口可移动
            Point poi = Mouse.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void img1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(panfu == "")
            {
                libel1.Content = "请插入U盘";
            }
            else
            {
                //检测如果没有该文件就在写一个
                string key = panfu + "\\" + password + ".log";
                if (!File.Exists(key))
                {
                    FileStream fs1 = new FileStream(key, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine("锁屏秘钥,没解锁前切勿删除");//要写入的信息。 
                    sw.Close();
                    fs1.Close();
                    libel1.Content = "秘钥保存成功";
                }
                else
                {
                    libel1.Content = "秘钥已存在";
                }
            }
        }

        private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //通知窗口已经关闭
            MainWindow.sw = true;
            //关闭当前窗口
            this.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            //通知窗口已经关闭(关闭事件)
            MainWindow.sw = true;
        }

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
                                //MessageBox.Show("U盘插入,盘符为：" + t.Name);
                                libel1.Content = "U盘插入,盘符为:" + t.Name;
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
                        libel1.Content = "U盘已卸载";
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

        private void img3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //第一个参数filename与普通文件名有所不同，设备驱动的“文件名”(常称为“设备路径”)形式固定为“\\.\DeviceName”, 如 string filename = @"\\.\I:";
                string filename = @"\\.\" + panfu.Remove(2);
                //打开设备，得到设备的句柄handle.
                IntPtr handle = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, 0x3, 0, IntPtr.Zero);

                // 向目标设备发送设备控制码，也就是发送命令。IOCTL_STORAGE_EJECT_MEDIA  弹出U盘。
                uint byteReturned;
                bool result = DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);

                //MessageBox.Show(result ? "U盘已退出" : "U盘退出失败");
                if(result == true)
                {
                    libel1.Content = "U盘已退出";
                }
                else
                {
                    libel1.Content = "U盘退出失败";
                }
            }
            catch
            {
                //MessageBox.Show("请开着本界面重新插入U盘");
                libel1.Content = "请开着本界面重新插入U盘";
            }
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

    }
}
