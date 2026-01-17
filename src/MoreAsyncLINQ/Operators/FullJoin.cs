using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Performs a full outer join on two homogeneous sequences.
    /// Additional arguments specify key selection functions and result
    /// projection functions.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector function.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two homogeneous sequences.
    /// Additional arguments specify key selection functions, result
    /// projection functions and a key comparer.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector function.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two heterogeneous sequences.
    /// Additional arguments specify key selection functions and result
    /// projection functions.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">
    /// The type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two heterogeneous sequences.
    /// Additional arguments specify key selection functions, result
    /// projection functions and a key comparer.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">
    /// The type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var secondKeyAndElements =
                await second.
                    Select(element => (key: secondKeySelector(element), element)).
                    ToArrayAsync(cancellationToken);
            var secondLookup =
                secondKeyAndElements.ToLookup(
                    tuple => tuple.key,
                    tuple => tuple.element,
                    comparer);
            var firstKeys = new HashSet<TKey>(comparer);
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
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

    /// <summary>
    /// Performs a full outer join on two homogeneous sequences.
    /// Additional arguments specify key selection functions and result
    /// projection functions.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector function.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two homogeneous sequences.
    /// Additional arguments specify key selection functions, result
    /// projection functions and a key comparer.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector function.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two heterogeneous sequences.
    /// Additional arguments specify key selection functions and result
    /// projection functions.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">
    /// The type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two heterogeneous sequences.
    /// Additional arguments specify key selection functions, result
    /// projection functions and a key comparer.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">
    /// The type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a full outer join on two homogeneous sequences.
    /// Additional arguments specify key selection functions and result
    /// projection functions.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector function.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> FullJoin<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector)
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

    /// <summary>
    /// Performs a full outer join on two homogeneous sequences.
    /// Additional arguments specify key selection functions, result
    /// projection functions and a key comparer.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector function.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> FullJoin<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector,
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

    /// <summary>
    /// Performs a full outer join on two heterogeneous sequences.
    /// Additional arguments specify key selection functions and result
    /// projection functions.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">
    /// The type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> FullJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector)
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

    /// <summary>
    /// Performs a full outer join on two heterogeneous sequences.
    /// Additional arguments specify key selection functions, result
    /// projection functions and a key comparer.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">
    /// The type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the result elements.</typeparam>
    /// <param name="first">
    /// The first sequence to join fully.</param>
    /// <param name="second">
    /// The second sequence to join fully.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a full
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> FullJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (firstSelector is null) throw new ArgumentNullException(nameof(firstSelector));
        if (secondSelector is null) throw new ArgumentNullException(nameof(secondSelector));
        if (bothSelector is null) throw new ArgumentNullException(nameof(bothSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                firstSelector,
                secondSelector,
                bothSelector,
                comparer,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
            Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
            Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector,
            IEqualityComparer<TKey>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var secondKeyAndElements =
                await second.
                    Select(async (element, cancellationToken) => (key: await secondKeySelector(element, cancellationToken), element)).
                    ToArrayAsync(cancellationToken);
            var secondLookup =
                secondKeyAndElements.ToLookup(
                    tuple => tuple.key,
                    tuple => tuple.element,
                    comparer);
            var firstKeys = new HashSet<TKey>(comparer);
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            {
                var firstKey = await firstKeySelector(firstElement, cancellationToken);
                firstKeys.Add(firstKey);

                using var secondEnumerator = secondLookup[firstKey].GetEnumerator();
                if (!secondEnumerator.MoveNext())
                {
                    secondEnumerator.Dispose();
                    yield return await firstSelector(firstElement, cancellationToken);

                    continue;
                }

                do
                {
                    yield return await bothSelector(firstElement, secondEnumerator.Current, cancellationToken);
                } while (secondEnumerator.MoveNext());
            }

            foreach (var (secondKey, secondElement) in secondKeyAndElements)
            {
                if (!firstKeys.Contains(secondKey))
                {
                    yield return await secondSelector(secondElement, cancellationToken);
                }
            }
        }
    }
}