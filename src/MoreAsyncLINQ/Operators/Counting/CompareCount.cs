using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Compares two sequences and returns an integer that indicates whether the first sequence
    /// has fewer, the same or more elements than the second sequence.
    /// </summary>
    /// <typeparam name="TFirst">Element type of the first sequence</typeparam>
    /// <typeparam name="TSecond">Element type of the second sequence</typeparam>
    /// <param name="first">The first sequence</param>
    /// <param name="second">The second sequence</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
    /// <returns><c>-1</c> if the first sequence has the fewest elements, <c>0</c> if the two sequences have the same number of elements
    /// or <c>1</c> if the first sequence has the most elements.</returns>
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