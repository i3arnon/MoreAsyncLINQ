using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Performs a left outer join on two homogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a left outer join on two homogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a left outer join on two heterogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a left outer join on two heterogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
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
                static (firstElement, secondElements) => (firstElement, secondElements: secondElements.Select(secondElement => (hasValue: true, value: secondElement))),
                comparer).
            SelectMany(
                tuple => tuple.secondElements.DefaultIfEmpty(),
                (tuple, secondElement) =>
                    secondElement.hasValue
                        ? bothSelector(tuple.firstElement, secondElement.value)
                        : firstSelector(tuple.firstElement));
    }

    /// <summary>
    /// Performs a left outer join on two homogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Performs a left outer join on two homogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Performs a left outer join on two heterogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Performs a left outer join on two heterogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(LeftJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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
    
    /// <summary>
    /// Performs a left outer join on two homogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> LeftJoin<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector)
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
    
    /// <summary>
    /// Performs a left outer join on two homogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element of one of the
    /// sequences to join.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> LeftJoin<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector,
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
    
    /// <summary>
    /// Performs a left outer join on two heterogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> LeftJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector)
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
    
    /// <summary>
    /// Performs a left outer join on two heterogeneous sequences.
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
    /// The first sequence of the join operation.</param>
    /// <param name="second">
    /// The second sequence of the join operation.</param>
    /// <param name="firstKeySelector">
    /// Function that projects the key given an element from <paramref name="first"/>.</param>
    /// <param name="secondKeySelector">
    /// Function that projects the key given an element from <paramref name="second"/>.</param>
    /// <param name="firstSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="first"/> where there is no corresponding element
    /// in <paramref name="second"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> instance used to compare
    /// keys for equality.</param>
    /// <returns>A sequence containing results projected from a left
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> LeftJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector,
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
                firstKeySelector,
                secondKeySelector,
                (firstElement, secondElements, _) => ValueTasks.FromResult((firstElement, secondElements: secondElements.Select(secondElement => (hasValue: true, value: secondElement)))),
                comparer).
            SelectMany(
                (tuple, _) => ValueTasks.FromResult(tuple.secondElements.DefaultIfEmpty()),
                (tuple, secondElement, cancellationToken) =>
                    secondElement.hasValue
                        ? bothSelector(tuple.firstElement, secondElement.value, cancellationToken)
                        : firstSelector(tuple.firstElement, cancellationToken));
    }
}