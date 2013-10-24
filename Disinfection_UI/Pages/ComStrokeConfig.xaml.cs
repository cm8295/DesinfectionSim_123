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
    /// Interaction logic for ComStrokeConfig.xaml
    /// </summary>
    public partial class ComStrokeConfig : UserControl
    {
        public ComStrokeConfig()
        {
            InitializeComponent();
            LoadXML();
        }
        int com; XmlDocument xd = new XmlDocument();
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            com = System.Convert.ToInt32(((ComboBoxItem)sender).Content.ToString().Substring(3));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("COM");
            XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
            xn1.Attributes["comnum"].Value = com.ToString();
            xn2.Attributes["st"].Value = tb.Text;
            xd.Save("config.xml");
            MessageBox.Show("修改成功");
        }
        void LoadXML()
        {
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("COM");
            XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
            tb.Text = xn2.Attributes["st"].Value;
            com = System.Convert.ToInt32(xn1.Attributes["comnum"].Value);
            COMcb.SelectedIndex = com - 1;
        }

    }
}
