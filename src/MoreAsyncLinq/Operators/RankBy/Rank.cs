using System;
using System.Collections.Generic;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<int> Rank<TSource>(this IAsyncEnumerable<TSource> source)
            where TSource : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Rank(comparer: null);
        }

        public static IAsyncEnumerable<int> Rank<TSource>(
            this IAsyncEnumerable<TSource> source,
            IComparer<TSource>? comparer)
            where TSource : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.RankBy(element => element, comparer);
        }
    }
}