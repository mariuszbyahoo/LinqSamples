using System;
using System.Collections.Generic;
using System.Text;

namespace Queries
{
    public static class MyLinq
    {

        public static IEnumerable<double> Random()
        {
            var random = new Random();
            while(true)
            {
                yield return random.NextDouble();
            }
        }

        public static IEnumerable<T> Filter<T> (this IEnumerable<T> source,
                                                Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item; // Short desc on yield keyword:
                    // It will help me to build an IEnumerable
                    // In other words: every time I use an yield return syntax, 
                    // my return type has to be an IEnumerable or IEnumerable<T>
                }
            }
        }
    }
}
