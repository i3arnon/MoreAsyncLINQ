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
    /// Performs a Full Group Join between the <paramref name="first"/> and <paramref name="second"/> sequences.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// The results are yielded in the order of the elements found in the first sequence
    /// followed by those found only in the second. In addition, the callback responsible
    /// for projecting the results is supplied with sequences which preserve their source order.
    /// </remarks>
    /// <typeparam name="TFirst">The type of the elements in the first input sequence</typeparam>
    /// <typeparam name="TSecond">The type of the elements in the second input sequence</typeparam>
    /// <typeparam name="TKey">The type of the key to use to join</typeparam>
    /// <param name="first">First sequence</param>
    /// <param name="second">Second sequence</param>
    /// <param name="firstKeySelector">The mapping from first sequence to key</param>
    /// <param name="secondKeySelector">The mapping from second sequence to key</param>
    /// <param name="comparer">The equality comparer to use to determine whether keys are equal.
    /// If null, the default equality comparer for <c>TKey</c> is used.</param>
    /// <returns>A sequence of elements joined from <paramref name="first"/> and <paramref name="second"/>.
    /// </returns>
    public static IAsyncEnumerable<(TKey Key, IEnumerable<TFirst> First, IEnumerable<TSecond> Second)> FullGroupJoin<TFirst, TSecond, TKey>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, TKey> firstKeySelector,
        Func<TSecond, TKey> secondKeySelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));

        return first.FullGroupJoin(
            second,
            firstKeySelector,
            secondKeySelector,
            ValueTuple.Create,
            comparer);
    }

    /// <summary>
    /// Performs a full group-join between two sequences.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// The results are yielded in the order of the elements found in the first sequence
    /// followed by those found only in the second. In addition, the callback responsible
    /// for projecting the results is supplied with sequences which preserve their source order.
    /// </remarks>
    /// <typeparam name="TFirst">The type of the elements in the first input sequence</typeparam>
    /// <typeparam name="TSecond">The type of the elements in the second input sequence</typeparam>
    /// <typeparam name="TKey">The type of the key to use to join</typeparam>
    /// <typeparam name="TResult">The type of the elements of the resulting sequence</typeparam>
    /// <param name="first">First sequence</param>
    /// <param name="second">Second sequence</param>
    /// <param name="firstKeySelector">The mapping from first sequence to key</param>
    /// <param name="secondKeySelector">The mapping from second sequence to key</param>
    /// <param name="resultSelector">Function to apply to each pair of elements plus the key</param>
    /// <param name="comparer">The equality comparer to use to determine whether keys are equal.
    /// If null, the default equality comparer for <c>TKey</c> is used.</param>
    /// <returns>A sequence of elements joined from <paramref name="first"/> and <paramref name="second"/>.
    /// </returns>
    public static IAsyncEnumerable<TResult> FullGroupJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, TKey> firstKeySelector,
        Func<TSecond, TKey> secondKeySelector,
        Func<TKey, IEnumerable<TFirst>, IEnumerable<TSecond>, TResult> resultSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TKey, IEnumerable<TFirst>, IEnumerable<TSecond>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var firstLookup = await Lookup<TKey, TFirst>.CreateForJoinAsync(first, firstKeySelector, comparer, cancellationToken);
            var secondLookup = await Lookup<TKey, TSecond>.CreateForJoinAsync(second, secondKeySelector, comparer, cancellationToken);
            
            foreach (var firstGrouping in firstLookup)
            {
                yield return resultSelector(
                    firstGrouping.Key,
                    firstGrouping,
                    secondLookup[firstGrouping.Key]);
            }
            
            foreach (var secondGrouping in secondLookup)
            {
                if (!firstLookup.Contains(secondGrouping.Key))
                {
                    yield return resultSelector(
                        secondGrouping.Key,
                        [],
                        secondGrouping);
                }
            }
        }
    }

    /// <summary>
    /// Performs a Full Group Join between the <paramref name="first"/> and <paramref name="second"/> sequences.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// The results are yielded in the order of the elements found in the first sequence
    /// followed by those found only in the second. In addition, the callback responsible
    /// for projecting the results is supplied with sequences which preserve their source order.
    /// </remarks>
    /// <typeparam name="TFirst">The type of the elements in the first input sequence</typeparam>
    /// <typeparam name="TSecond">The type of the elements in the second input sequence</typeparam>
    /// <typeparam name="TKey">The type of the key to use to join</typeparam>
    /// <param name="first">First sequence</param>
    /// <param name="second">Second sequence</param>
    /// <param name="firstKeySelector">The mapping from first sequence to key</param>
    /// <param name="secondKeySelector">The mapping from second sequence to key</param>
    /// <param name="comparer">The equality comparer to use to determine whether keys are equal.
    /// If null, the default equality comparer for <c>TKey</c> is used.</param>
    /// <returns>A sequence of elements joined from <paramref name="first"/> and <paramref name="second"/>.
    /// </returns>
    public static IAsyncEnumerable<(TKey Key, IEnumerable<TFirst> First, IEnumerable<TSecond> Second)> FullGroupJoin<TFirst, TSecond, TKey>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));

        return first.FullGroupJoin(
            second,
            firstKeySelector,
            secondKeySelector,
            static (key, firstElements, secondElements, _) => ValueTasks.FromResult((key, firstElements, secondElements)),
            comparer);
    }

    /// <summary>
    /// Performs a full group-join between two sequences.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// The results are yielded in the order of the elements found in the first sequence
    /// followed by those found only in the second. In addition, the callback responsible
    /// for projecting the results is supplied with sequences which preserve their source order.
    /// </remarks>
    /// <typeparam name="TFirst">The type of the elements in the first input sequence</typeparam>
    /// <typeparam name="TSecond">The type of the elements in the second input sequence</typeparam>
    /// <typeparam name="TKey">The type of the key to use to join</typeparam>
    /// <typeparam name="TResult">The type of the elements of the resulting sequence</typeparam>
    /// <param name="first">First sequence</param>
    /// <param name="second">Second sequence</param>
    /// <param name="firstKeySelector">The mapping from first sequence to key</param>
    /// <param name="secondKeySelector">The mapping from second sequence to key</param>
    /// <param name="resultSelector">Function to apply to each pair of elements plus the key</param>
    /// <param name="comparer">The equality comparer to use to determine whether keys are equal.
    /// If null, the default equality comparer for <c>TKey</c> is used.</param>
    /// <returns>A sequence of elements joined from <paramref name="first"/> and <paramref name="second"/>.
    /// </returns>
    public static IAsyncEnumerable<TResult> FullGroupJoin<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TKey, IEnumerable<TFirst>, IEnumerable<TSecond>, CancellationToken, ValueTask<TResult>> resultSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (firstKeySelector is null) throw new ArgumentNullException(nameof(firstKeySelector));
        if (secondKeySelector is null) throw new ArgumentNullException(nameof(secondKeySelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() && 
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            Func<TKey, IEnumerable<TFirst>, IEnumerable<TSecond>, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var firstLookup = await Lookup<TKey, TFirst>.CreateForJoinAsync(first, firstKeySelector, comparer, cancellationToken);
            var secondLookup = await Lookup<TKey, TSecond>.CreateForJoinAsync(second, secondKeySelector, comparer, cancellationToken);
            
            foreach (var firstGrouping in firstLookup)
            {
                yield return await resultSelector(
                    firstGrouping.Key,
                    firstGrouping,
                    secondLookup[firstGrouping.Key],
                    cancellationToken);
            }
            
            foreach (var secondGrouping in secondLookup)
            {
                if (!firstLookup.Contains(secondGrouping.Key))
                {
                    yield return await resultSelector(
                        secondGrouping.Key,
                        [],
                        secondGrouping,
                        cancellationToken);
                }
            }
        }
    }
}

