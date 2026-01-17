using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    private static MemoizedAsyncEnumerable<TSource> Memoize<TSource>(this IAsyncEnumerable<TSource> source) =>
        source as MemoizedAsyncEnumerable<TSource> ?? new MemoizedAsyncEnumerable<TSource>(source);

    private sealed class MemoizedAsyncEnumerable<T>(IAsyncEnumerable<T> source) : IAsyncEnumerable<T>, IAsyncDisposable
    {
        private readonly IAsyncEnumerable<T> _source = source ?? throw new ArgumentNullException(nameof(source));

        private List<T>? _cache;
        private ConfiguredCancelableAsyncEnumerable<T>.Enumerator? _enumerator;
        private ExceptionDispatchInfo? _exception;
        private int? _exceptionIndex;

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (_cache is null)
            {
                _exception?.Throw();

                try
                {
                    _enumerator = _source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                    _cache = [];
                }
                catch (Exception exception)
                {
                    _exception = ExceptionDispatchInfo.Capture(exception);
                    throw;
                }
            }

            var index = 0;
            while (true)
            {
                if (_cache is null)
                {
                    throw new ObjectDisposedException(nameof(MemoizedAsyncEnumerable<T>));
                }

                if (index >= _cache.Count)
                {
                    if (index == _exceptionIndex)
                    {
                        _exception!.Throw();
                    }

                    if (_enumerator is not { } enumerator)
                    {
                        break;
                    }

                    bool hasNext;
                    try
                    {
                        hasNext = await enumerator.MoveNextAsync();
                    }
                    catch (Exception exception)
                    {
                        _exception = ExceptionDispatchInfo.Capture(exception);
                        _exceptionIndex = index;
                        await enumerator.DisposeAsync();
                        _enumerator = null;
                        throw;
                    }

                    if (hasNext)
                    {
                        _cache.Add(enumerator.Current);
                    }
                    else
                    {
                        await enumerator.DisposeAsync();
                        _enumerator = null;
                        break;
                    }
                }

                yield return _cache[index];

                index++;
            }
        }

        public async ValueTask DisposeAsync()
        {
            _cache = null;
            _exception = null;
            _exceptionIndex = null;
            if (_enumerator is { } enumerator)
            {
                await enumerator.DisposeAsync();
                _enumerator = null;
            }
        }
    }
}