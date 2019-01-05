namespace GraveyardShift
{
    public class ZombieController : CreatureController
    {
        public ZombieController(Creature owner) : base(owner)
        {
            
        }

        internal override void Initialize()
        {
            Owner.glyph = 'Z';
            
            Owner.Speed = 10;
            Owner.IsActive = true;
            Owner.Faction = Faction.EVIL;

            Owner.components.Add(new MoveComponent(Owner));
            Owner.components.Add(new ZombieAI(Owner));
            Owner.components.Add(new AttackComponent(Owner));
            Owner.components.Add(new EffectComponent(Owner));
        }

        internal override void CreateBody()
        {
            Attack bite = new Attack() { name = "Bite", attack_damage = 5, effect = EffectTypes.ZombieEffect };
            BodyPart mouth = new BodyPart() { name = "Mouth", attack = bite, hitPoints = 2, maxHitPoints = 2 };
            Owner.body.bodyparts.Add("Mouth", mouth);

            BodyPart face = new BodyPart() { name = "Face", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Face", face);

            BodyPart brain = new BodyPart() { name = "Brain", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Brain", brain);


            BodyPart neck = new BodyPart() { name = "Neck", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Neck", neck);


            BodyPart left_arm = new BodyPart() { name = "Left arm", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Left arm", left_arm);

            BodyPart right_arm = new BodyPart() { name = "Right arm", maxHitPoints = 2, hitPoints = 2 };
            Owner.body.bodyparts.Add("Right arm", right_arm);


            BodyPart left_foot = new BodyPart() { name = "Left foot", maxHitPoints = 2, hitPoints = 2, mobility = true };
            Owner.body.bodyparts.Add("Left foot", left_foot);

            BodyPart right_foot = new BodyPart() { name = "Right foot", maxHitPoints = 2, hitPoints = 2, mobility = true };
            Owner.body.bodyparts.Add("Right foot", right_foot);
        }

        internal override void Update()
        {
            if ( Owner.body.bodyparts.Count < 2 ) { Owner.IsActive = false; }
        }
        public override string ToString()
        {
            return "Zombie";
        }


    }

}


