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
        public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            Func<TSource, TSource?, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.
                Select(Option.Some).
                Lead(
                    offset,
                    defaultLeadValue: default,
                    (elementOption, leadOption) => resultSelector(elementOption.Value, leadOption.OrDefault()));
        }

        public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLeadValue,
            Func<TSource, TSource, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, offset, defaultLeadValue, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int offset,
                TSource defaultLeadValue,
                Func<TSource, TSource, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var queue = new Queue<TSource>(offset);

                var hasMore = await enumerator.MoveNextAsync();
                while (hasMore && queue.Count < offset)
                {
                    queue.Enqueue(enumerator.Current);
                    hasMore = await enumerator.MoveNextAsync();
                }

                while (hasMore)
                {
                    yield return resultSelector(queue.Dequeue(), enumerator.Current);

                    queue.Enqueue(enumerator.Current);
                    hasMore = await enumerator.MoveNextAsync();
                }

                while (queue.Count > 0)
                {
                    yield return resultSelector(queue.Dequeue(), defaultLeadValue);
                }
            }
        }

        public static IAsyncEnumerable<TResult> LeadAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            Func<TSource, TSource?, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.
                Select(Option.Some).
                LeadAwait(
                    offset,
                    defaultLeadValue: default,
                    (elementOption, leadOption) => resultSelector(elementOption.Value, leadOption.OrDefault()));
        }

        public static IAsyncEnumerable<TResult> LeadAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLeadValue,
            Func<TSource, TSource, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, offset, defaultLeadValue, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int offset,
                TSource defaultLeadValue,
                Func<TSource, TSource, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var queue = new Queue<TSource>(offset);

                var hasMore = await enumerator.MoveNextAsync();
                while (hasMore && queue.Count < offset)
                {
                    queue.Enqueue(enumerator.Current);
                    hasMore = await enumerator.MoveNextAsync();
                }

                while (hasMore)
                {
                    yield return await resultSelector(queue.Dequeue(), enumerator.Current).ConfigureAwait(false);

                    queue.Enqueue(enumerator.Current);
                    hasMore = await enumerator.MoveNextAsync();
                }

                while (queue.Count > 0)
                {
                    yield return await resultSelector(queue.Dequeue(), defaultLeadValue).ConfigureAwait(false);
                }
            }
        }
    }
}