using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public class ItemFactory
    {
        Random rnd;
        public ItemFactory() { rnd = new Random(); }

        internal Item CreateItem(string Wanted_mateial, params string[] Wanted_tags)
        {
            List<int> availableItems = new List<int>();
            foreach (string tag in Wanted_tags)
            {
                Tag t = GetTag(tag);
                if (t != Tag.NONE)
                {
                    foreach (KeyValuePair<int, ItemForm> kvp in StaticItemsBank.ItemFormByID)
                    {

                        if (kvp.Value.Tags.Contains(t)) { availableItems.Add(kvp.Key); }

                    }
                }
            }
            Item item = LoadValuesIntoItemFromForm(StaticItemsBank.ItemFormByID[availableItems[rnd.Next(availableItems.Count)]]);
            item.Elements.Add(StaticItemsBank.ItemFormByName[Wanted_mateial]);
            return item;
        }

       
            private Tag GetTag(string tagString)
            {
                foreach (Tag t in Enum.GetValues(typeof(Tag)))
                {
                    if (t.ToString() == tagString.ToUpper()) { return t; }
                }

                return Tag.NONE;
            }
        
        private Item LoadValuesIntoItemFromForm(ItemForm itemForm)
        {
            Item item = new Item();
            item.Name = itemForm.Name;

            foreach (Tag tag in itemForm.Tags) {
                switch (tag)
            {
                case Tag.WEARABLE: { CreateWerable(); break; }
                case Tag.WEAPON: { CreateWeapon(); break; }
                }
            }

            return item;

            void CreateWerable()
            {
                item.Components.Add(new WearableComponent(item, itemForm.Slot));
               
            }

            void CreateWeapon()
            {
                item.Components.Add(new WeaponComponent(item, itemForm.Grip, itemForm.Attack, itemForm.Defence));
                
            }
        }

    }
}