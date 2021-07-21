using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Creates a left-aligned sliding window of a given size over the
        /// source sequence.
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
        /// especially as it slides over the end of the sequence.</para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<IList<TSource>> WindowLeft<TSource>(
            this IAsyncEnumerable<TSource> source,
            int size)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            return Core(source, size);

            static async IAsyncEnumerable<IList<TSource>> Core(
                IAsyncEnumerable<TSource> source,
                int size,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var window = new List<TSource>(size);
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    window.Add(element);
                    if (window.Count == size)
                    {
                        var nextWindow = new List<TSource>(size);
                        nextWindow.AddRange(window.Skip(count: 1));
                        yield return window;

                        window = nextWindow;
                    }
                }

                while (window.Count > 0)
                {
                    var nextWindow = new List<TSource>(size);
                    nextWindow.AddRange(window.Skip(count: 1));
                    yield return window;

                    window = nextWindow;
                }
            }
        }
    }
}