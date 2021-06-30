using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        private sealed class ExtremaAsyncEnumerable<TElement, TKey> : IExtremaAsyncEnumerable<TElement>
        {
            private readonly IAsyncEnumerable<TElement> _source;
            private readonly Func<TElement, TKey> _selector;
            private readonly Func<TKey, TKey, int> _comparer;

            public ExtremaAsyncEnumerable(
                IAsyncEnumerable<TElement> source,
                Func<TElement, TKey> selector,
                Func<TKey, TKey, int> comparer)
            {
                _source = source;
                _selector = selector;
                _comparer = comparer;
            }

            public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
                _source.
                    ExtremaBy(Extrema<TElement>.First, limit: null, _selector, _comparer, CancellationToken.None).
                    GetAsyncEnumerator(cancellationToken);

            public IAsyncEnumerable<TElement> Take(int count) =>
                count switch
                {
                    0 => Empty<TElement>(),
                    1 => _source.ExtremaBy(Extremum<TElement>.First, limit: 1, _selector, _comparer),
                    _ => _source.ExtremaBy(Extrema<TElement>.First, count, _selector, _comparer)
                };

            public IAsyncEnumerable<TElement> TakeLast(int count) =>
                count switch
                {
                    0 => Empty<TElement>(),
                    1 => _source.ExtremaBy(Extremum<TElement>.Last, limit: 1, _selector, _comparer),
                    _ => _source.ExtremaBy(Extrema<TElement>.Last, count, _selector, _comparer)
                };
        }

        private sealed class ExtremaAsyncEnumerableWithTask<TElement, TKey> : IExtremaAsyncEnumerable<TElement>
        {
            private readonly IAsyncEnumerable<TElement> _source;
            private readonly Func<TElement, ValueTask<TKey>> _selector;
            private readonly Func<TKey, TKey, int> _comparer;

            public ExtremaAsyncEnumerableWithTask(
                IAsyncEnumerable<TElement> source,
                Func<TElement, ValueTask<TKey>> selector,
                Func<TKey, TKey, int> comparer)
            {
                _source = source;
                _selector = selector;
                _comparer = comparer;
            }

            public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
                _source.
                    ExtremaByAwait(Extrema<TElement>.First, limit: null, _selector, _comparer, CancellationToken.None).
                    GetAsyncEnumerator(cancellationToken);

            public IAsyncEnumerable<TElement> Take(int count) =>
                count switch
                {
                    0 => Empty<TElement>(),
                    1 => _source.ExtremaByAwait(Extremum<TElement>.First, limit: 1, _selector, _comparer),
                    _ => _source.ExtremaByAwait(Extrema<TElement>.First, count, _selector, _comparer)
                };

            public IAsyncEnumerable<TElement> TakeLast(int count) =>
                count switch
                {
                    0 => Empty<TElement>(),
                    1 => _source.ExtremaByAwait(Extremum<TElement>.Last, limit: 1, _selector, _comparer),
                    _ => _source.ExtremaByAwait(Extrema<TElement>.Last, count, _selector, _comparer)
                };
        }
    }
}