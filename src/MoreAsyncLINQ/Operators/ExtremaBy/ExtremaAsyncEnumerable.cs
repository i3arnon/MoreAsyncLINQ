using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    private static class ExtremaAsyncEnumerable
    {
        public static IExtremaAsyncEnumerable<T> Empty<T>() => EmptyExtremaAsyncEnumerable<T>.Instance;
        
        private sealed class EmptyExtremaAsyncEnumerable<T> : IExtremaAsyncEnumerable<T>
        {
            public static readonly EmptyExtremaAsyncEnumerable<T> Instance = new();
        
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) => 
                AsyncEnumerable.Empty<T>().GetAsyncEnumerator(cancellationToken);

            public IAsyncEnumerable<T> Take(int count) => AsyncEnumerable.Empty<T>();

            public IAsyncEnumerable<T> TakeLast(int count) => AsyncEnumerable.Empty<T>();
        }
    }

    private sealed class ExtremaAsyncEnumerable<TElement, TKey>(
        IAsyncEnumerable<TElement> source,
        Func<TElement, TKey> selector,
        Func<TKey, TKey, int> comparer)
        : IExtremaAsyncEnumerable<TElement>
    {
        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            source.
                ExtremaBy(Extrema<TElement>.First, limit: null, selector, comparer).
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
        Func<TElement, CancellationToken, ValueTask<TKey>> selector,
        Func<TKey, TKey, int> comparer)
        : IExtremaAsyncEnumerable<TElement>
    {
        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            source.
                ExtremaBy(Extrema<TElement>.First, limit: null, selector, comparer).
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
}