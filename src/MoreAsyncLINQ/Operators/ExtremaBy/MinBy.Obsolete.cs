#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Minima)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IExtremaAsyncEnumerable<TSource> MinByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> selector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return MinByAwait(source, selector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(Minima)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IExtremaAsyncEnumerable<TSource> MinByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> selector,
        IComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return new ExtremaAsyncEnumerableWithTask<TSource, TKey>(
            source,
            (element, _) => selector(element),
            GetMinimaComparer(comparer));
    }
}

