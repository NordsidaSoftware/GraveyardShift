using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraveyardShift
{
    internal class ItemFileParser
    {
        string[] source;
        int index;
        string currentString;

        public bool EOF { get { return index >= source.Length; } }
        public ItemFileParser()
        {
            source = File.ReadAllLines("Items.txt");
        }
        internal Dictionary<int, ItemForm> ParseTextFile()
        {
            Dictionary<int, ItemForm> returnDict = new Dictionary<int, ItemForm>();
            while (!EOF )
            {
                GetNextString();
                if ( currentString.StartsWith("ITEM")) { returnDict.Add(StaticItemsBank.NextID, RecordItem()); }
                Advance();

            }
            return returnDict;
        }

        private ItemForm RecordItem()
        {
            ItemForm item = new ItemForm();
            while (currentString.Length > 0 && !EOF)
            {
                string[] parts = currentString.Split(':');
                switch (parts[0].Trim())
                {
                   
                    case "TAG": { item.Tags = RecordTags (parts[1].Trim()); break; }
                    case "NAME": { item.Name = parts[1].Trim(); break; }
                    case "SLOT": { item.Slot = GetSlot(parts[1].Trim());break; }
                    case "GRIP": { item.Grip = ParseNumber(parts[1].Trim()); break; }
                    case "ATTACK": { item.Attack = ParseNumber(parts[1].Trim()); break; }
                    case "DEFENCE": { item.Defence = ParseNumber( parts[1].Trim()); break; }
                    case "EDGE": { item.Edge = ParseNumber(parts[1].Trim()); break; }
                   
                }

                Advance();
                GetNextString();
            }

            return item;
        }

        private byte ParseNumber(string parseString)
        {
            int value;
            if (int.TryParse(parseString, out value)) { return (byte)value; }
            else return 0;
        }

       

        private Slot GetSlot(string slotString)
        {
            foreach (Slot s in Enum.GetValues(typeof(Slot)))
            {
                if (s.ToString() == slotString.ToUpper()) { return s; }
            }

            return Slot.NONE;
        }

        private Tag GetTag(string tagString)
        {
            foreach (Tag t in Enum.GetValues(typeof(Tag)))
            {
                if (t.ToString() == tagString.Trim().ToUpper()) { return t; }
            }

            return Tag.NONE;
        }

        private HashSet<Tag> RecordTags(string tagsString)
        {
            HashSet<Tag> returnHash = new HashSet<Tag>();
            string[] tags = tagsString.Split(',');
            foreach (string t in tags )
            {
                Tag thisTag = GetTag(t);
                if ( thisTag != Tag.NONE ) { returnHash.Add(thisTag); }
            }

            return returnHash;
        }

        void Advance() { index++; }
        void GetNextString() {  if (!EOF) { currentString =  source[index]; } }
    }

    public class ItemForm
    {
        public HashSet<Tag> Tags { get; internal set; }
        public string Name { get; internal set; }
        public Slot Slot { get; internal set; }
        public byte ItemType { get; internal set; }
        public byte Grip { get; internal set; }
        public byte Attack { get; internal set; }
        public byte Defence { get; internal set; }
        public byte Edge { get; internal set; }
      
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + " ");
            foreach ( Tag t in Tags )
            {
                sb.Append(" " + t.ToString());
            }
            return sb.ToString();
        }
    }
}