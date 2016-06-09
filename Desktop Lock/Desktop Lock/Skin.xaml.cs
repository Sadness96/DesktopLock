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
    /// Skin.xaml 的交互逻辑
    /// </summary>
    public partial class Skin : Window
    {
        public Skin()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2;
            this.Left = (workWidth - this.Width) / 2;
            //获得配置文件skin
            MainWindow ma = new MainWindow();
            string mode = ma.Obtain("image");
            string skin = ma.Obtain("skin");
            //打开这个窗口后给定之前选择的图片路径
            textbox1.Text = mode;
            //设置单选框默认值
            if (skin == "1")
            {
                this.radio1.IsChecked = true;
            }
            else if (skin == "2")
            {
                this.radio2.IsChecked = true;
            }
            else if (skin == "3")
            {
                this.radio3.IsChecked = true;
            }
            else if (skin == "4")
            {
                this.radio4.IsChecked = true;
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
            //判断选择的单选
            if (this.radio1.IsChecked == true)
            {
                //把确认的选择给配置文件
                MainWindow ma = new MainWindow();
                ma.Modify("skin", "1");
                //通知窗口已经关闭
                MainWindow.sw = true;
                //关闭当前窗口
                this.Close();
            }
            else if (this.radio2.IsChecked == true)
            {
                //把确认的选择给配置文件
                MainWindow ma = new MainWindow();
                ma.Modify("skin", "2");
                //通知窗口已经关闭
                MainWindow.sw = true;
                //关闭当前窗口
                this.Close();
            }
            else if (this.radio3.IsChecked == true)
            {
                //把确认的选择给配置文件
                MainWindow ma = new MainWindow();
                ma.Modify("skin", "3");
                //通知窗口已经关闭
                MainWindow.sw = true;
                //关闭当前窗口
                this.Close();
            }
            else if(this.radio4.IsChecked == true)
            {
                //把确认的选择给配置文件
                MainWindow ma = new MainWindow();
                ma.Modify("skin", "4");
                string image = ma.Obtain("image");
                if (image != null && textbox1.Text != "")
                {
                    //通知窗口已经关闭
                    MainWindow.sw = true;
                    //关闭当前窗口
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请选择一张图片!");
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "jpg flie|*.jpg|png file|*.png";   //选择可见的文件格式
            if (ofd.ShowDialog() == true)
            {
                //选定一张图片就存给配置文件
                MainWindow ma = new MainWindow();
                ma.Modify("image", ofd.FileName);
                //选择的图片路径输出在textbox1
                textbox1.Text = ofd.FileName;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            //通知窗口已经关闭(关闭事件)
            MainWindow.sw = true;
        }

        #region 只有第四个选项才允许选择图片
        private void radio1_Checked(object sender, RoutedEventArgs e)
        {
            //屏蔽文本框和按钮
            textbox1.IsEnabled = false;
            button.IsEnabled = false;
        }

        private void radio2_Checked(object sender, RoutedEventArgs e)
        {
            //屏蔽文本框和按钮
            textbox1.IsEnabled = false;
            button.IsEnabled = false;
        }

        private void radio3_Checked(object sender, RoutedEventArgs e)
        {
            //屏蔽文本框和按钮
            textbox1.IsEnabled = false;
            button.IsEnabled = false;
        }

        private void radio4_Checked(object sender, RoutedEventArgs e)
        {
            //解除屏蔽文本框和按钮
            textbox1.IsEnabled = false;
            button.IsEnabled = true;
        }
        #endregion

    }
}
