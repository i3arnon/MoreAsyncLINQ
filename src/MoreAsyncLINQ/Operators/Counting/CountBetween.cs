using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Determines whether or not the number of elements in the sequence is between
        /// an inclusive range of minimum and maximum integers.
        /// </summary>
        /// <typeparam name="TSource">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="min">The minimum number of items a sequence must have for this
        /// function to return true</param>
        /// <param name="max">The maximum number of items a sequence must have for this
        /// function to return true</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is negative or <paramref name="max"/> is less than min</exception>
        /// <returns><c>true</c> if the number of elements in the sequence is between (inclusive)
        /// the min and max given integers or <c>false</c> otherwise.</returns>
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