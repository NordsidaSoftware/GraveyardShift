using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    public static class DB
    {
        public enum Palette
        {
            Black = 0,
            White = 1,

            Darkest_Blue = 2, 
            Dark_Blue = 3, 
            Blue = 4,
            Light_Blue = 5,
            Hilite_Blue = 6,

            Darkest_Yellow = 7,
            Dark_Yellow = 8,
            Yellow = 9,
            Light_Yellow = 10,
            Hilite_Yellow = 11,

            Darkest_Green = 12,
            Dark_Green = 13,
            Green = 14,
            Light_Green = 15,
            Hilite_Green = 16,

            Darkest_Red = 17,
            Dark_Red = 18,
            Red = 19,
            Light_Red = 20,
            Hilite_Red = 21,

            Darkest_Gray = 22,
            Dark_Gray = 23,
            Gray = 24, 
            Light_Gray = 25,
            Hilite_Gray = 26
        }

        public static Dictionary<byte, VAColor> ByteToColor = new Dictionary<byte, VAColor>()
        {
            {0, VAColor.Black },
            {1, VAColor.AntiqueWhite },

            {2, new VAColor(6, 24, 86) },  // Darkest blue
            {3, new VAColor(11, 35, 114) }, // dark blue
            {4, new VAColor(18, 47, 139) }, // blue
            {5, new VAColor(39, 68, 165) }, // light blue
            {6, new VAColor(70, 94, 173) }, // hilite blue

            {7, new VAColor(126, 126, 59) }, // Darkest yellow
            {8, new VAColor(165, 165, 95) }, // dark yellow
            {9, new VAColor(197, 197, 136) }, // yellow
            {10, new VAColor(233, 233, 188) }, // light yellow
            {11, new VAColor(100, 100, 91) }, // hilite yellow

            {12, new VAColor(11, 47, 2) }, // Darkest green
            {13, new VAColor(28, 76, 15) }, // Dark green
            {14, new VAColor(49, 103, 34) }, // green
            {15, new VAColor(73, 127, 59) }, // light green
            {16, new VAColor(105, 159, 91) }, // hilite green

            {17, new VAColor(142, 0, 0) }, // Darkest red
            {18, new VAColor(183, 2, 2) }, // Dark red
            {19, new VAColor(230, 12, 12) }, // red
            {20, new VAColor(247, 60, 60) }, // light red
            {21, new VAColor(255, 103, 103) }, // hilite red



            {22, new VAColor(49, 36, 36) },  //Darkest gray
            {23, new VAColor(95, 82, 82) },  // Dark gray
            {24, new VAColor(139, 124, 124) }, // gray
            {25, new VAColor(180, 165, 165) }, // light gray
            {26, new VAColor(227, 216,  216) } // hilite gray
        };


        public enum Features { SPACE = 0, TREE = 1 }

        public static Dictionary<int, Thing> IntToItem = new Dictionary<int, Thing>()
        {
            {0, new Thing() {glyph = (int)Glyph.SPACE1, blocked = false, blockSight = false } },
            {1, new Thing() {glyph = (int)Glyph.SUN, blocked = false, blockSight = true, fgColor = (int)Palette.Darkest_Green } }

        };
    }
}
