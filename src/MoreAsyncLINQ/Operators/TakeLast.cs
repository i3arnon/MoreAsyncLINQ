using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;
using static System.Math;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns a specified number of contiguous elements from the end of
    /// a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to return the last element of.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that contains the specified number of
    /// elements from the end of the input sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> TakeLast<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return count <= 0
            ? Empty<TSource>()
            : Core(source, count);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
            var result =
                collectionCount is null
                    ? source.
                        CountDown(count).
                        SkipWhile(tuple => tuple.Countdown is null).
                        Select(tuple => tuple.Element)
                    : source.Slice(Max(0, collectionCount.Value - count), int.MaxValue);

            await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;
            }
        }
    }
}