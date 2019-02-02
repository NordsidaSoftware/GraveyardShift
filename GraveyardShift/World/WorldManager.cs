//using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    [Serializable]
    public struct Point
    {
        public int X;
        public int Y;
        public Point (int X, int Y) { this.X = X; this.Y = Y; }

        public static Point operator +(Point A, Point B)
        {
            return new Point(A.X + B.X, A.Y + B.Y );
        }

        public static Point operator -(Point A, Point B)
        {
            return new Point(A.X - B.X, A.Y - B.Y);
        }
    }

    [Serializable]
    public struct Tile
    {
        public Glyph glyph;
        public VAColor fgColor;
        public VAColor bgColor;
        public bool blocked;
        public bool blockSight;
    }

    [Serializable]
    public class Map
    {
        // reimplement this class as an array ? Mod to get line/row etc

        public Tile this[int x, int y] { get { return tiles[x, y]; } set { tiles[x, y] = value; } }
        public Tile[,] tiles;
       

        public Map() { tiles = new Tile[200, 200]; }
    }

    
    [Serializable]
    public class WorldManager
    {
        Random rnd;

       
        public int ScreenWidth, ScreenHeight;
       

       


        // ****************************************************************
        //|      NEW WORLD STRUCTURE                                      |
        //*****************************************************************

        public Overworld overWorld;
        public Point Camera { get; set; }
        public Point RegionCoordinate { get; set; }
        public Region CurrentRegionHeightmap { get; set; }
        public Map RegionMap;
        public int MapWidth, MapHeight;

       

        public WorldManager(int width, int height, int width_parts, int height_parts, int screenWidth, int screenHeight)
        {
            rnd = new Random();
            this.MapWidth = width;
            this.MapHeight = height;
           
            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;

          

          

            //**************************************
            //|         NEW SUFF                   |
            //**************************************
            int seed = 050776;                      // Seed to generate all.Hardcoded for now...
            overWorld = new Overworld(seed);        // Master overworld 50 x 50 bytes
            overWorld.Create(75);                     // Creates overworld. Iterations is number of hills !
            Camera = new Point(0, 0);               // Viewport origin. Size like screen
            RegionCoordinate = new Point(20, 20);        // Current Region 
            GenerateCurrentRegionHeightmap();
            GenerateLocalHeight(5);
            GenerateBackgroundColorFromRegionHeightmap();
            //GenerateContourlinesOnMap();
        }

        private void GenerateCurrentRegionHeightmap()
        {
            CurrentRegionHeightmap = new Region();

            byte hNW = overWorld[RegionCoordinate.X - 1, RegionCoordinate.Y - 1];
            byte hN = overWorld[RegionCoordinate.X, RegionCoordinate.Y - 1];
            byte hNE = overWorld[RegionCoordinate.X + 1, RegionCoordinate.Y - 1];
            byte hE = overWorld[RegionCoordinate.X + 1, RegionCoordinate.Y];
            byte hW = overWorld[RegionCoordinate.X - 1, RegionCoordinate.Y];
            byte hSE = overWorld[RegionCoordinate.X + 1, RegionCoordinate.Y + 1];
            byte hS = overWorld[RegionCoordinate.X, RegionCoordinate.Y + 1];
            byte hSW = overWorld[RegionCoordinate.X - 1, RegionCoordinate.Y + 1];
            byte h = overWorld[RegionCoordinate.X, RegionCoordinate.Y];

            // 1. NW QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(hNW, hN, x);
                    byte hBottom = (byte)InterpolateLine(hW, h, x);

                    byte cellHeight = (byte)InterpolateLine(hTop, hBottom, y);
                    CurrentRegionHeightmap[x, y] = cellHeight;
                }
            }

            // 2. NE QUADRANT
            
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(hN, hNE, x);
                    byte hBottom = (byte)InterpolateLine(h, hE, x);

                    byte cellHeight = (byte)InterpolateLine(hTop, hBottom, y);
                    CurrentRegionHeightmap[x+100 , y] = cellHeight;
                }
            }

            // 3. SE QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(h, hE, x);
                    byte hBottom = (byte)InterpolateLine(hS, hSE, x);

                    byte cellHeight = (byte)InterpolateLine(hTop, hBottom, y);
                    CurrentRegionHeightmap[x+100, y+100] = cellHeight;
                }
            }

            // 4. SW QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(hW, h, x);
                    byte hBottom = (byte)InterpolateLine(hSW, hS, x);

                    byte cellHeight = (byte)InterpolateLine(hTop, hBottom, y);
                    CurrentRegionHeightmap[x, y+100] = cellHeight;
                }
            }
        }

        // LERP method - To be moved to a utility class later ?
        private float InterpolateLine(byte valueA, byte valueB, int x)
        {
            return (valueA + (valueB - valueA) * (0.5f + x / 100.0f));
        }



        internal bool IsInsideCenterArea(int x_pos, int y_pos)
        {
            if ( x_pos > Camera.X + 25 && x_pos < (Camera.X + ScreenWidth)-25)
            {
                if ( y_pos > Camera.Y + 15 && y_pos < (Camera.Y + ScreenHeight)-15)
                {
                    return true;
                }
            }
            return false;
        }

        internal void EnterNewRegion(int dx, int dy)
        {
            RegionCoordinate += new Point(dx, dy);
            

            GenerateCurrentRegionHeightmap();
            GenerateLocalHeight(5);
            GenerateBackgroundColorFromRegionHeightmap();
        }

       

        internal void MoveCamera(int dx, int dy)
        {
           

            if (Camera.X + dx >= 0 && Camera.X + dx  + ScreenWidth < MapWidth )
            {
                if (Camera.Y + dy >= 0 && Camera.Y + dy + ScreenHeight < MapHeight )
                {
                    Camera += new Point(dx, dy);
                }
            }
        }

        internal void SetCameraAt(int x, int y)
        {
            if  ( x < 0 ) { x = 0; }
            if ( y < 0 ) { y = 0; }
            if ( y > MapHeight - ScreenHeight) { y = MapHeight-ScreenHeight; }
            if ( x > MapWidth -ScreenWidth) { x = MapWidth -ScreenWidth ; }
            Camera = new Point(x, y);

        }

        internal bool LocationIsBlocked(int x, int y)
        {
            return false;             // TODO implement a new bool map ?
        }


        internal bool IsOnCurrentScreen(int x, int y)
        {
            if (x >= 0 && x < ScreenWidth)
            {
                if (y >= 0 && y < ScreenHeight)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOnCamera(Creature c)
        {
            Rectangle camera = new Rectangle(Camera.X * ScreenWidth,
                                             Camera.Y * ScreenHeight,
                                             ScreenWidth, ScreenHeight);
            if (camera.Contains(new Microsoft.Xna.Framework.Point(c.X_pos, c.Y_pos)))
            {
                return true;
            }
            return false;
        }

        internal bool IsOnCamera(int v1, int v2)
        {
            Rectangle camera = new Rectangle(Camera.X * ScreenWidth,
                                              Camera.Y * ScreenHeight,
                                              ScreenWidth, ScreenHeight);
            if (camera.Contains(new Microsoft.Xna.Framework.Point(v1, v2)))
            {
                return true;
            }
            return false;
        }

        public bool IsInRegion(Creature c)
        {
            return (RegionCoordinate.X == c.RegionX && RegionCoordinate.Y == c.RegionY);
        }


        
        private void GenerateLocalHeight(int iterations)
        {
            /*
            int counter = 0;
            while (counter < iterations)
            {
                int radius = rnd.Next(10, 30);
                int xh = rnd.Next(40, 160);            // NB NEED SEED VALUE FROM OVERWORLD!!
                int yh = rnd.Next(40, 160);


                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapHeight; y++)
                    {
                        int height = radius * radius - ((x - xh) * (x - xh)) - ((y - yh) * (y - yh));
                        if (height > 0)
                            CurrentRegionHeightmap[x, y] += (byte)height;
                    }
                }

                counter++;
            }
            */


            for (int x = 1; x < MapWidth-1; x++)
            {
                for (int y = 1; y < MapHeight-1; y++)
                {

                    CurrentRegionHeightmap[x, y] += (byte)rnd.Next(50);
                }
            }

                    // 2. Smooth by neighbor values
                    int smoothtimer = 0;
            while (smoothtimer < 6 )  // bit arbitrary 3 times smooth
            {
                smoothtimer++;
                for (int y = 1; y < MapHeight-1; y++)
                {
                    for (int x = 1; x < MapWidth-1; x++)
                    {
                        int over = GetNeighborValue(x, y - 1);
                        int under = GetNeighborValue(x, y + 1);
                        int left = GetNeighborValue(x - 1, y);
                        int right = GetNeighborValue(x + 1, y);
                        CurrentRegionHeightmap[x, y] = (byte)((over + under + left + right) / 4);
                    }
                }
            }

          
            

            int GetNeighborValue(int x, int y)
            {
                int neighborHeight = CurrentRegionHeightmap[x, y];
                return neighborHeight;
               
            }

        }

      

        private void GenerateBackgroundColorFromRegionHeightmap()
        {
            RegionMap = new Map();

            // 1. Set background color of location based on height
            VAColor topColor = VAColor.LightGreen;
            VAColor bottomColor = VAColor.DarkGreen;
            VAColor basecolor;

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Tile newLoc = new Tile();
                    newLoc.glyph = Glyph.SPACE1;
                    newLoc.fgColor = VAColor.WhiteSmoke;
                    newLoc.blocked = false;
                    newLoc.blockSight = false;

                    // newLoc.bgColor = VAColor.Blend(topColor, bottomColor, (CurrentRegionHeightmap[x, y] / 256));

                    newLoc.bgColor = VAColor.White * CurrentRegionHeightmap[x, y];

                    if (CurrentRegionHeightmap[x, y] > 100) { basecolor = VAColor.Gray; }
                    else if (CurrentRegionHeightmap[x, y] > 50) { basecolor = VAColor.Green; }
                    else if (CurrentRegionHeightmap[x, y] > 20) { basecolor = VAColor.Yellow; }
                    else { basecolor = VAColor.Blue; }

                    newLoc.bgColor = VAColor.Blend(basecolor, newLoc.bgColor, 0.2f);
                    RegionMap[x, y] = newLoc;
                }
            }
        }

        // Set right edge contourline, bottom edge contourline and lower right corner glyph and color
        private void GenerateContourlinesOnMap()
        {
            for (int y = 0; y < MapHeight - 1; y++)
            {
                for (int x = 0; x < MapWidth - 1; x++)
                {
                    bool right = false;
                    bool bottom = false;
                    int CurrentGridHeight = GetGridHeight(x, y);
                    if (GetGridHeight(x + 1, y) < CurrentGridHeight)
                    {
                        Tile l = RegionMap[x + 1, y];
                        l.bgColor = VAColor.SaddleBrown;
                        RegionMap[x + 1, y] = l;
                        right = true;
                    }

                    if (GetGridHeight(x, y + 1) < CurrentGridHeight)
                    {
                        Tile l = RegionMap[x, y + 1];
                        l.bgColor = VAColor.SaddleBrown;
                        RegionMap[x, y + 1] = l;
                        bottom = true;
                    }

                    if (GetGridHeight(x + 1, y + 1) < CurrentGridHeight)
                    {
                        if (right && bottom)
                        {
                            Tile l = RegionMap[x + 1, y + 1];
                            l.bgColor = VAColor.SaddleBrown;
                            l.fgColor = VAColor.SandyBrown;
                            l.glyph = Glyph.BACK_SLASH;
                            RegionMap[x + 1, y + 1] = l;
                        }
                    }


                }

            }
            int GetGridHeight(int x, int y)
            {
                int neighborHeight = CurrentRegionHeightmap[x, y];

                return neighborHeight;

            }
        }

        internal void Update()
        {
            
        }

        internal void Draw(VirtualConsole screen)
        {
            /*
            Map currentGrid = Maps[new Point(0, 0)];

            int x_offset_into_mapgrid = Camera.X;
            int y_offset_into_mapgrid = Camera.Y;

            for (int x = 0; x < ScreenWidth; x++)
            { 
                for (int y = 0; y < ScreenHeight; y++)
                { 
                    screen.PutGlyphForeBack(currentGrid[x + x_offset_into_mapgrid, y+ y_offset_into_mapgrid].glyph,
                        x, y, 
                        currentGrid[x+ x_offset_into_mapgrid, y + y_offset_into_mapgrid].fgColor, 
                        currentGrid[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].bgColor);  
                }
                
            }
            */
            int x_offset_into_mapgrid = Camera.X;
            int y_offset_into_mapgrid = Camera.Y;
            for (int x = 0; x < ScreenWidth; x++)
            {
                for (int y = 0; y < ScreenHeight; y++)
                {
                    screen.PutGlyphForeBack(RegionMap[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].glyph,
                         x, y,
                         RegionMap[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].fgColor,
                         RegionMap[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].bgColor);
                }
            }

                }

        internal bool IsOnMap(int v1, int v2)
        {
            if ( v1 > 0 && v1 < MapWidth-1 )
            {
                if ( v2 > 0 && v2 < MapHeight-1 )
                {
                    return true;
                }
            }
            return false;
        }
    }
        
}