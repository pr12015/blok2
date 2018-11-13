using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer
{
    /// <summary>
    /// Helper class to replace Tuple<T1, T2> since Item1 and Item2
    /// are immutable.
    /// First <=> Item1
    /// Second <=> Item2
    /// </summary>
    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }    
    }
}
