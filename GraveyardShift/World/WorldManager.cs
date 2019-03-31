﻿
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{

    //*********************************************************************************
    //                                        THE FEATURE CLASS
    //                                      called 'feature' for now
    //                                     
    //*********************************************************************************

    [Serializable]
    public struct Feature
    {
        public int glyph;
        public int fgColor;
        public bool blocked;
        public bool blockSight;
    }


    
    public class Color_Map // TODO use byte instead of VAColor...
    {
        public VAColor this[int x, int y] { get { return colors[x, y]; } set { colors[x, y] = value; } }
        private VAColor[,] colors;

        public Color_Map() { colors = new VAColor[200, 200]; }
    }
    

    public class Features_Map
    {
        public int this[int x, int y] { get { return things[x, y]; } set { things[x, y] = value; } }
        private int[,] things;

        public Features_Map() { things = new int[200, 200]; }


        public bool Blocked(int x, int y)
        {
            return DB.IntToItem[things[x, y]].blocked;
        }

        public bool Block_Sight(int x, int y)
        {
            return DB.IntToItem[things[x, y]].blockSight;
        }
    }


    public class Bool_Map
    {
        public bool this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
        private bool[,] cells;

        public Bool_Map() { cells = new bool[200, 200]; }
    }


    [Serializable]
    public class WorldManager
    {
        //Random rnd;

        public int ScreenWidth, ScreenHeight;

        // ****************************************************************
        //|      NEW WORLD STRUCTURE                                      |
        //*****************************************************************

        public Overworld overWorld;
        public Point Camera { get; set; }
        public Point RegionCoordinate { get; set; }

        //public Region Region_Heightmap { get; set; }
        //public Color_Map Region_Background_Color_Map;
        //public Features_Map Region_Features;

        public FOV_Map fov_Map;
        public Bool_Map visited_Map;

        public int MapWidth, MapHeight;

        // restructuring this class.
        // instead of features, fov-map etc here. Have an current region containing all
        // save and reload issues ?

        public Region currentRegion;

        public WorldManager(int width, int height, int screenWidth, int screenHeight, int seed)
        {

            this.MapWidth = width;
            this.MapHeight = height;

            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;

            overWorld = new Overworld(seed);             //    Master overworld 50 x 50 bytes
            overWorld.Create();                          //    Creates overworld.

            // following code to generate region that player starts in 

            Camera = new Point(0, 0);                    //    Viewport origin. Size like screen
            RegionCoordinate = new Point(33, 10);        //    Current Region 

           // GenerateCurrentRegionHeightmap();
           // GenerateLocalHeight(5);
           // GenerateBackgroundColorFromRegionHeightmap();
           // GenerateTiles();
           // GenerateContourlinesOnMap();

            visited_Map = new Bool_Map();
            fov_Map = new FOV_Map(visited_Map);

            EnterNewRegion(0,0);
            //currentRegion = CreateRegionFromOverwoldData();

        }

        private Region CreateRegionFromOverwoldData()
        {
            Region generatedRegion = new Region(overWorld.GetSeed(RegionCoordinate.X, RegionCoordinate.Y));
            generatedRegion.GenerateCurrentRegionHeightmap(overWorld, RegionCoordinate);
            generatedRegion.GenerateLocalHeight();
            generatedRegion.GenerateBackgroundColorFromRegionHeightmap();
            generatedRegion.GenerateTiles(overWorld);
            generatedRegion.GenerateContourlinesOnMap();
            if ( overWorld.settlements.Contains(RegionCoordinate)) { generatedRegion.GenerateSettlement(); }

            return generatedRegion;
        }

       

        #region moved Code
        //*******************************************************************
        //                      MAP GENERATION METHODS
        //*******************************************************************
        /*
        private void GenerateCurrentRegionHeightmap()
        {
            Region_Heightmap = new Region();
            rnd = new Random(Region_Heightmap.Seed);   // you need to move!!!
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
                    byte hTop = (byte)InterpolateLine((hNW + hN) / 2.0f, hN, x / 100.0f);
                    byte hBottom = (byte)InterpolateLine((hW + h) / 2.0f, h, x / 100.0f);

                    byte cellHeight = (byte)InterpolateLine((hTop + hBottom) / 2.0f, hBottom, y / 100.0f);

                    Region_Heightmap[x, y] = cellHeight;
                }
            }

            // 2. NE QUADRANT

            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(hN, (hNE + hN) / 2.0f, x / 100.0f);
                    byte hBottom = (byte)InterpolateLine(h, (hE + h) / 2.0f, x / 100.0f);

                    byte cellHeight = (byte)InterpolateLine((hTop + hBottom) / 2.0f, hBottom, y / 100.0f);
                    Region_Heightmap[x + 100, y] = cellHeight;
                }
            }

            // 3. SE QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine(h, (hE + h) / 2.0f, x / 100.0f);
                    byte hBottom = (byte)InterpolateLine(hS, (hSE + hS) / 2.0f, x / 100.0f);

                    byte cellHeight = (byte)InterpolateLine(hTop, (hBottom + hTop) / 2.0f, y / 100.0f);
                    Region_Heightmap[x + 100, y + 100] = cellHeight;
                }
            }

            // 4. SW QUADRANT
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    byte hTop = (byte)InterpolateLine((hW + h) / 2.0f, h, x / 100.0f);
                    byte hBottom = (byte)InterpolateLine((hSW + hS) / 2.0f, hS, x / 100.0f);

                    byte cellHeight = (byte)InterpolateLine(hTop, (hBottom + hTop) / 2.0f, y / 100.0f);
                    Region_Heightmap[x, y + 100] = cellHeight;
                }
            }
        }

        private void GenerateBackgroundColorFromRegionHeightmap()
        {
            Region_Background_Color_Map = new Color_Map();

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Region_Background_Color_Map[x, y] = DB.HightToColor[Region_Heightmap[x, y]];
                }
            }
        }

        private void GenerateTiles()
        {
            Region_Features = new Features_Map();

            // Test code that generates a circle of forest
            for (int i = 0; i < 5; i++)
            {
                int centerX = rnd.Next(20, MapWidth - 20);
                int centerY = rnd.Next(20, MapHeight - 20);
                int radius = rnd.Next(5, 20);

                Circle feature = new Circle(new Point(centerX, centerY), radius);

                foreach (Point p in feature.cells)
                {
                    Region_Features[p.X, p.Y] = (int)DB.Features.TREE;
                }
            }
            // If currentRegion contains a settlement. Create the house(s):  // for now only one house...
            if (overWorld.settlements.Contains(RegionCoordinate))
            {
                CreateHouse();
            }
        }

        private void CreateHouse()
        {
            // Random generation of house size
            int x = rnd.Next(20, MapWidth - 20);
            int y = rnd.Next(20, MapHeight - 20);
            int w = rnd.Next(5, 15);
            int h = rnd.Next(5, 15);
            Rectangle house = new Rectangle(x, y, w, h);

            // Create the walls
            for (int Hor_Wall = house.Left; Hor_Wall <= house.Right; Hor_Wall++)
            {
                Region_Features[Hor_Wall, house.Top] = (int)DB.Features.WALL;
                Region_Features[Hor_Wall, house.Bottom] = (int)DB.Features.WALL;
            }
            for (int Vert_Wall = house.Top; Vert_Wall < house.Bottom; Vert_Wall++)
            {
                Region_Features[house.Left, Vert_Wall] = (int)DB.Features.WALL;
                Region_Features[house.Right, Vert_Wall] = (int)DB.Features.WALL;
            }

            // Create a door into the house
            Point door = house.Walls()[rnd.Next(house.Walls().Length)];
            Region_Features[door.X, door.Y] = (int)DB.Features.DOOR;
        }

        private void GenerateLocalHeight(int iterations)
        {
            int counter = 0;
            while (counter < iterations)
            {
                int radius = rnd.Next(3, 10);

                Point origo = new Point(rnd.Next(radius + 2, MapWidth - radius - 2),
                                           rnd.Next(radius + 2, MapHeight - radius - 2));
                Circle c = new Circle(origo, radius);

                foreach (Point p in c.cells)
                {
                    Region_Heightmap[p.X, p.Y] += 5;
                }

                Circle s = new Circle(origo, radius - 2);
                foreach (Point p in s.cells)
                {
                    Region_Heightmap[p.X, p.Y] += 5;
                }

                counter++;
            }


            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Region_Heightmap[x, y] = (byte)(Region_Heightmap[x, y] / 5);
                }
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
            return (valueA + (valueB - valueA) * (x));
        }
        */

        #endregion  

        internal void EnterNewRegion(int dx, int dy)
        {
            RegionCoordinate += new Point(dx, dy);

            currentRegion = CreateRegionFromOverwoldData();

            visited_Map = new Bool_Map();
            fov_Map = new FOV_Map(visited_Map);
        }



        internal void MoveCamera(int dx, int dy)
        {
            if (Camera.X + dx >= 0 && Camera.X + dx + ScreenWidth < MapWidth)
            {
                if (Camera.Y + dy >= 0 && Camera.Y + dy + ScreenHeight < MapHeight)
                {
                    Camera += new Point(dx, dy);
                }
            }
        }

        internal void SetCameraAt(int x, int y)
        {
            if (x < 0) { x = 0; }
            if (y < 0) { y = 0; }
            if (y > MapHeight - ScreenHeight) { y = MapHeight - ScreenHeight; }
            if (x > MapWidth - ScreenWidth) { x = MapWidth - ScreenWidth; }
            Camera = new Point(x, y);
        }

        internal bool LocationIsBlocked(int x, int y)
        {
            return currentRegion.Foreground.Blocked(x, y);
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



        /*                      =========== P A T H  F I N D I N G ==========
         * 
         * 
         * 
         * */

        static public double Heuristic (Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        internal Stack<Point> GreedyBestFirstSearch(Point origin, Point target)
        {
            Stack<Point> path = new Stack<Point>();
            PriorityQueue<Point> Frontier = new PriorityQueue<Point>();
            Dictionary<Point, Point> CameFrom = new Dictionary<Point, Point>();

            Frontier.Enqueue(origin, 0);
            CameFrom[origin] = origin;

            while (Frontier.Count > 0)
            {
                Point current = Frontier.Dequeue();

                if (current.X == target.X && current.Y == target.Y) { break; } // Early exit here.

                foreach (Point p in current.Neighbors())
                {
                    if (IsOnMap(p.X, p.Y) && !currentRegion.Foreground.Blocked(p.X, p.Y))
                    {
                        if (!CameFrom.ContainsKey(p))
                        { 
                            double priority = Heuristic(target, p);
                            Frontier.Enqueue(p, priority);
                            CameFrom[p] = current;
                        }
                    }
                }
            }

           Point step = target;
        
            while (true)
            {
                path.Push(step);
                step = CameFrom[step]; 
                if (step.X == origin.X && step.Y == origin.Y) { path.Push(step); break; }
            }
            path.TrimExcess();

            return path;
        }


        public bool IsInRegion(Creature c)
        {
            return (RegionCoordinate.X == c.RegionX && RegionCoordinate.Y == c.RegionY);
        }


        internal bool IsOnMap(int v1, int v2)
        {
            if (v1 > 0 && v1 < MapWidth - 1)
            {
                if (v2 > 0 && v2 < MapHeight - 1)
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
            int x_offset_into_mapgrid = Camera.X;
            int y_offset_into_mapgrid = Camera.Y;

            for (int x = 0; x < ScreenWidth; x++)
            {
                for (int y = 0; y < ScreenHeight; y++)
                {
                    int feature = currentRegion.Foreground[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid];

                    Feature thing = DB.IntToItem[feature];
                    Glyph glyph = (Glyph)thing.glyph;
                    VAColor fg = DB.HightToColor[(byte)thing.fgColor];

                    //           BACKGROUND 
                    //         ******************** IN FOV ********************

                    if (fov_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid])
                    {
                        screen.PutGlyphForeBack( glyph, x, y, fg,
                        currentRegion.Background[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid]);


                    }
                    else
                    {
                        //                     BACKGROUND
                        //     ********************  OUTSIDE OF FOV BUT REMEMBERED *******************

                        if (visited_Map[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid])
                        {
                            screen.PutGlyphForeBack ( glyph, x, y, fg * 0.9f,
                              currentRegion.Background[x + x_offset_into_mapgrid, y + y_offset_into_mapgrid] * 0.9f);
                        }
                        else
                        {
                            //  ********************** NOT VISIBLE *************************
                            screen.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.Black);
                        }
                    }
                }
            }
        }
    }

}