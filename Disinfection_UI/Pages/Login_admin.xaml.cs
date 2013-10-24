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
using System.Xml;
using System.IO;
using System.Data;
using System.Windows.Threading;

namespace Disinfection_UI.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class Login_admin : UserControl
    {
        public Login_admin()
        {
            InitializeComponent();
            LaunchTimer();
            sv.Visibility = Visibility.Collapsed;
            loginbtm.Click += new RoutedEventHandler(admin_click);
            Exitbt.Click += new RoutedEventHandler(admin_exit);

            btn_register.Click += new RoutedEventHandler(register_click);
            txt_num.LostFocus += new RoutedEventHandler(txt_num_LostFocus);
            txt_pass.LostFocus += new RoutedEventHandler(txt_pass_LostFocus);
            txt_pass_Copy.LostFocus += new RoutedEventHandler(rpwbox_LostFocus);
            
        }
        private void admin_click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + @"/Userinformation.mdf") && File.Exists(Environment.CurrentDirectory + @"/Userinformation_log.ldf"))
            {
                if (File.Exists(Environment.CurrentDirectory + @"/SensorDataGet.dll"))
                {
                    DatabaseControl datc = new DatabaseControl();
                    if (datc.Login(uidbox.Text, pwbox.Password, "Administrator") == "Success")
                    {
                        XmlDocument xd = new XmlDocument();
                        xd.Load("config.xml");
                        XmlNode xn1 = xd.SelectSingleNode("SysConfig");
                        XmlNode xn2 = xn1.SelectSingleNode("Admin");                        
                        xn2.Attributes["name"].Value = uidbox.Text;
                        xd.Save("config.xml");
                        svlogin.Visibility = Visibility.Hidden;
                        sv.Visibility = Visibility.Visible;
                        CheckStudent();
                    }
                    else
                    {                        
                        MessageBox.Show("登录名/密码错误!");
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
        private void admin_exit(object sender, RoutedEventArgs e)
        {
            svlogin.Visibility = Visibility.Visible;
            sv.Visibility = Visibility.Collapsed;
            uidbox.Clear();
            pwbox.Clear();
        }
        DatabaseControl dbc = new DatabaseControl();
        DataSet ds = new DataSet();
        private string[] GetSelectId()
        {
            try
            {
                DataRowView[] drv = new DataRowView[StudentDataG.SelectedItems.Count];
                for (int j = 0; j < StudentDataG.SelectedItems.Count; j++)
                {
                    drv[j] = StudentDataG.SelectedItems[j] as DataRowView;
                }
                string[] selectid = new string[drv.Count()];

                for (int i = 0; i < drv.Count(); i++)
                {
                    selectid[i] = drv[i].Row[0].ToString();

                }
                return selectid;
            }
            catch
            {
                return null;
            }
        }
        private string[] GetSelectAdminId()
        {
            try
            {
                DataRowView[] drv = new DataRowView[AdimistratorDG.SelectedItems.Count];
                for (int j = 0; j < AdimistratorDG.SelectedItems.Count; j++)
                {                    
                    drv[j] = AdimistratorDG.SelectedItems[j] as DataRowView;
                }
                string[] selectid = new string[drv.Count()];

                for (int i = 0; i < drv.Count(); i++)
                {
                    selectid[i] = drv[i].Row[0].ToString();

                }
                return selectid;
            }
            catch
            {
                return null;
            }
        }

        private void CheckStudent()
        {
            ds = dbc.GetAllStudentInfor();
            StudentDataG.DataContext = ds;
        }
        private void CheckSearch()
        {
            ds = dbc.Search_stuInfor(this.txt_search.Text);            
            StudentDataG.DataContext = ds;            
        }
        private void CheckAdmin()
        {
            ds = dbc.GetAllAdminInfor();
            AdimistratorDG.DataContext = ds;
        }
        private void Datagrenew(Object sender, EventArgs e)
        {
            ds.Clear();
            ds = dbc.GetAllStudentInfor();
            StudentDataG.DataContext = ds;
        }
        private void changestuinfor_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectId() != null && GetSelectId().Count() > 0)
            {
                if (GetSelectId().Count() <= 1)
                {
                    UserData ud = new UserData();
                    ud = dbc.GetStuInfor("idnumber", GetSelectId()[0]);                    
                    Content.StuInforChange sicwin = new Content.StuInforChange();
                    sicwin.IDbox1.Text = ud.getidnum();
                    sicwin.Namebox1.Text = ud.getusername();
                    sicwin.otherbox1.Text = ud.getclass();
                    sicwin.pwbox1.Text = ud.getpassword();
                    sicwin.Show();
                    sicwin.Closed += new EventHandler(Datagrenew);
                }
                else
                {
                    MessageBox.Show("请只选择一个对象进行编辑!");
                }
            }
            else
            {
                MessageBox.Show("请先选择要操作的学员！");
            }
        }
        private void Pass_Change_Click(object sender, RoutedEventArgs e)   //修改管理员密码
        {
            if (GetSelectAdminId() != null && GetSelectAdminId().Count() > 0)
            {
                if (GetSelectAdminId().Count() <= 1)
                {
                    UserData ud = new UserData();
                    ud = dbc.GetAdminInfor("idnumber", GetSelectAdminId()[0]);
                    Content.AdminInforChange sicwin = new Content.AdminInforChange();
                    sicwin.IDbox1.Text = ud.getidnum();
                    sicwin.pwbox1.Text = ud.getpassword();
                    sicwin.Show();
                    sicwin.Closed += new EventHandler(Datagrenew);
                }
                else
                {
                    MessageBox.Show("请只选择一个对象进行编辑！");
                }
            }
            else
            {
                MessageBox.Show("请选择要操作的管理员！");
            }
        }
    
        private void delstudentinfor_Click_1(object sender, RoutedEventArgs e)   //删除用户
        {
            DeleteUser();
        }
        private void DeleteUser()    //删除用户函数
        {

            if (GetSelectId() != null && GetSelectId().Count() > 0)
            {
                if (MessageBox.Show( "确定进行删除，删除后将无法恢复？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < GetSelectId().Count(); i++)
                    {
                        dbc.DelUser(GetSelectId()[i]);
                    }
                    ds.Clear();
                    ds = dbc.GetAllStudentInfor();
                    StudentDataG.DataContext = ds;
                }

            }
        }
        private void DeleteAdmin()    //删除管理员函数
        {
            for (int i = 0; i < GetSelectAdminId().Count(); i++)
            {
                if (GetSelectAdminId()[i] == admin_user_Copy.Text)
                {
                    MessageBox.Show("对不起，不能删除管理员自己，请重新操作！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (GetSelectAdminId() != null && GetSelectAdminId().Count() > 0)
            {
                if (MessageBox.Show("确定进行删除，删除后将无法恢复？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < GetSelectAdminId().Count(); i++)
                    {
                        dbc.DelAdmin(GetSelectAdminId()[i]);
                    }
                    ds.Clear();
                    ds = dbc.GetAllAdminInfor();
                    AdimistratorDG.DataContext = ds;
                }

            }
            else
            {
                MessageBox.Show("删除不成功！");
            }
        }

        private void Export_click(object sender, RoutedEventArgs e)   //导出
        {
            try
            {
                ExcelControl xlcon = new ExcelControl();
                if (xlcon.ExportCanExcute(ds))
                {
                    xlcon.ExportExcute(ds.Tables[0]);
                }
            }
            catch
            {
                MessageBox.Show("无法导出空的表格！");
            }
        }  //导出表格

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    XmlDocument xd = new XmlDocument();
        //    xd.Load("config.xml");
        //    XmlNode xn1 = xd.SelectSingleNode("SysConfig");
        //    XmlNode xn2 = xn1.SelectSingleNode("Admin");
        //    string name = xn2.Attributes["name"].Value;
        //    DatabaseControl dbc = new DatabaseControl();
        //    if (pbx.Password == dbc.GetDbInfor("idnumber", name, "password", "Administrator"))
        //    {
        //        pbx.Clear();
        //        admingrid.Visibility = Visibility.Visible;
        //        passgrid.Visibility = Visibility.Hidden;
        //        
        //    }
        //    else
        //    {
        //        pbx.Clear();
        //        MessageBox.Show("密码错误！");
        //    }
        //}
        //private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{            
        //    passgrid.Visibility = Visibility.Visible;
        //    admingrid.Visibility = Visibility.Hidden;
        //}

        private void addadmin_Click(object sender, RoutedEventArgs e)   // 添加管理员
        {
            //admintb.Visibility = Visibility.Hidden;
            Add_Admin.Visibility = Visibility.Visible;
        }

        private void deladmin_Click(object sender, RoutedEventArgs e)    //删除管理员
        {
            DeleteAdmin();
        }

        private void endbtm_Click(object sender, RoutedEventArgs e)  //退出应用程序
        {
            Application.Current.Shutdown();
        }

        private void loginbtm_Click(object sender, RoutedEventArgs e)
        {
           // loginbtm.Click += new RoutedEventHandler(admin_click);
            admin_user.Text = uidbox.Text;
            admin_user_Copy.Text = uidbox.Text;
        }


        private void showadmin_Click(object sender, RoutedEventArgs e)
        {
            CheckAdmin();
        }

        ///
        ///增加管理员
        ///     
        DatabaseControl dbcon = new DatabaseControl();
        BitmapImage errbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\err.png"));
        BitmapImage rightbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\right.png"));
        UserData admindat = new UserData();
        bool bl = false;
        private void register_click(object sender, RoutedEventArgs e)
        {
            if (admindat.notnull_admin())
            {
                dbcon.AddAdmin(admindat);
                admindat.cleardata();
            }
        }
        private void txt_num_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();
            if (((TextBox)sender).Text.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                iderr.Background = imbrash;
            }
            else if (dbcon.CheckSameID_Admin(((TextBox)sender).Text, "IDnumber", "Administrator"))
            {
                imbrash.ImageSource = errbitmap;
                iderr.Background = imbrash;
            }
            else
            {
                imbrash.ImageSource = rightbitmap;
                iderr.Background = imbrash;
                admindat.addidnum(txt_num.Text.Trim());
            }
        }
        private void txt_pass_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();
            if (((PasswordBox)sender).Password.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                pwerr.Background = imbrash;
                bl = false;
            }
            else
            {
                imbrash.ImageSource = rightbitmap;
                pwerr.Background = imbrash;
                bl = true;
            }
        }
        private void rpwbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();

            if (((PasswordBox)sender).Password.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                repwerr.Background = imbrash;
            }
            else if (((PasswordBox)sender).Password != txt_pass.Password)
            {
                imbrash.ImageSource = errbitmap;
                repwerr.Background = imbrash;
            }
            else
            {
                imbrash.ImageSource = rightbitmap;
                repwerr.Background = imbrash;
                if (bl)
                {
                    admindat.addpassword(txt_pass_Copy.Password.Trim());
                }
            }
        }

        private void return_admin_Click(object sender, RoutedEventArgs e)
        {
            Add_Admin.Visibility = Visibility.Hidden;
        }
        private void Hyperlink2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.casit.com.cn");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)   //单个查询学生信息
        {
            CheckSearch();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            this.timer0.Text = DateTime.Now.ToLongTimeString();
            this.timer1.Text = DateTime.Now.ToLongTimeString();
        }
        private void LaunchTimer()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);  //
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }
        private void Show_stuInfo_Click(object sender, RoutedEventArgs e)  //显示学员信息
        {
            ds = dbc.GetAllStudentInfor();            
            StudentDataG.DataContext = ds;
        }
    }
}
