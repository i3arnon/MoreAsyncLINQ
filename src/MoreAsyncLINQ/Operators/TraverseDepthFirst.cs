﻿using System;
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
        /// Traverses a tree in a depth-first fashion, starting at a root node
        /// and using a user-defined function to get the children at each node
        /// of the tree.
        /// </summary>
        /// <typeparam name="TSource">The tree node type.</typeparam>
        /// <param name="root">The root of the tree to traverse.</param>
        /// <param name="childrenSelector">
        /// The function that produces the children of each element.</param>
        /// <returns>A sequence containing the traversed values.</returns>
        /// <remarks>
        /// <para>
        /// The tree is not checked for loops. If the resulting sequence needs
        /// to be finite then it is the responsibility of
        /// <paramref name="childrenSelector"/> to ensure that loops are not
        /// produced.</para>
        /// <para>
        /// This function defers traversal until needed and streams the
        /// results.</para>
        /// </remarks>
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