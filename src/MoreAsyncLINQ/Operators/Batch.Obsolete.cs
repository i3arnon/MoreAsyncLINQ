#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Batch)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> BatchAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        int size,
        Func<TSource[], ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, size, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            int size,
            Func<TSource[], ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var index = 0;
            TSource[]? batch = null;
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (batch is null)
                {
                    batch = new TSource[size];
                    index = 0;
                }

                batch[index] = element;
                index++;
                if (index == size)
                {
                    yield return await resultSelector(batch).ConfigureAwait(false);

                    batch = null;
                }
            }

            if (batch is not null && index > 0)
            {
                Array.Resize(ref batch, index);
                yield return await resultSelector(batch).ConfigureAwait(false);
            }
        }
    }
}

