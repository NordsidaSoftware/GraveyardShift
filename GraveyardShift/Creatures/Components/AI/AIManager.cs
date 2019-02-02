using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    [Serializable]
    public abstract class AI_ACTION
    {
        public string name;
        public AIManager brain;

        public abstract void Update();
        public abstract void OnEnter();
        public abstract void OnExit();

    }
    [Serializable]
    public class AIManager : ComponentsParts
    {
        
        Random rnd;
        Stack<AI_ACTION> states;

        public Creature Antagonist;

        public int Position_Target_X;
        public int Position_Target_Y;

        internal Rectangle sensoryRange;

        public AIManager(Creature owner) : base(owner)
        {
            rnd = Randomizer.GetRandomizer();
            states = new Stack<AI_ACTION>();

            
            sensoryRange = CalculateSensoryRange();
        }

        private Rectangle CalculateSensoryRange()
        {
            int sensoryValue = owner.body.GetAllSensoryValues();
            sensoryRange = new Rectangle(owner.X_pos - sensoryValue / 2, owner.Y_pos - sensoryValue / 2, sensoryValue, sensoryValue);
            return sensoryRange;
        }

        public List<Creature> GetCreaturesInSensoryRange()
        {
            sensoryRange = CalculateSensoryRange();
            List<Creature> returnList = new List<Creature>();
            for (int x = sensoryRange.Left; x < sensoryRange.Right; x++)
            {
                for (int y = sensoryRange.Top; y < sensoryRange.Bottom; y--)
                {
                    if (owner.manager.LocationIsOccupied(x, y)) { returnList.Add(owner.manager.GetCreatureAtLocation(x, y)); }
                }
            }

            return returnList;
        }

        public Creature GetClosestCreatureInSensoryRange()
        {
            Creature closestCreature = null;
            List<Creature> creaturesInRange = GetCreaturesInSensoryRange();
            if (creaturesInRange.Count == 0) { return null; }

            for (int index = creaturesInRange.Count; index >= 1; index--)
            {
                if (DistanceTo(creaturesInRange[index].X_pos, creaturesInRange[index].Y_pos) <=
                     DistanceTo(creaturesInRange[index - 1].X_pos, creaturesInRange[index - 1].Y_pos))
                { closestCreature = creaturesInRange[index]; }
            }

            return closestCreature;
        }

        private double DistanceTo(int x, int y)
        {
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }


        internal override void Update()
        {
          if ( states.Count == 0 )
            {
                PushState(new AI_ACTION_DRUNKEN_WALK(this));
            }


            if (states.Count > 0)
            { states.Peek().Update(); }
        }

      

        internal void PushState(AI_ACTION state)
        {
            states.Push(state);
            states.Peek().OnEnter();
        }

        internal void PopState()
        {
            if ( states.Count > 0 )
            {
                states.Peek().OnExit();
                states.Pop();
            }
        }


        internal override void Recieve(CPMessage message)
        {
           
        }
    }

    internal class AI_ACTION_ATTACK : AI_ACTION
    {
        private AIManager aIManager;

        public AI_ACTION_ATTACK(AIManager aIManager)
        {
            this.aIManager = aIManager;
            name = "Attack";
        }

       
        public override void OnEnter()
        {
            Console.WriteLine(brain.owner.Name + "Enter AI state "  + name);
        }

        public override void OnExit()
        {
            Console.WriteLine(brain.owner.Name + "Exit AI state " + name);
        }

        public override void Update()
        {
            if ( brain.GetCreaturesInSensoryRange().Count == 0 ) { brain.PopState(); }
        }
    }

    public class AI_ACTION_DRUNKEN_WALK : AI_ACTION
    {
        Random rnd;
        public int Position_Target_X;
        public int Position_Target_Y;

        public  AI_ACTION_DRUNKEN_WALK(AIManager manager)
        {
            rnd = Randomizer.GetRandomizer();
            this.brain = manager;
            name = "Random Walker AI";
        }

        public override void OnEnter()
        {
            Console.WriteLine(brain.owner.Name + " Enter AI state " + name);
        }

        public override void OnExit()
        {
            Console.WriteLine(brain.owner.Name + " Exit AI state " + name);
        }

        public override void Update()
        {
            if (rnd.Next(0, 100) > 75 )
            {

                Position_Target_X = rnd.Next(brain.owner.manager.worldManager.MapWidth);
                Position_Target_Y = rnd.Next(brain.owner.manager.worldManager.MapHeight);
                brain.Send(new CPMessage() { type = CPMessageType.TARGET,
                                               x_position = Position_Target_X,
                                               y_position = Position_Target_Y });
            }
        }
    }
}

