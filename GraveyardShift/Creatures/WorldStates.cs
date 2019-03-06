using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    [Serializable]
    public class WorldStates
    {
        private WorldManager world;

        public Dictionary<string, object> worldStates;

        public WorldStates(WorldManager world)
        {
            this.world = world;
            
            worldStates = new Dictionary<string, object>();
            worldStates.Add("day", true);                         // Ex. of 'global' world state for GOAP planner
        }
        public Dictionary<string, object> GetWorldStates()
        {
            return worldStates;
        }


    }

}