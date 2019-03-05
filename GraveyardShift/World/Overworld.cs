using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public class Overworld
    {
        int Seed;
        byte[,] Regions;
        Random rnd;

        public List<Point> rivers;
        public List<Point> settlements;

        public byte[,] Biome;

        public int[,] settlementScore = new int[50, 50];
        public Point[,] waterDirection = new Point[50, 50];
        public int[,] waterAmount = new int[50, 50];

        public byte this[int x, int y] { get { return Regions[x, y]; } }

        public Overworld(int Seed)
        {
            this.Seed = Seed;
            Regions = new byte[50, 50];
            rnd = new Random(Seed);

            rivers = new List<Point>();
            settlements = new List<Point>();

            Biome = new byte[50, 50];
        }

        public void Create()
        {
            CreateMap();

            // Clamp height values to range 0 - 100 :
            ClampHeight();

            AnalyzeBiomes();
            AnalyzeSettlementLocations();
        }

        private void ClampHeight()
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Regions[x, y] = (byte)MathHelper.Clamp(Regions[x, y], 0, 100);
                }
            }
        }

        private int GetHeightFromParabola(int value)
        {
            return (int)((-0.16f * (value * value) + 8 * value));
        }

        private void CreateMap()
        {
            for ( int x = 0; x < 50; x++ )
            {
                for ( int y = 0; y < 50; y++ )
                {
                    Regions[x, y] = 0;
                }
            }
            for (int x = 1; x < 49; x++)
            {
                 int x_height = GetHeightFromParabola(x);
                for (int y = 1; y < 49; y++)
                {
                    int y_height = GetHeightFromParabola(y);
                    Regions[x, y] = (byte)((x_height + y_height) / 2 );
                    
                }
            }


            
            for (int x = 1; x < 49; x++)
            {
                for (int y = 1; y < 49; y++)
                {
                    if (rnd.Next(100) > 90)
                        Regions[x, y] += 50; // (byte)(rnd.Next(50));
                }
            }

 
            Smooth(10);

            for (int iteration = 0; iteration < 10; iteration++)
            {
                Erosion(false);
                Smooth(1);
            }
           
            Erosion(true);
          
           
           
          
            
            
        }

        private void RaiseLand(int amount)
        {
            for (int x = 1; x < 49; x++)
            {
                for (int y = 1; y < 49; y++)
                {
                 
                    Regions[x, y] += (byte)amount;
                }
            }
        }

        private void Erosion(bool drawRivers)
        {
            //            Initialize water flow for each point on map. Initially set to 0,0
            //        ***************************************************************************
            
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    waterDirection[x, y] = new Point(0, 0);
                }
            }

            //             Calculate water flow for each point on map. Water flows to lowest neighbor
            //          **********************************************************************************
          //  bool finished = false;
          //  while (!finished)
          //  {
                CalculateFlowDirection(waterDirection);

            //    finished = RaiseHeightInDepressions(waterDirection);
            //}


            //              Initialize water amount array. All cells recieve 1 unit of water.
            //            *********************************************************************
            
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    waterAmount[x, y] = 1;
                }
            }


            //                       Create Dictionary containing all cells with same height
            //                   ***************************************************************

            Dictionary<int, List<Point>> CellsByHeight = new Dictionary<int, List<Point>>();
            for (int height = 0; height <= 100; height++)
            {
                CellsByHeight[height] = new List<Point>();

                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        if (Regions[x, y] == height) { CellsByHeight[height].Add(new Point(x, y)); }
                    }
                }
            }

            //          Calculate water amount from cell to neighbor based on flow map. From highest to lowest cell
            //       ************************************************************************************************
            for (int height = 100; height >= 0; height--)
            {
                List<Point> cellsAtCurrentHeight = CellsByHeight[height];
                foreach (Point p in cellsAtCurrentHeight)
                {
                    int Amount = waterAmount[p.X, p.Y];
                    Point flow = waterDirection[p.X, p.Y];
                    if (flow.X == 0 && flow.Y == 0) { continue; }
                    waterAmount[p.X + flow.X, p.Y + flow.Y] += Amount;
                }
            }


            //                        Finally, erode height in cells with large amount of water flow
            //                    *********************************************************************
            for (int x = 1; x < 49; x++)
            {
                for (int y = 1; y < 49; y++)
                {
                  
                        if (waterAmount[x, y] > 2)
                        {
                            byte erosionFactor = (byte)Math.Sqrt(waterAmount[x, y] * 2);
                            if (Regions[x, y] >= erosionFactor) 
                                Regions[x, y] -= erosionFactor;
                            else { Regions[x, y] = 0; }
                        }
                    
                  
                }
            }
            //PrintFlowToConsole(waterDirection);

            if (drawRivers) { CalculateRiver(); }

            void CalculateRiver()
            {
                for (int x = 1; x < 49; x++)
                {
                    for (int y = 1; y < 49; y++)
                    {
                        if (waterAmount[x, y] > 10 && Regions[x,y] > 0) { rivers.Add(new Point(x, y)); }  // 

                    }
                }
            }
        }


        internal void AnalyzeBiomes()
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (Regions[x, y] == 0) { Biome[x, y] = 0; continue; }        // Salt water/ ocean / sea
                    if ( DistanceToCoast(x, y) == 1 ) { Biome[x, y] = 1; continue; } // Shore
                    if ( waterDirection[x,y].X == 0 && waterDirection[x,y].Y == 0 ) { Biome[x,y] = 2; continue; } //Lake
                }
            }
        }
        internal void AnalyzeSettlementLocations()
        {

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (Regions[x, y] == 0) { continue; }
                    settlementScore[x, y] = -DistanceToCoast(x, y);
                    settlementScore[x, y] += waterAmount[x, y];
                }
            }
            
        }


        // Bit hackey. returns point with largest settlement score or point (-1, -1)
        public Point GetNextSettlement()
        {
            int hiscore = 0;
            Point p = new Point();
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (settlementScore[x, y] > hiscore)
                    {
                        hiscore = settlementScore[x, y];
                        p = new Point(x, y);
                    }
                }
            }

            settlements.Add(p);
            settlementScore[p.X, p.Y] = 0;            // score set to 0. Prevents same settlement selected

            return p;
        }

        // WORKS VERY GOOD. CAN BE USED TO CALCULATE HEIGHT INLAND ALSO
        private int DistanceToCoast(int Check_X, int Check_Y)
        {
            int distance = 100;
            for ( int x = 0; x < 50; x++ )
            {
                for ( int y = 0; y < 50; y++ )
                {
                    if ( Regions[x, y] == 0 )
                    {
                        int this_distance = (int) DistanceTo(Check_X, Check_Y, x, y);
                        if ( this_distance < distance ) { distance = this_distance; }
                    }
                }
            }
            return distance;
        }

        private double DistanceTo(int x1, int y1, int x2, int y2)
        {
            int dx = x1 - x2;
            int dy = y1 - y2;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        private void CalculateFlowDirection(Point[,] waterDirection)
        {
            for (int x = 1; x < 49; x++)
            {
                for (int y = 1; y < 49; y++)
                {
                    Point p = GetLowestNeighbor(x, y);

                    waterDirection[x, y] = p;
                }
            }
        }

        private bool RaiseHeightInDepressions(Point[,] waterDirection)
        {
            bool raised = false;
            for (int x = 1; x < 49; x++)
            {
                for (int y = 1; y < 49; y++)
                {
                    //if (waterDirection[x, y].X == 0 && waterDirection[x, y].Y == 0) { Regions[x, y] += 1; raised = true; }
                }
            }
            return raised;
        }

        private static void PrintFlowToConsole(Point[,] waterDirection)
        {
            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    if (waterDirection[x, y].X == 0 && waterDirection[x, y].Y == 0)
                    {
                        Console.Write("0");
                    }
                    else if (waterDirection[x, y].X == 1 && waterDirection[x, y].Y == 0)
                    {
                        Console.Write(">");
                    }
                    else if (waterDirection[x, y].X == -1 && waterDirection[x, y].Y == 0)
                    {
                        Console.Write("<");
                    }
                    else if (waterDirection[x, y].X == 0 && waterDirection[x, y].Y == 1)
                    {
                        Console.Write("V");
                    }
                    else if (waterDirection[x, y].X == 0 && waterDirection[x, y].Y == -1)
                    {
                        Console.Write("A");
                    }
                    else if (waterDirection[x, y].X == 1 && waterDirection[x, y].Y == 1)
                    {
                        Console.Write("\\");
                    }
                    else if (waterDirection[x, y].X == 1 && waterDirection[x, y].Y == -1)
                    {
                        Console.Write("/");
                    }
                    else if (waterDirection[x, y].X == -1 && waterDirection[x, y].Y == -1)
                    {
                        Console.Write("\\");
                    }
                    else if (waterDirection[x, y].X == -1 && waterDirection[x, y].Y == 1)
                    {
                        Console.Write("/");
                    }

                    else
                    {
                        Console.Write("X");
                    }
                }
                Console.WriteLine();
            }
        }

        private Point GetLowestNeighbor(int x, int y)
        {
            List<Point> targets = new List<Point>();

            int origin = GetNeighborValue(x, y);
            if (GetNeighborValue(x, y - 1) < origin ) { targets.Add(new Point(0, -1)); }
            if (GetNeighborValue(x, y+1 ) < origin) { targets.Add(new Point(0, 1)); }
            if (GetNeighborValue(x-1, y ) < origin) { targets.Add(new Point(-1, 0)); }
            if (GetNeighborValue(x+1, y ) < origin) { targets.Add(new Point(1, 0)); }

            if (GetNeighborValue(x-1, y - 1) < origin) { targets.Add(new Point(-1, - 1)); }
            if (GetNeighborValue(x+1, y + 1) < origin) { targets.Add(new Point(1, 1)); }
            if (GetNeighborValue(x - 1, y+1) < origin) { targets.Add(new Point(- 1, 1)); }
            if (GetNeighborValue(x + 1, y-1) < origin) { targets.Add(new Point(1, -1)); }

            if (targets.Count > 0)
            {
                return targets[rnd.Next(targets.Count)];
            }

            else return new Point(0, 0);

            /*
            int origin = GetNeighborValue(x, y);
            int N = GetNeighborValue(x, y - 1);
            int S = GetNeighborValue(x, y + 1);
            int E = GetNeighborValue(x - 1, y);
            int W = GetNeighborValue(x + 1, y);
            int NE = GetNeighborValue(x - 1, y - 1);
            int NW = GetNeighborValue(x + 1, y - 1);
            int SE = GetNeighborValue(x - 1, y + 1);
            int SW = GetNeighborValue(x + 1, y + 1);




            
            if (NE < N && NE < S && NE < E && NE < W && NE < NW && NE < SE && NE < SW) { return new Point(-1, -1); }
            if (SE < N && SE < E && SE < W && SE < NE && SE < NW && SE < S && SE < SW) { return new Point(-1, 1); }
            if (NW < N && NW < W && NW < S && NW < NE && NW < W && NW < SE && NW < SW) { return new Point(1, -1); }
            if (SW < N && SW < E && SW < S && SW < NE && SW < NW && SW < SE && SW < W) { return new Point(1, 1); }

            if ( N < S && N < E && N < W && N < NE && N < NW && N < SE && N < SW ) { return new Point(0, -1); }
            if ( S < N && S < E && S < W && S < NE && S < NW && S < SE && S < SW) { return new Point(0, 1); }
            if ( E < N && E < W && E < S && E < NE && E < NW && E < SE && E < SW) { return new Point(-1, 0); }
            if ( W < N && W < E && W < S && W < NE && S < NW && W < SE && W < SW) { return new Point(1, 0); }

           
            else { return new Point(0, 0); }
            */
        }

        private void CreateMountain()
        {
            int radius = rnd.Next(2, 8);
            int xh = rnd.Next(10, 40);
            int yh = rnd.Next(10, 40);


            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    int height = radius * radius - ((x - xh) * (x - xh)) - ((y - yh) * (y - yh));
                    if (height == 0)
                        Regions[x, y] += (byte)height;
                }
            }
        }


        public byte Max()
        {
            byte max = 0;
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (Regions[x, y] > max) { max = Regions[x, y]; }
                }
            }
            return max;
        }

        public byte Min()
        {
            byte min = 255;    // max byte value

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (Regions[x, y] < min) { min = Regions[x, y]; }
                }
            }
            return min;

        }

        private void Normalize()
        {
            byte max = Max();
            byte min = Min();

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    var norm = (Regions[x, y] - min) / (max - min);
                    Regions[x, y] = (byte)norm;
                }
            }
        }

        private void Smooth(int iterations)
        {
            int iteration = 0;
            while (iteration < iterations)
            {
                iteration++;
                for (int y = 1; y < 49; y++)
                {
                    for (int x = 1; x < 49; x++)
                    {
                        int over = GetNeighborValue(x, y - 1);
                        int under = GetNeighborValue(x, y + 1);
                        int left = GetNeighborValue(x - 1, y);
                        int right = GetNeighborValue(x + 1, y);
                        Regions[x, y] = (byte)((over + under + left + right) / 4);
                    }
                }
            }


        }

        internal int GetNeighborValue(int x, int y)
        {
            int neighborHeight = Regions[x, y];
            return neighborHeight;
        }

        internal int GetSeed(int x, int y)
        {
            return Seed * x * x + y;
        }
    }

    
    public class Region
    {
        public int Seed { get; set; }
        byte[,] Cells;
        public byte this[int x, int y] {  get { return Cells[x, y]; }  set { Cells[x, y] = value; } }
        public Region() { Cells = new byte[200, 200]; }
    }
}
