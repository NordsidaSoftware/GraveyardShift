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
        public ViewDetailActorMenu(StateManager manager, Virtual_root_Console root, Creature creature) : base(manager, root)
        {
            this.creature = creature;
            menu = new VirtualConsole(60, 60);
            details = new VirtualConsole(60, 30);
            details.X_Offset = menu.screen_width;
            root.SetBackgroundColor(details, VAColor.DarkGreen);
            consoles.Add(menu);
            consoles.Add(details);
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
