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

        public static Dictionary<byte, VAColor> HightToColor = new Dictionary<byte, VAColor>()
        {
            {0, new VAColor(11, 35, 114)},            // WATER AT LEVEL 0
            {1, VAColor.AntiqueWhite },               // WHITE SHORES
            {2, VAColor.AntiqueWhite},
            {3, VAColor.AntiqueWhite },
            {4,  new VAColor(73, 127, 59)},          // light green
            {5, new VAColor(73, 127, 59) },
            {6,  new VAColor(73, 127, 59)},
            {7,  new VAColor(73, 127, 59)},
            {8,  new VAColor(73, 127, 59)},
            {9,  new VAColor(73, 127, 59)},
            {10,  new VAColor(73, 127, 59)},
            {11,  new VAColor(73, 127, 59)},
            {12,  new VAColor(73, 127, 59)},
            {13,  new VAColor(73, 127, 59)},
            {14,  new VAColor(73, 127, 59)},
            {15,  new VAColor(73, 127, 59)},
            {16,  new VAColor(73, 127, 59)},
            {17,  new VAColor(73, 127, 59)},
            {18,  new VAColor(73, 127, 59)},
            {19,  new VAColor(73, 127, 59)},
            {20,  new VAColor(73, 127, 59)},
            {21, new VAColor(49, 103, 34)},                        // green
             {22, new VAColor(49, 103, 34)},
            { 23,new VAColor(49, 103, 34)},
            { 24,new VAColor(49, 103, 34)},
            { 25,new VAColor(49, 103, 34)},
            { 26,new VAColor(49, 103, 34)},
            { 27,new VAColor(49, 103, 34)},
            { 28,new VAColor(49, 103, 34)},
            { 29,new VAColor(49, 103, 34)},
            { 30,new VAColor(49, 103, 34)},
            { 31,new VAColor(28, 76, 15)},
            { 32,new VAColor(28, 76, 15)},
            { 33,new VAColor(28, 76, 15)},
            { 34,new VAColor(28, 76, 15)},
            { 35,new VAColor(28, 76, 15)},
            { 36,new VAColor(28, 76, 15)},
            { 37,new VAColor(28, 76, 15)},
            { 38,new VAColor(28, 76, 15)},
            { 39,new VAColor(28, 76, 15)},
            { 40,new VAColor(28, 76, 15)},
            { 41,new VAColor(28, 76, 15)},
             { 42,new VAColor(28, 76, 15)},
            { 43,new VAColor(28, 76, 15)},
            { 44,new VAColor(28, 76, 15)},
            { 45,new VAColor(28, 76, 15)},
            { 46,new VAColor(28, 76, 15)},
            { 47,new VAColor(28, 76, 15)},
            { 48,new VAColor(28, 76, 15)},
            { 49,new VAColor(28, 76, 15)},
            { 50,new VAColor(28, 76, 15)},
            { 51,new VAColor(28, 76, 15)},
            { 52,new VAColor(28, 76, 15)},
            { 53,new VAColor(28, 76, 15)},
            { 54,new VAColor(28, 76, 15)},
            { 55,new VAColor(28, 76, 15)},
            { 56,new VAColor(28, 76, 15)},
            { 57,new VAColor(28, 76, 15)},
            { 58,new VAColor(28, 76, 15)},
            { 59,new VAColor(28, 76, 15)},
            { 60,new VAColor(28, 76, 15)},
            { 61,new VAColor(11, 47, 2)},
             { 62,new VAColor(11, 47, 2)},
            { 63,new VAColor(11, 47, 2)},
            { 64,new VAColor(11, 47, 2)},
            { 65,new VAColor(11, 47, 2)},
            { 66,new VAColor(11, 47, 2)},
            { 67,new VAColor(11, 47, 2)},
            { 68,new VAColor(11, 47, 2)},
            { 69,new VAColor(11, 47, 2)},
            { 70,new VAColor(11, 47, 2)},
            { 71,new VAColor(11, 47, 2)},
            { 72,new VAColor(11, 47, 2)},
            { 73,new VAColor(11, 47, 2)},
            { 74,new VAColor(11, 47, 2)},
            { 75,new VAColor(11, 47, 2)},
            { 76,new VAColor(11, 47, 2)},
            { 77,new VAColor(11, 47, 2)},
            { 78,new VAColor(11, 47, 2)},
            { 79,new VAColor(11, 47, 2)},
            { 80,new VAColor(49, 36, 36)},
            { 81,new VAColor(49, 36, 36)},
             { 82,new VAColor(49, 36, 36)},
            { 83,new VAColor(49, 36, 36)},
            { 84,new VAColor(49, 36, 36)},
            { 85,new VAColor(95, 82, 82)},
            { 86,new VAColor(95, 82, 82)},
            { 87,new VAColor(95, 82, 82)},
            { 88,new VAColor(95, 82, 82)},
            { 89,new VAColor(95, 82, 82)},
            { 90,new VAColor(139, 124, 124)},
            { 91,new VAColor(139, 124, 124)},
            { 92,new VAColor(139, 124, 124)},
            { 93,new VAColor(139, 124, 124)},
            { 94,new VAColor(139, 124, 124)},
            { 95,new VAColor(180, 165, 165)},
            { 96,new VAColor(180, 165, 165)},
            { 97,new VAColor(180, 165, 165)},
            { 98,new VAColor(180, 165, 165)},
            { 99,new VAColor(227, 216,  216)},
            { 100,new VAColor(227, 216,  216)}
          
            /*
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
            */
        };


        public enum Features { SPACE = 0, TREE = 1 }

        public static Dictionary<int, Feature> IntToItem = new Dictionary<int, Feature>()
        {
            {0, new Feature() {glyph = (int)Glyph.SPACE1, blocked = false, blockSight = false } },
            {1, new Feature() {glyph = (int)Glyph.SUN, blocked = false, blockSight = true, fgColor = (int)Palette.Darkest_Green } }

        };
    }
}
