#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(CountDown)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CountDownAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
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

