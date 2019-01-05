using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VAC;

namespace GraveyardShift
{
    public class PlayState : State
    {
        VirtualConsole screen;
        WorldManager world;

        public CreatureManager creatureManager;

        public PlayState(StateManager manager, Virtual_root_Console root, int width, int height) 
            : base(manager, root)
        {
            screen = root.AddConsole(width, height);
            screen.X_Offset = 10;
            consoles.Add(screen);

            world = new WorldManager(width, height);
            creatureManager = new CreatureManager(world);
            
        }

        public override void Update()
        {
            if (root.input.wasKeyPressed(Keys.Escape)) {  manager.PopState(); }

            if (root.input.wasKeyPressed(Keys.Enter)) { manager.PushState(new IngameMenu(manager, root, this)); }

            world.Update();
            creatureManager.Update();
           

        }

        public override void Draw()
        {
            root.Clear(screen, VAColor.Green);
            world.Draw(screen);
            creatureManager.Draw(screen);

            base.Draw();
        }

    }
}
