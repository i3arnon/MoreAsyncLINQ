using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<(int Index, TSource Element)> IndexBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.IndexBy(keySelector, comparer: null);
        }

        public static IAsyncEnumerable<(int Index, TSource Element)> IndexBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.
                ScanBy<TSource, TKey, (int index, TSource element)>(
                    keySelector,
                    _ => (-1, default!),
                    (state, _, element) => (state.index + 1, element),
                    comparer).
                Select(tuple => (tuple.State.index, tuple.State.element));
        }

        public static IAsyncEnumerable<(int Index, TSource Element)> IndexByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.IndexByAwait(keySelector, comparer: null);
        }

        public static IAsyncEnumerable<(int Index, TSource Element)> IndexByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            
            return source.
                ScanByAwait<TSource, TKey, (int index, TSource element)>(
                    keySelector,
                    _ => ValueTasks.FromResult((-1, default(TSource)!)),
                    (state, _, element) => ValueTasks.FromResult((state.index + 1, element)),
                    comparer).
                Select(tuple => (tuple.State.index, tuple.State.element));
        }
    }
}