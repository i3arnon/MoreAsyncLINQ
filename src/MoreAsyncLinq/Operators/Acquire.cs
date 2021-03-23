using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<TSource[]> AcquireAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<TSource[]> Core(
                IAsyncEnumerable<TSource> source,
                CancellationToken cancellationToken)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                var elements = new List<TSource>(collectionCount ?? 0);

                try
                {
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        elements.Add(element);
                    }

                    return elements.ToArray();
                }
                catch
                {
                    foreach (var element in elements)
                    {
                        switch (element)
                        {
                            case IAsyncDisposable asyncDisposable:
                                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                                break;
                            case IDisposable disposable:
                                disposable.Dispose();
                                break;
                        }
                    }

                    throw;
                }
            }
        }
    }
}