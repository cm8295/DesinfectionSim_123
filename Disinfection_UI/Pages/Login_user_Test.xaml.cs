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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;

namespace Disinfection_UI.Pages
{
    /// <summary>
    /// Interaction logic for Login_user_Test.xaml
    /// </summary>
    public partial class Login_user_Test : UserControl
    {
        public Login_user_Test()
        {
            InitializeComponent();
            loginbtm.Click += new RoutedEventHandler(Login_down);
        }
        private void Login_down(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + @"/Userinformation.mdf") && File.Exists(Environment.CurrentDirectory + @"/Userinformation_log.ldf"))
            {
                if (File.Exists(Environment.CurrentDirectory + @"/SensorDataGet.dll"))
                {
                    DatabaseControl datc = new DatabaseControl();
                    if (datc.Login(uidbox.Text, pwbox.Password, "student") == "Success")
                    {

                    }
                    else
                    {
                        MessageBox.Show("密码错误!");
                        pwbox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("找不到SensorDataGet.dll");
                }
            }
            else
            {
                MessageBox.Show("缺少数据库文件！");
            }
        }

        private void outbtm_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void loginbtm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Allright");
        }
        private void Hyperlink1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.casit.com.cn");
        }
    }
}
