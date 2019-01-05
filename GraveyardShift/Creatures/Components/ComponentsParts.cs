namespace GraveyardShift
{
    public enum CPMessageType { TARGET,
        ATTACK,
        DAMAGE,
        EFFECT
    }
    public struct CPMessage
    {
        public CPMessageType type;
        public Attack attack;
        public string text;
        public int value;
        public int x_position;
        public int y_position;
    }

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