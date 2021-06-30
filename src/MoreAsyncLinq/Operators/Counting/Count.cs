using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        private static async ValueTask<int> CountAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int limit,
            CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            var count = 0;
            while (count < limit && await enumerator.MoveNextAsync())
            {
                count++;
            }

            return count;
        }
    }
}