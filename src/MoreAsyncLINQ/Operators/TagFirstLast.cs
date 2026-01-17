using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns a sequence resulting from applying a function to each
    /// element in the source sequence with additional parameters
    /// indicating whether the element is the first and/or last of the
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="resultSelector">A function that determines how to
    /// project the each element along with its first or last tag.</param>
    /// <returns>
    /// Returns the resulting sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> TagFirstLast<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool, bool, TResult> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.Index().CountDown(
            count: 1,
            (indexedElement, countDownCount) =>
                resultSelector(
                    indexedElement.Item,
                    indexedElement.Index == 0,
                    countDownCount == 0));
    }

    /// <summary>
    /// Returns a sequence resulting from applying a function to each
    /// element in the source sequence with additional parameters
    /// indicating whether the element is the first and/or last of the
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="resultSelector">A function that determines how to
    /// project the each element along with its first or last tag.</param>
    /// <returns>
    /// Returns the resulting sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> TagFirstLastAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool, bool, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.Index().CountDownAwait(
            count: 1,
            (indexedElement, countDownCount) =>
                resultSelector(
                    indexedElement.Item,
                    indexedElement.Index == 0,
                    countDownCount == 0));
    }
    
    /// <summary>
    /// Returns a sequence resulting from applying a function to each
    /// element in the source sequence with additional parameters
    /// indicating whether the element is the first and/or last of the
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="resultSelector">A function that determines how to
    /// project the each element along with its first or last tag.</param>
    /// <returns>
    /// Returns the resulting sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> TagFirstLast<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool, bool, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.
            Index().
            CountDown(
                count: 1,
                (indexedElement, countDownCount, cancellationToken) =>
                    resultSelector(
                        indexedElement.Item,
                        indexedElement.Index == 0,
                        countDownCount == 0,
                        cancellationToken));
    }
}