using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
            this IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToDictionaryAsync(comparer: null, cancellationToken);
        }

        public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
            this IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
            IEqualityComparer<TKey>? comparer,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToDictionaryAsync(
                pair => pair.Key,
                pair => pair.Value,
                comparer,
                cancellationToken);
        }

        public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
            this IAsyncEnumerable<(TKey Key, TValue Value)> source,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToDictionaryAsync(comparer: null, cancellationToken);
        }

        public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
            this IAsyncEnumerable<(TKey Key, TValue Value)> source,
            IEqualityComparer<TKey>? comparer,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToDictionaryAsync(
                tuple => tuple.Key,
                tuple => tuple.Value,
                comparer,
                cancellationToken);
        }
    }
}