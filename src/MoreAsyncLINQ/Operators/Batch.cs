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
        /// Batches the source sequence into sized buckets.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <returns>A sequence of equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// When more than one bucket is streamed, all buckets except the last
        /// is guaranteed to have <paramref name="size"/> elements. The last
        /// bucket may be smaller depending on the remaining elements in the
        /// <paramref name="source"/> sequence.</para>
        /// <para>
        /// Each bucket is pre-allocated to <paramref name="size"/> elements.
        /// If <paramref name="size"/> is set to a very large value, e.g.
        /// <see cref="int.MaxValue"/> to effectively disable batching by just
        /// hoping for a single bucket, then it can lead to memory exhaustion
        /// (<see cref="OutOfMemoryException"/>).
        /// </para>
        /// </remarks>
        public static IAsyncEnumerable<TSource[]> Batch<TSource>(this IAsyncEnumerable<TSource> source, int size) => 
            source.Batch(size, static result => result);

        /// <summary>
        /// Batches the source sequence into sized buckets and applies a projection to each bucket.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="resultSelector">The projection to apply to each bucket.</param>
        /// <returns>A sequence of projections on equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// When more than one bucket is streamed, all buckets except the last
        /// is guaranteed to have <paramref name="size"/> elements. The last
        /// bucket may be smaller depending on the remaining elements in the
        /// <paramref name="source"/> sequence.</para>
        /// Each bucket is pre-allocated to <paramref name="size"/> elements.
        /// If <paramref name="size"/> is set to a very large value, e.g.
        /// <see cref="int.MaxValue"/> to effectively disable batching by just
        /// hoping for a single bucket, then it can lead to memory exhaustion
        /// (<see cref="OutOfMemoryException"/>).
        /// </para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> Batch<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int size,
            Func<TSource[], TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, size, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int size,
                Func<TSource[], TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var count = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                switch (count)
                {
                    case 0:
                        yield break;
                    case > 0 when count <= size:
                        size = count.Value;
                        break;
                }

                var index = 0;
                TSource[]? batch = null;
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (batch is null)
                    {
                        batch = new TSource[size];
                        index = 0;
                    }

                    batch[index] = element;
                    index++;
                    if (index == size)
                    {
                        yield return resultSelector(batch);

                        batch = null;
                    }
                }

                if (batch is not null && index > 0)
                {
                    Array.Resize(ref batch, index);
                    yield return resultSelector(batch);
                }
            }
        }

        /// <summary>
        /// Batches the source sequence into sized buckets and applies a projection to each bucket.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="resultSelector">The projection to apply to each bucket.</param>
        /// <returns>A sequence of projections on equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results
        /// (buckets are streamed but their content buffered).</para>
        /// <para>
        /// <para>
        /// When more than one bucket is streamed, all buckets except the last
        /// is guaranteed to have <paramref name="size"/> elements. The last
        /// bucket may be smaller depending on the remaining elements in the
        /// <paramref name="source"/> sequence.</para>
        /// Each bucket is pre-allocated to <paramref name="size"/> elements.
        /// If <paramref name="size"/> is set to a very large value, e.g.
        /// <see cref="int.MaxValue"/> to effectively disable batching by just
        /// hoping for a single bucket, then it can lead to memory exhaustion
        /// (<see cref="OutOfMemoryException"/>).
        /// </para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> BatchAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int size,
            Func<TSource[], ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, size, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                int size,
                Func<TSource[], ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var count = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                switch (count)
                {
                    case 0:
                        yield break;
                    case > 0 when count <= size:
                        size = count.Value;
                        break;
                }

                var index = 0;
                TSource[]? batch = null;
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (batch is null)
                    {
                        batch = new TSource[size];
                        index = 0;
                    }

                    batch[index] = element;
                    index++;
                    if (index == size)
                    {
                        yield return await resultSelector(batch).ConfigureAwait(false);

                        batch = null;
                    }
                }

                if (batch is not null && index > 0)
                {
                    Array.Resize(ref batch, index);
                    yield return await resultSelector(batch).ConfigureAwait(false);
                }
            }
        }
    }
}