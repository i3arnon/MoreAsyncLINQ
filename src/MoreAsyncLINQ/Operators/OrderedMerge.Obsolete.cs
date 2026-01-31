#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> OrderedMergeAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return OrderedMergeAwait(
            first,
            second,
            keySelector,
            keySelector,
            ValueTasks.FromResult,
            ValueTasks.FromResult,
            (firstElement, _) => ValueTasks.FromResult(firstElement),
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> OrderedMergeAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> firstSelector,
        Func<TSource, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return OrderedMergeAwait(
            first,
            second,
            keySelector,
            keySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> OrderedMergeAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> firstSelector,
        Func<TSource, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector,
        IComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return OrderedMergeAwait(
            first,
            second,
            keySelector,
            keySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer);
    }

    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> OrderedMergeAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, ValueTask<TResult>> firstSelector,
        Func<TSecond, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return OrderedMergeAwait(
            first,
            second,
            firstKeySelector,
            secondKeySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> OrderedMergeAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, ValueTask<TResult>> firstSelector,
        Func<TSecond, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector,
        IComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return Core(
            first,
            second,
            firstKeySelector,
            secondKeySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer ?? Comparer<TKey>.Default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, ValueTask<TKey>> secondKeySelector,
            Func<TFirst, ValueTask<TResult>> firstSelector,
            Func<TSecond, ValueTask<TResult>> secondSelector,
            Func<TFirst, TSecond, ValueTask<TResult>> bothSelector,
            IComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var firstEnumerator = first.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
            await using var secondEnumerator = second.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            var hasFirstElement = await firstEnumerator.MoveNextAsync();
            var hasSecondElement = await secondEnumerator.MoveNextAsync();

            while (hasFirstElement || hasSecondElement)
            {
                switch (hasFirstElement, hasSecondElement)
                {
                    case (true, true):
                        var firstElement = firstEnumerator.Current;
                        var firstKey = await firstKeySelector(firstElement).ConfigureAwait(false);
                        var secondElement = secondEnumerator.Current;
                        var secondKey = await secondKeySelector(secondElement).ConfigureAwait(false);
                        switch (comparer.Compare(firstKey, secondKey))
                        {
                            case < 0:
                                yield return await firstSelector(firstElement).ConfigureAwait(false);

                                hasFirstElement = await firstEnumerator.MoveNextAsync();
                                break;
                            case > 0:
                                yield return await secondSelector(secondElement).ConfigureAwait(false);

                                hasSecondElement = await secondEnumerator.MoveNextAsync();
                                break;
                            case 0:
                                yield return await bothSelector(firstElement, secondElement).ConfigureAwait(false);

                                hasFirstElement = await firstEnumerator.MoveNextAsync();
                                hasSecondElement = await secondEnumerator.MoveNextAsync();
                                break;
                        }

                        break;
                    case (false, true):
                        yield return await secondSelector(secondEnumerator.Current).ConfigureAwait(false);

                        hasSecondElement = await secondEnumerator.MoveNextAsync();
                        break;
                    case (true, false):
                        yield return await firstSelector(firstEnumerator.Current).ConfigureAwait(false);

                        hasFirstElement = await firstEnumerator.MoveNextAsync();
                        break;
                    default:
                        Debug.Fail((hasFirstElement, hasSecondElement).ToString());
                        break;
                }
            }
        }
    }
}
