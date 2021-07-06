using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> LeftJoin<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TSource, TResult> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

            return first.LeftJoin(
                second,
                keySelector,
                firstSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> LeftJoin<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TSource, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

            return first.LeftJoin(
                second,
                keySelector,
                keySelector,
                firstSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> LeftJoin<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TFirst, TSecond, TResult> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

            return first.LeftJoin(
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> LeftJoin<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

            return first.
                GroupJoin(
                    second,
                    firstKeySelector,
                    secondKeySelector,
                    (firstElement, secondElements) => (firstElement, secondElements: secondElements.Select(secondElement => (hasValue: true, value: secondElement))),
                    comparer).
                SelectMany(
                    tuple => tuple.secondElements.DefaultIfEmpty(),
                    (tuple, secondElement) =>
                        secondElement.hasValue
                            ? bothSelector(tuple.firstElement, secondElement.value)
                            : firstSelector(tuple.firstElement));
        }

        public static IAsyncEnumerable<TResult> LeftJoinAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, ValueTask<TKey>> keySelector,
            Func<TSource, ValueTask<TResult>> firstSelector,
            Func<TSource, TSource, ValueTask<TResult>> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));

            return first.LeftJoinAwait(
                second,
                keySelector,
                firstSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> LeftJoinAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
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

            return first.LeftJoinAwait(
                second,
                keySelector,
                keySelector,
                firstSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> LeftJoinAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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

            return first.LeftJoinAwait(
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> LeftJoinAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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
                GroupJoinAwait(
                    second,
                    firstKeySelector,
                    secondKeySelector,
                    (firstElement, secondElements) => ValueTasks.FromResult((firstElement, secondElements: secondElements.Select(secondElement => (hasValue: true, value: secondElement)))),
                    comparer).
                SelectManyAwait(
                    tuple => ValueTasks.FromResult(tuple.secondElements.DefaultIfEmpty()),
                    (tuple, secondElement) =>
                        secondElement.hasValue
                            ? bothSelector(tuple.firstElement, secondElement.value)
                            : firstSelector(tuple.firstElement));
        }
    }
}