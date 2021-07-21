using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Determines whether the end of the first sequence is equivalent to
        /// the second sequence, using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of elements.</typeparam>
        /// <param name="first">The sequence to check.</param>
        /// <param name="second">The sequence to compare to.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="first" /> ends with elements
        /// equivalent to <paramref name="second" />.
        /// </returns>
        /// <remarks>
        /// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
        /// <see cref="string.EndsWith(string)" /> and
        /// it calls <see cref="IEqualityComparer{T}.Equals(T,T)" /> using
        /// <see cref="EqualityComparer{T}.Default" /> on pairs of elements at
        /// the same index.
        /// </remarks>
        public static ValueTask<bool> EndsWithAsync<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            CancellationToken cancellationToken = default)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));

            return first.EndsWithAsync(second, comparer: null, cancellationToken);
        }

        /// <summary>
        /// Determines whether the end of the first sequence is equivalent to
        /// the second sequence, using the specified element equality comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of elements.</typeparam>
        /// <param name="first">The sequence to check.</param>
        /// <param name="second">The sequence to compare to.</param>
        /// <param name="comparer">Equality comparer to use.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="first" /> ends with elements
        /// equivalent to <paramref name="second" />.
        /// </returns>
        /// <remarks>
        /// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
        /// <see cref="string.EndsWith(string)" /> and it calls
        /// <see cref="IEqualityComparer{T}.Equals(T,T)" /> on pairs of
        /// elements at the same index.
        /// </remarks>
        public static ValueTask<bool> EndsWithAsync<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer,
            CancellationToken cancellationToken = default)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));

            return Core(first, second, comparer, cancellationToken);

            static async ValueTask<bool> Core(
                IAsyncEnumerable<TSource> first,
                IAsyncEnumerable<TSource> second,
                IEqualityComparer<TSource>? comparer,
                CancellationToken cancellationToken)
            {
                var secondCollectionCount = await second.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                if (secondCollectionCount is null)
                {
                    var secondList = await second.ToListAsync(cancellationToken).ConfigureAwait(false);
                    secondCollectionCount = secondList.Count;
                    second = secondList.ToAsyncEnumerable();
                }

                var firstCollectionCount = await first.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                if (firstCollectionCount is not null &&
                    firstCollectionCount < secondCollectionCount.Value)
                {
                    return false;
                }

                comparer ??= EqualityComparer<TSource>.Default;

                await using var firstEnumerator =
                    first.
                        TakeLast(secondCollectionCount.Value).
                        WithCancellation(cancellationToken).
                        ConfigureAwait(false).
                        GetAsyncEnumerator();

                return await second.
                    AllAwaitAsync(
                        async secondElement =>
                            await firstEnumerator.MoveNextAsync() &&
                            comparer.Equals(firstEnumerator.Current, secondElement),
                        cancellationToken).
                    ConfigureAwait(false);
            }
        }
    }
}