using VAC;

namespace GraveyardShift
{
    public class VicarController : CreatureController
    {
        public VicarController(Creature owner) : base(owner)
        {
        }

        internal override void Initialize()
        {
            Owner.glyph = (int)Glyph.FACE1;
            Owner.X_pos = rnd.Next(0, 50);
            Owner.Y_pos = rnd.Next(0, 50);
            Owner.Speed = 10;
            Owner.IsActive = true;
            Owner.Faction = Faction.GOOD;

            Owner.components.Add(new MoveComponent(Owner));
            Owner.components.Add(new ScoutMonsterAI(Owner));
            Owner.components.Add(new AttackComponent(Owner));
            Owner.components.Add(new EffectComponent(Owner));
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
            // Spesific Vicar update processing
        }

        public override string ToString()
        {
            return "Vicar";
        }
    }

}


