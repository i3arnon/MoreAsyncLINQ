using System;
using System.Collections.Generic;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> PartialSort<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.PartialSort(count, comparer: null);
        }

        public static IAsyncEnumerable<TSource> PartialSort<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.PartialSort(count, comparer: null, direction);
        }

        public static IAsyncEnumerable<TSource> PartialSort<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            IComparer<TSource>? comparer,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.PartialSort(count, Comparers.Get(comparer, direction));
        }

        public static IAsyncEnumerable<TSource> PartialSort<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            IComparer<TSource>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.PartialSortBy<TSource, TSource>(
                count,
                keySelector: null,
                keyComparer: null,
                comparer);
        }
    }
}