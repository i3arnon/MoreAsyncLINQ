using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

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

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(Array.Empty<TSource>())
            : Core(source.WithCancellation(cancellationToken));

        static async ValueTask<TSource[]> Core(ConfiguredCancelableAsyncEnumerable<TSource> source) 
        {
            var elements = new List<TSource>();

            try
            {
                await foreach (var element in source)
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
                            await asyncDisposable.DisposeAsync();
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