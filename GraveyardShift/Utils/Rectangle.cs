using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public struct Rectangle
    {
        public int x1, y1, x2, y2;

        public int Left    { get { return x1; } }
        public int Top     { get { return y1; } }
        public int Right   { get { return x2; } }
        public int Bottom  { get { return y2; } }

        public Rectangle(int x, int y, int w, int h)
        {
            x1 = x;
            y1 = y;
            x2 = x + w;
            y2 = y + h;
        }


        public Point Center()
        {
            int center_x = (x1 + x2) / 2;
            int center_y = (y1 + y2) / 2;
            return new Point(center_x, center_y);
        }

        public bool Intersects(Rectangle other)
        {
            return (x1 <= other.x2 && x2 >= other.x1 &&
                y1 <= other.y2 && y2 >= other.y1);
        }

        public Point[] Walls()
        {
            List<Point> points = new List<Point>();
            for (int x = x1 - 1; x < x2 + 1; x++)
            {
                points.Add(new Point(x, y1 - 1));
                points.Add(new Point(x, y2));
            }
            for (int y = y1 - 1; y < y2; y++)
            {
                points.Add(new Point(x1 - 1, y));
                points.Add(new Point(x2, y));
            }
            return points.ToArray();
        }
    }
}
