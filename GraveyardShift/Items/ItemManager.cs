using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class ItemManager
    {
        private WorldManager world;
        public ItemCollection worldItems;
        public List<Item> RegionItems;
        public Point CurrentRegion;

        public ItemFactory itemFactory;

        public ItemManager( WorldManager world )
        {
            ItemFileParser itemFileParser = new ItemFileParser();
            StaticItemsBank.LoadParsedItems(itemFileParser.ParseTextFile());
            itemFactory = new ItemFactory();
     
            this.world = world;
            worldItems = new ItemCollection();
            SetRegion(0, 0);
        }

        public void SetRegion(int dx, int dy)
        {
            CurrentRegion += new Point(dx, dy);

            RegionItems = worldItems.GetItemCollectionInRegion(CurrentRegion);
        }

        public void AddItem(Item i, Point region)
        {
            worldItems.AddItem(i, region);
        }

        internal Item GetItemAtLocation(Point current)
        {
            Item returnItem = null;
           // foreach (Item i in RegionItems)
           // {
           //     if (i.Position.X == current.X && i.Position.Y == current.Y) { returnItem = i; }
           // }
            return returnItem;
        }

        internal void Update()
        {
            for (int index = RegionItems.Count - 1; index >= 0; index--)
            {
               
               // if (RegionItems[index] is IUpdatable ) { }
            }
        }

       

        internal List<Item> GetRegionItems(int regionX, int regionY)
        {
            return worldItems.GetItemCollectionInRegion(new Point(regionX, regionY));
        }
        /*





internal void Draw(VirtualConsole map)
{
foreach (Creature c in RegionCreatures)
{
  if (worldManager.IsOnCurrentScreen(c.X_pos - worldManager.Camera.X, c.Y_pos - worldManager.Camera.Y))
  {
      map.PutGlyph(c.glyph,
      c.X_pos - worldManager.Camera.X,
      c.Y_pos - worldManager.Camera.Y,
      c.color);

      /*                                Mirror reflection draw. Used over water ?
      map.PutGlyph(c.glyph,
      c.X_pos - worldManager.Camera.X+1,
      c.Y_pos - worldManager.Camera.Y+1,
      VAColor.DarkGray);

  }
}
}
}
*/

    }
}
