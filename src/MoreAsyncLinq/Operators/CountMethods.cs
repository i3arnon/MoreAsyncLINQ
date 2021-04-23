using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.Int32;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<bool> AtLeastAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");

            return source.CountBetweenAsync(count, count, MaxValue, cancellationToken);
        }

        public static ValueTask<bool> AtMostAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");

            return source.CountBetweenAsync(count + 1, min: 0, count, cancellationToken);
        }

        public static ValueTask<int> CompareCountAsync<TFirst, TSecond>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            CancellationToken cancellationToken = default)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));

            return Core(first, second, cancellationToken);

            static async ValueTask<int> Core(
                IAsyncEnumerable<TFirst> first,
                IAsyncEnumerable<TSecond> second,
                CancellationToken cancellationToken)
            {
                var firstCollectionCount = await first.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                var secondCollectionCount = await second.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);

                switch (firstCollectionCount, secondCollectionCount)
                {
                    case ({ } firstCount, { } secondCount):
                    {
                        return firstCount.CompareTo(secondCount);
                    }
                    case ({ } firstCount, null):
                    {
                        var secondCount = await second.CountAsync(firstCount + 1, cancellationToken).ConfigureAwait(false);
                        return firstCount.CompareTo(secondCount);
                    }
                    case (null, { } secondCount):
                    {
                        var firstCount = await first.CountAsync(secondCount + 1, cancellationToken).ConfigureAwait(false);
                        return firstCount.CompareTo(secondCount);
                    }
                    case (null, null):
                    {
                        await using var firstEnumerator = first.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                        await using var secondEnumerator = second.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                        bool firstHasNext;
                        bool secondHasNext;
                        do
                        {
                            firstHasNext = await firstEnumerator.MoveNextAsync();
                            secondHasNext = await secondEnumerator.MoveNextAsync();
                        } while (firstHasNext && secondHasNext);

                        return firstHasNext.CompareTo(secondHasNext);
                    }
                }
            }
        }

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

        public static ValueTask<bool> ExactlyAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");

            return source.CountBetweenAsync(count + 1, count, count, cancellationToken);
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

        private static async ValueTask<int> CountAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int limit,
            CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            var count = 0;
            while (count < limit && await enumerator.MoveNextAsync())
            {
                count++;
            }

            return count;
        }
    }
}