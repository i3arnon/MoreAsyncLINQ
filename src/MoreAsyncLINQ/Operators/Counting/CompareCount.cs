using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? ValueTasks.FromResult(0)
            : Core(
                first.WithCancellation(cancellationToken),
                second.WithCancellation(cancellationToken));

        static async ValueTask<int> Core(
            ConfiguredCancelableAsyncEnumerable<TFirst> first,
            ConfiguredCancelableAsyncEnumerable<TSecond> second)
        {
            await using var firstEnumerator = first.GetAsyncEnumerator();
            await using var secondEnumerator = second.GetAsyncEnumerator();

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