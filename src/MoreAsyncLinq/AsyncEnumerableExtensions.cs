using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MoreAsyncLinq
{
    internal static class AsyncEnumerableExtensions
    {
        public static ValueTask<int?> TryGetCollectionCountAsync<TSource>([NoEnumeration] this IAsyncEnumerable<TSource> source)
        {
            return source is IAsyncIListProvider<TSource> asyncIListProvider
                ? Core(asyncIListProvider)
                : new ValueTask<int?>((int?) null);

            static async ValueTask<int?> Core(IAsyncIListProvider<TSource> asyncIListProvider)
            {
                var count = await asyncIListProvider.GetCountAsync(onlyIfCheap: true, cancellationToken: default).ConfigureAwait(false);
                return count == -1 ? (int?) null : count;
            }
        }
    }
}