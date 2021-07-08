using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TraverseBreadthFirst<TSource>(
            TSource root,
            Func<TSource, IAsyncEnumerable<TSource>> childrenSelector)
        {
            if (childrenSelector is null) throw new ArgumentNullException(nameof(childrenSelector));

            return Core(root, childrenSelector);

            static async IAsyncEnumerable<TSource> Core(
                TSource root,
                Func<TSource, IAsyncEnumerable<TSource>> childrenSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var queue = new Queue<TSource>();
                queue.Enqueue(root);

                while (queue.Count > 0)
                {
                    var element = queue.Dequeue();
                    yield return element;

                    await foreach (var child in childrenSelector(element).WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }
    }
}