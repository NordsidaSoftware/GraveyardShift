using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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

        Random rnd = Randomizer.GetRandomizer();


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
            int seed = 12345;//  rnd.Next();

            world = new WorldManager(MapWidth, MapHeight, width, height, seed);

            Population population = new Population(world);
           

            creatureManager = new CreatureManager(world, population);
            List<Creature> initialPopulation = population.GenerateInitialPopulation(creatureManager, seed);


            // initial population into  a settlement
            Point settlement = world.overWorld.GetNextSettlement();
            if (settlement.X != -1)  // Not used pr. now...
            {
                foreach ( Creature c in initialPopulation )
                {
                    population.AddCreature(c, settlement);
                }
               
            }
            
            // Time simulation here.
            for ( int year = 0; year < 100; year++ )
            {

            }


            //


            // load world ( map ) and creatures into main
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