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

namespace Disinfection_UI.Pages
{
    /// <summary>
    /// Register_Admin.xaml 的交互逻辑
    /// </summary>
    public partial class Register_Admin : Window
    {
        public Register_Admin()
        {
            InitializeComponent();            
            btn_register.Click += new RoutedEventHandler(register_click);
            txt_num.LostFocus += new RoutedEventHandler(txt_num_LostFocus);
            txt_pass.LostFocus +=new RoutedEventHandler(txt_pass_LostFocus);
            txt_pass_Copy.LostFocus += new RoutedEventHandler(rpwbox_LostFocus);
        }
        DatabaseControl dbcon = new DatabaseControl();
        BitmapImage errbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\err.png"));
        BitmapImage rightbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\right.png"));
        UserData admindat = new UserData();
        bool bl = false;
        private void register_click(object sender, RoutedEventArgs e)
        {
            if (admindat.notnull())
            {
                dbcon.AddUser(admindat);
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
            else if (dbcon.CheckSameID(((TextBox)sender).Text, "IDnumber", "Administrator"))
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
                bl = false;
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
    }
    
}
