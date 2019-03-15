using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    public interface IUsable
    {
        void Use(Creature user);
    }

    public abstract class Item
    {
        public List<string> tags;
        public Glyph glyph;
        public VAColor color;

        public Point Position { get; internal set; }
    }

    public class Building : Item
    {
        public Building(Point position)
        { glyph = Glyph.SPACE1; color = VAColor.Black; Position = position; tags = new List<string>(); }
    }

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
}
