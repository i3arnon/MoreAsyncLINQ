using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<(int? Countdown, TSource Element)> CountDown<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.CountDown(count, static(element, count) => (count, element));
        }

        public static IAsyncEnumerable<TResult> CountDown<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, int?, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(
                source,
                count,
                resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int count,
                Func<TSource, int?, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                if (collectionCount is not null)
                {
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(element, collectionCount <= count ? collectionCount : null);
                        collectionCount--;
                    }
                }
                else
                {
                    var queue = new Queue<TSource>(Max(1, count + 1));
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        queue.Enqueue(element);
                        if (queue.Count > count)
                        {
                            yield return resultSelector(queue.Dequeue(), null);
                        }
                    }

                    while (queue.Count > 0)
                    {
                        yield return resultSelector(queue.Dequeue(), queue.Count);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CountDownAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, int?, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(
                source,
                count,
                resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int count,
                Func<TSource, int?, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                if (collectionCount is not null)
                {
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(element, collectionCount <= count ? collectionCount : null).ConfigureAwait(false);
                        collectionCount--;
                    }
                }
                else
                {
                    var queue = new Queue<TSource>(Max(1, count + 1));
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        queue.Enqueue(element);
                        if (queue.Count > count)
                        {
                            yield return await resultSelector(queue.Dequeue(), null).ConfigureAwait(false);
                        }
                    }

                    while (queue.Count > 0)
                    {
                        yield return await resultSelector(queue.Dequeue(), queue.Count).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}