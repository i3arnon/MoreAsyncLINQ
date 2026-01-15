using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Merges two or more sequences that are in a common order (either ascending or descending) into
    /// a single sequence that preserves that order.
    /// </summary>
    /// <remarks>
    /// Using SortedMerge on sequences that are not ordered or are not in the same order produces
    /// undefined results.<br/>
    /// <c>SortedMerge</c> uses performs the merge in a deferred, streaming manner. <br/>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
    /// <param name="source">The primary sequence with which to merge</param>
    /// <param name="direction">The ordering that all sequences must already exhibit</param>
    /// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
    /// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
    public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
        this IAsyncEnumerable<TSource> source,
        OrderByDirection direction,
        params IAsyncEnumerable<TSource>[] otherSequences)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (otherSequences is null) throw new ArgumentNullException(nameof(otherSequences));

        return source.SortedMerge(direction, comparer: null, otherSequences);
    }

    /// <summary>
    /// Merges two or more sequences that are in a common order (either ascending or descending) into
    /// a single sequence that preserves that order.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The primary sequence with which to merge</param>
    /// <param name="direction">The ordering that all sequences must already exhibit</param>
    /// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
    /// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
    /// <returns>A merged, order-preserving sequence containing al of the elements of the original sequences</returns>
    public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
        this IAsyncEnumerable<TSource> source,
        OrderByDirection direction,
        IComparer<TSource>? comparer,
        params IAsyncEnumerable<TSource>[] otherSequences)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (otherSequences is null) throw new ArgumentNullException(nameof(otherSequences));
        
        if (source.IsKnownEmpty() &&
            otherSequences.All(static sequence => sequence.IsKnownEmpty()))
        {
            return AsyncEnumerable.Empty<TSource>();
        }

        if (otherSequences.Length == 0)
        {
            return source;
        }

        comparer ??= Comparer<TSource>.Default;
        return Core(
            new[] { source }.Concat(otherSequences),
            direction == Ascending
                ? (first, second) => comparer.Compare(second, first) < 0
                : (first, second) => comparer.Compare(second, first) > 0,
            default);

        static async IAsyncEnumerable<TSource> Core(
            IEnumerable<IAsyncEnumerable<TSource>> sequences,
            Func<TSource, TSource, bool> precedenceFunc,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var enumerators =
                await sequences.
                    Select(sequence => sequence.GetAsyncEnumerator(cancellationToken)).
                    ToAsyncEnumerable().
                    AcquireAsync(cancellationToken);

            var enumeratorsDisposable = new EnumeratorsDisposable<TSource>(enumerators);
            await using (enumeratorsDisposable)
            {
                for (var index = enumeratorsDisposable.Count - 1; index >= 0; index--)
                {
                    if (!await enumeratorsDisposable[index].MoveNextAsync())
                    {
                        await enumeratorsDisposable.DisposeEnumeratorAsync(index);
                    }
                }

                while (enumeratorsDisposable.Count > 0)
                {
                    var nextIndex = 0;
                    var nextElement = enumeratorsDisposable[0].Current;
                    for (var index = 1; index < enumeratorsDisposable.Count; index++)
                    {
                        var element = enumeratorsDisposable[index].Current;
                        if (precedenceFunc(nextElement, element))
                        {
                            nextIndex = index;
                            nextElement = element;
                        }
                    }

                    yield return nextElement;

                    if (!await enumeratorsDisposable[nextIndex].MoveNextAsync())
                    {
                        await enumeratorsDisposable.DisposeEnumeratorAsync(nextIndex);
                    }
                }
            }
        }
    }

    private sealed class EnumeratorsDisposable<T>(IEnumerable<IAsyncEnumerator<T>> enumerators) : IAsyncDisposable
    {
        private readonly List<IAsyncEnumerator<T>> _enumerators = [..enumerators];

        public int Count => _enumerators.Count;

        public IAsyncEnumerator<T> this[int index] => _enumerators[index];

        public async ValueTask DisposeEnumeratorAsync(int index)
        {
            await _enumerators[index].DisposeAsync().ConfigureAwait(false);
            _enumerators.RemoveAt(index);
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var iterator in _enumerators)
            {
                await iterator.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}