using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class Player : Creature
    {
        public Player(CreatureManager manager) : base(manager)
        {
        }

        internal override void Distribute(CPMessage cpMessage)
        {
            base.Distribute(cpMessage);
        }

        internal override void Update()
        {
            base.Update();
        }
    }
}
