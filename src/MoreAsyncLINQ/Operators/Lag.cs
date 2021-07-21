using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements prior to the lag offset, <c>default(T)</c> is used as the lagged value.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate lag</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
        public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            Func<TSource, TSource?, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.
                Select(Option.Some).
                Lag(
                    offset,
                    defaultLagValue: default,
                    (elementOption, lagOption) => resultSelector(elementOption.Value, lagOption.OrDefault()));
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate lag</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
        public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLagValue,
            Func<TSource, TSource, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(
                source,
                offset,
                defaultLagValue,
                resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int offset,
                TSource defaultLagValue,
                Func<TSource, TSource, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var queue = new Queue<TSource>(offset);

                var hasMore = await enumerator.MoveNextAsync();
                while (hasMore && offset > 0)
                {
                    queue.Enqueue(enumerator.Current);

                    yield return resultSelector(enumerator.Current, defaultLagValue);

                    hasMore = await enumerator.MoveNextAsync();
                    offset--;
                }

                if (!hasMore)
                {
                    yield break;
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return resultSelector(enumerator.Current, queue.Dequeue());

                    queue.Enqueue(enumerator.Current);
                }
            }
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements prior to the lag offset, <c>default(T)</c> is used as the lagged value.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate lag</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
        public static IAsyncEnumerable<TResult> LagAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            Func<TSource, TSource?, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.
                Select(Option.Some).
                LagAwait(
                    offset,
                    defaultLagValue: default,
                    (elementOption, lagOption) => resultSelector(elementOption.Value, lagOption.OrDefault()));
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate lag</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
        public static IAsyncEnumerable<TResult> LagAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int offset,
            TSource defaultLagValue,
            Func<TSource, TSource, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(
                source,
                offset,
                defaultLagValue,
                resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int offset,
                TSource defaultLagValue,
                Func<TSource, TSource, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var queue = new Queue<TSource>(offset);

                var hasMore = await enumerator.MoveNextAsync();
                while (hasMore && offset > 0)
                {
                    queue.Enqueue(enumerator.Current);

                    yield return await resultSelector(enumerator.Current, defaultLagValue).ConfigureAwait(false);

                    hasMore = await enumerator.MoveNextAsync();
                    offset--;
                }

                if (!hasMore)
                {
                    yield break;
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return await resultSelector(enumerator.Current, queue.Dequeue()).ConfigureAwait(false);

                    queue.Enqueue(enumerator.Current);
                }
            }
        }
    }
}