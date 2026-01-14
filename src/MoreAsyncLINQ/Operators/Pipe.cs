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
    /// Executes the given action on each element in the source sequence
    /// and yields it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element</param>
    /// <returns>A sequence with source elements in their original order.</returns>
    /// <remarks>
    /// The returned sequence is essentially a duplicate of
    /// the original, but with the extra action being executed while the
    /// sequence is evaluated. The action is always taken before the element
    /// is yielded, so any changes made by the action will be visible in the
    /// returned sequence. This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Pipe<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(source, action, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Action<TSource> action,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                action(element);

                yield return element;
            }
        }
    }

    /// <summary>
    /// Executes the given action on each element in the source sequence
    /// and yields it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element</param>
    /// <returns>A sequence with source elements in their original order.</returns>
    /// <remarks>
    /// The returned sequence is essentially a duplicate of
    /// the original, but with the extra action being executed while the
    /// sequence is evaluated. The action is always taken before the element
    /// is yielded, so any changes made by the action will be visible in the
    /// returned sequence. This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PipeAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> action)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return Core(source, action);

        static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                await action(element).ConfigureAwait(false);
                    
                yield return element;
            }
        }
    }

    /// <summary>
    /// Executes the given action on each element in the source sequence
    /// and yields it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element</param>
    /// <returns>A sequence with source elements in their original order.</returns>
    /// <remarks>
    /// The returned sequence is essentially a duplicate of
    /// the original, but with the extra action being executed while the
    /// sequence is evaluated. The action is always taken before the element
    /// is yielded, so any changes made by the action will be visible in the
    /// returned sequence. This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Pipe<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask> action)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));
        
        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(source, action, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, CancellationToken, ValueTask> action,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                await action(element, cancellationToken);

                yield return element;
            }
        }
    }
}