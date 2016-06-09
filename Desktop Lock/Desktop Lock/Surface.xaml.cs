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
    /// Surface.xaml 的交互逻辑
    /// </summary>
    public partial class Surface : Window
    {
        public Surface()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2;
            this.Left = (workWidth - this.Width) / 2;
            //初始化按钮
            Refresh();
        }
        //全局计时器
        DispatcherTimer clock = new DispatcherTimer();
        //全局变量,如果锁屏了就关闭计时器界面
        public static Boolean sd { get; set; }  //设置开关控制只能打开一个辅窗口
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //设置计时器属性
            clock.Interval = new TimeSpan(0, 0, 0, 0, 200);
            clock.Tick += new EventHandler(timer_kkick);
            clock.Start();
            //默认打开当前窗口,锁屏后关闭
            sd = false;
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
            //关闭当前窗口
            this.Close();
        }
        private void img3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //定时锁屏按键事件
            //如果单击定时锁屏
            if (Timing.timing_lock == false)
            {
                //获得配置文件中的mode和password
                MainWindow ma = new MainWindow();
                string mode = ma.Obtain("mode");
                string password = ma.Obtain("password");
                //如果timing_lock等于off则修改为on
                img3.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
                //判断是否使用功能
                int text1 = Convert.ToInt32(textbox1.Text);
                if (text1 <= 0)
                {
                    //如果textbox1等于0,则弹窗提示无法计时
                    //调用自定义弹窗
                    Warning1 warn = new Warning1();
                    Warning1.txt = "定时数值无效!";
                    if (warn.ShowDialog() == true)
                    {
                        warn.Show();
                    }
                    //如果timing_lock等于on则修改为off
                    img3.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                }
                else if (mode == "1" && password == null)
                {   //如果配置文件的设置,锁屏方式为密码锁屏,并且没有储存的密码,则报错
                    //调用自定义弹窗
                    Warning1 warn = new Warning1();
                    Warning1.txt = "密码为空,无法锁屏!";
                    if (warn.ShowDialog() == true)
                    {
                        warn.Show();
                    }
                    //如果timing_lock等于on则修改为off
                    img3.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                }
                else
                {
                    //修改timing_lock全局变量的值为on
                    Timing.timing_lock = true;
                    //把锁屏的值传给主函数用于做计时
                    int lock_time = Convert.ToInt32(textbox1.Text) * 60;
                    Timing.lock_time = lock_time;
                }
            }
            else if (Timing.timing_lock == true)
            {
                //如果timing_lock等于on则修改为off
                img3.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                //修改timing_lock全局变量的值为off
                Timing.timing_lock = false;
            }
        }

        private void img4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //定时关机按键事件
            //如果单击定时关机
            if (Timing.timing_shutdown == false)
            {
                //如果timing_shutdown等于off则修改为on
                img4.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
                //判断是否使用功能
                int text2 = Convert.ToInt32(textbox2.Text);
                if (text2 <= 0)
                {
                    //如果textbox1等于0,则弹窗提示无法计时
                    //调用自定义弹窗
                    Warning1 warn = new Warning1();
                    Warning1.txt = "定时数值无效!";
                    if (warn.ShowDialog() == true)
                    {
                        warn.Show();
                    }
                    //如果timing_shutdown等于on则修改为off
                    img4.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                }
                else
                {
                    //修改timing_shutdown全局变量的值为on
                    Timing.timing_shutdown = true;
                    //把锁屏的值传给主函数用于做计时
                    int shutdown_time = Convert.ToInt32(textbox2.Text) * 60;
                    Timing.shutdown_time = shutdown_time;
                }
            }
            else if (Timing.timing_shutdown == true)
            {
                //如果timing_shutdown等于on则修改为off
                img4.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                //修改timing_shutdown全局变量的值为off
                Timing.timing_shutdown = false;
            }
        }

        #region 计时器属性
        private void timer_kkick(object sender, EventArgs e)
        {
            //刷新按钮和textbox;
            Refresh();
            //如果已经锁屏了就关闭定时器窗口
            if(sd == true)
            {
                //关闭计时器窗口
                this.Close();
            }
        }
        #endregion

        #region 初始化定时时间和按钮
        private void Refresh()
        {
            //初始化定时器锁屏开关
            if (Timing.timing_lock == true)
            {
                //如果配置文件中timing_lock为off则更改初始为off
                img3.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
            }
            else if (Timing.timing_lock == false)
            {
                //如果配置文件中timing_lock为off则更改初始为on
                img3.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
            }
            //初始化定时器关机开关
            if (Timing.timing_shutdown == true)
            {
                //如果配置文件中timing_shutdown为off则更改初始为off
                img4.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
            }
            else if (Timing.timing_shutdown == false)
            {
                //如果配置文件中timing_shutdown为off则更改初始为on
                img4.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
            }
            //如果定时锁屏开启,则写入减少的时间
            if(Timing.timing_lock == true)
            {
                if(Timing.lock_time > 0)
                {
                    textbox1.Text = Math.Ceiling((float)Timing.lock_time / 60).ToString();
                }
                else
                {
                    textbox1.Text = "0";
                }
            }
            //如果定时关机开启,则写入减少的时间
            if (Timing.timing_shutdown == true)
            {
                if (Timing.shutdown_time > 0)
                {
                    textbox2.Text = Math.Ceiling((float)Timing.shutdown_time / 60).ToString();
                }
                else
                {
                    textbox2.Text = "0";
                }
            }
        }
        #endregion

        #region textbox1 设置只能输入数字
        private void textBox1_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!isNumberic(text))
                { e.CancelCommand(); }
            }
            else { e.CancelCommand(); }
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!isNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
        }
        #endregion

        #region textbox2 设置只能输入数字
        private void textBox2_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!isNumberic(text))
                { e.CancelCommand(); }
            }
            else { e.CancelCommand(); }
        }

        private void textBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void textBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!isNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
        }
        #endregion

        #region isDigit是否是数字
        public static bool isNumberic(string _string) 
        { 
            if (string.IsNullOrEmpty(_string)) 
                return false; 
            foreach (char c in _string) 
            { 
                if (!char.IsDigit(c)) 
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右 
                    return false; 
            } 
            return true; 
        } 
        #endregion

        #region 监听回车
        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //调用按钮的事件处理代码
                img3_MouseDown(null, null);
            }
        }

        private void textbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //调用按钮的事件处理代码
                img4_MouseDown(null, null);
            }
        }
        #endregion

    }
}
