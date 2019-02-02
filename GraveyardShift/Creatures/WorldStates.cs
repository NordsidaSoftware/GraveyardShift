using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    [Serializable]
    public class WorldStates
    {
        private WorldManager world;
        private List<Creature> creatures;

        public Dictionary<string, object> worldStates;

        public WorldStates(WorldManager world, List<Creature> creatures)
        {
            this.world = world;
            this.creatures = creatures;
            worldStates = new Dictionary<string, object>();
            worldStates.Add("day", true);                         // Ex. of 'global' world state for GOAP planner
        }
        public Dictionary<string, object> GetWorldStates()
        {

            return worldStates;
        }


    }

}