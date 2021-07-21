using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Returns every N-th element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="step">Number of elements to bypass before returning the next element.</param>
        /// <returns>
        /// A sequence with every N-th element of the input sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> TakeEvery<TSource>(this IAsyncEnumerable<TSource> source, int step)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));

            return source.Where((_, index) => index % step == 0);
        }
    }
}