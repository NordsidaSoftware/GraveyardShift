using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    [Serializable]
    public class FSM:ComponentsParts
    {
        public Stack<FSMstate> states;
        public GOAPlanner goaplanner;
        public Queue<GOAP_action> currentPlan;

        // DEBUG to get the path of creature...
        public Stack<Point> Path;
        
        public FSM(Creature owner):base(owner)
        {
            goaplanner = new GOAPlanner(this);
            states = new Stack<FSMstate>();
            currentPlan = new Queue<GOAP_action>();
            SetupStates();

            // Path debug :
            Path = new Stack<Point>();
        }

      
        private void SetupStates()
        {
            PushState(new IdleState(this));
        }

        internal override void Update()
        {
            if ( states.Count > 0 )
            {
                states.Peek().Update();
            }
        }

        public void PopState()
        {
            if (states.Count > 0 )  { states.Peek().OnExit(); states.Pop(); }
        }

        public void PushState(FSMstate state)
        {
            states.Push(state);
            states.Peek().OnEnter();
        }

       

        internal override void Recieve(CPMessage message)
        {
         
        }

    }

    [Serializable]
    internal class IdleState : FSMstate
    {
        public IdleState(FSM fsm) : base(fsm) {  }

        public override void OnEnter()
        {
            Console.WriteLine(fsm.owner.Name + " On Enter Idle State");
        }

        public override void OnExit()
        {
            Console.WriteLine(fsm.owner.Name + " On Exit Idle State");
        }

        public override void Update()
        {
            IGoap c = (IGoap)fsm.owner.controller;
            Queue<GOAP_action> plan = fsm.goaplanner.MakePlan(c.GetGoalsState(), c.GetWorldState(), c.GetActions());
            if (plan.Count > 0)
            {
                fsm.currentPlan = plan;
                fsm.PushState(new PerformActionState(fsm));
            }
         
        }
    }

    [Serializable]
    internal class PerformActionState : FSMstate
    {
        public PerformActionState(FSM fsm) : base(fsm)
        {
        }

        public override void OnEnter()
        {
            Console.WriteLine(fsm.owner.Name + " On Enter Perform State");
        }

        public override void OnExit()
        {
            Console.WriteLine(fsm.owner.Name + " On Exit Perform State");
        }

        public override void Update()
        {
            if ( fsm.currentPlan.Count > 0 )
            {
                if ( fsm.currentPlan.Peek().IsDone())
                {
                    fsm.currentPlan.Peek().Reset();
                    fsm.currentPlan.Dequeue();
                }
            }

            if ( fsm.currentPlan.Count > 0 )
            {
                bool inRange = fsm.currentPlan.Peek().RequiresInRange() ? fsm.currentPlan.Peek().InRange(fsm.owner) : true;

                if ( inRange)
                {
                   bool sucess =  fsm.currentPlan.Peek().Perform(fsm.owner);
                }

                else
                {
                    fsm.PushState(new MoveState(fsm));
                }
            }
            else
            {
                fsm.PopState(); // back to idle state
            }
        }
    }

    [Serializable]
    internal class MoveState : FSMstate
    {
        Stack<Point> path;
        Point nextStep;
        public MoveState(FSM fsm) : base(fsm)
        {
            path = new Stack<Point>();
        }

        public override void OnEnter()
        {
            Console.WriteLine(fsm.owner.Name + " On Enter Move State");
            path = fsm.owner.manager.worldManager.GreedyBestFirstSearch(new Point(fsm.owner.X_pos, fsm.owner.Y_pos), fsm.currentPlan.Peek().target);

            // PAth debug
            fsm.Path = path;
        }

        public override void OnExit()
        {
            Console.WriteLine(fsm.owner.Name + " On Exit Move State");
            path.Clear();
        }

        public override void Update()
        {
            if ( fsm.currentPlan.Peek().InRange(fsm.owner)) { fsm.PopState(); path.Clear(); return; } // in range. Stop moving

            // still not in range. 
            if (path.Count > 0)
            {
                // nextStep = path.Pop();     
                // Test to check next step is a possible move
                Point testNext = path.Peek();
                if ( ! fsm.owner.manager.worldManager.LocationIsBlocked(testNext.X, testNext.Y))
                {
                    if ( ! fsm.owner.manager.LocationIsOccupied(testNext.X, testNext.Y))
                    {
                        nextStep = path.Pop();
                    }
                }

            }
            fsm.owner.Distribute(new CPMessage()
            {
                type = CPMessageType.TARGET,
                x_position = nextStep.X,
                y_position = nextStep.Y
               // x_position = fsm.currentPlan.Peek().target.X,
               // y_position = fsm.currentPlan.Peek().target.Y
            });
        }
    }
}
