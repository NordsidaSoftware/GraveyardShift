using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public class FSM:ComponentsParts
    {
        public Stack<FSMstate> states;
        public GOAPlanner goaplanner;
        public Queue<GOAP_action> currentPlan;
        
        public FSM(Creature owner):base(owner)
        {
            goaplanner = new GOAPlanner(this);
            states = new Stack<FSMstate>();
            currentPlan = new Queue<GOAP_action>();
            SetupStates();
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

    internal class MoveState : FSMstate
    {
        public MoveState(FSM fsm) : base(fsm)
        {
        }

        public override void OnEnter()
        {
            Console.WriteLine(fsm.owner.Name + " On Enter Move State");
        }

        public override void OnExit()
        {
            Console.WriteLine(fsm.owner.Name + " On Exit Move State");
        }

        public override void Update()
        {
            if ( fsm.currentPlan.Peek().InRange(fsm.owner)) { fsm.PopState();  return; }
            fsm.owner.Distribute(new CPMessage()
            {
                type = CPMessageType.TARGET,
                x_position = fsm.currentPlan.Peek().target.X,
                y_position = fsm.currentPlan.Peek().target.Y
            });
        }
    }
}
