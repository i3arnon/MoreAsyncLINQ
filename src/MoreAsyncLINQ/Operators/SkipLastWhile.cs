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
    /// Removes elements from the end of a sequence as long as a specified condition is true.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The predicate to use to remove items from the tail of the sequence.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This operator uses deferred execution and streams its results. At any given time, it
    /// will buffer as many consecutive elements as satisfied by <paramref name="predicate"/>.
    /// </remarks>
    public static IAsyncEnumerable<TSource> SkipLastWhile<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(source, predicate, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Queue<TSource>? queue = null;
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                if (predicate(element))
                {
                    queue ??= new Queue<TSource>();
                    queue.Enqueue(element);
                }
                else
                {
                    while (queue is { Count: > 0 })
                    {
                        yield return queue.Dequeue();
                    }
                    yield return element;
                }
            }
        }
    }

    /// <summary>
    /// Removes elements from the end of a sequence as long as a specified condition is true.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The predicate to use to remove items from the tail of the sequence.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This operator uses deferred execution and streams its results. At any given time, it
    /// will buffer as many consecutive elements as satisfied by <paramref name="predicate"/>.
    /// </remarks>
    public static IAsyncEnumerable<TSource> SkipLastWhile<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(source, predicate, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, CancellationToken, ValueTask<bool>> predicate,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Queue<TSource>? queue = null;
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                if (await predicate(element, cancellationToken))
                {
                    queue ??= new Queue<TSource>();
                    queue.Enqueue(element);
                }
                else
                {
                    while (queue is { Count: > 0 })
                    {
                        yield return queue.Dequeue();
                    }

                    yield return element;
                }
            }
        }
    }
}

