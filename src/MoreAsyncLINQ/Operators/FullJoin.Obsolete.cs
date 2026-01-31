#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(FullJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> FullJoinAwait<TSource, TKey, TResult>(
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

        return FullJoinAwait(
            first,
            second,
            keySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(FullJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> FullJoinAwait<TSource, TKey, TResult>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TResult>> firstSelector,
        Func<TSource, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return FullJoinAwait(
            first,
            second,
            keySelector,
            keySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer);
    }

    [Obsolete($"Use an overload of {nameof(FullJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> FullJoinAwait<TFirst, TSecond, TKey, TResult>(
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

        return FullJoinAwait(
            first,
            second,
            firstKeySelector,
            secondKeySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(FullJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> FullJoinAwait<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, ValueTask<TResult>> firstSelector,
        Func<TSecond, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
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
            comparer);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, ValueTask<TKey>> secondKeySelector,
            Func<TFirst, ValueTask<TResult>> firstSelector,
            Func<TSecond, ValueTask<TResult>> secondSelector,
            Func<TFirst, TSecond, ValueTask<TResult>> bothSelector,
            IEqualityComparer<TKey>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondKeyAndElements =
                await second.
                    Select(async (TSecond element, CancellationToken _) => (key: await secondKeySelector(element).ConfigureAwait(false), element)).
                    ToArrayAsync(cancellationToken).
                    ConfigureAwait(false);
            var secondLookup =
                secondKeyAndElements.ToLookup(
                    tuple => tuple.key,
                    tuple => tuple.element,
                    comparer);
            var firstKeys = new HashSet<TKey>(comparer);
            await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var firstKey = await firstKeySelector(firstElement).ConfigureAwait(false);
                firstKeys.Add(firstKey);

                using var secondEnumerator = secondLookup[firstKey].GetEnumerator();
                if (!secondEnumerator.MoveNext())
                {
                    secondEnumerator.Dispose();
                    yield return await firstSelector(firstElement).ConfigureAwait(false);

                    continue;
                }

                do
                {
                    yield return await bothSelector(firstElement, secondEnumerator.Current).ConfigureAwait(false);
                } while (secondEnumerator.MoveNext());
            }

            foreach (var (secondKey, secondElement) in secondKeyAndElements)
            {
                if (!firstKeys.Contains(secondKey))
                {
                    yield return await secondSelector(secondElement).ConfigureAwait(false);
                }
            }
        }
    }
}

