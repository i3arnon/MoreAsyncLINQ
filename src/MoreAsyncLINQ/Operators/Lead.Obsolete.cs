#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Lead)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LeadAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        int offset,
        Func<TSource, TSource?, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return LeadAwait(
                source.
                    Select(Option.Some),
                offset,
                defaultLeadValue: default,
                (elementOption, leadOption) => resultSelector(elementOption.Value, leadOption.OrDefault()));
    }

    [Obsolete($"Use an overload of {nameof(Lead)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LeadAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
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

