using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Immediately executes the given action on each element in the source sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    public static ValueTask ForEachAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Action<TSource> action,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return source.IsKnownEmpty()
            ? new ValueTask()
            : Core(
                source.WithCancellation(cancellationToken),
                action);

        static async ValueTask Core(
            ConfiguredCancelableAsyncEnumerable<TSource> source,
            Action<TSource> action)
        {
            await foreach (var element in source)
            {
                action(element);
            }
        }
    }

    /// <summary>
    /// Immediately executes the given action on each element in the source sequence.
    /// Each element's index is used in the logic of the action.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element; the second parameter
    /// of the action represents the index of the source element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    public static ValueTask ForEachAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Action<TSource, int> action,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return source.IsKnownEmpty()
            ? new ValueTask()
            : Core(
                source.WithCancellation(cancellationToken),
                action);

        static async ValueTask Core(
            ConfiguredCancelableAsyncEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            var index = 0;
            await foreach (var element in source)
            {
                action(element, index++);
            }
        }
    }

    /// <summary>
    /// Immediately executes the given action on each element in the source sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    public static ValueTask ForEachAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask> action,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return source.IsKnownEmpty()
            ? new ValueTask()
            : Core(source, action, cancellationToken);

        static async ValueTask Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, CancellationToken, ValueTask> action,
            CancellationToken cancellationToken)
        {
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                await action(element, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Immediately executes the given action on each element in the source sequence.
    /// Each element's index is used in the logic of the action.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
    /// <param name="source">The sequence of elements</param>
    /// <param name="action">The action to execute on each element; the second parameter
    /// of the action represents the index of the source element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    public static ValueTask ForEachAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, int, CancellationToken, ValueTask> action,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return source.IsKnownEmpty()
            ? new ValueTask()
            : Core(source, action, cancellationToken);

        static async ValueTask Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, int, CancellationToken, ValueTask> action,
            CancellationToken cancellationToken)
        {
            var index = 0;
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                await action(element, index++, cancellationToken);
            }
        }
    }
}

