using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class Circle
    {
        public int Radius { get; set; }
        public Point Origo { get; set; }

        public List<Point> cells
        {
            get
            {
                List<Point> cells_in_circle = new List<Point>();
                for (int x = -Radius; x <= Radius; x++)
                {
                    for (int y = -Radius; y <= Radius; y++)
                    {
                        if ((x * x) + (y * y) <= (Radius * Radius)) { cells_in_circle.Add(new Point(x+Origo.X, y+Origo.Y)); }
                    }
                }

                return cells_in_circle;
            }
        }

        public List<Point> circumference
        {
            get
            {
               
                List<Point> circumference = new List<Point>();
                int d = (5 - Radius * 4) / 4;
                int x = 0;
                int y = Radius;

                do
                {
                    circumference.Add(new Point(Origo.X + x, Origo.Y + y));
                    circumference.Add(new Point(Origo.X + x, Origo.Y - y));
                    circumference.Add(new Point(Origo.X - x, Origo.Y + y));
                    circumference.Add(new Point(Origo.X - x, Origo.Y - y));
                    circumference.Add(new Point(Origo.X + y, Origo.Y + x));
                    circumference.Add(new Point(Origo.X + y, Origo.Y - x));
                    circumference.Add(new Point(Origo.X - y, Origo.Y + x));
                    circumference.Add(new Point(Origo.X - y, Origo.Y - x));

                    if (d < 0)
                    {
                        d += 2 * x + 1;
                    }
                    else
                    {
                        d += 2 * (x - y) + 1;
                        y--;
                    }
                    x++;
                } while (x <= y);
              
                return circumference;
            }
        }

        public Circle(Point Origo, int Radius)
        {
            this.Origo = Origo;
            this.Radius = Radius;
        }
    }
}
