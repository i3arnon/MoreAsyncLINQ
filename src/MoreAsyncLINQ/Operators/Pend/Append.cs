using System;
using System.Collections.Generic;
using MoreAsyncLINQ.Pend;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Returns a sequence consisting of the head elements and the given tail element.
        /// </summary>
        /// <typeparam name="TSource">Type of sequence</typeparam>
        /// <param name="head">All elements of the head. Must not be null.</param>
        /// <param name="tail">Tail element of the new sequence.</param>
        /// <returns>A sequence consisting of the head elements and the given tail element.</returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>
        public static IAsyncEnumerable<TSource> Append<TSource>(this IAsyncEnumerable<TSource> head, TSource tail)
        {
            if (head is null) throw new ArgumentNullException(nameof(head));

            var node = head as PendNode<TSource> ?? PendNode<TSource>.WithSource(head);
            return node.Append(tail);
        }
    }
}