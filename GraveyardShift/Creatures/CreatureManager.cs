using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    [Serializable]
    public  class CreatureManager
    {
        public List<Creature> creatures;
        public List<Effect> effects;

        // ***********************  NEW STRUCTURE *******************
        public Population worldPopulation;
        public Creature[,] Region_Creature_Grid;
        public List<Creature> RegionCreatures;
        public Point CurrentRegion = new Point(20, 20);                     // TODO : Same as in worldManager...

        public WorldManager worldManager;
        public WorldStates worldStates;

        public bool ResetUpdateLoop = false;            // Needed when player exits region. 

     

        public CreatureManager(WorldManager world, Population population)
        {
            Random rnd = Randomizer.GetRandomizer();
            creatures = new List<Creature>();
            effects = new List<Effect>();
            worldManager = world;

            worldStates = new WorldStates(world, creatures);


            worldPopulation = population;
            SetRegion(0, 0);


            /*
            for (int i = 0; i < 3; i++)
            {
                Creature c = new Creature(this)
                {
                    Name = "GOAP agent",
                    X_pos = 25,
                    Y_pos = i *2,
                };
                c.controller = new SoldierController(c);
                c.controller.Initialize();
                c.controller.CreateBody();
                creatures.Add(c);
            }
            */
            //                 REMOVE CREAURE GENERATION HERE FOR A WHILE
            /*
            for (int i = 0; i < 3; i++)
            {
                Creature c = new Creature(this)
                {
                    Name = "ZombiDude",
                    X_pos = 5,
                    Y_pos = i,
                };
                c.controller = new ZombieController(c);
                c.controller.Initialize();
                c.controller.CreateBody();
                creatures.Add(c);
            }

            for (int i = 0; i < 5; i++)
            {
                Creature v = new Creature(this)
                {
                    Name = "Vicar #" + i.ToString(),
                    X_pos = 40,
                    Y_pos = i
                };
                v.controller = new VicarController(v);
                v.controller.Initialize();
                v.controller.CreateBody();
                creatures.Add(v);
            }

            Creature b = new Creature(this)
            {
                Name = "Vicar with bomb",
                X_pos = 42,
                Y_pos = 3
            };
            b.controller = new VicarController(b);
            b.controller.Initialize();
            b.controller.CreateBody();
            Attack fragmentation_grenade = new Attack() { name = "Frag grenade", attack_damage = 5, effect = EffectTypes.BLAST };
            b.Inventory.AddItem(new Item("Holy Handgranade of Antioc", fragmentation_grenade, 15));
            creatures.Add(b);
            */
        }

    
        public void SetRegion(int dx, int dy)
        {
            CurrentRegion += new Point(dx, dy);

            RegionCreatures = worldPopulation.GetPopulationInRegion(CurrentRegion);
        }

        public void MoveToNewRegion(Creature c, int dx, int dy)
        {
            worldPopulation.MoveCreatureToRegion(c, CurrentRegion, new Point(CurrentRegion.X + dx, CurrentRegion.Y + dy));
        }

        public void AddCreature(Creature c, Point region)
        {
            //creatures.Add(c);
            worldPopulation.AddCreature(c, region);
        }

       

        internal void Update()
        {
           // foreach (Creature c in RegionCreatures) { c.Update(); }
            for ( int index = RegionCreatures.Count-1; index >= 0; index-- )
            {
                if ( ResetUpdateLoop ) { ResetUpdateLoop = false;  break; }
                RegionCreatures[index].Update();
            }
         
            /*
            foreach (Creature c in creatures)
            {
                if (c.IsActive)
                {
                        c.Update();
                }
            }

            foreach (Creature c in creatures)
            {
                if (c.effects.Count > 0 )
                {
                    foreach (Effect e in c.effects)
                    {
                        if (e.isActive) { e.Update(); }
                    }
                }
            }



            List<Creature> bufferlist = new List<Creature>();
            foreach ( Creature c in creatures)
            {
                if ( c.IsActive) { bufferlist.Add(c); }
            }
            creatures = bufferlist;
    */        
        }

        internal void Draw(VirtualConsole map)
        {

            //foreach (Creature c in creatures)
            foreach (Creature c in RegionCreatures)
            {
                if (worldManager.IsOnCurrentScreen(c.X_pos - worldManager.Camera.X, c.Y_pos - worldManager.Camera.Y))
                {
                    map.PutGlyph(c.glyph,
                    c.X_pos - worldManager.Camera.X,
                    c.Y_pos - worldManager.Camera.Y,
                    c.color);

                   /*                                Mirror reflection draw. Used over water ?
                   map.PutGlyph(c.glyph,
                   c.X_pos - worldManager.Camera.X+1,
                   c.Y_pos - worldManager.Camera.Y+1,
                   VAColor.DarkGray);
                   */
                }
               
            }

        }

      

        internal bool LocationIsOccupied(int x, int y)
        {
            bool blocked = false;
            foreach ( Creature c in creatures)
            {
                if ( c.X_pos == x && c.Y_pos == y) { blocked = true; }
            }
            return blocked;
        }

        internal Creature GetCreatureAtLocation(Point current)
        {
            Creature returnCreature = null;
            foreach (Creature c in creatures)
            {
                if (c.X_pos == current.X && c.Y_pos == current.Y) { returnCreature = c; }
            }
            return returnCreature;
        }

        internal Creature GetCreatureAtLocation(int x, int y)
        {
            return GetCreatureAtLocation(new Point(x, y));
        }
    }
}