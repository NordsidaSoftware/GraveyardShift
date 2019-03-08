using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    [Serializable]
    public struct Point
    {
        public int X;
        public int Y;
        public Point(int X, int Y) { this.X = X; this.Y = Y; }

        public static Point operator +(Point A, Point B)
        {
            return new Point(A.X + B.X, A.Y + B.Y);
        }

        public static Point operator -(Point A, Point B)
        {
            return new Point(A.X - B.X, A.Y - B.Y);

        }

        private static Point[] NeighborPoints = new Point[4] { new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(0, -1) };

        internal IEnumerable<Point> Neighbors()
        {
            foreach (Point d in NeighborPoints)
            {
                Point next = new Point(X + d.X, Y + d.Y);
                yield return next;
            }
        }
    }
}
