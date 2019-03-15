using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    [Serializable]
    public class AttackComponent : ComponentsParts
    {
        Random rnd;
        public AttackComponent(Creature owner) : base(owner)
        {
            rnd = new Random();
        }

        internal override void Recieve(CPMessage message)
        {
            Creature target = null;

            if ( message.type == CPMessageType.MELEE_ATTACK)
            {
                foreach ( Creature creature in owner.manager.RegionCreatures)
                {
                    if ( creature.X_pos == message.x_position && creature.Y_pos == message.y_position && creature != owner)
                    {
                        target = creature;
                    }
                }

                if (target != null)
                {
                    Dictionary<string, Attack> availableAttacks = owner.body.GetAllAvailableAttacks();

                    if (availableAttacks.Count > 0)
                    {
                        
                        int random_attack_index = rnd.Next(availableAttacks.Count);
                        Attack attack = availableAttacks.ElementAt(random_attack_index).Value;
                   
                        target.Distribute(new CPMessage() { type = CPMessageType.DAMAGE, attack = attack });
                    }
                }
            }

            if (message.type == CPMessageType.RANGED_ATTACK)
            {
                foreach (Creature creature in owner.manager.RegionCreatures)
                {
                    if (creature.X_pos == message.x_position && creature.Y_pos == message.y_position && creature != owner)
                    {
                        target = creature;
                    }
                }

                if ( target != null )
                {
                  //  List<Attack> availableRangedAttacks = owner.Inventory.GetRangedAttacks();
                  //  int random_attack_index = rnd.Next(availableRangedAttacks.Count);
                  //  Attack attack = availableRangedAttacks.ElementAt(random_attack_index);

                  //  target.Distribute(new CPMessage() { type = CPMessageType.DAMAGE, attack = attack });
                }
            }
        }

        private double DistanceTo(int x, int y)               
        {
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        internal override void Update()
        {
           
        }
    }
}
