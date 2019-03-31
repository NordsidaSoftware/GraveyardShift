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
        ItemManager itemManager;


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
            int seed = 12345;   // here the world generation seed is set

            world = new WorldManager(MapWidth, MapHeight, width, height, seed);

        
            
            itemManager = new ItemManager( world );
            

            Population population = new Population( world );
           
            creatureManager = new CreatureManager(world, population, itemManager);
            List<Creature> initialPopulation = population.GenerateInitialPopulation(creatureManager, seed);


            // put initial population into  a number of settlements, where the initial population in each settlement is
            // 5...
            int number_of_initial_settlements = initialPopulation.Count / 5;
            for (int index = 0; index < number_of_initial_settlements; index++)
            {
                Point settlement = world.overWorld.GetNextSettlement();
                if (settlement.X != -1)  // Not used pr. now...
                {
                    // create houses in settlement here :
                    // --->

                    // Setup the historical figures in this population :
                    foreach (Creature c in initialPopulation.GetRange(index * 5, 5))
                    {
                        // Adding creature c to this settlement
                        population.AddCreature(c, settlement);

                        // Give this creature his/hers inventory
                        c.Inventory.AddItem(itemManager.itemFactory.CreateItem("cheese", "weapon"));

                    }

                }
            }


            
            // Time simulation here.
            for ( int year = 0; year < 100; year++ )
            {

            }


            //


            // load world ( map ) and creatures and items into main
            main.world = world;
            main.creatureManager = creatureManager;
            main.itemManager = itemManager;
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
                manager.PushState(new CharacterGenerationMenu(manager, world, creatureManager, itemManager, root));
            }
            base.Update();
        }
    }
}