using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class ZombieEffect : Effect
    {
        public ZombieEffect()
        {
            name = "Zombie Sickness";
            type = EffectTypes.ZombieEffect;
            turns_to_start = 50;
            timer = 0;
            isActive = true;
        }

        public override void Update()
        {
            timer++;
            if ( timer == turns_to_start ) { ApplyEffect(); }
            
        }

        private void ApplyEffect()
        {
            if (owner == null) return;

            Console.WriteLine(owner.Name + " : Effect : " + name );
            
            
            foreach ( BodyPart BP in owner.body.bodyparts.Values) {  if ( BP.vital ) { BP.status = BodyPartStatus.DESTROYED; } }
            owner.Name = owner.Name + "(Zombie)";

            ZombieSetup();


            isActive = false;
            
        }

        private void ZombieSetup()
        {
            owner.components.Clear();
            owner.controller = new ZombieController(owner);
            owner.controller.Initialize();

            // REMOVE ALL ATTACKS AND MAKE SURE CREATURE HAS A ZOMBIE BITE
            foreach (BodyPart BP in owner.body.bodyparts.Values)
            {
                if ( BP.attack != null ) { BP.attack = null; }
            }

            Attack bite = new Attack() { name = "Bite", attack_damage = 5, effect = EffectTypes.ZombieEffect };
            if (!owner.body.bodyparts.ContainsKey("Mouth"))
            {
                BodyPart mouth = new BodyPart() { name = "Mouth", attack = bite, hitPoints = 2, maxHitPoints = 2 };
                owner.body.bodyparts.Add("Mouth", mouth);
            }
            else
            {
                owner.body.bodyparts["Mouth"].attack = bite;
            }
        }
    }
}
