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
    /// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
    /// This operator is the right-associative version of the
    /// <see cref="MoreAsyncEnumerable.Scan{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="func">
    /// A right-associative accumulator function to be invoked on each element.
    /// Its first argument is the current value in the sequence; second argument is the previous accumulator value.
    /// </param>
    /// <returns>The scanned sequence.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
    /// </remarks>
    public static IAsyncEnumerable<TSource> ScanRight<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, TSource> func)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.ScanRight(
            func,
            list =>
                list.Count > 0
                    ? (list[list.Count - 1], list.Count - 1)
                    : null);
    }

    /// <summary>
    /// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
    /// The specified seed value is used as the initial accumulator value.
    /// This operator is the right-associative version of the
    /// <see cref="MoreAsyncEnumerable.Scan{TSource, TState}(IAsyncEnumerable{TSource}, TState, Func{TState, TSource, TState})"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <returns>The scanned sequence.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
    /// </remarks>
    public static IAsyncEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, TAccumulate> func)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.ScanRight(func, list => (seed, list.Count));
    }

    private static async IAsyncEnumerable<TResult> ScanRight<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TResult, TResult> func,
        Func<List<TSource>, (TResult seed, int count)?> seeder,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

        if (seeder(list) is not var (accumulator, count))
        {
            yield break;
        }

        var stack = new Stack<TResult>(count + 1);
        stack.Push(accumulator);
        while (count > 0)
        {
            count--;
            accumulator = func(list[count], accumulator);
            stack.Push(accumulator);
        }

        foreach (var result in stack)
        {
            yield return result;
        }
    }

    /// <summary>
    /// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
    /// This operator is the right-associative version of the
    /// <see cref="MoreAsyncEnumerable.ScanAwait{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, ValueTask{TSource}})"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="func">
    /// A right-associative accumulator function to be invoked on each element.
    /// Its first argument is the current value in the sequence; second argument is the previous accumulator value.
    /// </param>
    /// <returns>The scanned sequence.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
    /// </remarks>
    public static IAsyncEnumerable<TSource> ScanRightAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TSource>> func)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.ScanRightAwait(
            func,
            list =>
                list.Count > 0
                    ? (list[list.Count - 1], list.Count - 1)
                    : null);
    }

    /// <summary>
    /// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
    /// The specified seed value is used as the initial accumulator value.
    /// This operator is the right-associative version of the
    /// <see cref="MoreAsyncEnumerable.ScanAwait{TSource, TState}(IAsyncEnumerable{TSource}, TState, Func{TState, TSource, ValueTask{TState}})"/> LINQ operator.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
    /// <returns>The scanned sequence.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
    /// </remarks>
    public static IAsyncEnumerable<TAccumulate> ScanRightAwait<TSource, TAccumulate>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TSource, TAccumulate, ValueTask<TAccumulate>> func)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (func is null) throw new ArgumentNullException(nameof(func));

        return source.ScanRightAwait(func, list => (seed, list.Count));
    }

    private static async IAsyncEnumerable<TResult> ScanRightAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TResult, ValueTask<TResult>> func,
        Func<List<TSource>, (TResult seed, int count)?> seeder,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

        if (seeder(list) is not var (accumulator, count))
        {
            yield break;
        }

        var stack = new Stack<TResult>(count + 1);
        stack.Push(accumulator);
        while (count > 0)
        {
            count--;
            accumulator = await func(list[count], accumulator).ConfigureAwait(false);
            stack.Push(accumulator);
        }

        foreach (var result in stack)
        {
            yield return result;
        }
    }
}