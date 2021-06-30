using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
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
    }
}