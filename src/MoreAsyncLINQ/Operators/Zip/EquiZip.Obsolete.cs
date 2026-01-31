#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(EquiZip)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> EquiZipAwait<T1, T2, TResult>(
        IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        Func<T1, T2, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.EquiZipAwaitCore<T1, T2, T2, T2, TResult>(
            second,
            third: null,
            fourth: null,
            (firstElement, secondElement, _, _) => resultSelector(firstElement, secondElement));
    }

    [Obsolete($"Use an overload of {nameof(EquiZip)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> EquiZipAwait<T1, T2, T3, TResult>(
        IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        Func<T1, T2, T3, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.EquiZipAwaitCore<T1, T2, T3, T3, TResult>(
            second,
            third,
            fourth: null,
            (firstElement, secondElement, thirdElement, _) => resultSelector(firstElement, secondElement, thirdElement));
    }

    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. An exception is thrown
    /// if the input sequences are of different lengths.
    /// </summary>
    /// <typeparam name="T1">Type of elements in first sequence</typeparam>
    /// <typeparam name="T2">Type of elements in second sequence</typeparam>
    /// <typeparam name="T3">Type of elements in third sequence</typeparam>
    /// <typeparam name="T4">Type of elements in fourth sequence</typeparam>
    /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
    /// <param name="first">The first sequence.</param>
    /// <param name="second">The second sequence.</param>
    /// <param name="third">The third sequence.</param>
    /// <param name="fourth">The fourth sequence.</param>
    /// <param name="resultSelector">
    /// Function to apply to each quadruplet of elements.</param>
    /// <returns>
    /// A sequence that contains elements of the four input sequences,
    /// combined by <paramref name="resultSelector"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The input sequences are of different lengths.
    /// </exception>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(EquiZip)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> EquiZipAwait<T1, T2, T3, T4, TResult>(
        IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.EquiZipAwaitCore(second, third, fourth, resultSelector);
    }

    private static IAsyncEnumerable<TResult> EquiZipAwaitCore<T1, T2, T3, T4, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3>? third,
        IAsyncEnumerable<T4>? fourth,
        Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector)
    {
        var limit = 1;

        if (third is not null)
        {
            limit++;
        }

        if (fourth is not null)
        {
            limit++;
        }

        return first.ZipAwait(
            second,
            third,
            fourth,
            resultSelector,
            limit,
            EquiZipErrorSelector);
    }
}

