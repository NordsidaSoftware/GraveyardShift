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

        public PlayState(StateManager manager, WorldManager world, CreatureManager creature, Virtual_root_Console root, int width, int height) 
            : base(manager, root)
        {
            screen = root.AddConsole(width, height);
            screen.X_Offset = 10;
            consoles.Add(screen);

            this.world = world;
            this.creatureManager = creature;
            
        }

        public override void Update()
        {
            if (root.input.wasKeyPressed(Keys.Escape)) {  manager.PopState(); }

            if (root.input.wasKeyPressed(Keys.Enter)) { manager.PushState(new IngameMenu(manager, root, this)); }

            if ( root.input.wasKeyPressed(Keys.Tab)) { world.drawOverWorld = !world.drawOverWorld; }

            world.Update();
            creatureManager.Update();
           

        }

        public override void Draw()
        {
            root.Clear(screen, VAColor.DarkGreen);
            world.Draw(screen);
            creatureManager.Draw(screen);

            base.Draw();
        }

        public override void OnExit()
        {
            foreach ( VirtualConsole con in consoles)
            {
                root.Clear(con);
            }

            root.Flush();
        }

    }
}
