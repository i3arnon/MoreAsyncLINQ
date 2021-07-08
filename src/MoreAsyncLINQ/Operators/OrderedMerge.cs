using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> OrderedMerge<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));

            return first.OrderedMerge(
                second,
                element => element,
                element => element,
                element => element,
                element => element,
                (firstElement, _) => firstElement,
                comparer: null);
        }

        public static IAsyncEnumerable<TSource> OrderedMerge<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            IComparer<TSource>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return first.OrderedMerge(
                second,
                element => element,
                element => element,
                element => element,
                element => element,
                (firstElement, _) => firstElement,
                comparer);
        }

        public static IAsyncEnumerable<TSource> OrderedMerge<TSource, TKey>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return first.OrderedMerge(
                second,
                keySelector,
                keySelector,
                element => element,
                element => element,
                (firstElement, _) => firstElement,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> OrderedMerge<TSource, TKey, TResult>(
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

            return first.OrderedMerge(
                second,
                keySelector,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> OrderedMerge<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TResult> secondSelector,
            Func<TSource, TSource, TResult> bothSelector,
            IComparer<TKey>? comparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
            if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
            if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

            return first.OrderedMerge(
                second,
                keySelector,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
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

            return first.OrderedMerge(
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
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
                Func<TFirst, TKey> firstKeySelector,
                Func<TSecond, TKey> secondKeySelector,
                Func<TFirst, TResult> firstSelector,
                Func<TSecond, TResult> secondSelector,
                Func<TFirst, TSecond, TResult> bothSelector,
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
                            var firstKey = firstKeySelector(firstElement);
                            var secondElement = secondEnumerator.Current;
                            var secondKey = secondKeySelector(secondElement);
                            switch (comparer.Compare(firstKey, secondKey))
                            {
                                case < 0:
                                    yield return firstSelector(firstElement);

                                    hasFirstElement = await firstEnumerator.MoveNextAsync();
                                    break;
                                case > 0:
                                    yield return secondSelector(secondElement);

                                    hasSecondElement = await secondEnumerator.MoveNextAsync();
                                    break;
                                case 0:
                                    yield return bothSelector(firstElement, secondElement);

                                    hasFirstElement = await firstEnumerator.MoveNextAsync();
                                    hasSecondElement = await secondEnumerator.MoveNextAsync();
                                    break;
                            }

                            break;
                        case (false, true):
                            yield return secondSelector(secondEnumerator.Current);

                            hasSecondElement = await secondEnumerator.MoveNextAsync();
                            break;
                        case (true, false):
                            yield return firstSelector(firstEnumerator.Current);

                            hasFirstElement = await firstEnumerator.MoveNextAsync();
                            break;
                        default:
                            Debug.Fail((hasFirstElement, hasSecondElement).ToString());
                            break;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> OrderedMergeAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return first.OrderedMergeAwait(
                second,
                keySelector,
                keySelector,
                ValueTasks.FromResult,
                ValueTasks.FromResult,
                (firstElement, _) => ValueTasks.FromResult(firstElement),
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> OrderedMergeAwait<TSource, TKey, TResult>(
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

            return first.OrderedMergeAwait(
                second,
                keySelector,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> OrderedMergeAwait<TSource, TKey, TResult>(
            this IAsyncEnumerable<TSource> first,
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

            return first.OrderedMergeAwait(
                second,
                keySelector,
                keySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer);
        }

        public static IAsyncEnumerable<TResult> OrderedMergeAwait<TFirst, TSecond, TKey, TResult>(
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

            return first.OrderedMergeAwait(
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer: null);
        }

        public static IAsyncEnumerable<TResult> OrderedMergeAwait<TFirst, TSecond, TKey, TResult>(
            this IAsyncEnumerable<TFirst> first,
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
}