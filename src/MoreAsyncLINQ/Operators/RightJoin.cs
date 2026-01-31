using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Performs a right outer join on two homogeneous sequences.
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
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a right outer join on two homogeneous sequences.
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
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a right outer join on two heterogeneous sequences.
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
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a right outer join on two heterogeneous sequences.
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
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
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

    /// <summary>
    /// Performs a right outer join on two homogeneous sequences.
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
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Performs a right outer join on two homogeneous sequences.
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
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Performs a right outer join on two heterogeneous sequences.
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
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Performs a right outer join on two heterogeneous sequences.
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
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    [Obsolete($"Use an overload of {nameof(RightJoin)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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
    
    /// <summary>
    /// Performs a right outer join on two homogeneous sequences.
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
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> RightJoin<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector)
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
    
    /// <summary>
    /// Performs a right outer join on two homogeneous sequences.
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
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> RightJoin<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector,
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
    
    /// <summary>
    /// Performs a right outer join on two heterogeneous sequences.
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
    /// <param name="secondSelector">
    /// Function that projects the result given just an element from
    /// <paramref name="second"/> where there is no corresponding element
    /// in <paramref name="first"/>.</param>
    /// <param name="bothSelector">
    /// Function that projects the result given an element from
    /// <paramref name="first"/> and an element from <paramref name="second"/>
    /// that match on a common key.</param>
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> RightJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector)
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
    
    /// <summary>
    /// Performs a right outer join on two heterogeneous sequences.
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
    /// <returns>A sequence containing results projected from a right
    /// outer join of the two input sequences.</returns>
    public static IAsyncEnumerable<TResult> RightJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector,
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
            (secondElement, firstElement, cancellationToken) => bothSelector(firstElement, secondElement, cancellationToken),
            comparer);
    }
}