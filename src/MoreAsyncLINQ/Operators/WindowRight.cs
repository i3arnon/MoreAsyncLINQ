using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Creates a right-aligned sliding window over the source sequence
    /// of a given size.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// The sequence over which to create the sliding window.</param>
    /// <param name="size">Size of the sliding window.</param>
    /// <returns>A sequence representing each sliding window.</returns>
    /// <remarks>
    /// <para>
    /// A window can contain fewer elements than <paramref name="size"/>,
    /// especially as it slides over the start of the sequence.</para>
    /// <para>
    /// This operator uses deferred execution and streams its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<IList<TSource>> WindowRight<TSource>(
        this IAsyncEnumerable<TSource> source,
        int size)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<IList<TSource>>()
            : Core(source, size);

        static async IAsyncEnumerable<IList<TSource>> Core(
            IAsyncEnumerable<TSource> source,
            int size,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var window = new List<TSource>(size);
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                window.Add(element);

                var nextWindow = new List<TSource>(size);
                nextWindow.AddRange(window.Count == size ? window.Skip(count: 1) : window);
                yield return window;

                window = nextWindow;
            }
        }
    }
}