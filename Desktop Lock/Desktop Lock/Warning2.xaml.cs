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
    /// Warning2.xaml 的交互逻辑
    /// </summary>
    public partial class Warning2 : Window
    {
        public Warning2()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2 - 20;
            this.Left = (workWidth - this.Width) / 2 - 70;
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
            //获得配置文件closemain
            MainWindow ma = new MainWindow();
            //关闭主窗口
            ma.Modify("closemain", "true");
            //打开U盘锁屏窗口，关闭当前窗口
            LockScreen screen = new LockScreen();
            screen.Show();
            this.Close();
        }

        private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //通知窗口已经关闭
            MainWindow.sw = true;
            //关闭当前窗口
            this.Close();
        }
    }
}
