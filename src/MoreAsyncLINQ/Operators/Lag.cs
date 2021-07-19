using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            Func<TSource, TSource?, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.
                Select(Option.Some).
                Lag(
                    offset,
                    defaultLagValue: default,
                    (elementOption, lagOption) => resultSelector(elementOption.Value, lagOption.OrDefault()));
        }

        public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLagValue,
            Func<TSource, TSource, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(
                source,
                offset,
                defaultLagValue,
                resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int offset,
                TSource defaultLagValue,
                Func<TSource, TSource, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var queue = new Queue<TSource>(offset);

                var hasMore = await enumerator.MoveNextAsync();
                while (hasMore && offset > 0)
                {
                    queue.Enqueue(enumerator.Current);

                    yield return resultSelector(enumerator.Current, defaultLagValue);

                    hasMore = await enumerator.MoveNextAsync();
                    offset--;
                }

                if (!hasMore)
                {
                    yield break;
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return resultSelector(enumerator.Current, queue.Dequeue());

                    queue.Enqueue(enumerator.Current);
                }
            }
        }

        public static IAsyncEnumerable<TResult> LagAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            Func<TSource, TSource?, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.
                Select(Option.Some).
                LagAwait(
                    offset,
                    defaultLagValue: default,
                    (elementOption, lagOption) => resultSelector(elementOption.Value, lagOption.OrDefault()));
        }

        public static IAsyncEnumerable<TResult> LagAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLagValue,
            Func<TSource, TSource, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(
                source,
                offset,
                defaultLagValue,
                resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int offset,
                TSource defaultLagValue,
                Func<TSource, TSource, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var queue = new Queue<TSource>(offset);

                var hasMore = await enumerator.MoveNextAsync();
                while (hasMore && offset > 0)
                {
                    queue.Enqueue(enumerator.Current);

                    yield return await resultSelector(enumerator.Current, defaultLagValue).ConfigureAwait(false);

                    hasMore = await enumerator.MoveNextAsync();
                    offset--;
                }

                if (!hasMore)
                {
                    yield break;
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return await resultSelector(enumerator.Current, queue.Dequeue()).ConfigureAwait(false);

                    queue.Enqueue(enumerator.Current);
                }
            }
        }
    }
}