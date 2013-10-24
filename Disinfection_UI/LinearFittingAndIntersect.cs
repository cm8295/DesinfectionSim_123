using System;
using System.Collections.Generic;
using System.Windows;

namespace Disinfection_UI
{
    class Fitting
    {
        
        #region LeastSquares
        public List<Point> LinearFitting(int startnum, int endnum, List<Point> list)
        {
            List<Point> Plist = new List<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            double lxy = 0, lxx = 0, _x, _y, sumx = 0, sumy = 0, a, b, xmax = 0, xmin = list[startnum].X, ymax = 0, ymin = list[startnum].Y;
            for (int n = startnum; n <= endnum; n++)
            {
                xmax = Math.Max(xmax, list[n].X);
                xmin = Math.Min(xmin, list[n].X);
                ymax = Math.Max(ymax, list[n].Y);
                ymin = Math.Min(ymin, list[n].Y);
                sumx += list[n].X;
                sumy += list[n].Y;
            }
            if (xmax == xmin | ymax == ymin)
            {
                Plist.Add(list[startnum]);
                Plist.Add(list[endnum]);
            }
            else
            {
                _x = sumx / (endnum - startnum + 1);
                _y = -sumy / (endnum - startnum + 1);
                for (int m = startnum; m <= endnum; m++)
                {
                    lxy += (-(_x * _y) - list[m].X * list[m].Y);
                    lxx += (list[m].X * list[m].X - _x * _x);
                }
                b = lxy / lxx;
                a = _y - b * _x;
                p1.Y = list[endnum].Y;
                p1.X = (-list[endnum].Y - a) / b;
                p2.Y = list[startnum].Y;
                p2.X = (-list[startnum].Y - a) / b;
                if (Math.Abs(p1.X - p2.X) > 3 * (ymax - ymin))
                {
                    p1.X = list[endnum].X;
                    p1.Y = -(a + b * list[endnum].X);
                    p2.X = list[startnum].X;
                    p2.Y = -(a + b * list[startnum].X);
                }
                Plist.Add(p1);
                Plist.Add(p2);
            }
            return (Plist);
        }
        #endregion
        #region Intersect
        // List<List<Point>> Linepoint = new List<List<Point>>();
        Point PointOfIntersection(List<Point> lp1, List<Point> lp2)
        {
            double a = lp1[0].X, b = -lp1[0].Y, c = lp1[1].X, d = -lp1[1].Y;
            double a1 = lp2[0].X, b1 = -lp2[0].Y, c1 = lp2[1].X, d1 = -lp2[1].Y;
            double x, y, k, k1;
            Point p = new Point();
            k = (d - b) / (c - a);
            k1 = (d1 - b1) / (c1 - a1);
            x = (b1 - k1 * a1 + a * k - b) / (k - k1);
            y = k * (x - a) + b;
            p.X = x;
            p.Y = -y;
            return (p);
        }
        bool IntersectJudge(List<Point> pl/**画好的线条*/, List<DataSave> ll/**所有已有直线的集合*/, out List<Point> IntersectPoint)
        {
            List<Point> Line = new List<Point>();
            bool boolval = true;
            Line = LinearFitting(0, pl.Count - 1, pl);
            if (ll.Count == 0)
            {
                IntersectPoint = new List<Point>();
            }
            else
            {
                IntersectPoint = new List<Point>();
                for (int i = 0; i < ll.Count; i++)
                {
                    List<Point> templist = LinearFitting(0, ll[i].Getline().Count-1, ll[i].Getline());
                    double a = Line[0].X, b = -Line[0].Y, c = Line[1].X, d = -Line[1].Y, m = templist[0].X, n = -templist[0].Y, p = templist[1].X, q = -templist[1].Y;
                    bool ma, mb;
                    ma = (m - a) * (n - d) * (p - a) * (q - d) + (m - c) * (n - b) * (p - c) * (q - d) < (m - a) * (n - d) * (p - c) * (q - b) + (m - c) * (n - b) * (p - a) * (q - d);
                    mb = (a - m) * (b - q) * (c - m) * (d - q) + (a - p) * (b - n) * (c - p) * (d - n) < (a - m) * (b - q) * (c - p) * (d - n) + (a - p) * (b - n) * (c - m) * (d - q);

                    if (ma&mb)
                     {
                        boolval = false;
                        IntersectPoint.Add(PointOfIntersection(Line,  templist));
                    }
                }
            }
            return (boolval);

        }

        public bool IntersectJudge(List<Point> pl/**画好的线条*/, List<DataSave> ll/**除去刚画线条已有直线的集合*/)
        {
            List<Point> Line = new List<Point>();
            bool boolval = true;
            Line = LinearFitting(0, pl.Count - 1, pl);
            if (ll.Count == 0)
            {

            }
            else
            {
                
                for (int i = 0; i < ll.Count; i++)
                {
                    List<Point> templist = LinearFitting(0, ll[i].Getline().Count-1, ll[i].Getline());
                    double a = Line[0].X, b = -Line[0].Y, c = Line[1].X, d = -Line[1].Y, m = templist[0].X, n = -templist[0].Y, p = templist[1].X, q = -templist[1].Y;
                    bool ma, mb;
                    ma = (m - a) * (n - d) * (p - a) * (q - d) + (m - c) * (n - b) * (p - c) * (q - d) < (m - a) * (n - d) * (p - c) * (q - b) + (m - c) * (n - b) * (p - a) * (q - d);
                    mb = (a - m) * (b - q) * (c - m) * (d - q) + (a - p) * (b - n) * (c - p) * (d - n) < (a - m) * (b - q) * (c - p) * (d - n) + (a - p) * (b - n) * (c - m) * (d - q);

                    if (ma & mb)
                    {
                        boolval = false;
                    }
                }
            }
            return (boolval);

        }

        public bool IntersectJudge(List<DataSave> ll/**所有直线的集合*/)
        {
            List<Point> Line = new List<Point>();
            bool boolval = false;
            List<Point>pl=ll[ll.Count-1].Getline();
            Line = LinearFitting(0, pl.Count - 1, pl);
            if (ll.Count == 0)
            {

            }
            else
            {
                
                for (int i = 0; i < ll.Count-1; i++)
                {
                    List<Point> templist = LinearFitting(0, ll[i].Getline().Count-1, ll[i].Getline());
                    double a = Line[0].X, b = -Line[0].Y, c = Line[1].X, d = -Line[1].Y, m = templist[0].X, n = -templist[0].Y, p = templist[1].X, q = -templist[1].Y;
                    bool ma, mb;
                    ma = (m - a) * (n - d) * (p - a) * (q - d) + (m - c) * (n - b) * (p - c) * (q - d) < (m - a) * (n - d) * (p - c) * (q - b) + (m - c) * (n - b) * (p - a) * (q - d);
                    mb = (a - m) * (b - q) * (c - m) * (d - q) + (a - p) * (b - n) * (c - p) * (d - n) < (a - m) * (b - q) * (c - p) * (d - n) + (a - p) * (b - n) * (c - m) * (d - q);

                    if (ma & mb)
                    {
                        boolval = true;
                    }
                }
            }
            return (boolval);

        }
        #endregion
        public double GetAngle(List<List<Point>> llp)
        {
            double a = llp[0][0].X - llp[0][1].X, b = llp[0][0].Y - llp[0][1].Y, c = llp[1][1].X - llp[1][0].X, d = llp[1][1].Y - llp[1][0].Y;
            double ans;
            ans = Math.Acos((a * c + b * d) / (Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * Math.Sqrt(Math.Pow(c, 2) + Math.Pow(d, 2))));
            ans = ans * 180 / Math.PI;
            return (ans);

        }
        public List<List<Point>> middot(List<Point> lp, int startnum, int endnum)
        {
            List<List<Point>> MidDot = new List<List<Point>>();
            List<Point> Line1 = new List<Point>();
            List<Point> Line2 = new List<Point>();
            Line1.Add(lp[startnum]);
            Line1.Add(lp[(endnum - startnum) / 2 + startnum]);
            Line2.Add(lp[(endnum - startnum) / 2 + startnum]);
            Line2.Add(lp[endnum]);
            MidDot.Add(Line1);
            MidDot.Add(Line2);
            return MidDot;
        }
        public bool JudgeLine(List<Point> lp)
        {
            if (GetAngle(middot(lp, 0, lp.Count - 1)) <= 170)
            {
                return false;
            }
            else
            {
                if (GetAngle(middot(lp, 0, (int)((lp.Count - 1) / 2))) <= 160 | GetAngle(middot(lp, (lp.Count - 1) / 4, 3 * (lp.Count - 1) / 4)) <= 160 | GetAngle(middot(lp, (lp.Count - 1) / 2, lp.Count - 1)) <= 160)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        
    }

}

