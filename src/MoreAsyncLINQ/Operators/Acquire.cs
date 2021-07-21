using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Ensures that a source sequence of <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>
        /// objects are all acquired successfully. If the acquisition of any
        /// one fails then those successfully
        /// acquired till that point are disposed.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence of <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/> objects.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// Returns an array of all the acquired <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>
        /// objects in source order.
        /// </returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>
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