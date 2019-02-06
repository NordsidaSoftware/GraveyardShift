using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    public class Randomizer
    {
        static Random rnd;

        public Randomizer(int seed) { rnd = new Random(seed); }
      
        public static Random GetRandomizer() { return rnd; }

    }
}
