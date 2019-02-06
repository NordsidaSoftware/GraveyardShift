using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class Line
    {
        public Point a, b;

        public Line(Point a, Point b)
        {
            this.a = a;
            this.b = b;
        }

        public int Length()
        {
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            return (int)(Math.Sqrt(dx * dx + dy * dy));
        }

        public static float Lerp(int a, int b, float index)
        {
            return (a + (b - a) * index);
        }

        public static Point LerpPoint(Point a, Point b, float index)
        {
            return (new Point((int)Lerp(a.X, b.X, index), (int)Lerp(a.Y, b.Y, index)));
        }

        public static int Diagonal_distance(Point a, Point b)
        {
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            return (Math.Max(Math.Abs(dx), Math.Abs(dy)));
        }


    }
}
