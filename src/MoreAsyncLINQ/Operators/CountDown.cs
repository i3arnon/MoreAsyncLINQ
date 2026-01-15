using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Provides a countdown counter for a given count of elements at the
    /// tail of the sequence where zero always represents the last element,
    /// one represents the second-last element, two represents the
    /// third-last element and so on.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements of <paramref name="source"/></typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Count of tail elements of
    /// <paramref name="source"/> to count down.</param>
    /// <returns>A sequence of <see cref="ValueTuple{T1,T2}"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. At most, <paramref name="count"/> elements of the source
    /// sequence may be buffered at any one time unless
    /// <paramref name="source"/> is a collection or a list.
    /// </remarks>
    public static IAsyncEnumerable<(int? Countdown, TSource Element)> CountDown<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.CountDown(
            count,
            static (element, count) => (count, element));
    }

    /// <summary>
    /// Provides a countdown counter for a given count of elements at the
    /// tail of the sequence where zero always represents the last element,
    /// one represents the second-last element, two represents the
    /// third-last element and so on.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TResult">
    /// The type of elements of the resulting sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Count of tail elements of
    /// <paramref name="source"/> to count down.</param>
    /// <param name="resultSelector">
    /// A function that receives the element and the current countdown
    /// value for the element and which returns those mapped to a
    /// result returned in the resulting sequence. For elements before
    /// the last <paramref name="count"/>, the countdown value is
    /// <c>null</c>.</param>
    /// <returns>
    /// A sequence of results returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. At most, <paramref name="count"/> elements of the source
    /// sequence may be buffered at any one time unless
    /// <paramref name="source"/> is a collection or a list.
    /// </remarks>
    public static IAsyncEnumerable<TResult> CountDown<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, int?, TResult> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                source,
                count,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, int?, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var queue = new Queue<TSource>(Max(1, count + 1));
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                queue.Enqueue(element);
                if (queue.Count > count)
                {
                    yield return resultSelector(queue.Dequeue(), null);
                }
            }

            while (queue.Count > 0)
            {
                yield return resultSelector(queue.Dequeue(), queue.Count);
            }
        }
    }

    /// <summary>
    /// Provides a countdown counter for a given count of elements at the
    /// tail of the sequence where zero always represents the last element,
    /// one represents the second-last element, two represents the
    /// third-last element and so on.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TResult">
    /// The type of elements of the resulting sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Count of tail elements of
    /// <paramref name="source"/> to count down.</param>
    /// <param name="resultSelector">
    /// A function that receives the element and the current countdown
    /// value for the element and which returns those mapped to a
    /// result returned in the resulting sequence. For elements before
    /// the last <paramref name="count"/>, the countdown value is
    /// <c>null</c>.</param>
    /// <returns>
    /// A sequence of results returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. At most, <paramref name="count"/> elements of the source
    /// sequence may be buffered at any one time unless
    /// <paramref name="source"/> is a collection or a list.
    /// </remarks>
    public static IAsyncEnumerable<TResult> CountDownAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, int?, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            count,
            resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, int?, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
            if (collectionCount is not null)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(element, collectionCount <= count ? collectionCount : null).ConfigureAwait(false);

                    collectionCount--;
                }
            }
            else
            {
                var queue = new Queue<TSource>(Max(1, count + 1));
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    queue.Enqueue(element);
                    if (queue.Count > count)
                    {
                        yield return await resultSelector(queue.Dequeue(), null).ConfigureAwait(false);
                    }
                }

                while (queue.Count > 0)
                {
                    yield return await resultSelector(queue.Dequeue(), queue.Count).ConfigureAwait(false);
                }
            }
        }
    }
    
    /// <summary>
    /// Provides a countdown counter for a given count of elements at the
    /// tail of the sequence where zero always represents the last element,
    /// one represents the second-last element, two represents the
    /// third-last element and so on.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TResult">
    /// The type of elements of the resulting sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Count of tail elements of
    /// <paramref name="source"/> to count down.</param>
    /// <param name="resultSelector">
    /// A function that receives the element and the current countdown
    /// value for the element and which returns those mapped to a
    /// result returned in the resulting sequence. For elements before
    /// the last <paramref name="count"/>, the countdown value is
    /// <c>null</c>.</param>
    /// <returns>
    /// A sequence of results returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. At most, <paramref name="count"/> elements of the source
    /// sequence may be buffered at any one time unless
    /// <paramref name="source"/> is a collection or a list.
    /// </remarks>
    public static IAsyncEnumerable<TResult> CountDown<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, int?, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                source,
                count,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, int?, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var queue = new Queue<TSource>(Max(1, count + 1));
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                queue.Enqueue(element);
                if (queue.Count > count)
                {
                    yield return await resultSelector(queue.Dequeue(), null, cancellationToken);
                }
            }

            while (queue.Count > 0)
            {
                yield return await resultSelector(queue.Dequeue(), queue.Count, cancellationToken);
            }
        }
    }
}