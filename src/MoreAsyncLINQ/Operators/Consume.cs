using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Completely consumes the given sequence. This method uses immediate execution,
    /// and doesn't store any data during execution.
    /// </summary>
    /// <typeparam name="TSource">Element type of the sequence</typeparam>
    /// <param name="source">Source to consume</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    public static ValueTask ConsumeAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.IsKnownEmpty()
            ? new ValueTask()
            : Core(source, cancellationToken);

        static async ValueTask Core(
            IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken)
        {
            await foreach (var _ in source.WithCancellation(cancellationToken))
            {
            }
        }
    }
}