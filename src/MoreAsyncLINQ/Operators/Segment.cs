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
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element begins a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        public static IAsyncEnumerable<IEnumerable<TSource>> Segment<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.Segment((current, _, _) => newSegmentPredicate(current));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        public static IAsyncEnumerable<IEnumerable<TSource>> Segment<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int, bool> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.Segment((current, _, index) => newSegmentPredicate(current, index));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given current element, previous element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        public static IAsyncEnumerable<IEnumerable<TSource>> Segment<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, int, bool> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return Core(source, newSegmentPredicate);

            static async IAsyncEnumerable<IEnumerable<TSource>> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, int, bool> newSegmentPredicate,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }

                var previous = enumerator.Current;
                var segment = new List<TSource> { previous };
                var index = 0;
                while (await enumerator.MoveNextAsync())
                {
                    var current = enumerator.Current;
                    index++;
                    if (newSegmentPredicate(current, previous, index))
                    {
                        yield return segment;

                        segment = new List<TSource>();
                    }

                    segment.Add(current);
                    previous = current;
                }

                yield return segment;
            }
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element begins a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.SegmentAwait((current, _, _) => newSegmentPredicate(current));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int, ValueTask<bool>> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.SegmentAwait((current, _, index) => newSegmentPredicate(current, index));
        }

        /// <summary>
        /// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The sequence to segment</param>
        /// <param name="newSegmentPredicate">A function, which returns <c>true</c> if the given current element, previous element or index indicate a new segment, and <c>false</c> otherwise</param>
        /// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
        /// </exception>
        public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, int, ValueTask<bool>> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return Core(source, newSegmentPredicate);

            static async IAsyncEnumerable<IEnumerable<TSource>> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, int, ValueTask<bool>> newSegmentPredicate,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }

                var previous = enumerator.Current;
                var segment = new List<TSource> { previous };
                var index = 0;
                while (await enumerator.MoveNextAsync())
                {
                    var current = enumerator.Current;
                    index++;
                    if (await newSegmentPredicate(current, previous, index).ConfigureAwait(false))
                    {
                        yield return segment;

                        segment = new List<TSource>();
                    }

                    segment.Add(current);
                    previous = current;
                }

                yield return segment;
            }
        }
    }
}