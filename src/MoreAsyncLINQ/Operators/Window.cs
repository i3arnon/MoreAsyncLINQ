using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Processes a sequence into a series of subsequences representing a windowed subset of the original
    /// </summary>
    /// <remarks>
    /// The number of sequences returned is: <c>Max(0, sequence.Count() - windowSize) + 1</c><br/>
    /// Returned subsequences are buffered, but the overall operation is streamed.<br/>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
    /// <param name="source">The sequence to evaluate a sliding window over</param>
    /// <param name="size">The size (number of elements) in each window</param>
    /// <returns>A series of sequences representing each sliding window subsequence</returns>
    public static IAsyncEnumerable<IList<TSource>> Window<TSource>(
        this IAsyncEnumerable<TSource> source,
        int size)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        return Core(source, size);

        static async IAsyncEnumerable<TSource[]> Core(
            IAsyncEnumerable<TSource> source,
            int size,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            var window = new TSource[size];
            int index;
            for (index = 0; index < size && await enumerator.MoveNextAsync(); index++)
            {
                window[index] = enumerator.Current;
            }

            if (index < size)
            {
                yield break;
            }

            while (await enumerator.MoveNextAsync())
            {
                var nextWindow = new TSource[size];
                Array.Copy(window, sourceIndex: 1, nextWindow, destinationIndex: 0, size - 1);
                nextWindow[size - 1] = enumerator.Current;

                yield return window;

                window = nextWindow;
            }

            yield return window;
        }
    }
}