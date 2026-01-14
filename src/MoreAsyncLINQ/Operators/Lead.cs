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
    /// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
    /// </summary>
    /// <remarks>
    /// This operator evaluates in a deferred and streaming manner.<br/>
    /// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
    /// default(T) is used as the lead value.<br/>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
    /// <param name="source">The sequence over which to evaluate Lead</param>
    /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
    /// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
    /// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
    public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int offset,
        Func<TSource, TSource?, TResult> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.
            Select(Option.Some).
            Lead(
                offset,
                defaultLeadValue: default,
                (elementOption, leadOption) => resultSelector(elementOption.Value, leadOption.OrDefault()));
    }

    /// <summary>
    /// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
    /// </summary>
    /// <remarks>
    /// This operator evaluates in a deferred and streaming manner.<br/>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
    /// <param name="source">The sequence over which to evaluate Lead</param>
    /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
    /// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
    /// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
    /// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
    public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int offset,
        TSource defaultLeadValue,
        Func<TSource, TSource, TResult> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, offset, defaultLeadValue, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLeadValue,
            Func<TSource, TSource, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            var queue = new Queue<TSource>(offset);

            var hasMore = await enumerator.MoveNextAsync();
            while (hasMore && queue.Count < offset)
            {
                queue.Enqueue(enumerator.Current);
                hasMore = await enumerator.MoveNextAsync();
            }

            while (hasMore)
            {
                yield return resultSelector(queue.Dequeue(), enumerator.Current);

                queue.Enqueue(enumerator.Current);
                hasMore = await enumerator.MoveNextAsync();
            }

            while (queue.Count > 0)
            {
                yield return resultSelector(queue.Dequeue(), defaultLeadValue);
            }
        }
    }

    /// <summary>
    /// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
    /// </summary>
    /// <remarks>
    /// This operator evaluates in a deferred and streaming manner.<br/>
    /// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
    /// default(T) is used as the lead value.<br/>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
    /// <param name="source">The sequence over which to evaluate Lead</param>
    /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
    /// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
    /// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
    public static IAsyncEnumerable<TResult> LeadAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int offset,
        Func<TSource, TSource?, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.
            Select(Option.Some).
            LeadAwait(
                offset,
                defaultLeadValue: default,
                (elementOption, leadOption) => resultSelector(elementOption.Value, leadOption.OrDefault()));
    }

    /// <summary>
    /// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
    /// </summary>
    /// <remarks>
    /// This operator evaluates in a deferred and streaming manner.<br/>
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
    /// <param name="source">The sequence over which to evaluate Lead</param>
    /// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
    /// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
    /// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
    /// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
    public static IAsyncEnumerable<TResult> LeadAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        int offset,
        TSource defaultLeadValue,
        Func<TSource, TSource, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, offset, defaultLeadValue, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLeadValue,
            Func<TSource, TSource, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            var queue = new Queue<TSource>(offset);

            var hasMore = await enumerator.MoveNextAsync();
            while (hasMore && queue.Count < offset)
            {
                queue.Enqueue(enumerator.Current);
                hasMore = await enumerator.MoveNextAsync();
            }

            while (hasMore)
            {
                yield return await resultSelector(queue.Dequeue(), enumerator.Current).ConfigureAwait(false);

                queue.Enqueue(enumerator.Current);
                hasMore = await enumerator.MoveNextAsync();
            }

            while (queue.Count > 0)
            {
                yield return await resultSelector(queue.Dequeue(), defaultLeadValue).ConfigureAwait(false);
            }
        }
    }
}