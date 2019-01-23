namespace GraveyardShift
{
    public class Item
    {
        public string name;
        public Attack attack;
        internal int range;

        public Item(string name, Attack attack, int range)
        {
            this.name = name;
            this.attack = attack;
            this.range = range;
        }
    }
}