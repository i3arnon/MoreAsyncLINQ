using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MoreAsyncLinq
{
    internal static class AsyncEnumerableExtensions
    {
        public static ValueTask<int?> TryGetCollectionCountAsync<TSource>(
            [NoEnumeration] this IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken)
        {
            return source is IAsyncIListProvider<TSource> asyncIListProvider
                ? Core(asyncIListProvider, cancellationToken)
                : ValueTasks.FromResult((int?) null);

            static async ValueTask<int?> Core(
                IAsyncIListProvider<TSource> asyncIListProvider,
                CancellationToken cancellationToken)
            {
                var count = await asyncIListProvider.GetCountAsync(onlyIfCheap: true, cancellationToken).ConfigureAwait(false);
                return count == -1 ? null : count;
            }
        }
    }
}