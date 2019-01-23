using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    public struct Location
    {
        public Glyph glyph;
        public bool blocked;
        public bool blockSight;
    }

    public class Map
    {
        public Location this[int x, int y] { get { return LocationGrid[x, y]; } set { LocationGrid[x, y] = value; } }

        public int TryGetHeight(int x, int y)
        {
            if ( x >= 0 && y >= 0 )
            {
                if ( x < HeightMap.GetLength(0) && y < HeightMap.GetLength(1))
                {
                    return HeightMap[x, y];
                }
            }
            return -1;
           
        }

        public Location[,] LocationGrid;
        public int[,] HeightMap;

        public int Width { get;  }
        public int Height { get; }

        public Map (int width, int height)
        {
            Width = width;
            Height = height;
            LocationGrid = new Location[width, height];
            HeightMap = new int[width, height];
        }
    }

    

    public class WorldManager
    {
        Random rnd;

        public Dictionary<Point, Map> Maps;
        public int WorldWidth, WorldHeight;

        private int PartsInWorld = 10;
        public Point Camera { get; set; }


        // public Map overWorld;                  <--- To be added later

       

        public WorldManager(int width, int height)
        {
            rnd = new Random();
            this.WorldWidth = width;
            this.WorldHeight = height;

            Maps = new Dictionary<Point, Map>();
            Camera = new Point(0, 0);

            SetupWorld();

        }

        private void SetupWorld()
        {
           // overWorld = new Map(width, height); // To be used later! Create a overworld for local map gen.
           // CreateHightMap(overWorld);
        
           
            for (int x = 0; x <PartsInWorld; x++)
            {
                for (int y = 0; y < PartsInWorld; y++)
                {
                    Map m = new Map(WorldWidth, WorldHeight);
                    CreateHightMap(m);
                    SetupMap(m);
                    Maps.Add(new Point(x, y), m);
                }
            }
        }

        internal bool LocationIsBlocked(int x, int y)
        {
            return Maps[Camera].LocationGrid[x,y].blocked;
        }

        internal bool IsOnCurrentGrid(int x, int y)
        {
            if (x >= 0 && x < Maps[Camera].Width)
            {
                if (y >= 0 && y < Maps[Camera].Height)
                {
                    return true;
                }
            }
            return false;
        }

        /*
        private Map SetupMap(Map overWorld, int x, int y)
        {
            Map newMap = new Map(width, height);
            int x_part = ( overWorld.HeightMap.GetLength(0) / PartsInWorld );   
            int y_part = ( overWorld.HeightMap.GetLength(1) / PartsInWorld );

            Console.WriteLine("World parted into " + x_part.ToString() + " x parts and " + y_part.ToString() + " y parts.");

            Console.WriteLine("X and Y values : " + x.ToString() + ", " + y.ToString());
            Console.WriteLine("X.offset : " + (x_part * x).ToString());
            Console.WriteLine("Y.offset : " + (y_part * y).ToString());

            for (int x_step = x * x_part; x_step < x_part + x_part * x; x_step++)
            {
                for ( int y_step = y * y_part; y_step < y_part + y_part * y; y_step++)
                {
                   // Console.Write("(" + x_step.ToString() + "," + y_step.ToString() + ")"); // these values shall populate 
                                                                                            // entire newmap height
                   for ( int small_x = 0; small_x < x_part; small_x++ )
                    {
                        for ( int small_y = 0; small_y < y_part; small_y++)
                        {

                        }
                    }
                }
            }
            Console.Write("\n\n");

          
            return newMap;

        }
        */

        private void CreateHightMap(Map sourceMap)
        {
           
            // 1. Set all cells to random height
            for (int y = 0; y < sourceMap.HeightMap.GetLength(1); y++)
            {
                for (int x = 0; x < sourceMap.HeightMap.GetLength(0); x++)
                {
                    sourceMap.HeightMap[x, y] = rnd.Next(9);
                }
            }

            // 2. Smooth by neighbor values
            int smoothtimer = 0;
            while (smoothtimer < 3 )  // bit arbitrary 3 times smooth
            {
                smoothtimer++;
                for (int y = 0; y < sourceMap.HeightMap.GetLength(1); y++)
                {
                    for (int x = 0; x < sourceMap.HeightMap.GetLength(0); x++)
                    {
                        int over = GetNeighborValue(x, y - 1);
                        int under = GetNeighborValue(x, y + 1);
                        int left = GetNeighborValue(x - 1, y);
                        int right = GetNeighborValue(x + 1, y);
                        sourceMap.HeightMap[x, y] = (over + under + left + right) / 4;
                    }
                }
            }

            // PrintHeightMap(sourceMap);
            

            int GetNeighborValue(int x, int y)
            {
                int neighborHeight = sourceMap.TryGetHeight(x, y);
                if (neighborHeight == -1) { return rnd.Next(9); }
                else return neighborHeight;
               
            }

        }

        private static void PrintHeightMap(Map sourceMap)
        {
            for (int y = 0; y < sourceMap.HeightMap.GetLength(1); y++)
            {
                for (int x = 0; x < sourceMap.HeightMap.GetLength(0); x++)
                {
                    Console.Write(sourceMap.HeightMap[x, y].ToString());
                }
                Console.WriteLine("");
            }
        }

        private void SetupMap(Map sourceMap)
        {
            for (int x = 0; x < sourceMap.Width; x++)
            {
                for (int y = 0; y < sourceMap.Height; y++)
                {

                    sourceMap[x, y] = new Location() { glyph = Glyph.PERIOD, blocked = false, blockSight = false };
                    VAColorManager.RLColor.AliceBlue
                }
            }   
        }

        internal void Update()
        {
            
        }

        internal void Draw(VirtualConsole screen)
        {
            Map currentGrid = Maps[Camera];
            for (int x = 0; x < WorldWidth; x++)
            {
                for (int y = 0; y < WorldHeight; y++)
                { 
                    screen.PutGlyphBackGround(currentGrid[x, y].glyph, x, y, currentGrid.HeightMap[x,y]);
                }
            }
        }
    }
}