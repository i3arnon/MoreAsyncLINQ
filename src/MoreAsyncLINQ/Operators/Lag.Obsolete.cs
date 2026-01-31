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
    [Obsolete($"Use an overload of {nameof(Lag)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LagAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        int offset,
        Func<TSource, TSource?, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return LagAwait(
                source.
                    Select(Option.Some),
                offset,
                defaultLagValue: default,
                (elementOption, lagOption) => resultSelector(elementOption.Value, lagOption.OrDefault()));
    }

    [Obsolete($"Use an overload of {nameof(Lag)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LagAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
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

