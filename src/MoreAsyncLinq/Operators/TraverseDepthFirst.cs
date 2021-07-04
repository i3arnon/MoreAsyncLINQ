using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TraverseDepthFirst<TSource>(
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
                var stack = new Stack<TSource>();
                stack.Push(root);

                while (stack.Count > 0)
                {
                    var element = stack.Pop();
                    yield return element;

                    await foreach (var child in childrenSelector(element).Reverse().WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        stack.Push(child);
                    }
                }
            }
        }
    }
}