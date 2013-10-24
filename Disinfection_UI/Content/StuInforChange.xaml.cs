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

namespace Disinfection_UI.Content
{
    /// <summary>
    /// Interaction logic for StuInforChange.xaml
    /// </summary>
    public partial class StuInforChange : Window 
    {
        public StuInforChange()
        {
            InitializeComponent();
            okbt.Click += new RoutedEventHandler(okbt_click);
        }
        private void okbt_click(object sender, RoutedEventArgs e)
        {
            DatabaseControl dbc = new DatabaseControl();
            UserData ud = new UserData();
            ud.addidnum(IDbox1.Text);
            ud.addpassword(pwbox1.Text.Trim());
            ud.addusername(Namebox1.Text);
            ud.addclass(otherbox1.Text);
            dbc.ChangeInfor(ud);            
            MessageBox.Show("更改完成！");
            this.Close();
        }

    }
}
