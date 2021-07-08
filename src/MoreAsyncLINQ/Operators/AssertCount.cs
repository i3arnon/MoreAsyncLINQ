using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> AssertCount<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.AssertCount(
                count,
                static (comparison, count) => new InvalidOperationException($"Sequence contains too {(comparison < 0 ? "few" : "many")} elements when exactly {count:N0} were expected."));
        }

        public static IAsyncEnumerable<TSource> AssertCount<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<int, int, Exception> errorSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return Core(source, count, errorSelector);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int count,
                Func<int, int, Exception> errorSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                if (collectionCount is null)
                {
                    var currentCount = 0;
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        currentCount++;
                        if (currentCount > count)
                        {
                            throw errorSelector(1, count);
                        }

                        yield return element;
                    }

                    if (currentCount != count)
                    {
                        throw errorSelector(-1, count);
                    }
                }
                else if (collectionCount == count)
                {
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return element;
                    }
                }
                else
                {
                    throw errorSelector(collectionCount.Value.CompareTo(count), count);
                }
            }
        }

        public static IAsyncEnumerable<TSource> AssertCountAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<int, int, ValueTask<Exception>> errorSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return Core(source, count, errorSelector);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int count,
                Func<int, int, ValueTask<Exception>> errorSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                if (collectionCount is null)
                {
                    var currentCount = 0;
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        currentCount++;
                        if (currentCount > count)
                        {
                            throw await errorSelector(1, count).ConfigureAwait(false);
                        }

                        yield return element;
                    }

                    if (currentCount != count)
                    {
                        throw await errorSelector(-1, count).ConfigureAwait(false);
                    }
                }
                else if (collectionCount == count)
                {
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return element;
                    }
                }
                else
                {
                    throw await errorSelector(collectionCount.Value.CompareTo(count), count).ConfigureAwait(false);
                }
            }
        }
    }
}