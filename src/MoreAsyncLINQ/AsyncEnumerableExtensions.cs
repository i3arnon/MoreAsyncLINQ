using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MoreAsyncLINQ;

internal static class AsyncEnumerableExtensions
{
    [Obsolete]
    public static ValueTask<int?> TryGetCollectionCountAsync<TSource>(
        [NoEnumeration] this IAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken) =>
        ValueTasks.FromResult((int?)null);
}