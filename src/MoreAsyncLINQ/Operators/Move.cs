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
        /// Returns a sequence with a range of elements in the source sequence
        /// moved to a new offset.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fromIndex">
        /// The zero-based index identifying the first element in the range of
        /// elements to move.</param>
        /// <param name="count">The count of items to move.</param>
        /// <param name="toIndex">
        /// The index where the specified range will be moved.</param>
        /// <returns>
        /// A sequence with the specified range moved to the new position.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> Move<TSource>(
            this IAsyncEnumerable<TSource> source,
            int fromIndex,
            int count,
            int toIndex)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (fromIndex < 0) throw new ArgumentOutOfRangeException(nameof(fromIndex), $"{nameof(fromIndex)} must be non-negative");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
            if (toIndex < 0) throw new ArgumentOutOfRangeException(nameof(toIndex), $"{nameof(toIndex)} must be non-negative");

            if (toIndex == fromIndex || count == 0)
            {
                return source;
            }

            return toIndex < fromIndex
                ? Core(source, toIndex, fromIndex - toIndex, count)
                : Core(source, fromIndex, count, toIndex - fromIndex);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int bufferStartIndex,
                int bufferSize,
                int bufferYieldIndex,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                var hasMore = true;

                for (var index = 0; index < bufferStartIndex && await MoveNextAsync().ConfigureAwait(false); index++)
                {
                    yield return enumerator.Current;
                }

                var buffer = new TSource[bufferSize];
                var length = 0;
                for (; length < bufferSize && await MoveNextAsync().ConfigureAwait(false); length++)
                {
                    buffer[length] = enumerator.Current;
                }

                for (var index = 0; index < bufferYieldIndex && await MoveNextAsync().ConfigureAwait(false); index++)
                {
                    yield return enumerator.Current;
                }

                for (var index = 0; index < length; index++)
                {
                    yield return buffer[index];
                }

                while (await MoveNextAsync().ConfigureAwait(false))
                {
                    yield return enumerator.Current;
                }

                async ValueTask<bool> MoveNextAsync()
                {
                    if (!hasMore)
                    {
                        return false;
                    }

                    hasMore = await enumerator.MoveNextAsync();
                    return hasMore;
                }
            }
        }
    }
}