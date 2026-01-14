using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    private sealed class ExtremaAsyncEnumerable<TElement, TKey>(
        IAsyncEnumerable<TElement> source,
        Func<TElement, TKey> selector,
        Func<TKey, TKey, int> comparer)
        : IExtremaAsyncEnumerable<TElement>
    {
        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            source.
                ExtremaBy(Extrema<TElement>.First, limit: null, selector, comparer, CancellationToken.None).
                GetAsyncEnumerator(cancellationToken);

        public IAsyncEnumerable<TElement> Take(int count) =>
            count switch
            {
                0 => Empty<TElement>(),
                1 => source.ExtremaBy(Extremum<TElement>.First, limit: 1, selector, comparer),
                _ => source.ExtremaBy(Extrema<TElement>.First, count, selector, comparer)
            };

        public IAsyncEnumerable<TElement> TakeLast(int count) =>
            count switch
            {
                0 => Empty<TElement>(),
                1 => source.ExtremaBy(Extremum<TElement>.Last, limit: 1, selector, comparer),
                _ => source.ExtremaBy(Extrema<TElement>.Last, count, selector, comparer)
            };
    }

    private sealed class ExtremaAsyncEnumerableWithTask<TElement, TKey>(
        IAsyncEnumerable<TElement> source,
        Func<TElement, ValueTask<TKey>> selector,
        Func<TKey, TKey, int> comparer)
        : IExtremaAsyncEnumerable<TElement>
    {
        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            source.
                ExtremaByAwait(Extrema<TElement>.First, limit: null, selector, comparer, CancellationToken.None).
                GetAsyncEnumerator(cancellationToken);

        public IAsyncEnumerable<TElement> Take(int count) =>
            count switch
            {
                0 => Empty<TElement>(),
                1 => source.ExtremaByAwait(Extremum<TElement>.First, limit: 1, selector, comparer),
                _ => source.ExtremaByAwait(Extrema<TElement>.First, count, selector, comparer)
            };

        public IAsyncEnumerable<TElement> TakeLast(int count) =>
            count switch
            {
                0 => Empty<TElement>(),
                1 => source.ExtremaByAwait(Extremum<TElement>.Last, limit: 1, selector, comparer),
                _ => source.ExtremaByAwait(Extrema<TElement>.Last, count, selector, comparer)
            };
    }
}