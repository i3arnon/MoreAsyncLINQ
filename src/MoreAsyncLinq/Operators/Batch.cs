using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource[]> Batch<TSource>(this IAsyncEnumerable<TSource> source, int size) => 
            source.Batch(size, static result => result);

        public static IAsyncEnumerable<TResult> Batch<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int size,
            Func<TSource[], TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, size, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int size,
                Func<TSource[], TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var count = await source.TryGetCollectionCountAsync().ConfigureAwait(false);
                switch (count)
                {
                    case 0:
                        yield break;
                    case > 0 when count <= size:
                        size = count.Value;
                        break;
                }

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
                        yield return resultSelector(batch);

                        batch = null;
                    }
                }

                if (batch is not null && index > 0)
                {
                    Array.Resize(ref batch, index);
                    yield return resultSelector(batch);
                }
            }
        }

        public static IAsyncEnumerable<TResult> BatchAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
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
                var count = await source.TryGetCollectionCountAsync().ConfigureAwait(false);
                switch (count)
                {
                    case 0:
                        yield break;
                    case > 0 when count <= size:
                        size = count.Value;
                        break;
                }

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
}