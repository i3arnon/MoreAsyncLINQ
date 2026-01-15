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
    /// Run-length encodes a sequence by converting consecutive instances of the same element into
    /// a tuple representing the item and its occurrence count. This overload
    /// uses a custom equality comparer to identify equivalent items.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence to run length encode</param>
    /// <returns>A sequence of <see cref="ValueTuple{T1,T2}"/> of the element and the occurrence count</returns>
    public static IAsyncEnumerable<(TSource Element, int RunCount)> RunLengthEncode<TSource>(this IAsyncEnumerable<TSource> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.RunLengthEncode(comparer: null);
    }

    /// <summary>
    /// Run-length encodes a sequence by converting consecutive instances of the same element into
    /// a tuple representing the item and its occurrence count. This overload
    /// uses a custom equality comparer to identify equivalent items.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence to run length encode</param>
    /// <param name="comparer">The comparer used to identify equivalent items</param>
    /// <returns>A sequence of <see cref="ValueTuple{T1,T2}"/> of the element and the occurrence count</returns>
    public static IAsyncEnumerable<(TSource Element, int RunCount)> RunLengthEncode<TSource>(
        this IAsyncEnumerable<TSource> source,
        IEqualityComparer<TSource>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<(TSource Element, int RunCount)>()
            : Core(
                source,
                comparer ?? EqualityComparer<TSource>.Default,
                default);

        static async IAsyncEnumerable<(TSource, int)> Core(
            IAsyncEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var previousItem = enumerator.Current;
            var runCount = 1;
            while (await enumerator.MoveNextAsync())
            {
                if (comparer.Equals(previousItem, enumerator.Current))
                {
                    runCount++;
                }
                else
                {
                    yield return (previousItem, runCount);

                    previousItem = enumerator.Current;
                    runCount = 1;
                }
            }

            yield return (previousItem, runCount);
        }
    }
}