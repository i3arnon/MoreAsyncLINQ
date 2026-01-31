#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return PartitionAsync(source.GroupBy(predicate), resultSelector, cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
        IAsyncEnumerable<IGrouping<bool, TSource>> source,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAsync(
            source,
            key1: true,
            key2: false,
            (grouping1, grouping2, _) => resultSelector(grouping1, grouping2),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
        IAsyncEnumerable<IGrouping<bool?, TSource>> source,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAsync(
            source,
            key1: true,
            key2: false,
            key3: null,
            (grouping1, grouping2, grouping3, _) => resultSelector(grouping1, grouping2, grouping3),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
        return PartitionAsync(
            source,
            key,
            comparer: null,
            resultSelector,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAsync(
            source,
            count: 1,
            key,
            key,
            key,
            comparer,
            (grouping1, _, _, groupings) => resultSelector(grouping1, groupings),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
        return PartitionAsync(
            source,
            key1,
            key2,
            comparer: null,
            resultSelector,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAsync(
            source,
            count: 2,
            key1,
            key2,
            key2,
            comparer,
            (grouping1, grouping2, _, groupings) => resultSelector(grouping1, grouping2, groupings),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        TKey key3,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
        return PartitionAsync(
            source,
            key1,
            key2,
            key3,
            comparer: null,
            resultSelector,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with {nameof(IEnumerable<>)} parameters.")]
    public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        TKey key3,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
        return PartitionAsync(
            source,
            count: 3,
            key1,
            key2,
            key3,
            comparer,
            resultSelector,
            cancellationToken);
    }

    private static async ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TSource>> source,
        int count,
        TKey key1,
        TKey key2,
        TKey key3,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IGrouping<TKey, TSource>>, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        Debug.Assert(count is >= 1 and <= 3);

        comparer ??= EqualityComparer<TKey>.Default;

        var grouping1 = count >= 1 ? null : Empty<TSource>();
        var grouping2 = count >= 2 ? null : Empty<TSource>();
        var grouping3 = count == 3 ? null : Empty<TSource>();
        List<IGrouping<TKey, TSource>>? groupings = null;
        await foreach (var grouping in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (grouping1 is null && comparer.Equals(grouping.Key, key1))
            {
                grouping1 = grouping.ToAsyncEnumerable();
            }
            else if (grouping2 is null && comparer.Equals(grouping.Key, key2))
            {
                grouping2 = grouping.ToAsyncEnumerable();
            }
            else if (grouping3 is null && comparer.Equals(grouping.Key, key3))
            {
                grouping3 = grouping.ToAsyncEnumerable();
            }
            else
            {
                groupings ??= new List<IGrouping<TKey, TSource>>();
                groupings.Add(grouping);
            }
        }

        return resultSelector(
            grouping1 ?? Empty<TSource>(),
            grouping2 ?? Empty<TSource>(),
            grouping3 ?? Empty<TSource>(),
            groupings?.ToAsyncEnumerable() ?? Empty<IGrouping<TKey, TSource>>());
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<(IAsyncEnumerable<TSource> True, IAsyncEnumerable<TSource> False)> PartitionAwaitAsync<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return PartitionAwaitAsync(
            source,
            predicate,
            static (grouping1, grouping2) => ValueTasks.FromResult((grouping1, grouping2)),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return PartitionAwaitAsync(source.GroupBy((element, _) => predicate(element)), resultSelector, cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<IGrouping<bool, TSource>> source,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            key1: true,
            key2: false,
            (grouping1, grouping2, _) => resultSelector(grouping1, grouping2),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<IGrouping<bool?, TSource>> source,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            key1: true,
            key2: false,
            key3: null,
            (grouping1, grouping2, grouping3, _) => resultSelector(grouping1, grouping2, grouping3),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            key,
            comparer: null,
            resultSelector,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            count: 1,
            key,
            key,
            key,
            comparer,
            (grouping1, _, _, groupings) => resultSelector(grouping1, groupings),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            key1,
            key2,
            comparer: null,
            resultSelector,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            count: 2,
            key1,
            key2,
            key2,
            comparer,
            (grouping1, grouping2, _, groupings) => resultSelector(grouping1, grouping2, groupings),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        TKey key3,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            key1,
            key2,
            key3,
            comparer: null,
            resultSelector,
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(PartitionAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TElement>> source,
        TKey key1,
        TKey key2,
        TKey key3,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return PartitionAwaitAsync(
            source,
            count: 3,
            key1,
            key2,
            key3,
            comparer,
            resultSelector,
            cancellationToken);
    }

    private static async ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
        IAsyncEnumerable<IGrouping<TKey, TSource>> source,
        int count,
        TKey key1,
        TKey key2,
        TKey key3,
        IEqualityComparer<TKey>? comparer,
        Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        Debug.Assert(count is >= 1 and <= 3);

        comparer ??= EqualityComparer<TKey>.Default;

        var grouping1 = count >= 1 ? null : Empty<TSource>();
        var grouping2 = count >= 2 ? null : Empty<TSource>();
        var grouping3 = count == 3 ? null : Empty<TSource>();
        List<IGrouping<TKey, TSource>>? groupings = null;
        await foreach (var grouping in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (grouping1 is null && comparer.Equals(grouping.Key, key1))
            {
                grouping1 = grouping.ToAsyncEnumerable();
            }
            else if (grouping2 is null && comparer.Equals(grouping.Key, key2))
            {
                grouping2 = grouping.ToAsyncEnumerable();
            }
            else if (grouping3 is null && comparer.Equals(grouping.Key, key3))
            {
                grouping3 = grouping.ToAsyncEnumerable();
            }
            else
            {
                groupings ??= new List<IGrouping<TKey, TSource>>();
                groupings.Add(grouping);
            }
        }

        return await resultSelector(
                grouping1 ?? Empty<TSource>(),
                grouping2 ?? Empty<TSource>(),
                grouping3 ?? Empty<TSource>(),
                groupings?.ToAsyncEnumerable() ?? Empty<IGrouping<TKey, TSource>>()).
            ConfigureAwait(false);
    }
}

