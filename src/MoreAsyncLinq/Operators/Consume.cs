using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask ConsumeAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask Core(
                IAsyncEnumerable<TSource> source,
                CancellationToken cancellationToken)
            {
                await foreach (var _ in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                }
            }
        }
    }
}