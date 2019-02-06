//using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    [Serializable]
    public struct Tile
    {
        public Glyph glyph;
        public VAColor fgColor;
       // public VAColor bgColor;
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

    public class Color_Map
    {
        public VAColor this[int x, int y] { get { return colors[x, y]; } set { colors[x, y] = value; } }
        private VAColor[,] colors;

        public Color_Map() { colors = new VAColor[200, 200]; }
    }


    public class Tile_Map
    {
        public Tile this[int x, int y] {  get { return tiles[x, y]; }  set { tiles[x, y] = value; } }
        private Tile[,] tiles;

        public Tile_Map () { tiles = new Tile[200, 200]; }
    }


    public class Bool_Map
    {
        public bool this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
        private bool[,] cells;

        public Bool_Map() { cells = new bool[200, 200]; }
    
    }
    public class FOV_Map
    {
        public bool this[int x, int y] { get { return cells[x, y]; } }
        private Bool_Map visited;
        private bool[,] cells;

        public FOV_Map(Bool_Map visited) { cells = new bool[200, 200];  this.visited = visited; }

        internal void CalculateFOV(Tile_Map map, int originX, int originY)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = false;
                }
            }

            int range = 10;
            Point origin = new Point(originX, originY);

            int x_start = originX - range;
            int y_start = originY - range;
            int x_stop = originX + range;
            int y_stop = originY + range;

            if (x_start < 2) x_start = 2;
            if (y_start < 2) y_start = 2;
            if (x_stop >= cells.GetLength(0)) x_stop = cells.GetLength(0) - 2;
            if (y_stop >= cells.GetLength(1)) y_stop = cells.GetLength(1) - 2;

            Point[] targets = new Rectangle(x_start, y_start, x_stop - x_start, y_stop - y_start).Walls();

            bool visible;

            foreach (Point target in targets)
            {
                int N = Line.Diagonal_distance(origin, target);
                visible = true;

                for (int step = 0; step <= N; step++)
                {
                    if (N == 0) continue;
                    Point p = (Line.LerpPoint(origin, target, (float)step / N));

                    if ( map[p.X, p.Y].blocked) { visible = false; }

                    cells[p.X, p.Y] = visible;
                    visited[p.X, p.Y] = true;
                }
            }
        }

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

        public Region Region_Heightmap { get; set; }
        public Color_Map Region_Background_Color_Map;
        public Tile_Map Region_Tile_Map;
        public FOV_Map fov_Map;
        public Bool_Map visited_Map;

        public int MapWidth, MapHeight;

        public bool drawOverWorld; // TEMP hack

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

            int seed = 050776;                           //    Seed to generate all.Hardcoded for now...
            overWorld = new Overworld(seed);             //    Master overworld 50 x 50 bytes
            overWorld.Create(75);                        //    Creates overworld. Iterations is number of hills !
            Camera = new Point(0, 0);                    //    Viewport origin. Size like screen
            RegionCoordinate = new Point(20, 20);        //    Current Region 

            GenerateCurrentRegionHeightmap();
            GenerateLocalHeight(5);
            GenerateBackgroundColorFromRegionHeightmap();
            GenerateTiles();
            //GenerateContourlinesOnMap();

            visited_Map = new Bool_Map();
            fov_Map = new FOV_Map(visited_Map);
           

            drawOverWorld = false;                       // TEMP hack
        }

        //*******************************************************************
        //                      MAP GENERATION METHODS
        //*******************************************************************

        private void GenerateCurrentRegionHeightmap()
        {
            Region_Heightmap = new Region();
            Region_Heightmap.Seed = overWorld.GetSeed(RegionCoordinate.X, RegionCoordinate.Y);

            byte hNW = overWorld[RegionCoordinate.X - 1, RegionCoordinate.Y - 1];
            byte hN = overWorld[RegionCoordinate.X, RegionCoordinate.Y - 1];
            byte hNE = overWorld[RegionCoordinate.X + 1, RegionCoordinate.Y - 1];
            byte hE = overWorld[RegionCoordinate.X + 1, RegionCoordinate.Y];
            byte hW = overWorld[RegionCoordinate.X - 1, RegionCoordinate.Y];
            byte hSE = overWorld[RegionCoordinate.X + 1, RegionCoordinate.Y + 1];
            byte hS = overWorld[RegionCoordinate.X, RegionCoordinate.Y + 1];
            byte hSW = overWorld[RegionCoordinate.X - 1, RegionCoordinate.Y + 1];
            byte h = overWorld[RegionCoordinate.X, RegionCoordinate.Y];


            // Upgrade possibility : use util class line 
            // 1. NW QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine((hNW+hN)/2.0f, hN, x/100.0f );
                    byte hBottom = (byte)InterpolateLine((hW+h)/2.0f, h, x/100.0f );

                    byte cellHeight = (byte)InterpolateLine((hTop+hBottom)/2.0f, hBottom, y/100.0f );

                    Region_Heightmap[x, y] = cellHeight;
                }
            }

            // 2. NE QUADRANT
            
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(hN, (hNE+hN)/2.0f, x / 100.0f);
                    byte hBottom = (byte)InterpolateLine(h, (hE+h)/2.0f, x / 100.0f);

                    byte cellHeight = (byte)InterpolateLine((hTop+hBottom)/2.0f, hBottom, y / 100.0f);
                    Region_Heightmap[x+100 , y] = cellHeight;
                }
            }

            // 3. SE QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(h, (hE+h)/2.0f, x / 100.0f);
                    byte hBottom = (byte)InterpolateLine(hS, (hSE+hS)/2.0f, x / 100.0f);

                    byte cellHeight = (byte)InterpolateLine(hTop, (hBottom+hTop)/2.0f, y/100.0f);
                    Region_Heightmap[x+100, y+100] = cellHeight;
                }
            }

            // 4. SW QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine((hW+h)/2.0f, h, x/100.0f);
                    byte hBottom = (byte)InterpolateLine((hSW+hS)/2.0f, hS, x/100.0f);

                    byte cellHeight = (byte)InterpolateLine(hTop, (hBottom+hTop)/2.0f, y /100.0f);
                    Region_Heightmap[x, y+100] = cellHeight;
                }
            }
        }

        private void GenerateBackgroundColorFromRegionHeightmap()
        {
            Region_Background_Color_Map = new Color_Map();

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

                    // newLoc.bgColor = VAColor.White * CurrentRegionHeightmap[x, y];

                    if (Region_Heightmap[x, y] > 80) { basecolor = VAColor.Gray; }
                    else if (Region_Heightmap[x, y] > 30) { basecolor = VAColor.Green; }
                    else if (Region_Heightmap[x, y] > 10) { basecolor = VAColor.Yellow; }
                    else { basecolor = VAColor.Blue; }

                    // newLoc.bgColor = VAColor.Blend(basecolor, newLoc.bgColor, 0.2f);
                   // basecolor = basecolor * Region_Heightmap[x, y];
                    Region_Background_Color_Map[x, y] = basecolor;
                }
            }
        }

        private void GenerateTiles()
        {
            Random rnd = new Random(Region_Heightmap.Seed);   // you need to move!!!

            Region_Tile_Map = new Tile_Map();
            for ( int x = 0; x < 200; x++)
            {
                for ( int y = 0; y < 200; y++ )
                {
                    Region_Tile_Map[x, y] = new Tile() { glyph = Glyph.SPACE1, fgColor = VAColor.LightGreen};
                    if (rnd.Next(100) > 97) { Region_Tile_Map[x, y] = new Tile()
                    {
                        glyph = Glyph.UP_ARROW, fgColor = VAColor.LightGreen,
                        blocked = true
                    }; }
                }
            }
        }


        private void GenerateLocalHeight(int iterations)
        {
            
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
                            Region_Heightmap[x, y] += (byte)height;
                    }
                }

                counter++;
            }
            

            
            for (int x = 1; x < MapWidth-1; x++)
            {
                for (int y = 1; y < MapHeight-1; y++)
                {

                    Region_Heightmap[x, y] += (byte)rnd.Next(-10, 10);
                }
            }
            

                    // 2. Smooth by neighbor values
                    int smoothtimer = 0;
            while (smoothtimer < 6 )  
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
                        Region_Heightmap[x, y] = (byte)((over + under + left + right) / 4);
                    }
                }

              
            
            }

          
            

            int GetNeighborValue(int x, int y)
            {
                int neighborHeight = Region_Heightmap[x, y];
                return neighborHeight;  
            }          
        }

        private void GenerateContourlinesOnMap()
        {
            // Set right edge contourline, bottom edge contourline and lower right corner glyph and color

            for (int y = 0; y < MapHeight - 1; y++)
            {
                for (int x = 0; x < MapWidth - 1; x++)
                {
                    bool right = false;
                    bool bottom = false;
                    int CurrentGridHeight = GetGridHeight(x, y);
                    if (GetGridHeight(x + 1, y) < CurrentGridHeight)
                    {
                        VAColor color = Region_Background_Color_Map[x + 1, y];
                        color *= 0.5f;
                        Region_Background_Color_Map[x + 1, y] = color;
                        right = true;
                    }

                    if (GetGridHeight(x, y + 1) < CurrentGridHeight)
                    {
                        VAColor color = Region_Background_Color_Map[x, y + 1];
                        color *= 0.5f;
                        Region_Background_Color_Map[x, y + 1] = color;
                        bottom = true;
                    }

                    if (GetGridHeight(x + 1, y + 1) < CurrentGridHeight)
                    {
                        if (right && bottom)
                        {
                            VAColor color = Region_Background_Color_Map[x + 1, y + 1];
                            color *= 0.5f;
                           // l.fgColor = VAColor.SandyBrown;             // Need glyphmap here
                           // l.glyph = Glyph.BACK_SLASH;
                            Region_Background_Color_Map[x + 1, y + 1] = color;
                        }
                    }


                }

            }
            int GetGridHeight(int x, int y)
            {
                int neighborHeight = Region_Heightmap[x, y];

                return neighborHeight;

            }
        }

        private float InterpolateLine(float valueA, float valueB, float x)
        {
            return (valueA + (valueB - valueA) * ( x ) );
        }

        internal void EnterNewRegion(int dx, int dy)
        {
            RegionCoordinate += new Point(dx, dy);
            

            GenerateCurrentRegionHeightmap();
            GenerateLocalHeight(5);
            GenerateBackgroundColorFromRegionHeightmap();
            GenerateTiles();
            //GenerateContourlinesOnMap();
            visited_Map = new Bool_Map();
            fov_Map = new FOV_Map(visited_Map);
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
            return Region_Tile_Map[x, y].blocked;
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

        /*
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
        */

        /*
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
        */

        public bool IsInRegion(Creature c)
        {
            return (RegionCoordinate.X == c.RegionX && RegionCoordinate.Y == c.RegionY);
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
            if (drawOverWorld)
            {
                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        screen.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.White * overWorld[x, y]);

                        if ( x == RegionCoordinate.X && y == RegionCoordinate.Y)
                        {
                            screen.PutGlyphBackGround(Glyph.CAPS_X, x, y, VAColor.Yellow);
                        }
                    }
                }
            }
            else
            {
                int x_offset_into_mapgrid = Camera.X;
                int y_offset_into_mapgrid = Camera.Y;
                for (int x = 0; x < ScreenWidth; x++)
                {
                    for (int y = 0; y < ScreenHeight; y++)
                    {
                        if (fov_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid])
                        {
                            screen.PutGlyphForeBack(Region_Tile_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].glyph,
                            x, y,
                            Region_Tile_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].fgColor,
                            Region_Background_Color_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid]);
                        }
                        else
                        {
                            if (visited_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid])
                            {
                                screen.PutGlyphForeBack(Region_Tile_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].glyph,
                                                            x, y,
                                                            Region_Tile_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid].fgColor,
                                                            Region_Background_Color_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid] * 0.5f);
                            }
                            else
                            {
                                screen.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.Black);
                            }
                        }
                    }
                }

            }
        }

    }
        
}