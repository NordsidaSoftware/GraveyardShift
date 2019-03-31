using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public static class StaticItemsBank
    {
        internal static Dictionary<int, ItemForm> ItemFormByID = new Dictionary<int, ItemForm>();
        internal static Dictionary<string, ItemForm> ItemFormByName = new Dictionary<string, ItemForm>();

        private static int ID;

        public static int NextID { get { return ID++; } }

        internal static void LoadParsedItems(Dictionary<int, ItemForm> parsedItems)
        {
            ItemFormByID = parsedItems;

            foreach ( KeyValuePair<int, ItemForm> kvp in parsedItems)
            {
                ItemFormByName[kvp.Value.Name] = kvp.Value;
            }
        }

        internal static void PrintAllItems()
        {
            for ( int index = 0; index < ItemFormByID.Count; index++ )
            {
                PrintItem(ItemFormByID[index]);
            }
        }

        private static void PrintItem(ItemForm itemForm)
        {
            Console.WriteLine("=== " + itemForm.Name + " ===");
            Console.WriteLine("Slot : " + itemForm.Slot.ToString().PadLeft(3));
            foreach (Tag t in itemForm.Tags ) { Console.Write(t.ToString() + ", "); }
            Console.WriteLine("");

        }
    }
}