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
    /// Warning1.xaml 的交互逻辑
    /// </summary>
    public partial class Warning1 : Window
    {
        public Warning1()
        {
            InitializeComponent();
            //窗口置顶
            this.Topmost = true;
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2 - 20;
            this.Left = (workWidth - this.Width) / 2 - 70;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //默认label2值为txt
            label2.Content = txt;
            //获得焦点
            img1.Focus();
        }
        //定义全局变量
        public static string txt { get; set; }  //
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
    }
}
