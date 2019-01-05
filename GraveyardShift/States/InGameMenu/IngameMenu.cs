using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    public class IngameMenu : State
    {
        VirtualConsole menu;
        public PlayState playState;
        public IngameMenu(StateManager manager, Virtual_root_Console root, PlayState playState) : base(manager, root)
        {
            this.playState = playState;
            menu = new VirtualConsole(20, 10);
            root.Print(menu, 0, 1, "Ingame menu");
            root.Print(menu, 0, 2, "------------");
            root.Print(menu, 0, 3, " A - View units");
            consoles.Add(menu);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            if (root.input.wasKeyPressed(Keys.Escape)) { manager.PopState(); }
            if ( root.input.wasKeyPressed(Keys.A)) { manager.PushState(new ViewActorsMenu(manager, root, this)); }
        }

        public override void OnExit()
        {
            
        }

        public override void OnEnter()
        {
            
        }
    }
}
