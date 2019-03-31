using System;
using System.Collections.Generic;
using System.Text;

namespace GraveyardShift
{
    public enum Slot {  NONE, HEAD, NECK, TORSO, LEFT_ARM, RIGHT_ARM, PELVIS, RIGHT_FOOT, LEFT_FOOT }
    public enum Tag { NONE, WEAPON, WEARABLE, EATABLE }

    public class Item
    {
        public string Name { get; set; }
        public List<ItemForm> Elements = new List<ItemForm>();
        public List<ItemComponents> Components = new List<ItemComponents>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("Name : " + Name);
            sb.Append(" <");
            foreach (ItemForm element in Elements)
            {
                sb.Append( element.Name + " ");
            }
            sb.Append(" >");
            sb.AppendLine();

            foreach (ItemComponents itemComponent in Components )
            {
                sb.Append(itemComponent.ToString());
            }

            return sb.ToString();
        }
    }

    public abstract class ItemComponents {
        public Item owner;
        public ItemComponents(Item owner ) { this.owner = owner; }
    }

    public class WeaponComponent : ItemComponents
    {
        byte Grip;
        private byte Attack;
        byte Defence;

        public byte GetAttack
        {
            get
            {
                if (owner.Elements.Count > 0)
                {
                    return (byte)(Attack * owner.Elements[0].Edge);

                }
                return Attack;
            }
        }
        public WeaponComponent(Item owner, byte grip = 1, byte Attack = 0, byte Defence = 0) : base(owner)
        {
            Grip = grip;
            this.Attack = Attack;
            this.Defence = Defence;
        }

        public override string ToString()
        {
            return "Attack : " + GetAttack + " Defence : " + Defence.ToString();
        }
    }

    public class WearableComponent:ItemComponents
    {
        public Slot Slot;
        public bool Equipped;

        public WearableComponent(Item owner, Slot Slot = Slot.NONE) : base(owner)
        {
            this.Slot = Slot;
        }

        public override string ToString()
        {
            return "Slot : " + Slot.ToString() + " Eq: " + Equipped.ToString();
        }
    }


}