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

        public CreatureManager(WorldManager world)
        {
            Random rnd = Randomizer.GetRandomizer();
            creatures = new List<Creature>();
            effects = new List<Effect>();
            worldManager = world;

            for (int i = 0; i < 10; i++)
            {
                Creature c = new Creature(this)
                {
                    Name = "ZombiDude",
                    X_pos = rnd.Next(0, world.width),
                    Y_pos = rnd.Next(0, world.height)
                };
                c.controller = new ZombieController(c);
                c.controller.Initialize();
                c.controller.CreateBody();
                creatures.Add(c);
            }

            for (int i = 0; i < 10; i++)
            {
                Creature v = new Creature(this)
                {
                    Name = "Vicar #" + i.ToString(),
                    X_pos = rnd.Next(0, world.width),
                    Y_pos = rnd.Next(0, world.height)
                };
                v.controller = new VicarController(v);
                v.controller.Initialize();
                v.controller.CreateBody();
                creatures.Add(v);
            }

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

        internal bool LocationIsBlocked(int x, int y)
        {
            bool blocked = false;
            foreach ( Creature c in creatures)
            {
                if ( c.X_pos == x && c.Y_pos == y) { blocked = true; }
            }
            return blocked;
        }
    }
}