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
        /// Performs a scan (inclusive prefix sum) on a sequence of elements.
        /// </summary>
        /// <remarks>
        /// An inclusive prefix sum returns an equal-length sequence where the
        /// N-th element is the sum of the first N input elements. More
        /// generally, the scan allows any commutative binary operation, not
        /// just a sum.
        /// The exclusive version of Scan is <see cref="MoreAsyncEnumerable.PreScan{TSource}"/>.
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="transformation">Transformation operation</param>
        /// <returns>The scanned sequence</returns>
        public static IAsyncEnumerable<TSource> Scan<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource> transformation)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, transformation);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource> transformation,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }

                var seed = enumerator.Current;
                yield return seed;

                while (await enumerator.MoveNextAsync())
                {
                    seed = transformation(seed, enumerator.Current);
                    yield return seed;
                }
            }
        }

        /// <summary>
        /// Like <see cref="AsyncEnumerable.AggregateAsync{TSource}"/> except returns
        /// the sequence of intermediate results as well as the final one.
        /// An additional parameter specifies a seed.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <typeparam name="TState">Type of state</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="seed">Initial state to seed</param>
        /// <param name="transformation">Transformation operation</param>
        /// <returns>The scanned sequence</returns>
        public static IAsyncEnumerable<TState> Scan<TSource, TState>(
            this IAsyncEnumerable<TSource> source,
            TState seed,
            Func<TState, TSource, TState> transformation)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, seed, transformation);

            static async IAsyncEnumerable<TState> Core(
                IAsyncEnumerable<TSource> source,
                TState seed,
                Func<TState, TSource, TState> transformation,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                yield return seed;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    seed = transformation(seed, element);
                    yield return seed;
                }
            }
        }

        /// <summary>
        /// Performs a scan (inclusive prefix sum) on a sequence of elements.
        /// </summary>
        /// <remarks>
        /// An inclusive prefix sum returns an equal-length sequence where the
        /// N-th element is the sum of the first N input elements. More
        /// generally, the scan allows any commutative binary operation, not
        /// just a sum.
        /// The exclusive version of Scan is <see cref="MoreAsyncEnumerable.PreScan{TSource}"/>.
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="transformation">Transformation operation</param>
        /// <returns>The scanned sequence</returns>
        public static IAsyncEnumerable<TSource> ScanAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TSource>> transformation)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, transformation);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, ValueTask<TSource>> transformation,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }

                var seed = enumerator.Current;
                yield return seed;

                while (await enumerator.MoveNextAsync())
                {
                    seed = await transformation(seed, enumerator.Current).ConfigureAwait(false);
                    yield return seed;
                }
            }
        }

        /// <summary>
        /// Like <see cref="AsyncEnumerable.AggregateAsync{TSource}"/> except returns
        /// the sequence of intermediate results as well as the final one.
        /// An additional parameter specifies a seed.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <typeparam name="TState">Type of state</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="seed">Initial state to seed</param>
        /// <param name="transformation">Transformation operation</param>
        /// <returns>The scanned sequence</returns>
        public static IAsyncEnumerable<TState> ScanAwait<TSource, TState>(
            this IAsyncEnumerable<TSource> source,
            TState seed,
            Func<TState, TSource, ValueTask<TState>> transformation)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, seed, transformation);

            static async IAsyncEnumerable<TState> Core(
                IAsyncEnumerable<TSource> source,
                TState seed,
                Func<TState, TSource, ValueTask<TState>> transformation,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                yield return seed;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    seed = await transformation(seed, element).ConfigureAwait(false);
                    yield return seed;
                }
            }
        }
    }
}