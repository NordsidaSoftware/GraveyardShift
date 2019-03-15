using Microsoft.Xna.Framework.Input;
using System;
using VAC;

namespace GraveyardShift
{
    internal class WorldTestState : State
        // Test state no longer used. Created an overworld for the first time...
    {
        private MainMenuState mainMenuState;
        private int screen_width;
        private int screen_height;

        private bool drawOverworld;

        Overworld overWorld;

        Point currentPosition;
        Region currentRegion;

        public WorldTestState(StateManager manager, MainMenuState mainMenuState, Virtual_root_Console root, int width, int height, int test):base(manager, root)
        {
            this.manager = manager;
            this.mainMenuState = mainMenuState;
            this.root = root;
            this.screen_width= width;
            this.screen_height = height;
            drawOverworld = true;
            currentPosition = new Point(25, 25);    // from 1 to 49 on x and y axis...


            int seed = 050776;

            overWorld = new Overworld(seed);
            overWorld.Create();
        }

        public override void Draw()
        {
            if (drawOverworld)
            {
                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        root.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.White * overWorld[x, y]);
                    }
                }
                root.PutGlyphBackGround(Glyph.SPACE1, currentPosition.X, currentPosition.Y, VAColor.DeepSkyBlue);
            }

            else
            {
                Circle c = new Circle(new Point(25, 25), 10);

                foreach ( Point p in c.circumference)
                {
                    root.PutGlyph(Glyph.AMPERSAND, p.X, p.Y);
                }
               // drawCurrentRegion();
               // for (int x = 0; x < 50; x++)
               // {
               //     for (int y = 0; y < 50; y++)
               //     {
                        
                       // root.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.White * currentRegion[x, y]);
               //     }
               // }
            }
           
            base.Draw();
        }

        private void drawCurrentRegion()
        {
            /*
            currentRegion = new Region();

            byte hNW = overWorld[currentPosition.X - 1, currentPosition.Y - 1];
            byte hN = overWorld[currentPosition.X, currentPosition.Y - 1];
            byte hNE = overWorld[currentPosition.X + 1, currentPosition.Y - 1];
            byte hE = overWorld[currentPosition.X + 1, currentPosition.Y];
            byte hW = overWorld[currentPosition.X - 1, currentPosition.Y];
            byte hSE = overWorld[currentPosition.X + 1, currentPosition.Y+1];
            byte hS = overWorld[currentPosition.X, currentPosition.Y+1];
            byte hSW = overWorld[currentPosition.X - 1, currentPosition.Y-1];
            byte h = overWorld[currentPosition.X, currentPosition.Y];

            // 1. NW QUADRANT
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    byte hTop = (byte)InterpolateLine(hNW, hN, x);
                    byte hBottom = (byte)InterpolateLine(hW, h, x);

                    byte cellHeight = (byte)InterpolateLine(hTop, hBottom, y);
                    currentRegion[x, y] = cellHeight;
                }
            }
            */
        }

        private float InterpolateLine(byte valueA, byte valueB, int x)
        {
            return (valueA + (valueB - valueA) * (0.5f + x/50.0f));
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Update()
        {

            if ( root.input.wasKeyPressed(Keys.Enter)) { manager.PopState(); }
            if ( root.input.wasKeyPressed(Keys.Tab)) { drawOverworld = !drawOverworld; }
            if ( root.input.wasKeyPressed(Keys.Left)) { currentPosition.X--; }
            if ( root.input.wasKeyPressed(Keys.Right)) { currentPosition.X++; }
            if ( root.input.wasKeyPressed(Keys.Up)) { currentPosition.Y--; }
            if (root.input.wasKeyPressed(Keys.Down)) { currentPosition.Y++; }
            
        }
    }
}