using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Tools
{
    public class Pair<T, U>
    {
        public Pair(U second, T first)
        {
            this.second = second;
            this.first = first;
        }

        public U second { get; set; }
        public T first { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Pair<T, U> pair &&
                   EqualityComparer<U>.Default.Equals(second, pair.second) &&
                   EqualityComparer<T>.Default.Equals(first, pair.first);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(second, first);
        }
    }
}
