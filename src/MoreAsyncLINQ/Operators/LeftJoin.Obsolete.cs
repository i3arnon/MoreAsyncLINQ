#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LeftJoinAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> firstSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

        return LeftJoinAwait(
            first,
            second,
            keySelector,
            firstSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LeftJoinAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> firstSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

        return LeftJoinAwait(
            first,
            second,
            keySelector,
            keySelector,
            firstSelector,
            bothSelector,
            comparer);
    }

    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LeftJoinAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, ValueTask<TResult>> firstSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

        return LeftJoinAwait(
            first,
            second,
            firstKeySelector,
            secondKeySelector,
            firstSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> LeftJoinAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, ValueTask<TResult>> firstSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

        comparer ??= EqualityComparer<TKey>.Default;
        return first.
            GroupJoin(
                second,
                (firstElement, _) => firstKeySelector(firstElement),
                (secondElement, _) => secondKeySelector(secondElement),
                (firstElement, secondElements, _) => ValueTasks.FromResult((firstElement, secondElements: secondElements.Select(secondElement => (hasValue: true, value: secondElement)))),
                comparer).
            SelectMany(
                (tuple, _) => ValueTasks.FromResult(tuple.secondElements.DefaultIfEmpty()),
                (tuple, secondElement, _) =>
                    secondElement.hasValue
                        ? bothSelector(tuple.firstElement, secondElement.value)
                        : firstSelector(tuple.firstElement));
    }
}

