using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    [Serializable]
    public abstract class GOAP_action
    {
        public string ID;
        public int Cost;
        public bool Finished;

        public int TurnsToComplete;
        public int Turns;

        public Dictionary<string, object> Preconditions;
        public Dictionary<string, object> Effects;
        public abstract bool CheckProceduralPrecondition(Creature creature);

       
        public Point target;
        public abstract bool Perform(Creature creature);
        public abstract bool RequiresInRange();
        public abstract bool InRange(Creature creature);
        internal abstract bool IsDone();
        public abstract void Reset();
    }

    [Serializable]
    public class GOAP_action_PATROL : GOAP_action
    {
        public GOAP_action_PATROL()
        {
            ID = "PATROL";
            Cost = 1;
            Turns = 0;
            TurnsToComplete = 10;
            Finished = false;

            Preconditions = new Dictionary<string, object>();
            Preconditions.Add("isMobile", true);
            Preconditions.Add("rested", true);

            Effects = new Dictionary<string, object>();
            Effects.Add("patrol", true);
            Effects.Add("rested", false);
        }

        public override bool CheckProceduralPrecondition(Creature creature)
        {
            Random rnd = Randomizer.GetRandomizer();
            int x_pos = rnd.Next(0, creature.manager.worldManager.MapWidth);
            int y_pos = rnd.Next(0, creature.manager.worldManager.MapHeight);
            if (!creature.manager.worldManager.LocationIsBlocked(x_pos, y_pos))
            {
                target = new Point(x_pos, y_pos);
                return true;
            }
            else
                return false;
        }

        public override bool InRange(Creature creature)
        {
            if ( creature.X_pos == target.X && creature.Y_pos == target.Y ) { return true; }
            return false;
        }

        public override bool Perform(Creature creature)
        {
            Turns++;
            if ( Turns > TurnsToComplete) { Console.WriteLine("Finished Patroling in area"); Finished = true; creature.body.isRested = false; }
            Console.WriteLine(creature.Name + "Patroling in area " + creature.X_pos.ToString() + "," + creature.Y_pos.ToString());
            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override void Reset()
        {
            Finished = false;
            Turns = 0;
        }

        internal override bool IsDone()
        {
            return Finished;
        }
    }

    [Serializable]
    public class GOAP_action_SLEEP : GOAP_action
    {
        public GOAP_action_SLEEP()
        {
            ID = "SLEEP";
            Cost = 1;
            Turns = 0;
            TurnsToComplete = 20;
            Finished = false;

            Preconditions = new Dictionary<string, object>();
            Preconditions.Add("rested", false);

            Effects = new Dictionary<string, object>();
            Effects.Add("rested", true);
        }

        public override bool CheckProceduralPrecondition(Creature creature)
        {
            Item sleepItem = creature.GetItem("bed");
            if ( sleepItem != null ) { target = sleepItem.Position; return true; }
            else { return false; }

            /*
            target = creature.manager.worldManager.GetCreatureBed(creature);
            if (target.X != 0)
                 return true;
            else return false;
            

            Random rnd = Randomizer.GetRandomizer();
            int x_pos = rnd.Next(0, creature.manager.worldManager.MapWidth);
            int y_pos = rnd.Next(0, creature.manager.worldManager.MapHeight);
            if (!creature.manager.worldManager.LocationIsBlocked(x_pos, y_pos))
            {
                target = new Point(x_pos, y_pos);
                return true;
            }
            else
                return false;
                */
        }

        public override bool InRange(Creature creature)
        {
            if (creature.X_pos == target.X && creature.Y_pos == target.Y) { return true; }
            return false;
        }

        public override bool Perform(Creature creature)
        {
            Turns++;
            if (Turns > TurnsToComplete) { Console.WriteLine("Finished Sleeping"); Finished = true; creature.body.isRested = true; }
            Console.WriteLine(creature.Name + "Sleeping " + creature.X_pos.ToString() + "," + creature.Y_pos.ToString());
            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override void Reset()
        {
            Finished = false;
            Turns = 0;
        }

        internal override bool IsDone()
        {
            return Finished;
        }
    }
}
