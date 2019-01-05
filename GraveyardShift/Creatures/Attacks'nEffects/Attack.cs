using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class Attack
    {
        public string name;
        public int attack_damage;
        public EffectTypes effect;

        public override string ToString()
        {
            return name + "(" + attack_damage.ToString() + ")" + " effect:" + effect.ToString() ;
        }
    }
}
