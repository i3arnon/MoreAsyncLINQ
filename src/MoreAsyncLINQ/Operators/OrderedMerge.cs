using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Merges two ordered sequences into one. Where the elements equal
    /// in both sequences, the element from the first sequence is
    /// returned in the resulting sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in input and output sequences.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <returns>
    /// A sequence with elements from the two input sequences merged, as
    /// in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered as inputs.
    /// </remarks>
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

    /// <summary>
    /// Merges two ordered sequences into one with an additional
    /// parameter specifying how to compare the elements of the
    /// sequences. Where the elements equal in both sequences, the
    /// element from the first sequence is returned in the resulting
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in input and output sequences.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    /// A sequence with elements from the two input sequences merged, as
    /// in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered as inputs.
    /// </remarks>
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

    /// <summary>
    /// Merges two ordered sequences into one with an additional
    /// parameter specifying the element key by which the sequences are
    /// ordered. Where the keys equal in both sequences, the
    /// element from the first sequence is returned in the resulting
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in input and output sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <returns>
    /// A sequence with elements from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
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

    /// <summary>
    /// Merges two ordered sequences into one. Additional parameters
    /// specify the element key by which the sequences are ordered,
    /// the result when element is found in first sequence but not in
    /// the second, the result when element is found in second sequence
    /// but not in the first and the result when elements are found in
    /// both sequences.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
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

    /// <summary>
    /// Merges two ordered sequences into one. Additional parameters
    /// specify the element key by which the sequences are ordered,
    /// the result when element is found in first sequence but not in
    /// the second, the result when element is found in second sequence
    /// but not in the first, the result when elements are found in
    /// both sequences and a method for comparing keys.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
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

    /// <summary>
    /// Merges two heterogeneous sequences ordered by a common key type
    /// into a homogeneous one. Additional parameters specify the
    /// element key by which the sequences are ordered, the result when
    /// element is found in first sequence but not in the second and
    /// the result when element is found in second sequence but not in
    /// the first, the result when elements are found in both sequences.
    /// </summary>
    /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="firstKeySelector">Function to extract a key given an
    /// element from the first sequence.</param>
    /// <param name="secondKeySelector">Function to extract a key given an
    /// element from the second sequence.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
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

    /// <summary>
    /// Merges two heterogeneous sequences ordered by a common key type
    /// into a homogeneous one. Additional parameters specify the
    /// element key by which the sequences are ordered, the result when
    /// element is found in first sequence but not in the second,
    /// the result when element is found in second sequence but not in
    /// the first, the result when elements are found in both sequences
    /// and a method for comparing keys.
    /// </summary>
    /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="firstKeySelector">Function to extract a key given an
    /// element from the first sequence.</param>
    /// <param name="secondKeySelector">Function to extract a key given an
    /// element from the second sequence.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
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
                comparer ?? Comparer<TKey>.Default,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var firstEnumerator = first.WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var secondEnumerator = second.WithCancellation(cancellationToken).GetAsyncEnumerator();

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

    /// <summary>
    /// Merges two ordered sequences into one with an additional
    /// parameter specifying the element key by which the sequences are
    /// ordered. Where the keys equal in both sequences, the
    /// element from the first sequence is returned in the resulting
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in input and output sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <returns>
    /// A sequence with elements from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Merges two ordered sequences into one. Additional parameters
    /// specify the element key by which the sequences are ordered,
    /// the result when element is found in first sequence but not in
    /// the second, the result when element is found in second sequence
    /// but not in the first and the result when elements are found in
    /// both sequences.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Merges two ordered sequences into one. Additional parameters
    /// specify the element key by which the sequences are ordered,
    /// the result when element is found in first sequence but not in
    /// the second, the result when element is found in second sequence
    /// but not in the first, the result when elements are found in
    /// both sequences and a method for comparing keys.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Merges two heterogeneous sequences ordered by a common key type
    /// into a homogeneous one. Additional parameters specify the
    /// element key by which the sequences are ordered, the result when
    /// element is found in first sequence but not in the second and
    /// the result when element is found in second sequence but not in
    /// the first, the result when elements are found in both sequences.
    /// </summary>
    /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="firstKeySelector">Function to extract a key given an
    /// element from the first sequence.</param>
    /// <param name="secondKeySelector">Function to extract a key given an
    /// element from the second sequence.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Merges two heterogeneous sequences ordered by a common key type
    /// into a homogeneous one. Additional parameters specify the
    /// element key by which the sequences are ordered, the result when
    /// element is found in first sequence but not in the second,
    /// the result when element is found in second sequence but not in
    /// the first, the result when elements are found in both sequences
    /// and a method for comparing keys.
    /// </summary>
    /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="firstKeySelector">Function to extract a key given an
    /// element from the first sequence.</param>
    /// <param name="secondKeySelector">Function to extract a key given an
    /// element from the second sequence.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(OrderedMerge)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    /// <summary>
    /// Merges two ordered sequences into one with an additional
    /// parameter specifying the element key by which the sequences are
    /// ordered. Where the keys equal in both sequences, the
    /// element from the first sequence is returned in the resulting
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in input and output sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <returns>
    /// A sequence with elements from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    public static IAsyncEnumerable<TSource> OrderedMerge<TSource, TKey>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return first.OrderedMerge(
            second,
            keySelector,
            keySelector,
            (source, _) => ValueTasks.FromResult(source),
            (source, _) => ValueTasks.FromResult(source),
            (firstElement, _, _) => ValueTasks.FromResult(firstElement),
            comparer: null);
    }

    /// <summary>
    /// Merges two ordered sequences into one. Additional parameters
    /// specify the element key by which the sequences are ordered,
    /// the result when element is found in first sequence but not in
    /// the second, the result when element is found in second sequence
    /// but not in the first and the result when elements are found in
    /// both sequences.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    public static IAsyncEnumerable<TResult> OrderedMerge<TSource, TKey, TResult>(
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

        return first.OrderedMerge(
            second,
            keySelector,
            keySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    /// <summary>
    /// Merges two ordered sequences into one. Additional parameters
    /// specify the element key by which the sequences are ordered,
    /// the result when element is found in first sequence but not in
    /// the second, the result when element is found in second sequence
    /// but not in the first, the result when elements are found in
    /// both sequences and a method for comparing keys.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequences.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="keySelector">Function to extract a key given an element.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    public static IAsyncEnumerable<TResult> OrderedMerge<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSource, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> bothSelector,
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

    /// <summary>
    /// Merges two heterogeneous sequences ordered by a common key type
    /// into a homogeneous one. Additional parameters specify the
    /// element key by which the sequences are ordered, the result when
    /// element is found in first sequence but not in the second and
    /// the result when element is found in second sequence but not in
    /// the first, the result when elements are found in both sequences.
    /// </summary>
    /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="firstKeySelector">Function to extract a key given an
    /// element from the first sequence.</param>
    /// <param name="secondKeySelector">Function to extract a key given an
    /// element from the second sequence.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    public static IAsyncEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
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

        return first.OrderedMerge(
            second,
            firstKeySelector,
            secondKeySelector,
            firstSelector,
            secondSelector,
            bothSelector,
            comparer: null);
    }

    /// <summary>
    /// Merges two heterogeneous sequences ordered by a common key type
    /// into a homogeneous one. Additional parameters specify the
    /// element key by which the sequences are ordered, the result when
    /// element is found in first sequence but not in the second,
    /// the result when element is found in second sequence but not in
    /// the first, the result when elements are found in both sequences
    /// and a method for comparing keys.
    /// </summary>
    /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
    /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
    /// <param name="first">The first input sequence.</param>
    /// <param name="second">The second input sequence.</param>
    /// <param name="firstKeySelector">Function to extract a key given an
    /// element from the first sequence.</param>
    /// <param name="secondKeySelector">Function to extract a key given an
    /// element from the second sequence.</param>
    /// <param name="firstSelector">Function to project the result element
    /// when only the first sequence yields a source element.</param>
    /// <param name="secondSelector">Function to project the result element
    /// when only the second sequence yields a source element.</param>
    /// <param name="bothSelector">Function to project the result element
    /// when only both sequences yield a source element whose keys are
    /// equal.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    /// A sequence with projections from the two input sequences merged
    /// according to a key, as in a full outer join.</returns>
    /// <remarks>
    /// This method uses deferred execution. The behavior is undefined
    /// if the sequences are unordered (by key) as inputs.
    /// </remarks>
    public static IAsyncEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
        Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
        Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector,
        IComparer<TKey>? comparer)
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
                comparer ?? Comparer<TKey>.Default,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TFirst> first,
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            Func<TFirst, CancellationToken, ValueTask<TResult>> firstSelector,
            Func<TSecond, CancellationToken, ValueTask<TResult>> secondSelector,
            Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> bothSelector,
            IComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var firstEnumerator = first.WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var secondEnumerator = second.WithCancellation(cancellationToken).GetAsyncEnumerator();

            var hasFirstElement = await firstEnumerator.MoveNextAsync();
            var hasSecondElement = await secondEnumerator.MoveNextAsync();

            while (hasFirstElement || hasSecondElement)
            {
                switch (hasFirstElement, hasSecondElement)
                {
                    case (true, true):
                        var firstElement = firstEnumerator.Current;
                        var firstKey = await firstKeySelector(firstElement, cancellationToken);
                        var secondElement = secondEnumerator.Current;
                        var secondKey = await secondKeySelector(secondElement, cancellationToken);
                        switch (comparer.Compare(firstKey, secondKey))
                        {
                            case < 0:
                                yield return await firstSelector(firstElement, cancellationToken);

                                hasFirstElement = await firstEnumerator.MoveNextAsync();
                                break;
                            case > 0:
                                yield return await secondSelector(secondElement, cancellationToken);

                                hasSecondElement = await secondEnumerator.MoveNextAsync();
                                break;
                            case 0:
                                yield return await bothSelector(firstElement, secondElement,  cancellationToken);

                                hasFirstElement = await firstEnumerator.MoveNextAsync();
                                hasSecondElement = await secondEnumerator.MoveNextAsync();
                                break;
                        }

                        break;
                    case (false, true):
                        yield return await secondSelector(secondEnumerator.Current,  cancellationToken);

                        hasSecondElement = await secondEnumerator.MoveNextAsync();
                        break;
                    case (true, false):
                        yield return await firstSelector(firstEnumerator.Current,   cancellationToken);

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