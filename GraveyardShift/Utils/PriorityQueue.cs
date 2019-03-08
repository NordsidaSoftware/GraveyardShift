using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraveyardShift
{
    // From Amit Patel 
    // RedBlobGames

    public class PriorityQueue<T>
    {
        private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

        public int Count { get { return elements.Count; } }

        public void Enqueue(T item, double value ) { elements.Add(new Tuple<T, double>(item, value)); }

        public T Dequeue()
        {
            int bestIndex = 0;
            for ( int index = 0; index < elements.Count; index++ )
            {
                if ( elements[index].Item2 < elements[bestIndex].Item2 )
                {
                    bestIndex = index;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);

            return bestItem;
        }
    }
}