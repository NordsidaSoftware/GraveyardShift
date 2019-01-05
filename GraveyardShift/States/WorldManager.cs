using System;
using VAC;

namespace GraveyardShift
{

    public class Location
    {
        public Glyph glyph;
        public bool blocked;
        public bool blockSight;
    }

    public class WorldManager
    {
        Random rnd;
       public Location[,] map;
       public int width, height;

        public WorldManager(int width, int height)
        {
            rnd = new Random();
            this.width = width;
            this.height = height;

            map = new Location[width, height];
            for ( int x = 0; x < width; x++ )
            {
                for ( int y = 0; y < height; y++ )
                {
                 //   if (rnd.Next(100) > 95)
                 //       map[x, y] = new Location() { glyph = Glyph.PLUS, blocked = true, blockSight = true };
                 //   else
                        map[x, y] = new Location() { glyph = Glyph.SPACE1, blocked = false, blockSight = false };
                }
            }
        }
        internal void Update()
        {
            
        }

        internal void Draw(VirtualConsole screen)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    screen.PutGlyph(map[x, y].glyph, x, y);
                }
            }
        }
    }
}