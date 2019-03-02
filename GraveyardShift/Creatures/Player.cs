using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    [Serializable]
    public class Player : Creature
    {
        
        public Player(CreatureManager manager, Virtual_root_Console root) : base(manager)
        {
            controller = new PlayerController(this, root);

            controller.Initialize();
            controller.CreateBody();
           
        }

        internal override void Distribute(CPMessage cpMessage)
        {
            base.Distribute(cpMessage);
        }

        public void MoveTo(int x, int y)
        {
            // vector from this object to the target, and distance
            int dx = x - X_pos;
            int dy = y - Y_pos;
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
            int dx = x - X_pos;
            int dy = y - Y_pos;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }


        //*****************************************************************************************
        //|                player move method                                                     |
        //|                                                                                       |
        //*****************************************************************************************
        public void Move(int dx, int dy)
        {
            if (!manager.worldManager.LocationIsBlocked(X_pos + dx, Y_pos + dy))  // movement is not blocked
            {
                if (!manager.LocationIsOccupied(X_pos + dx, Y_pos + dy))          // tile is not occupied
                {

                    if (manager.worldManager.IsOnMap(X_pos + dx, Y_pos + dy))     // movement is on map

                    {
                        //if ( manager.worldManager.IsInsideCenterArea(X_pos, Y_pos))  // fix this !!
                            manager.worldManager.SetCameraAt((X_pos + dx)-manager.worldManager.ScreenWidth/2,
                                                              (Y_pos + dy) - manager.worldManager.ScreenHeight/2); // camera follows player

                        X_pos += dx;
                        Y_pos += dy;

                    }
                    else
                    {                                              // movement is off map
                        manager.worldManager.EnterNewRegion(dx, dy);
                        if ( dx > 0 ) { X_pos =1; }
                        if ( dx < 0 ) { X_pos = manager.worldManager.MapWidth-2; }
                        if ( dy > 0 ) { Y_pos = 1; }
                        if ( dy < 0 ) { Y_pos = manager.worldManager.MapHeight-2; }

                        manager.worldManager.SetCameraAt(X_pos - manager.worldManager.ScreenWidth / 2,
                                                         Y_pos - manager.worldManager.ScreenHeight / 2);

                        manager.MoveToNewRegion(this, dx, dy);
                        manager.SetRegion(dx, dy);
                        manager.ResetUpdateLoop = true;
                        

                    }         

                        manager.worldManager.fov_Map.CalculateFOV(manager.worldManager.Region_Features, manager.worldManager.Region_Heightmap, X_pos, Y_pos);
                }
            }
             
        }

        internal override void Update()
        {
            
            base.Update();
        }
    }
}
