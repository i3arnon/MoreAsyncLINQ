using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.ThenBy(keySelector, comparer: null, direction);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            comparer ??= Comparer<TKey>.Default;
            return direction == Ascending
                ? source.ThenBy(keySelector, comparer)
                : source.ThenByDescending(keySelector, comparer);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(
            this IOrderedAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.ThenByAwait(keySelector, comparer: null, direction);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(
            this IOrderedAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            comparer ??= Comparer<TKey>.Default;
            return direction == Ascending
                ? source.ThenByAwait(keySelector, comparer)
                : source.ThenByDescendingAwait(keySelector, comparer);
        }
    }
}