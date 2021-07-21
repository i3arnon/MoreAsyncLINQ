using System;
using System.Collections.Generic;
using MoreAsyncLINQ.Pend;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Prepends a single value to a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to prepend to.</param>
        /// <param name="value">The value to prepend.</param>
        /// <returns>
        /// Returns a sequence where a value is prepended to it.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IAsyncEnumerable<TSource> Prepend<TSource>(this IAsyncEnumerable<TSource> source, TSource value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var node = source as PendNode<TSource> ?? PendNode<TSource>.WithSource(source);
            return node.Prepend(value);
        }
    }
}