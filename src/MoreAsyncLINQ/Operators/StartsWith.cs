using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<bool> StartsWithAsync<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            CancellationToken cancellationToken = default)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));

            return first.StartsWithAsync(second, comparer: null, cancellationToken);
        }

        public static ValueTask<bool> StartsWithAsync<TSource>(
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
                if (await first.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false) is { } firstCollectionCount &&
                    await second.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false) is { } secondCollectionCount &&
                    secondCollectionCount > firstCollectionCount)
                {
                    return false;
                }

                comparer ??= EqualityComparer<TSource>.Default;

                await using var firstEnumerator =
                    first.
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