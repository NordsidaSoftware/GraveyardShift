using System;

namespace GraveyardShift
{
    [Serializable]
    public abstract class FSMstate
    {
        public FSM fsm;

        public FSMstate(FSM fsm) { this.fsm = fsm; }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update();

    }
}
