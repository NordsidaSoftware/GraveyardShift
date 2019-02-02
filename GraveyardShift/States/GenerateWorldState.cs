using Microsoft.Xna.Framework.Input;
using VAC;

namespace GraveyardShift
{
    // TODO : vil ikke alle disse consolene ta mye plass i minnet. Lage en metode for å fjerne og rydde =`??
    internal class GenerateWorldState : State
    {
        private int width;
        private int height;

        private VirtualConsole screen;
        MainMenuState main;
        WorldManager world;
        CreatureManager creatureManager;


        public GenerateWorldState(StateManager manager, MainMenuState main, Virtual_root_Console root, int width, int height):base(manager, root)
        {
            this.manager = manager;
            this.main = main;
            this.root = root;
            this.width = width;
            this.height = height;
            screen = root.AddConsole(width, height);
            consoles.Add(screen);

            root.Print(screen, width / 2, 2, "Generating new world...");

        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void OnEnter()
        {
            int MapWidth = 200;
            int MapHeight = 200;

            world = new WorldManager(MapWidth, MapHeight, 2, 2, width, height);
            creatureManager = new CreatureManager(world);

            main.world = world;
            main.creatureManager = creatureManager;
        }

        public override void OnExit()
        {
            consoles.Clear();
            base.OnExit();
        }

        public override void Update()
        {
            if (root.input.wasKeyPressed(Keys.Enter))
            {
                manager.PushState(new CharacterGenerationMenu(manager, world, creatureManager, root));
            }
            base.Update();
        }
    }
}