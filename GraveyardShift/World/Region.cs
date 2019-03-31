using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    public class Region
    {
        public int Seed { get; set; }
        byte[,] Height;
        public Color_Map Background;
        public Features_Map Foreground;
        private Random rnd;

        //public List<Item> Items;

        public byte this[int x, int y] { get { return Height[x, y]; } set { Height[x, y] = value; } }

        public Region(int seed)
        {
            Seed = seed;
            Height = new byte[200, 200];
            rnd = new Random(Seed);
           // Items = new List<Item>();
        }

        public static float InterpolateLine(float valueA, float valueB, float x)
        {
            return (valueA + (valueB - valueA) * (x));
        }

        public void GenerateCurrentRegionHeightmap(Overworld overWorld, Point RegionCoordinate)
        {
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

                    Height[x, y] = cellHeight;
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
                    Height[x + 100, y] = cellHeight;
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
                    Height[x + 100, y + 100] = cellHeight;
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
                    Height[x, y + 100] = cellHeight;
                }
            }
        }

        public void GenerateLocalHeight()
        {
            #region oldCode
            /*
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
            */
            #endregion
        }

        public void GenerateBackgroundColorFromRegionHeightmap()
        {
            Background = new Color_Map();

            for (int x = 0; x < 200; x++)
            {
                for (int y = 0; y < 200; y++)
                {
                    Background[x, y] = DB.HightToColor[Height[x, y]];
                }
            }
        }

        public void GenerateTiles(Overworld overWorld)
        {
            Foreground = new Features_Map();

            // Test code that generates a circle of forest
            for (int i = 0; i < 5; i++)
            {
                int centerX = rnd.Next(20, 200 - 20);
                int centerY = rnd.Next(20, 200 - 20);
                int radius = rnd.Next(5, 20);

                Circle feature = new Circle(new Point(centerX, centerY), radius);

                foreach (Point p in feature.cells)
                {
                    Foreground[p.X, p.Y] = (int)DB.Features.TREE;
                }
            }

          
        }

        public void GenerateSettlement()
        {
            CreateHouse();
        }

       

        private void CreateHouse()
        {
            
            // Random generation of house size
            int x = rnd.Next(20, 200 - 20);
            int y = rnd.Next(20, 200 - 20);
            int w = rnd.Next(5, 15);
            int h = rnd.Next(5, 15);
            Rectangle house = new Rectangle(x, y, w, h);

            

            // Create the walls
            foreach ( Point p in house.Walls()) { Foreground[p.X, p.Y] = (int)DB.Features.WALL; }
           

            // Create a door into the house
            // Point door = house.Walls()[rnd.Next(house.Walls().Length)];
            Point door = new Point(house.Center().X, house.Bottom);
            Foreground[door.X, door.Y] = (int)DB.Features.DOOR;

            //Item ItemHouse = new Building(house.Center());
            //ItemHouse.AddElement(house);
            //ItemHouse.AddElement(door);

            //Item bed = new Bed(new Point(house.Center().X + 1, house.Center().Y));
            //bed.tags.Add("bed");
      
            //Items.Add(ItemHouse);
            //Items.Add(bed);

        }
    
        public void GenerateContourlinesOnMap()
        {
            // Set right edge contourline, bottom edge contourline and lower right corner glyph and color

            for (int y = 0; y < 200 - 1; y++)
            {
                for (int x = 0; x < 200 - 1; x++)
                {
                    bool right = false;
                    bool bottom = false;
                    int CurrentGridHeight = Height[x, y];
                    if (Height[x + 1, y] < CurrentGridHeight)
                    {
                        VAColor color = Background[x + 1, y];
                        color *= 0.5f;
                        Background[x + 1, y] = color;
                        right = true;
                    }

                    if (Height[x, y + 1] < CurrentGridHeight)
                    {
                        VAColor color = Background[x, y + 1];
                        color *= 0.5f;
                        Background[x, y + 1] = color;
                        bottom = true;
                    }

                    if (Height[x + 1, y + 1] < CurrentGridHeight)
                    {
                        if (right && bottom)
                        {
                            VAColor color = Background[x + 1, y + 1];
                            color *= 0.5f;
                            // l.fgColor = VAColor.SandyBrown;             // Need glyphmap here
                            // l.glyph = Glyph.BACK_SLASH;
                            Background[x + 1, y + 1] = color;
                        }
                    }


                }
            }
        }

        

    }
}
