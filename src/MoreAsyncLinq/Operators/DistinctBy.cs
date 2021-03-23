using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.DistinctBy(keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return Core(source, keySelector, comparer);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey>? comparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var set = new HashSet<TKey>(comparer);
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var key = keySelector(element);
                    if (set.Add(key))
                    {
                        yield return element;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> DistinctByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.DistinctBy(keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return Core(source, keySelector, comparer);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, ValueTask<TKey>> keySelector,
                IEqualityComparer<TKey>? comparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var set = new HashSet<TKey>(comparer);
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var key = await keySelector(element).ConfigureAwait(false);
                    if (set.Add(key))
                    {
                        yield return element;
                    }
                }
            }
        }
    }
}