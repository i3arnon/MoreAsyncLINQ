using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<bool> CountBetweenAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int min,
            int max,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (min < 0) throw new ArgumentOutOfRangeException(nameof(min), $"{nameof(min)} must be non-negative");
            if (max < min) throw new ArgumentOutOfRangeException(nameof(max), $"{nameof(max)} must be greater than or equal to {nameof(min)}");

            return source.CountBetweenAsync(max + 1, min, max, cancellationToken);
        }

        private static async ValueTask<bool> CountBetweenAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int limit,
            int min,
            int max,
            CancellationToken cancellationToken = default)
        {
            var collectionCount =
                await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false) ??
                await source.CountAsync(limit, cancellationToken).ConfigureAwait(false);

            return collectionCount >= min &&
                   collectionCount <= max;
        }
    }
}