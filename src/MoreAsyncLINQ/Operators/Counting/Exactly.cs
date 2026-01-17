using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Determines whether the number of elements in the sequence is equals to the given integer.
    /// </summary>
    /// <typeparam name="TSource">Element type of sequence</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="count">The exactly number of items a sequence must have for this
    /// function to return true</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
    /// <returns><c>true</c> if the number of elements in the sequence is equals
    /// to the given integer or <c>false</c> otherwise.</returns>
    public static ValueTask<bool> ExactlyAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(count == 0)
            : CountBetweenAsync(
                source.WithCancellation(cancellationToken),
                count + 1,
                count,
                count);
    }
}