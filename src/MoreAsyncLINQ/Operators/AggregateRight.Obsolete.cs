#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(AggregateRightAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TSource> AggregateRightAwaitAsync<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TSource>> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return Core(source, func, cancellationToken);

        static async ValueTask<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TSource>> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Source sequence doesn't contain any elements.");
            }

            return await list.AggregateRightAwaitAsync(list[list.Count - 1], func, list.Count - 1).ConfigureAwait(false);
        }
    }

    [Obsolete($"Use an overload of {nameof(AggregateRightAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TAccumulate> AggregateRightAwaitAsync<TSource, TAccumulate>(
        IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return Core(source, seed, func, cancellationToken);

        static async ValueTask<TAccumulate> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            return await list.AggregateRightAwaitAsync(seed, func, list.Count).ConfigureAwait(false);
        }
    }

    [Obsolete($"Use an overload of {nameof(AggregateRightAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateRightAwaitAsync<TSource, TAccumulate, TResult>(
        IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
        Func<TAccumulate, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, seed, func, resultSelector, cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
            Func<TAccumulate, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate = await AggregateRightAwaitAsync(source, seed, func, cancellationToken).ConfigureAwait(false);
            return await resultSelector(accumulate).ConfigureAwait(false);
        }
    }

    private static async ValueTask<TResult> AggregateRightAwaitAsync<TSource, TResult>(
        this IReadOnlyList<TSource> list,
        TResult accumulator,
        Func<TSource, TResult, ValueTask<TResult>> func,
        int count)
    {
        for (var index = count; index > 0; index--)
        {
            accumulator = await func(list[index - 1], accumulator).ConfigureAwait(false);
        }

        return accumulator;
    }
}

