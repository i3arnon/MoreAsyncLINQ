#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(IndexBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return IndexByAwait(source, keySelector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(IndexBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return ScanByAwait<TSource, TKey, (int index, TSource element)>(
                source,
                keySelector,
                _ => ValueTasks.FromResult((-1, default(TSource)!)),
                (state, _, element) => ValueTasks.FromResult((state.index + 1, element)),
                comparer).
            Select(tuple => (tuple.State.index, tuple.State.element));
    }
}

