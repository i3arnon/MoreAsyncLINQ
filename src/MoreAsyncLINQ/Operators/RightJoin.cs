using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> RightJoin<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> secondSelector,
            Func<TSource, TSource, TResult> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.RightJoin(
                second,
                keySelector,
                keySelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> RightJoin<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> secondSelector,
            Func<TSource, TSource, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.RightJoin(
                second,
                keySelector,
                keySelector,
                secondSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> RightJoin<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.RightJoin(
                second,
                firstKeySelector,
                secondKeySelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> RightJoin<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return second.LeftJoin(
                first,
                secondKeySelector,
                firstKeySelector,
                secondSelector,
                (secondElement, firstElement) => bothSelector(firstElement, secondElement),
                comparer);
        }

        public static IAsyncEnumerable<TResult> RightJoinAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
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

            return first.RightJoinAwait(
                second,
                keySelector,
                keySelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> RightJoinAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
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

            return first.RightJoinAwait(
                second,
                keySelector,
                keySelector,
                secondSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> RightJoinAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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

            return first.RightJoinAwait(
                second,
                firstKeySelector,
                secondKeySelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> RightJoinAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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

            return second.LeftJoinAwait(
                first,
                secondKeySelector,
                firstKeySelector,
                secondSelector,
                (secondElement, firstElement) => bothSelector(firstElement, secondElement),
                comparer);
        }
    }
}