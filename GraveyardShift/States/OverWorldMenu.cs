using System;
using Microsoft.Xna.Framework.Input;
using VAC;

namespace GraveyardShift
{
    public enum MapMode { HeightByColorMap, HeightByGrayscale,
        Biomes
    }
    public enum Overlay { None, Rivers, Sites }

    internal class OverWorldMenu : State
    {
        private PlayState playState;
        VirtualConsole mapDisplay;
        VirtualConsole mapStats;

        WorldManager world;
        CreatureManager creatures;

        int mapMode;
        int overlay;

        Point cursor;

        public OverWorldMenu(StateManager manager, Virtual_root_Console root, PlayState playState, WorldManager world, CreatureManager creatures):base(manager, root)
        {
            this.manager = manager;
            this.root = root;
            this.playState = playState;
            this.world = world;
            this.creatures = creatures;

            mapDisplay = new VirtualConsole(50, 50);
            mapStats = new VirtualConsole(10, 50);
            mapStats.X_Offset = 50;

            cursor = new Point(25, 25);

            consoles.Add(mapDisplay);
            consoles.Add(mapStats);

            mapMode = (int)MapMode.HeightByGrayscale;
            overlay = (int)Overlay.None;
            
        }

        private void SwitchMapDisplayMode()
        {
            mapMode++;
            if ( mapMode > Enum.GetNames(typeof(MapMode)).Length-1) { mapMode = 0; }
        }

        private void SwitchOverlayMode()
        {
            overlay++;
            if ( overlay > Enum.GetNames(typeof(Overlay)).Length-1 ) { overlay = 0; }
        }

        public override void Draw()
        {
            root.Clear(mapDisplay);
            root.Clear(mapStats);
            switch (mapMode)
            {
                case (int)MapMode.HeightByColorMap: { DrawOverWorldColor(); break; }
                case (int)MapMode.HeightByGrayscale: { DrawOverWorldHeightMap();break; }
                case (int)MapMode.Biomes: { DrawOverWOrldBiomes(); break; }
            }
            switch(overlay)
            {
                case (int)Overlay.None: { break; }
                case (int)Overlay.Rivers: { DrawOverWorldRiverMap(); break; }
                case (int)Overlay.Sites: { DrawOverWorldSitesMap(); break; }
            }


            mapDisplay.PutGlyph(Glyph.CAPS_X, world.RegionCoordinate.X, world.RegionCoordinate.Y);
            mapDisplay.PutGlyph(Glyph.LOW_X, cursor.X, cursor.Y);
            DrawMapStats();
            base.Draw();
        }

        private void DrawOverWOrldBiomes()
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    byte biome = world.overWorld.Biome[x, y];
                    switch(biome)
                    {
                        case 0: { mapDisplay.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.Blue); break; }
                        case 1: { mapDisplay.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.Yellow); break; }
                        case 2: { mapDisplay.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.LightBlue); break; }
                    }
                }
            }
        }

        private void DrawOverWorldSitesMap()
        {
            VAColor col = VAColor.Red;
            foreach ( Point p in world.overWorld.settlements )
            {
                mapDisplay.PutGlyphForeground(Glyph.HEART, p.X, p.Y, col / (world.overWorld.settlementScore[p.X,p.Y]*0.1f));
            }
        }

        private void DrawOverWorldRiverMap()
        {
            VAColor col = VAColor.LightBlue;
            foreach (Point p in world.overWorld.rivers)
            {
                Point flow = world.overWorld.waterDirection[p.X, p.Y];
                if ( flow.X == 0 && flow.Y == 0 ) { mapDisplay.PutGlyphForeground(Glyph.BULLET, p.X, p.Y, col); }
                if ( flow.X ==1 && flow.Y == 0 ) { mapDisplay.PutGlyphForeground(Glyph.RIGHT_ARROW, p.X, p.Y, col); }
                if (flow.X == -1 && flow.Y == 0) { mapDisplay.PutGlyphForeground(Glyph.LEFT_ARROW, p.X, p.Y, col); }
                if (flow.X == 0 && flow.Y == 1) { mapDisplay.PutGlyphForeground(Glyph.DOWN_ARROW, p.X, p.Y, col); }
                if (flow.X == 0 && flow.Y == -1) { mapDisplay.PutGlyphForeground(Glyph.UP_ARROW, p.X, p.Y, col); }
                if (flow.X == 1 && flow.Y == 1) { mapDisplay.PutGlyphForeground(Glyph.BACK_SLASH, p.X, p.Y, col); }
                if (flow.X == -1 && flow.Y == -1) { mapDisplay.PutGlyphForeground(Glyph.BACK_SLASH, p.X, p.Y, col); }
                if (flow.X == 1 && flow.Y == -1) { mapDisplay.PutGlyphForeground(Glyph.SLASH, p.X, p.Y, col); }
                if (flow.X == -1 && flow.Y == 1) { mapDisplay.PutGlyphForeground(Glyph.SLASH, p.X, p.Y, col); }
            }
        }

        private void DrawOverWorldColor()
        {
            byte height;
            VAColor regionColor;

            for ( int x = 0; x < 50; x++ )
            {
                for ( int y = 0; y < 50; y++ )
                {
                    height = world.overWorld[x, y];
                    regionColor = DB.HightToColor[height];
                   
                    
                    mapDisplay.PutGlyphBackGround(Glyph.SPACE1, x, y, regionColor);
                }
            }
           
        }

        private void DrawOverWorldHeightMap()
        {
            for ( int x = 0; x < 50; x ++ )
            {
                for (int y = 0; y < 50; y++)
                {
                    mapDisplay.PutGlyphBackGround(Glyph.SPACE1, x, y, VAColor.White * world.overWorld[x, y]);
                }
            }
        }

        private void DrawMapStats()
        {
            root.Print(mapStats, 1, 1, "MAP STATS:");
            root.Print(mapStats, 1, 3, "MAX:" + world.overWorld.Max().ToString());
            root.Print(mapStats, 1, 4, "MIN:" + world.overWorld.Min().ToString());
            root.Print(mapStats, 1, 5, "H:" + world.overWorld[cursor.X, cursor.Y].ToString());
            root.Print(mapStats, 1, 6, "dC:" + world.overWorld.settlementScore[cursor.X, cursor.Y].ToString());
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            consoles.Clear();
            root.Flush();
            base.OnExit();
        }

        public override void Update()
        {
            if ( root.input.wasKeyPressed(Keys.Tab)) { manager.PopState(); }
            if (root.input.wasKeyPressed(Keys.Enter)) { manager.PushState(new IngameMenu(manager, root, playState)); }

            if ( root.input.wasKeyPressed(Keys.M)) { SwitchMapDisplayMode(); }
            if ( root.input.wasKeyPressed(Keys.N)) { SwitchOverlayMode(); }

            if ( root.input.wasKeyPressed(Keys.Left)) { MoveCursor(-1, 0); }
            if ( root.input.wasKeyPressed(Keys.Right)) { MoveCursor(1, 0); }
            if ( root.input.wasKeyPressed(Keys.Up)) { MoveCursor(0, -1); }
            if ( root.input.wasKeyPressed(Keys.Down)) { MoveCursor(0, 1); }
            base.Update();
        }

        private void MoveCursor(int dx, int dy)
        {
            if ( cursor.X + dx > 0 && cursor.Y + dy > 0 )
            {
                if ( cursor.X + dx < 50 && cursor.Y + dy < 50 )
                {
                    cursor.X += dx;
                    cursor.Y += dy;
                }
            }
        }

    }
}