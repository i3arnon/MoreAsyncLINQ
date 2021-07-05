using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> FullJoin<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TResult> secondSelector,
            Func<TSource, TSource, TResult> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.FullJoin(
                second,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> FullJoin<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TResult> secondSelector,
            Func<TSource, TSource, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.FullJoin(
                second,
                keySelector,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> FullJoin<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.FullJoin(
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> FullJoin<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
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
                Func<TFirst, TKey> firstKeySelector,
                Func<TSecond, TKey> secondKeySelector,
                Func<TFirst, TResult> firstSelector,
                Func<TSecond, TResult> secondSelector,
                Func<TFirst, TSecond, TResult> bothSelector,
                IEqualityComparer<TKey>? comparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondKeyAndElements =
                    await second.
                        Select(element => (key: secondKeySelector(element), element)).
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
                    var firstKey = firstKeySelector(firstElement);
                    firstKeys.Add(firstKey);

                    using var secondEnumerator = secondLookup[firstKey].GetEnumerator();
                    if (!secondEnumerator.MoveNext())
                    {
                        secondEnumerator.Dispose();
                        yield return firstSelector(firstElement);

                        continue;
                    }

                    do
                    {
                        yield return bothSelector(firstElement, secondEnumerator.Current);
                    } while (secondEnumerator.MoveNext());
                }

                foreach (var (secondKey, secondElement) in secondKeyAndElements)
                {
                    if (!firstKeys.Contains(secondKey))
                    {
                        yield return secondSelector(secondElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> FullJoinAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
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

            return first.FullJoinAwait(
                second,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> FullJoinAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
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

            return first.FullJoinAwait(
                second,
                keySelector,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> FullJoinAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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

            return first.FullJoinAwait(
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> FullJoinAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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
                        SelectAwait(async element => (key: await secondKeySelector(element).ConfigureAwait(false), element)).
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
}