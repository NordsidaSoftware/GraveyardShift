using System;

namespace GraveyardShift
{
    public enum CPMessageType { TARGET,
        MELEE_ATTACK,
        DAMAGE,
        EFFECT,
        RANGED_ATTACK,
        SENSED_ENEMIES_IN_RANGE,
        MOVE
    }
    public struct CPMessage
    {
        public CPMessageType type;

        public Attack attack;
        public string text;
        public int value;
        public int x_position;
        public int y_position;
        public ComponentsParts sender;
    }

    [Serializable]
    public abstract class ComponentsParts
    {
        internal Creature owner;

        public ComponentsParts(Creature owner) { this.owner = owner; isActive = true; }

        public bool isActive { get; internal set; }

        internal abstract void Update();

        internal virtual void Send(CPMessage message) { owner.Distribute(message); }
        internal abstract void Recieve(CPMessage message);
        
    }
}