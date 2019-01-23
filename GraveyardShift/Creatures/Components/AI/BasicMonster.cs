using Microsoft.Xna.Framework;
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
        public Creature closest_enemy;

        public int Position_Target_X;
        public int Position_Target_Y;


        public ZombieAI(Creature owner) : base(owner)
        {
            rnd = Randomizer.GetRandomizer();

            state = ZombieStates.Wander;
            SensoryDistance = 5;

        }

        internal override void Update()
        {
            switch (state)
            {
                case ZombieStates.Wander:
                    {
                        bool isCloseToEnemy = false;
                        foreach (Creature c in owner.manager.creatures)
                        {
                            if (c != owner && c.Faction != owner.Faction)  // do not detect yourself or friends
                            {
                                if (DistanceTo(c.X_pos, c.Y_pos) < SensoryDistance) { isCloseToEnemy = true; }
                            }

                        }
                        if (isCloseToEnemy)
                        {
                            closest_enemy = FindClosestEnemyInSensoryRange();
                            Creature_Target = closest_enemy;
                            state = ZombieStates.Attack;
                           
                        }
                    
            

                       
                        if (closest_enemy != null) { Creature_Target = closest_enemy; state = ZombieStates.Attack; }


                        if (rnd.Next(100) > 50)
                        {
                            Position_Target_X = rnd.Next(0, owner.manager.worldManager.WorldWidth);
                            Position_Target_Y = rnd.Next(0, owner.manager.worldManager.WorldHeight);

                            Send(new CPMessage() { type = CPMessageType.TARGET, x_position = Position_Target_X, y_position = Position_Target_Y });
                        }

                        break;
                    }


                case ZombieStates.Attack:
                    {
                        if (Creature_Target != null)
                        {
                            if (! Creature_Target.body.IsAlive ) { Creature_Target = null; state = ZombieStates.Wander; break; } // job done!

                            if (DistanceTo(Creature_Target.X_pos, Creature_Target.Y_pos) < 2)
                            {
                                Send(new CPMessage()
                                {
                                    type = CPMessageType.MELEE_ATTACK,
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

            List<Point> visited = new List<Point>();
            Queue<Point> frontier = new Queue<Point>();

            Point origin = new Point(owner.X_pos, owner.Y_pos);
            frontier.Enqueue(origin);


            while (frontier.Count > 0)
            {
                Point current = frontier.Dequeue();
                visited.Add(current);


                Creature c = owner.manager.GetCreatureAtLocation(current);
                if (c != null && c != owner && c.Faction != owner.Faction)
                {
                    return owner.manager.GetCreatureAtLocation(current);
                }

                List<Point> neighbors = GetNeighborCells(current);
                foreach (Point point in neighbors)
                {
                    if (visited.Contains(point)) { continue; }
                    if (DistanceTo(point.X, point.Y) > SensoryDistance) { continue; }
                    frontier.Enqueue(point);
                }


            }

            return null;
        }

        private double DistanceTo(int x, int y)                  // Zomies don t need eyes or ears !
        {
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }


        private List<Point> GetNeighborCells(Point current)
        {
            List<Point> returnList = new List<Point>();
            if ( current.X - 1 >= 0 ) { returnList.Add(new Point(current.X - 1, current.Y)); }
            if ( current.X + 1 <= owner.manager.worldManager.WorldWidth ) { returnList.Add(new Point(current.X + 1, current.Y)); }
            if ( current.Y - 1 >= 0 ) { returnList.Add(new Point(current.X, current.Y - 1)); }
            if ( current.Y + 1 <= owner.manager.worldManager.WorldHeight) { returnList.Add(new Point(current.X, current.Y + 1)); }

            return returnList;
        }

        internal override void Recieve(CPMessage message)
        {

        }
    }
}
