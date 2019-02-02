using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public enum EffectTypes {  ZombieEffect,
        NONE,
        BLAST
    }
    [Serializable]
    public abstract class Effect
    {
        public Creature owner;

        public bool isActive;
        public int timer;
        public string name;
        public EffectTypes type;
        public int turns_to_start;
        public int turns_to_end;

        public abstract void Update();

        public override string ToString()
        {
            return name + " isActive " + isActive.ToString() + "T:" + timer.ToString();
        }

    }
}
