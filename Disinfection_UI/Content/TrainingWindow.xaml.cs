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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;

namespace Disinfection_UI.Content
{
    /// <summary>
    /// Interaction logic for TrainingWindow.xaml
    /// </summary>
    public partial class TrainingWindow : Window
    {

        public TrainingWindow()
        {
            InitializeComponent();
            Initialization();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            sta.Width = this.Width;
            sta.Height = this.Height;
            this.Cursor = Cursors.Arrow;
            this.PreviewMouseDown += new MouseButtonEventHandler(Window_MouseDown);
            this.PreviewMouseMove += new MouseEventHandler(Window_MouseMove);
            this.PreviewMouseUp += new MouseButtonEventHandler(Window_MouseUp);
            Thread td = new Thread(new ThreadStart(TimerStart));
            
        }

        [DllImport("SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetFinPressure();
        [DllImport("SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InitCOM(int com);
        [DllImport("SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void updatedata();
        #region parameter
        ResultData rd = new ResultData();
        DispatcherTimer Timer = new DispatcherTimer();
        DateTime StartTime, EndTime;
        double screeni = 1;
        int score = 100;
        List<Point> Linep;
        List<Point> LLine, RLine;
        List<double> pressurelist;
        List<DataSave> AllData = new List<DataSave>();
        Point startp;
        bool EnableDraw = false,DiviceReady = false,StartRun=false; int strokethick; bool setBase = true,LRstatus=false;
        double CurrentPres = 0, Basepres = 0; int LRCount = 0;
        #endregion
        enum ScreenMode
        {
            NScreen = 1,
            WScreen = 2,
            Unknow = 0
        }
        private ScreenMode GetScreenResolution(ref double i)//判断屏幕分辨率
        {
            double ScreenWidth = SystemParameters.PrimaryScreenWidth;
            double ScreenHight = SystemParameters.PrimaryScreenHeight;

            if (ScreenHight / 9 == ScreenWidth / 16)
            {
                i = ScreenWidth / 1920;
                return (ScreenMode.WScreen);
            }
            else if (ScreenHight / 3 == ScreenWidth / 4)
            {
                i = ScreenHight / 1200;
                return (ScreenMode.NScreen);
            }
            else
            {
                return (ScreenMode.Unknow);
            }
        }

        private void Initialization()
        {
            BitmapImage bti = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/Abdomen_1.jpg"));
            ImageBrush ib = new ImageBrush(bti);
            if (GetScreenResolution(ref screeni) == ScreenMode.NScreen)
            {
                backg.Width = 1000 * screeni; backg.Height = 1000 * screeni;
                backg.Background = ib;
                c1.Height = 534 * screeni; c1.Width = 400 * screeni;
                c1g.Height = 534 * screeni; c1g.Width = 400 * screeni;
                c1g.Margin = new Thickness(506 * screeni, 482 * screeni, 556 * screeni, 289 * screeni);
            }
            else if (GetScreenResolution(ref screeni) == ScreenMode.WScreen)
            {
                backg.Width = 1200 * screeni; backg.Height = 1100 * screeni;
                backg.Background = ib;
                c1.Width = 640 * screeni; c1.Height = 360 * screeni;
                c1g.Width = 640 * screeni; c1g.Height = 360 * screeni;
                c1g.Margin = new Thickness(607 * screeni, 436 * screeni, 667 * screeni, 255 * screeni);
            }
            else
            {
                backg.Width =800 ; backg.Height = 850;
                backg.Background = ib;
                c1.Height = 470; c1.Width = 470;
                c1g.Height = 470; c1g.Width = 470;
                c1g.Margin = new Thickness(0, 0, 0, 0);
            }
        }
        private void initdriver()
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("COM");
            XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
            strokethick = Convert.ToInt32(xn2.Attributes["st"].Value);
            int com = Convert.ToInt32(xn1.Attributes["comnum"].Value);
            if (InitCOM(com))
            {
                DiviceReady = true;
            }
            else
            {
                DiviceReady = false;
                MessageBox.Show("请检查COM口是否配置正确", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StartRun == false)
            {
                StartTime = DateTime.Now;
            }
            setBase = false;
            Errtb.Text = "";
            Linep = new List<Point>();
            pressurelist = new List<double>();
            if (DiviceReady == true)
            {
                EnableDraw = true;
                startp = e.GetPosition(this);
                pressurelist.Add(GetFinPressure());
                Linep.Add(startp);
            }            
        }
        private Point pointcontrol(Point p)
        {
            p.X = p.X / 3;
            p.Y = p.Y / 3;
            return (p);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            test.Text = e.GetPosition(this).X + "  " + e.GetPosition(this).Y;
            if (EnableDraw == true)
            {
                updatedata();
                Draw d = new Draw();
                double Pres = CurrentPres - Basepres;
                d.DrawLine(c1, strokethick, Colors.Black, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                startp = e.GetPosition(this);
                Linep.Add(startp);
                pressurelist.Add(Pres);
                tb1.Text = CurrentPres.ToString();
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BlankCheck bc=new BlankCheck();
           // MessageBox.Show(bc.bc(Environment.CurrentDirectory+@"\SavePic\1.jpg",c1).ToString());
            ActionJudge aj=new ActionJudge();
            DataSave ds=new DataSave();
            setBase = true;
            EnableDraw = false;
            if (Linep.Count > 5 && AllData.Count == 1)
            {
                if (Linep.Count > 5 && AllData[0].Getline()[0].X > Linep[0].X)
                {
                    RLine = AllData[0].Getline();
                    LLine = Linep;
                }
                else
                {
                    RLine = Linep;
                    LLine = AllData[0].Getline();
                }
                if (!aj.AreaJudge(Linep, AllData[0].Getline(), strokethick))
                {
                    rd.ResultDataUpdate(Error.DisinfectionArea);
                    Errtb.Text = Errtb.Text + "面积错误";
                }
            }         
            if (Linep.Count > 5 && AllData.Count>=2)
            {
                try
                {
                    if (!aj.TBSequenceJudge(Linep, AllData))
                    {
                        rd.ResultDataUpdate(Error.TopToBottom);
                        Errtb.Text = Errtb.Text + "上下错误";
                    }

                    if (!aj.LRSequenceJudge(Linep, AllData))
                    {
                        rd.ResultDataUpdate(Error.LeftToRight);
                        Errtb.Text = Errtb.Text + "左右错误";
                    }

                    if (Linep[0].X > AllData[0].Getline()[0].X)
                    {
                        if (!aj.AreaJudge(Linep, RLine, strokethick))
                        {
                            rd.ResultDataUpdate(Error.DisinfectionArea);
                            Errtb.Text = Errtb.Text + "面积错误";
                        }
                    }
                    else if (Linep[0].X < AllData[0].Getline()[0].X)
                    {
                        if (!aj.AreaJudge(Linep, LLine, strokethick))
                        {
                            rd.ResultDataUpdate(Error.DisinfectionArea);
                            Errtb.Text = Errtb.Text + "面积错误";
                        }
                    }
                }
                finally
                {
                    if (Linep.Count>5 && AllData[0].Getline()[0].X > Linep[0].X)
                    {
                        RLine = AllData[0].Getline();
                        LLine = Linep;
                    }
                    else
                    {
                        RLine = Linep;
                        LLine = AllData[0].Getline();
                    }
                }

            }
            if (Linep.Count > 5)
            {
                ds.Add(Linep);
                AllData.Add(ds);
            }
        }

        private void c1_Loaded(object sender, RoutedEventArgs e)
        {
            initdriver();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (AllData.Count > 1)
            {
                if (e.Key == Key.Escape)
                {
                    c1.Children.Clear();
                    AllData.Clear();
                    score = 100;
                    LLine.Clear();
                    RLine.Clear();
                    Linep.Clear();
                    Errtb.Text = "";
                    tb1.Text = "";
                    test.Text = "";
                    rd.ResultDataClear();
                }
            }
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        void TimerStart()
        {

            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0,500);
            Timer.Start();
        }
        void Timer_Tick(object sender,EventArgs e)
        {
            CurrentPres = GetFinPressure();
            if (setBase == true)
            {
                Basepres = CurrentPres;
            }
        }

        private void Finish_Button_Down(object sender, MouseButtonEventArgs e)
        {
            EndTime = DateTime.Now;
            StartRun = false;
        }

    }
}

