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

namespace Desktop_Lock
{
    /// <summary>
    /// SetUp.xaml 的交互逻辑
    /// </summary>
    public partial class SetUp : Window
    {
        private static string last1 = "";   //全局变量,last，只有确定后才保存到配置文件
        private static string startup1 = "";//全局变量,startup1，只有确定后才保存到配置文件
        public SetUp()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2;
            this.Left = (workWidth - this.Width) / 2;
            //获得配置文件mode
            MainWindow ma = new MainWindow();
            string mode = ma.Obtain("mode");
            //设置单选框默认值
            if (mode == "1")
            {
                this.radio1.IsChecked = true;
            }
            else if (mode == "2")
            {
                this.radio2.IsChecked = true;
            }
            else if (mode == "3")
            {
                this.radio3.IsChecked = true;
            }
            //设置last选项默认值
            //读取配置文件last选项
            MainWindow ma1 = new MainWindow();
            string last = ma1.Obtain("last");
            //把配置文件中的值给全局变量last1一份
            last1 = ma1.Obtain("last");
            if (last == "off")
            {
                //如果配置文件中last为off则更改初始为off
                img5.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
            }
            else if (last == "on")
            {
                //如果配置文件中last为off则更改初始为on
                img5.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
            }
            //设置startup选项默认值
            //读取配置文件startup选项
            string statrup = ma1.Obtain("startup");
            //把配置文件中的值给全局变量startup1一份
            startup1 = ma1.Obtain("startup");
            if (statrup == "off")
            {
                //如果配置文件中last为off则更改初始为off
                img6.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
            }
            else if (statrup == "on")
            {
                //如果配置文件中last为off则更改初始为on
                img6.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
            }
        }
        private void img1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //获得配置文件mode
            MainWindow ma = new MainWindow();
            //确定选择后存入配置文件
            if (this.radio1.IsChecked == true)
            {
                ma.Modify("mode", "1");
            }
            else if(this.radio2.IsChecked == true)
            {
                ma.Modify("mode", "2");
            }
            else if(this.radio3.IsChecked == true)
            {
                ma.Modify("mode", "3");
            }
            //把使用上次密码选项和开机自起选项给配置文件
            ma.Modify("last", last1);
            ma.Modify("startup", startup1);
            //通知窗口已经关闭
            MainWindow.sw = true;
            //关闭当前窗口
            this.Close();
        }
        private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //通知窗口已经关闭
            MainWindow.sw = true;
            //关闭当前窗口
            this.Close();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            //通知窗口已经关闭(关闭事件)
            MainWindow.sw = true;
        }

        private void radio1_Checked(object sender, RoutedEventArgs e)
        {
            //如果复选框选择密码解锁,则显示密码解锁图标
            img3.Source = new BitmapImage(new Uri("/Image/password.png", UriKind.Relative));
        }

        private void radio2_Checked(object sender, RoutedEventArgs e)
        {
            //如果复选框选择时间解锁,则显示时间解锁图标
            img3.Source = new BitmapImage(new Uri("/Image/time.png", UriKind.Relative));
        }

        private void radio3_Checked(object sender, RoutedEventArgs e)
        {
            //如果复选框选择U盘解锁,则显示U盘解锁图标
            img3.Source = new BitmapImage(new Uri("/Image/usb.png", UriKind.Relative));
        }

        private void img5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //如果单击使用上次密码解锁选项
            if(last1 == "off")
            {
                //如果last等于off则修改为on
                img5.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
                //修改last1全局变量的值为on
                last1 = "on";
                //获得上次解锁用的密码
                MainWindow ma = new MainWindow();
                if (ma.Obtain("password") == null)
                {
                    //调用自定义弹窗
                    Warning1 warn = new Warning1();
                    Warning1.txt = "密码为空!";
                    if (warn.ShowDialog() == true)
                    {
                        warn.Show();
                    }
                    //则修改回off
                    img5.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                    //修改last1回off
                    last1 = "off";
                }
            }
            else if(last1 == "on")
            {
                //如果last等于on则修改为off
                img5.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                //修改last1全局变量的值为off
                last1 = "off";
            }

        }

        private void img6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //如果单击开机自启
            if (startup1 == "off")
            {
                //如果last等于off则修改为on
                img6.Source = new BitmapImage(new Uri("/Image/on.png", UriKind.Relative));
                //修改last1全局变量的值为on
                startup1 = "on";
            }
            else if (startup1 == "on")
            {
                //如果last等于on则修改为off
                img6.Source = new BitmapImage(new Uri("/Image/off.png", UriKind.Relative));
                //修改last1全局变量的值为off
                startup1 = "off";
            }
        }

    }
}
