using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
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

            Effects = new Dictionary<string, object>();
            Effects.Add("patrol", true);
        }

        public override bool CheckProceduralPrecondition(Creature creature)
        {
            Random rnd = Randomizer.GetRandomizer();
            int x_pos = rnd.Next(0, creature.manager.worldManager.WorldWidth);
            int y_pos = rnd.Next(0, creature.manager.worldManager.WorldHeight);

            target = new Point(x_pos, y_pos);
            return true;
        }

        public override bool InRange(Creature creature)
        {
            if ( creature.X_pos == target.X && creature.Y_pos == target.Y ) { return true; }
            return false;
        }

        public override bool Perform(Creature creature)
        {
            Turns++;
            if ( Turns > TurnsToComplete) { Console.WriteLine("Finished Patroling in area"); Finished = true; }
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
}
