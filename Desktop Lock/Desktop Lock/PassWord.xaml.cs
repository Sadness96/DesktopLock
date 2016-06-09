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
using System.Security.Cryptography;//md5加密

namespace Desktop_Lock
{
    /// <summary>
    /// PassWord.xaml 的交互逻辑
    /// </summary>
    public partial class PassWord : Window
    {
        public PassWord()
        {
            InitializeComponent();
            //窗口居中
            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;
            this.Top = (workHeight - this.Height) / 2;
            this.Left = (workWidth - this.Width) / 2;
            //获取文本框焦点
            passwordbox1.Focus();
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
            //判断两次密码输入的值
            if (passwordbox1.Password == "")
            {
                //调用自定义弹窗
                Warning1 warn = new Warning1();
                Warning1.txt = "请输入密码!";
                if(warn.ShowDialog() == true)
                {
                    warn.Show();
                }
                //错误后清空输入框内容
                passwordbox1.Password = null;
                passwordbox2.Password = null;
            }
            else if (passwordbox2.Password == "")
            {
                //调用自定义弹窗
                Warning1 warn = new Warning1();
                Warning1.txt = "请确认密码!";
                if (warn.ShowDialog() == true)
                {
                    warn.Show();
                }
                //错误后清空输入框内容
                passwordbox1.Password = null;
                passwordbox2.Password = null;
            }
            else if (passwordbox1.Password != passwordbox2.Password)
            {
                //调用自定义弹窗
                Warning1 warn = new Warning1();
                Warning1.txt = "两次输入密码不一致";
                if (warn.ShowDialog() == true)
                {
                    warn.Show();
                }
                //错误后清空输入框内容
                passwordbox1.Password = null;
                passwordbox2.Password = null;
            }
            else
            {
                //把密码传给配置文件
                MainWindow main = new MainWindow();
                main.Modify("password", Pass());
                //打开密码锁屏窗口，关闭当前窗口
                LockScreenKey screen = new LockScreenKey();
                screen.Show();
                this.Close();
            }
        }

        private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //通知窗口已经关闭
            MainWindow.sw = true;
            //关闭当前窗口
            this.Close();
        }

        #region 监听回车
        private void passwordbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //调用按钮的事件处理代码
                img1_MouseDown(null, null);
            }
        }

        private void passwordbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //调用按钮的事件处理代码
                img1_MouseDown(null, null);
            }
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
            string HostName = passwordbox1.Password + "0461";
            //完整的文件格式
            string s = UserMd5(HostName);
            string key = s.Substring(0, s.Length - 2);
            return key;
        }
        #endregion

    }
}
