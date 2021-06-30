using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<bool> AtMostAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");

            return source.CountBetweenAsync(count + 1, min: 0, count, cancellationToken);
        }
    }
}