using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class EffectComponent : ComponentsParts
    {
        Random rnd;

        public EffectComponent(Creature owner) : base(owner)
        {
            rnd = Randomizer.GetRandomizer();
        }

        internal void ApplyEffect(CPMessage message)
        {
            BodyPart damaged_bodyPart = owner.body.bodyparts.ElementAt(rnd.Next(owner.body.bodyparts.Count)).Value;
            if (damaged_bodyPart != null)
            {
                damaged_bodyPart.hitPoints -= message.attack.attack_damage;
                damaged_bodyPart.status += 1;
                Console.WriteLine(owner.ToString() + " : " + damaged_bodyPart.name + "(" + message.attack.name + ")" +
                                                       message.attack.attack_damage + "=" +
                                                       (BodyPartStatus)damaged_bodyPart.status);
                if ((int)damaged_bodyPart.status > 5)
                    owner.body.bodyparts.Remove(damaged_bodyPart.name);
            }
        }

        internal override void Recieve(CPMessage message)
        {
            if (message.type == CPMessageType.DAMAGE)
            {
                if (owner.body.bodyparts.Count < 1) return;

                ApplyEffect(message);

                if (message.attack.effect != EffectTypes.NONE)
                {
                    switch (message.attack.effect)
                    {
                        case EffectTypes.ZombieEffect:
                            {
                                if (!HasEffect(EffectTypes.ZombieEffect))
                                {
                                    ZombieEffect effect = new ZombieEffect
                                    {
                                        owner = owner
                                    };


                                    owner.effects.Add(effect);
                                    break;
                                }
                                else { break; }
                            }      
                    }
                }
            } 
        }

        private bool HasEffect(EffectTypes zombieEffect)
        {
            bool hasEffect = false;

            foreach (Effect e in owner.effects)
            {
                if (e.type == EffectTypes.ZombieEffect) { hasEffect = true; }
            }

            if (hasEffect) { return true; }
            else { return false; }
        }

        internal override void Update() {     }
    }
}
