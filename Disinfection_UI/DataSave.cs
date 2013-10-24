using System.Collections.Generic;
using System.Windows;

namespace Disinfection_UI
{
    enum Error : int
    {
        LeftToRight = 1,
        TopToBottom = 2,
        DisinfectionArea = 3,
        Pressure = 4
    }
    class DataSave
    {
        List<Point> SaveP = new List<Point>();
        List<double> Pressure = new List<double>();
        public void Add(List<Point> lp)
        {
            SaveP = lp;
        }
        public void Add(List<double> a)
        {
            Pressure = a;
        }
        public void Clear()
        {
            SaveP.Clear();
            Pressure.Clear();
        }
        public List<Point> Getline()
        {
            return SaveP;
        }
        public List<double> Getpressure()
        {
            return Pressure;
        }
    }
    class UserData
    {
        string idnum = null; bool idbl = false;
        string username = null; bool namebl = false;
        string password = null; bool pwbl = false;
        string userclass = null;        
        public void addidnum(string a)
        {
            idnum = a; idbl = true;
        }
        public void addusername(string b)
        {
            username = b; namebl = true;
        }
        public void addpassword(string c)
        {
            password = c; pwbl = true;
        }
        public void addclass(string d)
        {
            userclass = d;
        }
        public bool notnull()
        {
            return (idbl && namebl && pwbl);
        }
        public bool notnull_admin()
        {
            return (idbl && pwbl);
        }
        public void cleardata()
        {
            idbl = false; namebl = false; pwbl = false; userclass = null;
        }
        public string getidnum()
        {
            return idnum;
        }
        public string getusername()
        {
            return username;
        }
        public string getpassword()
        {
            return password;
        }
        public string getclass()
        {
            return userclass;
        }

    }
    class ResultData
    {
        int LeftToRight = 0;
        int TopToBottom = 0;
        int DisinfectionArea = 0;
        int Pressure = 0;
        public void ResultDataUpdate(Error e)
        {
            if (e == Error.LeftToRight)
            {
                LeftToRight++;
            }
            if (e == Error.TopToBottom)
            {
                TopToBottom++;
            }
            if (e == Error.DisinfectionArea)
            {
                DisinfectionArea++;
            }
            if (e == Error.Pressure)
            {
                Pressure++;
            }
        }
        public int[] ResultDataReturn()
        {
            int[] Res = new int[] { LeftToRight, TopToBottom, DisinfectionArea, Pressure };
            return Res;
        }
        public void ResultDataClear()
        {
            LeftToRight = 0;
            TopToBottom = 0;
            DisinfectionArea = 0;
            Pressure = 0;
        }           
    }
}