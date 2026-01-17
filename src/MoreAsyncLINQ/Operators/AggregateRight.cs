using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAsync{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, TSource}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TSource> AggregateRightAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, TSource> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.IsKnownEmpty()
            ? throw new InvalidOperationException("Source sequence doesn't contain any elements.")
            : Core(source, func, cancellationToken);

        static async ValueTask<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            return list.Count == 0
                ? throw new InvalidOperationException("Source sequence doesn't contain any elements.")
                : AggregateRight(
                    list,
                    list[^1],
                    func,
                    list.Count - 1);
        }
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// The specified seed value is used as the initial accumulator value.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAsync{TSource, TAccumulate}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TAccumulate> AggregateRightAsync<TSource, TAccumulate>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, TAccumulate> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(seed)
            : Core(source, seed, func, cancellationToken);

        static async ValueTask<TAccumulate> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, TAccumulate> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken);

            return AggregateRight(
                list,
                seed,
                func,
                list.Count);
        }
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// The specified seed value is used as the initial accumulator value,
    /// and the specified function is used to select the result value.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAsync{TSource, TAccumulate, TResult}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the resulting value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The transformed final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateRightAsync<TSource, TAccumulate, TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, TAccumulate> func,
        Func<TAccumulate, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, seed, func, resultSelector, cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate = await source.AggregateRightAsync(seed, func, cancellationToken);
            return resultSelector(accumulate);
        }
    }

    private static  TResult AggregateRight<TSource, TResult>(
        IReadOnlyList<TSource> list,
        TResult accumulator,
        Func<TSource, TResult, TResult> func,
        int count)
    {
        for (var index = count; index > 0; index--)
        {
            accumulator = func(list[index - 1], accumulator);
        }

        return accumulator;
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAwaitAsync{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, ValueTask{TSource}}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(AggregateRightAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TSource> AggregateRightAwaitAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TSource>> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return Core(source, func, cancellationToken);

        static async ValueTask<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TSource>> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Source sequence doesn't contain any elements.");
            }

            return await list.AggregateRightAwaitAsync(list[list.Count - 1], func, list.Count - 1).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// The specified seed value is used as the initial accumulator value.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAwaitAsync{TSource, TAccumulate}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, ValueTask{TAccumulate}}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(AggregateRightAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TAccumulate> AggregateRightAwaitAsync<TSource, TAccumulate>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return Core(source, seed, func, cancellationToken);

        static async ValueTask<TAccumulate> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            return await list.AggregateRightAwaitAsync(seed, func, list.Count).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// The specified seed value is used as the initial accumulator value,
    /// and the specified function is used to select the result value.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAwaitAsync{TSource, TAccumulate, TResult}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, ValueTask{TAccumulate}}, Func{TAccumulate, ValueTask{TResult}}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the resulting value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The transformed final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(AggregateRightAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateRightAwaitAsync<TSource, TAccumulate, TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
        Func<TAccumulate, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, seed, func, resultSelector, cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, ValueTask<TAccumulate>> func,
            Func<TAccumulate, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate = await source.AggregateRightAwaitAsync(seed, func, cancellationToken).ConfigureAwait(false);
            return await resultSelector(accumulate).ConfigureAwait(false);
        }
    }

    private static async ValueTask<TResult> AggregateRightAwaitAsync<TSource, TResult>(
        this IReadOnlyList<TSource> list,
        TResult accumulator,
        Func<TSource, TResult, ValueTask<TResult>> func,
        int count)
    {
        for (var index = count; index > 0; index--)
        {
            accumulator = await func(list[index - 1], accumulator).ConfigureAwait(false);
        }

        return accumulator;
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAsync{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, CancellationToken, ValueTask{TSource}}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TSource> AggregateRightAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, CancellationToken, ValueTask<TSource>> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.IsKnownEmpty()
            ? throw new InvalidOperationException("Source sequence doesn't contain any elements.")
            : Core(source, func, cancellationToken);

        static async ValueTask<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, CancellationToken, ValueTask<TSource>> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken);
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Source sequence doesn't contain any elements.");
            }

            return await AggregateRightAsync(
                    list,
                    list[^1],
                    func,
                    list.Count - 1,
                    cancellationToken);
        }
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// The specified seed value is used as the initial accumulator value.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAsync{TSource, TAccumulate}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, CancellationToken, ValueTask{TAccumulate}}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TAccumulate> AggregateRightAsync<TSource, TAccumulate>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(seed)
            : Core(
                source,
                seed,
                func,
                cancellationToken);

        static async ValueTask<TAccumulate> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func,
            CancellationToken cancellationToken)
        {
            var list = await source.ToListAsync(cancellationToken);

            return await AggregateRightAsync(
                list,
                seed,
                func,
                list.Count,
                cancellationToken);
        }
    }

    /// <summary>
    /// Applies a right-associative accumulator function over a sequence.
    /// The specified seed value is used as the initial accumulator value,
    /// and the specified function is used to select the result value.
    /// This operator is the right-associative version of the
    /// <see cref="AsyncEnumerable.AggregateAsync{TSource, TAccumulate, TResult}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, CancellationToken, ValueTask{TAccumulate}}, Func{TAccumulate, CancellationToken, ValueTask{TResult}}, CancellationToken)"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the resulting value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The transformed final accumulator value.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateRightAsync<TSource, TAccumulate, TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func,
        Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed,
            func,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func,
            Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate = await source.AggregateRightAsync(seed, func, cancellationToken);
            return await resultSelector(accumulate, cancellationToken);
        }
    }

    private static async ValueTask<TResult> AggregateRightAsync<TSource, TResult>(
        IReadOnlyList<TSource> list,
        TResult accumulator,
        Func<TSource, TResult, CancellationToken, ValueTask<TResult>> func,
        int count,
        CancellationToken cancellationToken)
    {
        for (var index = count; index > 0; index--)
        {
            accumulator = await func(list[index - 1], accumulator, cancellationToken);
        }

        return accumulator;
    }
}