using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    public enum Faction { EVIL, GOOD }

    public class Body
    {
        public Dictionary<string, BodyPart> bodyparts;

        public bool IsAlive
        {
            get
            {
                bool returnvalue = false;
                foreach ( BodyPart BP in bodyparts.Values) { if (BP.vital && (int)BP.status < 4) returnvalue = true; }
                return returnvalue;
            }
          
        }
        public bool CanMove
        {
            get
            {
                int nr_of_mobilityParts = 0;

                foreach ( BodyPart BP in bodyparts.Values) { if (BP.mobility) nr_of_mobilityParts++; }

                if (nr_of_mobilityParts > 1) { return true; }
                else { return false; }
            }
        }

        public Body()
        {
            bodyparts = new Dictionary<string, BodyPart>();
        }

        public Dictionary<string, Attack> GetAllAvailableAttacks()
        {
            Dictionary<string, Attack> returnDict = new Dictionary<string, Attack>();
            foreach ( BodyPart BP in bodyparts.Values)
            {
                if (BP.attack != null) { returnDict.Add(BP.name, BP.attack); }
            }

            return returnDict;
        }
       
    }

    public enum BodyPartStatus { OK = 0, BRUISED = 1, CUT_SURFACE = 2, CUT_DEEP = 3, MAIMED = 4, DESTROYED = 5, LOST = 6}

    public class BodyPart
    {
        public string name;
        public int hitPoints;
        public int maxHitPoints;
        public bool mobility;
        public bool vital;
        public BodyPartStatus status;
        public Attack attack;

        public override string ToString()
        {
            return name + " HP:" + hitPoints.ToString() + "/"+maxHitPoints.ToString() + "(" + status.ToString() + ") " + attack?.ToString();
        }
    }


    public class Creature
    {
        public string Name { get; set; }
        public int Speed { get; internal set; }
        public int X_pos { get; internal set; }
        public int Y_pos { get; internal set; }
        public int glyph { get; internal set; }
        public bool IsActive { get; internal set; }
        public Faction Faction { get; set; }

        public List<Effect> effects;

        //public int target_X;
        //public int target_Y;

        public List<ComponentsParts> components { get; }
        public Body body { get; internal set; }
        public CreatureController controller { get; internal set; }

        public CreatureManager manager;

        private int energy = 0;

        public Creature(CreatureManager manager)
        {
            this.manager = manager;
            body = new Body();
            components = new List<ComponentsParts>();
            effects = new List<Effect>();

        }

      

        internal virtual void Update()
        {
            energy++;
            if ( energy > Speed ) { energy -= Speed; }
            else { return; }

            controller.Update();
            foreach (ComponentsParts CP in components)
            {
                if (CP.isActive)
                {
                    CP.Update();
                }
            }
        }

        internal virtual void Distribute(CPMessage cpMessage)
        {
            foreach ( ComponentsParts CP in components )
            {
                CP.Recieve(cpMessage);
            }
        }
    }
}