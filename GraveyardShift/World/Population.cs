using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public class Population
    {
        private Dictionary<Point, RegionPopulation> PopulationInRegion;
        WorldManager world;
        Random rnd;


        public Population(WorldManager world)
        {
            CreateRegionPopulationLists();
            this.world = world;
        }

        internal List<Creature> GenerateInitialPopulation(CreatureManager creatureManager, int seed)
        {
            rnd = new Random(seed);
            List<Creature> initialPopulation = new List<Creature>();

            for (int number_of_initial_persons = 0; number_of_initial_persons < 2; number_of_initial_persons++)
            {
                Creature c = new Creature(creatureManager)
                {
                    Name = "GOAP agent",
                    X_pos = 25,
                    Y_pos = 25
                };
                c.controller = new SoldierController(c);
                c.controller.Initialize();
                c.controller.CreateBody();
                initialPopulation.Add(c);
            }

            return initialPopulation;
           
        }

        private void CreateRegionPopulationLists()
        {
            PopulationInRegion = new Dictionary<Point, RegionPopulation>();

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    PopulationInRegion.Add(new Point(x, y), new RegionPopulation());
                }
            }
        }

        internal List<Creature> GetPopulationInRegion(Point region)
        {
            return PopulationInRegion[region].creatures;
        }

        internal void AddCreature(Creature c, Point region)
        {
            PopulationInRegion[region].creatures.Add(c);
            
        }

        internal void RemoveCreature(Creature c, Point region)
        {
            PopulationInRegion[region].creatures.Remove(c);
        }

        internal void MoveCreatureToRegion(Creature c, Point FromRegion, Point ToRegion)
        {
            AddCreature(c, ToRegion);
            RemoveCreature(c, FromRegion);
        }
    }

    internal class RegionPopulation
    {
        public List<Creature> creatures;

        public RegionPopulation () { creatures = new List<Creature>(); }
    }
}