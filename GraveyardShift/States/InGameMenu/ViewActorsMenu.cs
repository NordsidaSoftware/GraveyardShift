using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    public class ViewActorsMenu : State
    {
        int hilite;
        VirtualConsole menu;
        IngameMenu ingameMenu;
        public ViewActorsMenu(StateManager manager, Virtual_root_Console root, IngameMenu ingameMenu) : base(manager, root)
        {
            this.ingameMenu = ingameMenu;
            menu = new VirtualConsole(30, 20);
            menu.X_Offset = 10;
            consoles.Add(menu);
            root.Print(menu, 1, 1, "Creatures");
            root.Print(menu, 1, 2, "---------");

            hilite = ingameMenu.playState.creatureManager.creatures.Count - 1; /// ?


        }

        public override void Draw()
        {
            int line = 0;
            foreach (Creature c in ingameMenu.playState.creatureManager.creatures)
            {
                if ( line == hilite) { root.SetForegroundColor(menu, VAColor.Red); }
                else { root.SetForegroundColor(menu, VAColor.NavajoWhite); }
                root.Print(menu, 1, line + 3, c.Name);
                line++;
            }

            base.Draw();
        }

        public override void Update()
        {
          if (root.input.wasKeyPressed(Keys.Escape)) { manager.PopState(); }
            if (root.input.wasKeyPressed(Keys.Enter))
            {
                Creature selected_creature = GetSelectedCreature();
                if (selected_creature != null)
                {
                    manager.PushState(new ViewDetailActorMenu(manager, root, selected_creature));
                }
            }
            if (root.input.wasKeyPressed(Keys.Down))
            {
                hilite++;
                if (hilite > ingameMenu.playState.creatureManager.creatures.Count-1)
                {
                    hilite = 0;
                }
            }
          if ( root.input.wasKeyPressed(Keys.Up))
            {
                hilite--;
                if (hilite < 0)
                {
                    hilite = ingameMenu.playState.creatureManager.creatures.Count-1 ;
                }


            }
        }

        private Creature GetSelectedCreature()
        {
            if ( ingameMenu.playState.creatureManager.creatures.Count >= hilite )
            {
                return ingameMenu.playState.creatureManager.creatures[hilite];
            }

            return null;
        }

        public override void OnExit()
        {
           
        }

        public override void OnEnter()
        {
            
        }
    }
}
