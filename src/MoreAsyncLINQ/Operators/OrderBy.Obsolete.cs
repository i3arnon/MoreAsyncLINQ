#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(OrderBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return OrderByAwait(source, keySelector, comparer: null, direction);
    }

    [Obsolete($"Use an overload of {nameof(OrderBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        comparer ??= Comparer<TKey>.Default;
        return direction == Ascending
            ? source.OrderBy((element, _) => keySelector(element), comparer)
            : source.OrderByDescending((element, _) => keySelector(element), comparer);
    }
}

