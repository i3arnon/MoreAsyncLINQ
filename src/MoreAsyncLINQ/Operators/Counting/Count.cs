using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    private static async ValueTask<int> CountAsync<TSource>(
        ConfiguredCancelableAsyncEnumerable<TSource> source,
        int limit)
    {
        await using var enumerator = source.GetAsyncEnumerator();

        var count = 0;
        while (count < limit && await enumerator.MoveNextAsync())
        {
            count++;
        }

        return count;
    }
}