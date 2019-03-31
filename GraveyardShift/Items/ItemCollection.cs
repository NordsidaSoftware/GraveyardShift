using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public class ItemCollection
    {
        private Dictionary<Point, RegionItems> ItemsInRegion;

        public ItemCollection() { CreateRegionItemsList(); }

        internal List<Item> GetItemCollectionInRegion(Point region)
        {
            return ItemsInRegion[region].items;
        }

        internal void AddItem(Item i, Point region)
        {
            ItemsInRegion[region].items.Add(i);
        }

        private void CreateRegionItemsList()
        {
            ItemsInRegion = new Dictionary<Point, RegionItems>();

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    ItemsInRegion.Add(new Point(x, y), new RegionItems());
                }
            }
        }
    }

    internal class RegionItems
    {
        public List<Item> items;

        public RegionItems() { items = new List<Item>(); }
    }
}