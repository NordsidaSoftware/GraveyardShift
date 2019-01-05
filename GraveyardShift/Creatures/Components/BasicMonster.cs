using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class ZombieAI : ComponentsParts
    {
        Random rnd;

        public enum ZombieStates { Wander, Attack }

        public ZombieStates state;

        public int SensoryDistance;

        public Creature Creature_Target;

        public int Position_Target_X;
        public int Position_Target_Y;


        public ZombieAI(Creature owner) : base(owner)
        {
            rnd = Randomizer.GetRandomizer();

            state = ZombieStates.Wander;
            SensoryDistance = 50;

        }

        internal override void Update()
        {
            switch (state)
            {
                case ZombieStates.Wander:
                    {
                        /*
                        foreach (Creature c in owner.manager.creatures)
                        {
                            if (c != owner && c.Faction == Faction.GOOD)  // do not detect yourself, recently dead or other walking deads...
                            {
                               
                                if (DistanceTo(c.X_pos, c.Y_pos) < SensoryDistance)
                                {
                                    Creature_Target = c; state = ZombieStates.Attack;
                                }
                            }
                            */

                        Creature closest_enemy = FindClosestEnemyInSensoryRange();
                        if (closest_enemy != null) { Creature_Target = closest_enemy; state = ZombieStates.Attack; }


                        else if (rnd.Next(100) > 90)
                        {
                            Position_Target_X = rnd.Next(owner.manager.worldManager.width);
                            Position_Target_Y = rnd.Next(owner.manager.worldManager.height);

                            Send(new CPMessage() { type = CPMessageType.TARGET, x_position = Position_Target_X, y_position = Position_Target_Y });
                        }

                        break;
                    }


                case ZombieStates.Attack:
                    {
                        if (Creature_Target != null)
                        {
                            if (!Creature_Target.IsActive || !Creature_Target.body.IsAlive) { Creature_Target = null; state = ZombieStates.Wander; break; } // job done!

                            if (DistanceTo(Creature_Target.X_pos, Creature_Target.Y_pos) < 2)
                            {
                                Send(new CPMessage()
                                {
                                    type = CPMessageType.ATTACK,
                                    x_position = Creature_Target.X_pos,
                                    y_position = Creature_Target.Y_pos


                                });  // TEST CODE  
                                break;
                            }
                            else
                            {
                                Send(new CPMessage()
                                {
                                    type = CPMessageType.TARGET,
                                    x_position = Creature_Target.X_pos,
                                    y_position = Creature_Target.Y_pos
                                });
                            }
                        }
                        break;
                    }
            }
        }
        internal Creature FindClosestEnemyInSensoryRange()
        {
            // flood fill from creature position.
            // if location contains enemy creature, return it
            // else return null
        }

        private double DistanceTo(int x, int y)                  // Zomies don t need eyes or ears !
        {
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        internal override void Recieve(CPMessage message)
        {

        }
    }
}
