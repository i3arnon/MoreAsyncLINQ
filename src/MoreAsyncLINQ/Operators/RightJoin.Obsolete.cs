#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> RightJoinAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return RightJoinAwait(
            first,
            second,
            keySelector,
            keySelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> RightJoinAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return RightJoinAwait(
            first,
            second,
            keySelector,
            keySelector,
            secondSelector,
            bothSelector,
            comparer);
    }

    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> RightJoinAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TSecond, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return RightJoinAwait(
            first,
            second,
            firstKeySelector,
            secondKeySelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> RightJoinAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TSecond, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return LeftJoinAwait(
            second,
            first,
            secondKeySelector,
            firstKeySelector,
            secondSelector,
            (secondElement, firstElement) => bothSelector(firstElement, secondElement),
            comparer);
    }
}

