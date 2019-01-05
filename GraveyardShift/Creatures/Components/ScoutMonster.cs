using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{

    public class ScoutMonsterAI : ComponentsParts
    {
        public enum ScoutStates { SCOUTING, ATTACKING, RETREATING }
        Random rnd;

        public Creature Creature_Target;

        public int Position_Target_X;
        public int Position_Target_Y;
        public ScoutStates scoutState;

        public ScoutMonsterAI(Creature owner) : base(owner)
        {
            rnd = Randomizer.GetRandomizer();
            scoutState = ScoutStates.SCOUTING;
        }

        internal override void Update()
        {
            int max_damage = 0;
            int accumulated_damage = 0;
            foreach (BodyPart BP in owner.body.bodyparts.Values )
            {
                max_damage += BP.maxHitPoints;
                accumulated_damage += BP.maxHitPoints - BP.hitPoints;
            }
            
            if ( ( accumulated_damage > ( max_damage / 2) )) { scoutState = ScoutStates.RETREATING; }

            switch (scoutState)
            {
                case ScoutStates.SCOUTING:
                    {
                        foreach ( Creature c in owner.manager.creatures )
                        {
                            if ( c.Faction == Faction.EVIL )
                            {
                                if (DistanceTo(c.X_pos, c.Y_pos) < 10 )
                                {
                                    Creature_Target = c;
                                    scoutState = ScoutStates.ATTACKING;
                                }
                            }
                        }

                        if (rnd.Next(100) > 50)
                        {
                            Position_Target_X = rnd.Next(owner.manager.worldManager.width);
                            Position_Target_Y = rnd.Next(owner.manager.worldManager.height);

                            Send(new CPMessage()
                            {
                                type = CPMessageType.TARGET,
                                x_position = Position_Target_X,
                                y_position = Position_Target_Y
                            });
                        }
                        break;
                    }
                case ScoutStates.ATTACKING:
                    {
                        if (Creature_Target != null)
                        {
                            if (!Creature_Target.IsActive )  // job done!
                            {
                                Creature_Target = null; scoutState = ScoutStates.SCOUTING;
                                break;
                            }                

                            if (DistanceTo(Creature_Target.X_pos, Creature_Target.Y_pos) < 2)
                            {
                                Send(new CPMessage()
                                {
                                    type = CPMessageType.ATTACK,
                                    x_position = Creature_Target.X_pos,
                                    y_position = Creature_Target.Y_pos


                                });  
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
                case ScoutStates.RETREATING:
                    {
                        Send(new CPMessage() { type = CPMessageType.TARGET, x_position = 0, y_position = 0 });
                        break;
                    }
            }

        }

        private double DistanceTo(int x, int y)                  // Zomies dont need eyes or ears !
        {
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        internal override void Recieve(CPMessage message)
        {
            return;
        }
    }
}

