using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAC;

namespace GraveyardShift
{
    public static class ColorChart
    {
        public static Dictionary<byte, VAColor> IntToColor = new Dictionary<byte, VAColor>()
        {
            {0, new VAColor(6, 24, 86) },
            {1, new VAColor(11, 35, 114) },
            {2, new VAColor(18, 47, 139) },
            {3, new VAColor(39, 68, 165) },
            {4, new VAColor(70, 94, 173) },

            {5, new VAColor(126, 126, 59) },
            {6, new VAColor(165, 165, 95) },
            {7, new VAColor(197, 197, 136) },
            {8, new VAColor(233, 233, 188) },
            {9, new VAColor(100, 100, 91) },

            {10, new VAColor(11, 47, 2) },
            {11, new VAColor(28, 76, 15) },
            {12, new VAColor(49, 103, 34) },
            {13, new VAColor(73, 127, 59) },
            {14, new VAColor(105, 159, 91) },

            {15, new VAColor(92, 107, 114) }
        };


    }
}
