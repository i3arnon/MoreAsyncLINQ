using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
            this IAsyncEnumerable<TSource> source,
            OrderByDirection direction,
            params IAsyncEnumerable<TSource>[] otherSequences)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (otherSequences is null) throw new ArgumentNullException(nameof(otherSequences));

            return source.SortedMerge(direction, comparer: null, otherSequences);
        }

        public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
            this IAsyncEnumerable<TSource> source,
            OrderByDirection direction,
            IComparer<TSource>? comparer,
            params IAsyncEnumerable<TSource>[] otherSequences)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (otherSequences is null) throw new ArgumentNullException(nameof(otherSequences));

            if (otherSequences.Length == 0)
            {
                return source;
            }

            comparer ??= Comparer<TSource>.Default;
            return Core(
                new[] { source }.Concat(otherSequences),
                direction == Ascending
                    ? (first, second) => comparer.Compare(second, first) < 0
                    : (first, second) => comparer.Compare(second, first) > 0);

            static async IAsyncEnumerable<TSource> Core(
                IEnumerable<IAsyncEnumerable<TSource>> sequences,
                Func<TSource, TSource, bool> precedenceFunc,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var enumerators =
                    await sequences.
                        Select(sequence => sequence.GetAsyncEnumerator(cancellationToken)).
                        ToAsyncEnumerable().
                        AcquireAsync(cancellationToken).
                        ConfigureAwait(false);

                var enumeratorsDisposable = new EnumeratorsDisposable<TSource>(enumerators);
                await using (enumeratorsDisposable.ConfigureAwait(false))
                {
                    for (var index = enumeratorsDisposable.Count - 1; index >= 0; index--)
                    {
                        if (!await enumeratorsDisposable[index].MoveNextAsync().ConfigureAwait(false))
                        {
                            await enumeratorsDisposable.DisposeEnumeratorAsync(index).ConfigureAwait(false);
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

                        if (!await enumeratorsDisposable[nextIndex].MoveNextAsync().ConfigureAwait(false))
                        {
                            await enumeratorsDisposable.DisposeEnumeratorAsync(nextIndex).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        private sealed class EnumeratorsDisposable<T> : IAsyncDisposable
        {
            private readonly List<IAsyncEnumerator<T>> _enumerators;

            public int Count => _enumerators.Count;

            public EnumeratorsDisposable(IEnumerable<IAsyncEnumerator<T>> enumerators)
            {
                _enumerators = new List<IAsyncEnumerator<T>>(enumerators);
            }

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
}