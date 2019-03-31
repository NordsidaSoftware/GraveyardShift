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

        public WorldManager world;
        public CreatureManager creatureManager;
        public ItemManager itemManager;

        public PlayState(StateManager manager, WorldManager world, CreatureManager creature, ItemManager itemManager, 
                         Virtual_root_Console root, int width, int height) : base(manager, root)
        {
            screen = root.AddConsole(width, height);
            screen.X_Offset = 10;
            consoles.Add(screen);

            this.world = world;
            this.creatureManager = creature;
            this.itemManager = itemManager;
            
        }

        public override void Update()
        {
            if (root.input.wasKeyPressed(Keys.Escape)) {  manager.PopState(); }

            if ( root.input.wasKeyPressed(Keys.Tab)) { manager.PushState(new OverWorldMenu(manager, root, this, world, creatureManager)); }

            world.Update();
            creatureManager.Update();
           

        }

        public override void Draw()
        {
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
