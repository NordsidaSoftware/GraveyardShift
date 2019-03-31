using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    /*
    public interface IUsable
    {
        void Use(Creature user);
    }
    

        public interface IUpdatable { }

    public class Material
    {
        public List<string> tags;
    }

    public abstract class Item
    {
        public List<string> tags;  // list of enums id (byte ? )
        public List<Item> contains;
        public List<Material> materials; // list of enums id (byte? )

        public Glyph glyph;
        public VAColor color;
        public Point Position { get; internal set; }

        public Item() { tags = new List<string>(); contains = new List<Item>(); materials = new List<Material>(); }
    }

    public class Building : Item    // Multi tile Items !
    {
        public Dictionary<Point, DB.Features> Elements;
        public Building(Point position):base()
        {
            glyph = Glyph.SPACE1; color = VAColor.Black; Position = position; Elements = new Dictionary<Point, DB.Features>();
            tags.Add("building");   // use a enum of TOKENS ?
            materials.Add(new Material());
        }

        //public void AddElemet(List<Point> points)
    }

    public class Bed : Item
    {
        public Bed(Point position):base()
        {
            glyph = Glyph.AMPERSAND; color = VAColor.White; Position = position;
            tags.Add("bed");
        }
    }

    /*
    public class Bed : Item, IUsable
    {
        public Bed(Point position) { glyph = Glyph.LINE; color = VAColor.Red; Position = position; tags = new List<string>(); }

        public void Use(Creature user)
        {
            user.body.isRested = true;
        }

        public override string ToString()
        {
            return "Bed";
        }
    }
    */
}
