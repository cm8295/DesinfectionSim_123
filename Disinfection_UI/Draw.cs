using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Path = System.Windows.Shapes.Path;
using System.Collections.Generic;
using System.Windows.Shapes;
namespace Disinfection_UI
{
    class Draw
    {
        public GeometryGroup CreatGeometryGroup(Canvas canvas1,int StrokeThick, Color col)
        {
            Path path = new Path();
            GeometryGroup gg = new GeometryGroup();
            path.Data = gg;
            path.StrokeThickness = StrokeThick;
            path.Stroke = new SolidColorBrush(col);
            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.StrokeLineJoin = PenLineJoin.Round;
            path.Opacity = 0.7;
            canvas1.Children.Add(path);
            return gg;
        }
        public void DrawCircle(Canvas canvas1, int StrokeThick, Color col, Point Leftp)
        {
            GeometryGroup gg = CreatGeometryGroup(canvas1,StrokeThick, col);
            gg.Children.Add(new EllipseGeometry(Leftp,5,5));
        }
        public void DrawLine(Canvas canvas1, int StrokeThick, Color col, Point startp, Point endp)
        {
            GeometryGroup gg = CreatGeometryGroup(canvas1,StrokeThick, col);
            gg.Children.Add(new LineGeometry(startp, endp));
        }
        public void DrawLine(Canvas canvas1, int StrokeThick, Color col, List<Point> lp)
        {
            GeometryGroup gg = CreatGeometryGroup(canvas1,StrokeThick, col);
            gg.Children.Add(new LineGeometry(lp[0], lp[1]));
        }
        public void DrawPathBZ(Canvas canvas1,int StrokeThick, Color col, Point[] pp)
        {
            BezierSegment bsg = new BezierSegment();
            bsg.Point1 = pp[1];
            bsg.Point2 = pp[2];
            bsg.Point3 = pp[3];
            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(bsg);
            PathFigure pathfg = new PathFigure(pp[0], psc, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pathfg);
            GeometryGroup gg = CreatGeometryGroup(canvas1,StrokeThick, col);
            gg.Children.Add(new PathGeometry(pfc));
        }
        public void DrawPathQBZ(Canvas canvas1,int StrokeThick, Color col, Point[] pp)
        {
            QuadraticBezierSegment qbs = new QuadraticBezierSegment();
            qbs.Point1 = pp[1];
            qbs.Point2 = pp[2];
            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(qbs);
            PathFigure pathfg = new PathFigure(pp[0], psc, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pathfg);
            GeometryGroup gg = CreatGeometryGroup(canvas1,StrokeThick, col);
            gg.Children.Add(new PathGeometry(pfc));
        }
        public void DrawPathPBS(Canvas canvas1,int StrokeThick, Color col, Point[] pp)
        {
            PolyBezierSegment pbs = new PolyBezierSegment();
            for (int i = 1; i < pp.Length; i++)
            {
                pbs.Points.Add(pp[i]);
            }
            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(pbs);
            PathFigure pathfg = new PathFigure(pp[0], psc, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pathfg);
            GeometryGroup gg = CreatGeometryGroup(canvas1,StrokeThick, col);
            gg.Children.Add(new PathGeometry(pfc));
        }
    }
}
