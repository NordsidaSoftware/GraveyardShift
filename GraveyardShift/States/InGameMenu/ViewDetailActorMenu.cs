using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    public class ViewDetailActorMenu : State
    {
        Creature creature;
        VirtualConsole menu;
        VirtualConsole details;
        VirtualConsole goap;
        public ViewDetailActorMenu(StateManager manager, Virtual_root_Console root, Creature creature) : base(manager, root)
        {
            this.creature = creature;
            menu = new VirtualConsole(60, 60);
            details = new VirtualConsole(60, 30);
            goap = new VirtualConsole(60, 30);
            details.X_Offset = menu.screen_width;
            goap.Y_Offset = 30;

            root.SetBackgroundColor(details, VAColor.DarkGreen);
            root.SetForegroundColor(goap, VAColor.Yellow);
            consoles.Add(menu);
            consoles.Add(details);
            consoles.Add(goap);
        }

        public override void Draw()
        {
            root.Print(menu, 1, 1, "Creature Detail Menu");
            root.Print(menu, 1, 2, "--------------------");
            root.Print(menu, 1, 3, "Name : " + creature.Name);
            root.Print(menu, 1, 4, "Type : " + creature.controller.ToString());
            int line = 5;
            foreach ( ComponentsParts CP in creature.components)
            {
                root.Print(menu, 1, line, CP.ToString());
                line++;
            }
            line = line + 2;
            foreach ( BodyPart BP in creature.body.bodyparts.Values)
            {
                root.Print(menu, 1, line, BP.ToString());
                line++;
            }

            root.Print(menu, 1, line, "**** EFFECTS **** ");
            line++;

            foreach (Effect e in creature.effects)
            {
                root.Print(menu, 1, line, e.ToString());
                line++;
            }

            int details_line = 1;
            root.Print(details, 1, details_line, "DETAILS"); details_line += 2;
            root.Print(details, 1, details_line, "IsActive : " + creature.IsActive.ToString()); details_line += 2;
            root.Print(details, 1, details_line, "IsAlive : " + creature.body.IsAlive.ToString()); details_line += 2;
            root.Print(details, 1, details_line, "CanMove : " + creature.body.CanMove.ToString()); details_line += 2;
            root.Print(details, 1, details_line, "Faction : " + creature.Faction.ToString()); details_line += 2;


            int goap_line = 1;
            root.Print(goap, 1, goap_line, "Planner:"); goap_line += 2;

            
            Point center = new Point(creature.X_pos, creature.Y_pos);
            Point camera = new Point(center.X - goap.screen_width / 2, center.Y - goap.screen_height / 2);
            if ( camera.X < 0 ) { camera.X = 0; }
            if ( camera.Y < 0 ) { camera.Y = 0; }
            if ( camera.X > creature.manager.worldManager.MapWidth-(goap.screen_width+1) ) { camera.X = creature.manager.worldManager.MapWidth - (goap.screen_width + 1); }
            if ( camera.Y > creature.manager.worldManager.MapHeight-(goap.screen_height+1)) { camera.Y = creature.manager.worldManager.MapHeight - (goap.screen_height + 1); }

            for ( int x = 0; x < goap.screen_width; x++ )
            {
                for ( int y = goap_line; y < goap.screen_height; y++ )
                {
                    // BACKGROUND IN CELL
                    goap.PutGlyphBackGround(Glyph.SPACE1, x, y, creature.manager.worldManager.currentRegion.Background[x+camera.X, y+camera.Y]);

                    // FOREGROUND (ITEM ) IN CELL
                    int feature = creature.manager.worldManager.currentRegion.Foreground[x + camera.X, y + camera.Y];
                    Feature thing = DB.IntToItem[feature];
                    
                    goap.PutGlyph(thing.glyph, x, y, VAColor.White);
                }
            }
            // CREATURE GLYPH
            goap.PutGlyph(creature.glyph, center.X - camera.X, center.Y - camera.Y);

            foreach (ComponentsParts CP in creature.components )
            {
                if (CP is FSM fsm)
                {
                    foreach (Point p in fsm.Path)
                    {
                        goap.PutGlyphForeground(Glyph.BULLET, p.X - camera.X, p.Y - camera.Y, VAColor.Orange);
                    }
                }
            }
          

            base.Draw();
        }

        public override void Update()
        {
            if ( root.input.wasKeyPressed(Keys.Escape)) { manager.PopState(); }  
        }

        public override void OnExit()
        {
          
        }

        public override void OnEnter()
        {
            
        }

    }
}
