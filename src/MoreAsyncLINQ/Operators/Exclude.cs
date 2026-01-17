using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Excludes a contiguous number of elements from a sequence starting
    /// at a given index.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
    /// <param name="source">The sequence to exclude elements from</param>
    /// <param name="startIndex">The zero-based index at which to begin excluding elements</param>
    /// <param name="count">The number of elements to exclude</param>
    /// <returns>A sequence that excludes the specified portion of elements</returns>
    public static IAsyncEnumerable<TSource> Exclude<TSource>(this IAsyncEnumerable<TSource> source, int startIndex, int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        if (count == 0)
        {
            return source;
        }

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(source, startIndex, count, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int startIndex,
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            var index = 0;
            for (; index < startIndex && await enumerator.MoveNextAsync(); index++)
            {
                yield return enumerator.Current;
            }

            var endIndex = startIndex + count;
            for (; index < endIndex && await enumerator.MoveNextAsync(); index++)
            {
            }

            while (await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }
    }
}