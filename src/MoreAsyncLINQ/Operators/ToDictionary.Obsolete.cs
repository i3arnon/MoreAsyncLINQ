#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(ToDictionaryAsync)}.")]
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(comparer: null, cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToDictionaryAsync)}.")]
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
        IEqualityComparer<TKey>? comparer,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(
            static pair => pair.Key,
            static pair => pair.Value,
            comparer,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToDictionaryAsync)}.")]
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<(TKey Key, TValue Value)> source,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(comparer: null, cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToDictionaryAsync)}.")]
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<(TKey Key, TValue Value)> source,
        IEqualityComparer<TKey>? comparer,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(
            static tuple => tuple.Key,
            static tuple => tuple.Value,
            comparer,
            cancellationToken);
    }
}

