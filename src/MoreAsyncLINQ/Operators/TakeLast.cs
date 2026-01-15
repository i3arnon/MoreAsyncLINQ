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

        return count <= 0 ||
               source.IsKnownEmpty()
            ? Empty<TSource>()
            : Core(source, count, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var result =
                source.
                    CountDown(count).
                    SkipWhile(tuple => tuple.Countdown is null).
                    Select(tuple => tuple.Element);

            await foreach (var element in result.WithCancellation(cancellationToken))
            {
                yield return element;
            }
        }
    }
}