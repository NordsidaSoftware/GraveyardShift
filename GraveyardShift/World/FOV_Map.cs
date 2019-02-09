//using Microsoft.Xna.Framework;

namespace GraveyardShift
{
    public class FOV_Map
    {
        public bool this[int x, int y] { get { return cells[x, y]; } }
        private Bool_Map visited;
        private bool[,] cells;

        public FOV_Map(Bool_Map visited) { cells = new bool[200, 200];  this.visited = visited; }

        internal void CalculateFOV(Features_Map map, Region heightMap, int originX, int originY)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = false;
                }
            }

            int range = 11;
            Point origin = new Point(originX, originY);

            if ( origin.X - range < 0 ) { origin.X =  range; }
            if ( origin.Y - range < 0 ) { origin.Y =  range; }
            if ( origin.X + range >= cells.GetLength(0)) { origin.X = cells.GetLength(0) - range-2; }
            if ( origin.Y + range >= cells.GetLength(1)) { origin.Y = cells.GetLength(1) - range-2; }

          //  int x_start = originX - range;
          //  int y_start = originY - range;
          //  int x_stop = originX + range;
          //  int y_stop = originY + range;

          //  if (x_start < 2) x_start = 2;
          //  if (y_start < 2) y_start = 2;
          //  if (x_stop >= cells.GetLength(0)) x_stop = cells.GetLength(0) - 2;
          //  if (y_stop >= cells.GetLength(1)) y_stop = cells.GetLength(1) - 2;

            // Point[] targets = new Rectangle(x_start, y_start, x_stop - x_start, y_stop - y_start).Walls();
            Point[] targets = new Circle(new Point(origin.X, origin.Y), range).cells.ToArray();        // TODO :need circumference without artefacts..

            bool visible;

            foreach (Point target in targets)
            {
                int N = Line.Diagonal_distance(origin, target);
                visible = true;

                int currentHeight = 0;
                int previousHeight = 0;
                int sight_blocked_from_point = range;

                for (int step = 0; step <= N; step++)
                {
                    if (N == 0) { continue; }
                    if ( step == 0 ) currentHeight = heightMap[originX, originY];

                    Point p = (Line.LerpPoint(origin, target, (float)step / N));

                   if (map.Block_Sight(p.X, p.Y))
                    {
                        sight_blocked_from_point = step;
                    }

                    previousHeight = currentHeight;
                    currentHeight = heightMap[p.X, p.Y];
                    if ( currentHeight > previousHeight ) { sight_blocked_from_point = step; }

                   if ( step > sight_blocked_from_point ) { visible = false; }
                   
                   cells[p.X, p.Y] = visible;
                    

                   if (visible)
                        visited[p.X, p.Y] = true; ;
                }
            }
        }

    }
        
}