using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    private static async ValueTask<TSource[]> GetFoldElementsAsync<TSource>(
        IAsyncEnumerable<TSource> source,
        int count,
        CancellationToken cancellationToken)
    {
        var elements = new TSource[count];
        await foreach (var (index, element) in source.Index().AssertCount(count).WithCancellation(cancellationToken))
        {
            elements[index] = element;
        }

        return elements;
    }
}
