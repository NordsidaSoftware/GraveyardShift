using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    /*                    =============  C R E A T U R E  M A N A G E R  ==============
     * 
     *          
     * 
     * 
     * 
     * */

    [Serializable]
    public  class CreatureManager
    {
        public List<Creature> creatures;
        public List<Effect> effects;

        public Population worldPopulation;
       // public Creature[,] Region_Creature_Grid;
        public List<Creature> RegionCreatures;
        public Point CurrentRegion;

        public WorldManager worldManager;
        public WorldStates worldStates;

        public bool ResetUpdateLoop = false;            // Needed when player exits region. 

     

        public CreatureManager(WorldManager world, Population population)
        {
            Random rnd = Randomizer.GetRandomizer();
            creatures = new List<Creature>();
            effects = new List<Effect>();
            worldManager = world;
            CurrentRegion = world.RegionCoordinate;
            worldStates = new WorldStates(world, creatures);


            worldPopulation = population;
            SetRegion(0, 0);


          
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
            worldPopulation.AddCreature(c, region);
        }

        internal List<Point> CalculatePathTo(Point target)
        {
            throw new NotImplementedException();
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

        internal void Update()
        {
            for ( int index = RegionCreatures.Count-1; index >= 0; index-- )
            {
                if ( ResetUpdateLoop ) { ResetUpdateLoop = false;  break; }
                RegionCreatures[index].Update();
            }
         
          
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

      

    }
}