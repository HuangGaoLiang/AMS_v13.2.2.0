using System;
using System.Collections.Generic;

namespace AMS.Core
{
    public static class IEnumerableExt
    {
        public static IEnumerable<TSource> DistinctBys<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> hashSet = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (hashSet.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
