using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Disinfection_UI
{
    class ActionJudge
    {
        /*public JudgeResult SequenceJudge(List<DataSave> llp)//顺序判断
        {
            if ((llp[llp.Count - 1].Getline()[0].X - llp[0].Getline()[0].X) * (llp[llp.Count -2].Getline()[0].X - llp[0].Getline()[0].X) <= 0)//是否是左右交替
            {
                if ((llp[0].Getline()[1].Y - llp[0].Getline()[0].Y) * (llp[llp.Count - 1].Getline()[1].Y - llp[llp.Count - 1].Getline()[0].Y) >= 0)//是否是同一方向从上往下或从下往上
                {
                    return (JudgeResult.NoErr);
                }
                else return (JudgeResult.LeftRightErr);
            }
            else return (JudgeResult.UpDownErr);
        }
        public int SequenceJudge(List<DataSave> lds)
        {
            int result = 0;
            if (lds.Count > 2)//左右错误+1
            {
                if ((lds[lds.Count - 1].Getline()[0].X - lds[0].Getline()[0].X) * (lds[lds.Count - 2].Getline()[0].X - lds[0].Getline()[0].X) > 0)//是否是左右交替(+1)
                {
                    result = result + 1; ;
                }
            }
            if (lds.Count >= 2)//上下错误+10
            {
                if (((lds[0].Getline()[lds[0].Getline().Count - 1].Y - lds[0].Getline()[0].Y) * (lds[lds.Count - 1].Getline()[lds[lds.Count - 1].Getline().Count - 1].Y - lds[lds.Count - 1].Getline()[0].Y) <= 0))//是否是同一方向从上往下或从下往上(+10)
                {
                    result = result + 10;
                }
            }
            return result;
        }*/
        public bool LRSequenceJudge(List<Point> lp, List<DataSave> ld)//需要考虑之前如果有画错的问题？？
        {
            if ((lp[0].X - ld[0].Getline()[0].X) * (ld[ld.Count - 1].Getline()[0].X - ld[0].Getline()[0].X) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TBSequenceJudge(List<Point> lp, List<DataSave> ld)
        {
            if (((lp[lp.Count - 1].Y - lp[0].Y) * (ld[0].Getline()[ld[0].Getline().Count - 1].Y - ld[0].Getline()[0].Y)) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AreaJudge(List<Point> lp1, List<Point> lp2, int Strokethick)
        {
            // int a = (int)Math.Min(Math.Abs(lp1[0].Y - lp1[lp1.Count - 1].Y),Math.Abs(lp2[0].Y - lp1[lp2.Count - 1].Y));
            double a = 0;
            a = Math.Abs((lp1[0].X + lp1[lp1.Count - 1].X) / 6 - (lp2[0].X + lp2[lp2.Count - 1].X) / 6);
            double b = Strokethick * 0.7;
            if (b > a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}