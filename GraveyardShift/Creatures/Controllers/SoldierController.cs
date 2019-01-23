using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    internal class SoldierController : CreatureController,IGoap
    {
        public List<GOAP_action> Actions;
        public Dictionary<string, object> Goals;

        public SoldierController(Creature owner) : base(owner)
        {
            Actions = new List<GOAP_action>();
            Goals = new Dictionary<string, object>();
            // TODO : move action field to creature class ( where the getWorldStates method resides...Make all creatures GOAP compatible
        }

        internal override void Initialize()
        {
            Owner.glyph = (int)Glyph.FACE2;

            Owner.Speed = 30;
            Owner.IsActive = true;
            Owner.Faction = Faction.GOOD;

           
            Owner.components.Add(new FSM(Owner));

            Owner.components.Add(new MoveComponent(Owner));
            Owner.components.Add(new EffectComponent(Owner));

            Actions.Add(new GOAP_action_PATROL());
            Goals.Add("patrol", true);
        }

        internal override void CreateBody()
        {
            BodyPart face = new BodyPart() { name = "Face", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Face", face);

            BodyPart neck = new BodyPart() { name = "Neck", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Neck", neck);

            BodyPart heart = new BodyPart() { name = "Heart", maxHitPoints = 2, hitPoints = 2, vital = true };
            Owner.body.bodyparts.Add("Heart", heart);

            Attack punch = new Attack() { attack_damage = 2, name = "Punch", effect = EffectTypes.NONE };
            BodyPart left_arm = new BodyPart() { name = "Left arm", maxHitPoints = 2, hitPoints = 2, attack = punch };
            Owner.body.bodyparts.Add("Left arm", left_arm);

            BodyPart right_arm = new BodyPart() { name = "Right arm", maxHitPoints = 2, hitPoints = 2, attack = punch };
            Owner.body.bodyparts.Add("Right arm", right_arm);

            Attack kick = new Attack() { attack_damage = 3, name = "Kick", effect = EffectTypes.NONE };
            BodyPart left_foot = new BodyPart() { name = "Left foot", maxHitPoints = 2, hitPoints = 2, attack = kick, mobility = true };
            Owner.body.bodyparts.Add("Left foot", left_foot);

            BodyPart right_foot = new BodyPart() { name = "Right foot", maxHitPoints = 2, hitPoints = 2, attack = kick, mobility = true };
            Owner.body.bodyparts.Add("Right foot", right_foot);
        }

        internal override void Update()
        {
            // Spesific Soldier update processing
        }

        public override string ToString()
        {
            return "Soldier";
        }

        public Dictionary<string, object> GetWorldState()
        {
            Dictionary<string, object> myCollectedWorldStates = Owner.GetWorldStates();
            // here I can add CreatureController worldStates

            return myCollectedWorldStates;
        }

        public Dictionary<string, object> GetGoalsState()
        {
            return Goals;
        }

        public List<GOAP_action> GetActions()
        {
            return Actions;
        }
    }
}