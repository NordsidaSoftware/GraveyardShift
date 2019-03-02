using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    [Serializable]
    class PlayerController : CreatureController
    {
        [NonSerialized]
        Virtual_root_Console root;

        Player player;
        public PlayerController(Creature owner, Virtual_root_Console root) : base(owner)
        {
            this.root = root;
            player = (Player)Owner;
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

        internal override void Initialize()
        {
            Owner.Name = "This is you";
            Owner.IsActive = true;
            Owner.glyph = (int)Glyph.FACE1;
            Owner.color = VAColor.Yellow;
            Owner.Faction = Faction.NEUTRAL;
            Owner.X_pos = 25;
            Owner.Y_pos = 25;
            Owner.Speed = 1;
        }

        internal override void Update()
        {
            if (root.input.isKeyPressed(Keys.Left)) { player.Move(-1, 0); }
            if (root.input.isKeyPressed(Keys.Right)) {  player.Move(1, 0); }
            if (root.input.isKeyPressed(Keys.Up)) {  player.Move(0, -1); }
            if (root.input.isKeyPressed(Keys.Down)) { player.Move(0, 1); }
        }
    }
}
