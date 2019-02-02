using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    [Serializable]
    public abstract class CreatureController
    {
        public Random rnd;
        public CreatureController(Creature owner) { Owner = owner; rnd = Randomizer.GetRandomizer(); }
        internal Creature Owner { get; set; }
        internal abstract void Initialize();
        internal abstract void CreateBody();
        internal abstract void Update();
        
    }
}


