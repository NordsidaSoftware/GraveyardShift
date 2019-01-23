using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    public  class CreatureManager
    {
        public List<Creature> creatures;
        public List<Effect> effects;
        public WorldManager worldManager;
        public WorldStates worldStates;

        public CreatureManager(WorldManager world)
        {
            Random rnd = Randomizer.GetRandomizer();
            creatures = new List<Creature>();
            effects = new List<Effect>();
            worldManager = world;

            worldStates = new WorldStates(world, creatures);
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

    
        public void AddCreature(Creature c)
        {
            creatures.Add(c);
        }

        internal void Update()
        {
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
            
        }

        internal void Draw(VirtualConsole map)
        {
            
            foreach ( Creature c in creatures )
            {
                if (map.IsInBounds(c.X_pos, c.Y_pos))
                {
                    map.PutGlyph(c.glyph, c.X_pos, c.Y_pos);
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