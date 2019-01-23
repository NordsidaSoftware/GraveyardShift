using System;

namespace GraveyardShift
{
    //=====================================================================

    public class MoveComponent : ComponentsParts
    {
        Random rnd;
        int Position_Target_X;
        int Position_Target_Y;
      
        
        public MoveComponent(Creature owner) : base(owner)
        {
            rnd = new Random();
        }

        internal override void Update()
        { 
          //  if ( ! owner.body.CanMove) { isActive = false; }

            if (owner.X_pos != Position_Target_X || owner.Y_pos != Position_Target_Y)
            { MoveTo(Position_Target_X, Position_Target_Y); }
        }


        internal void MoveTo(int x, int y)
        {
            // vector from this object to the target, and distance
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            if (distance <= 0) { return; }

            // normalize it to length 1 (preserving direction), then round it and
            // convert to integer so the movement is restricted to the map grid
            dx = (int)(Math.Round(dx / distance));
            dy = (int)(Math.Round(dy / distance));
           
            Move(dx, dy);
        }

        private double DistanceTo(int x, int y)
        {
            int dx = x - owner.X_pos;
            int dy = y - owner.Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        private void Move(int dx, int dy)
        {
            if ( owner.manager.worldManager.IsOnCurrentGrid(owner.X_pos + dx, owner.Y_pos + dy))
            {
                if (!owner.manager.worldManager.LocationIsBlocked(owner.X_pos + dx, owner.Y_pos + dy))
                {
                    if (!owner.manager.LocationIsOccupied(owner.X_pos + dx, owner.Y_pos + dy))
                    {
                        owner.X_pos += dx;
                        owner.Y_pos += dy;
                    }
                }
            }
        }

       
        internal override void Recieve(CPMessage message)
        {
            if (message.type == CPMessageType.TARGET)
            {
                Position_Target_X = message.x_position;
                Position_Target_Y = message.y_position;
            }
        }

       
    }
}