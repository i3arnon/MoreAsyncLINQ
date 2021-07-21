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
        /// Bypasses a specified number of elements at the end of the sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">The number of elements to bypass at the end of the source sequence.</param>
        /// <returns>
        /// An <see cref="IAsyncEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
        /// </returns>
        public static IAsyncEnumerable<TSource> SkipLast<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return count <= 0
                ? source
                : Core(source, count);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int count,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                var result =
                    collectionCount is null
                        ? source.
                            CountDown(count).
                            TakeWhile(tuple => tuple.Countdown is null).
                            Select(tuple => tuple.Element)
                        : source.Take(collectionCount.Value - count);

                await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return element;
                }
            }
        }
    }
}