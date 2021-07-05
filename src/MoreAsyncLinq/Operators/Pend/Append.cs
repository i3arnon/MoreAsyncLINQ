using System;
using System.Collections.Generic;
using MoreAsyncLinq.Pend;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Append<TSource>(this IAsyncEnumerable<TSource> head, TSource tail)
        {
            if (head is null) throw new ArgumentNullException(nameof(head));

            var node = head as PendNode<TSource> ?? PendNode<TSource>.WithSource(head);
            return node.Append(tail);
        }
    }
}