using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Asserts that all elements of a sequence meet a given condition
        /// otherwise throws an <see cref="Exception"/> object.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Function that asserts an element of the <paramref name="source"/> sequence for a condition.</param>
        /// <returns>
        /// Returns the original sequence.
        /// </returns>
        /// <exception cref="InvalidOperationException">The input sequence
        /// contains an element that does not meet the condition being
        /// asserted.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> Assert<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate) =>
            source.Assert(predicate, errorSelector: null);

        /// <summary>
        /// Asserts that all elements of a sequence meet a given condition
        /// otherwise throws an <see cref="Exception"/> object.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Function that asserts an element of the input sequence for a condition.</param>
        /// <param name="errorSelector">Function that returns the <see cref="Exception"/> object to throw.</param>
        /// <returns>
        /// Returns the original sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> Assert<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TSource, Exception>? errorSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return Core(
                source,
                predicate,
                errorSelector ?? (static _ => new InvalidOperationException("Sequence contains an invalid item.")));

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, bool> predicate,
                Func<TSource, Exception> errorSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return predicate(element)
                        ? element
                        : throw errorSelector(element);
                }
            }
        }

        /// <summary>
        /// Asserts that all elements of a sequence meet a given condition
        /// otherwise throws an <see cref="Exception"/> object.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Function that asserts an element of the <paramref name="source"/> sequence for a condition.</param>
        /// <returns>
        /// Returns the original sequence.
        /// </returns>
        /// <exception cref="InvalidOperationException">The input sequence
        /// contains an element that does not meet the condition being
        /// asserted.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> AssertAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate) =>
            source.AssertAwait(predicate, errorSelector: null);

        /// <summary>
        /// Asserts that all elements of a sequence meet a given condition
        /// otherwise throws an <see cref="Exception"/> object.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Function that asserts an element of the input sequence for a condition.</param>
        /// <param name="errorSelector">Function that returns the <see cref="Exception"/> object to throw.</param>
        /// <returns>
        /// Returns the original sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> AssertAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, ValueTask<Exception>>? errorSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return Core(
                source,
                predicate,
                errorSelector ?? (static _ => ValueTasks.FromResult<Exception>(new InvalidOperationException("Sequence contains an invalid item."))));

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, ValueTask<bool>> predicate,
                Func<TSource, ValueTask<Exception>> errorSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await predicate(element).ConfigureAwait(false)
                        ? element
                        : throw (await errorSelector(element).ConfigureAwait(false));
                }
            }
        }
    }
}