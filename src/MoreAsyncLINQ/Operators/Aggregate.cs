using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Applies two accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        Func<TAccumulate1, TAccumulate2, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            Func<TAccumulate1, TAccumulate2, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2);
        }
    }

    /// <summary>
    /// Applies three accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2,
                    seed3))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                seed3,
                accumulator3,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
                accumulate3 = accumulator3(accumulate3, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2,
                accumulate3);
        }
    }

    /// <summary>
    /// Applies four accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2,
                    seed3,
                    seed4))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                seed3,
                accumulator3,
                seed4,
                accumulator4,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
                accumulate3 = accumulator3(accumulate3, element);
                accumulate4 = accumulator4(accumulate4, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2,
                accumulate3,
                accumulate4);
        }
    }

    /// <summary>
    /// Applies five accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2,
                    seed3,
                    seed4,
                    seed5))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                seed3,
                accumulator3,
                seed4,
                accumulator4,
                seed5,
                accumulator5,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
                accumulate3 = accumulator3(accumulate3, element);
                accumulate4 = accumulator4(accumulate4, element);
                accumulate5 = accumulator5(accumulate5, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2,
                accumulate3,
                accumulate4,
                accumulate5);
        }
    }

    /// <summary>
    /// Applies six accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, TAccumulate6> accumulator6,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2,
                    seed3,
                    seed4,
                    seed5,
                    seed6))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                seed3,
                accumulator3,
                seed4,
                accumulator4,
                seed5,
                accumulator5,
                seed6,
                accumulator6,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, TAccumulate6> accumulator6,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
                accumulate3 = accumulator3(accumulate3, element);
                accumulate4 = accumulator4(accumulate4, element);
                accumulate5 = accumulator5(accumulate5, element);
                accumulate6 = accumulator6(accumulate6, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2,
                accumulate3,
                accumulate4,
                accumulate5,
                accumulate6);
        }
    }

    /// <summary>
    /// Applies seven accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TAccumulate7,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, TAccumulate6> accumulator6,
        TAccumulate7 seed7,
        Func<TAccumulate7, TSource, TAccumulate7> accumulator7,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (accumulator7 is null) throw new ArgumentNullException(nameof(accumulator7));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2,
                    seed3,
                    seed4,
                    seed5,
                    seed6,
                    seed7))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                seed3,
                accumulator3,
                seed4,
                accumulator4,
                seed5,
                accumulator5,
                seed6,
                accumulator6,
                seed7,
                accumulator7,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, TAccumulate6> accumulator6,
            TAccumulate7 seed7,
            Func<TAccumulate7, TSource, TAccumulate7> accumulator7,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;
            var accumulate7 = seed7;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
                accumulate3 = accumulator3(accumulate3, element);
                accumulate4 = accumulator4(accumulate4, element);
                accumulate5 = accumulator5(accumulate5, element);
                accumulate6 = accumulator6(accumulate6, element);
                accumulate7 = accumulator7(accumulate7, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2,
                accumulate3,
                accumulate4,
                accumulate5,
                accumulate6,
                accumulate7);
        }
    }

    /// <summary>
    /// Applies eight accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TAccumulate8">The type of eighth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="seed8">The seed value for the eighth accumulator.</param>
    /// <param name="accumulator8">The eighth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TAccumulate7,
        TAccumulate8,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, TAccumulate6> accumulator6,
        TAccumulate7 seed7,
        Func<TAccumulate7, TSource, TAccumulate7> accumulator7,
        TAccumulate8 seed8,
        Func<TAccumulate8, TSource, TAccumulate8> accumulator8,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (accumulator7 is null) throw new ArgumentNullException(nameof(accumulator7));
        if (accumulator8 is null) throw new ArgumentNullException(nameof(accumulator8));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? ValueTasks.FromResult(
                resultSelector(
                    seed1,
                    seed2,
                    seed3,
                    seed4,
                    seed5,
                    seed6,
                    seed7,
                    seed8))
            : Core(
                source,
                seed1,
                accumulator1,
                seed2,
                accumulator2,
                seed3,
                accumulator3,
                seed4,
                accumulator4,
                seed5,
                accumulator5,
                seed6,
                accumulator6,
                seed7,
                accumulator7,
                seed8,
                accumulator8,
                resultSelector,
                cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, TAccumulate1> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, TAccumulate2> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, TAccumulate3> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, TAccumulate4> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, TAccumulate5> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, TAccumulate6> accumulator6,
            TAccumulate7 seed7,
            Func<TAccumulate7, TSource, TAccumulate7> accumulator7,
            TAccumulate8 seed8,
            Func<TAccumulate8, TSource, TAccumulate8> accumulator8,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;
            var accumulate7 = seed7;
            var accumulate8 = seed8;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = accumulator1(accumulate1, element);
                accumulate2 = accumulator2(accumulate2, element);
                accumulate3 = accumulator3(accumulate3, element);
                accumulate4 = accumulator4(accumulate4, element);
                accumulate5 = accumulator5(accumulate5, element);
                accumulate6 = accumulator6(accumulate6, element);
                accumulate7 = accumulator7(accumulate7, element);
                accumulate8 = accumulator8(accumulate8, element);
            }

            return resultSelector(
                accumulate1,
                accumulate2,
                accumulate3,
                accumulate4,
                accumulate5,
                accumulate6,
                accumulate7,
                accumulate8);
        }
    }

    /// <summary>
    /// Applies two accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        Func<TAccumulate1, TAccumulate2, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            Func<TAccumulate1, TAccumulate2, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2).
                ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies three accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
                accumulate3 = await accumulator3(accumulate3, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3).
                ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies four accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
                accumulate3 = await accumulator3(accumulate3, element).ConfigureAwait(false);
                accumulate4 = await accumulator4(accumulate4, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4).
                ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies five accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
                accumulate3 = await accumulator3(accumulate3, element).ConfigureAwait(false);
                accumulate4 = await accumulator4(accumulate4, element).ConfigureAwait(false);
                accumulate5 = await accumulator5(accumulate5, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5).
                ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies six accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, ValueTask<TAccumulate6>> accumulator6,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            seed6,
            accumulator6,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, ValueTask<TAccumulate6>> accumulator6,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
                accumulate3 = await accumulator3(accumulate3, element).ConfigureAwait(false);
                accumulate4 = await accumulator4(accumulate4, element).ConfigureAwait(false);
                accumulate5 = await accumulator5(accumulate5, element).ConfigureAwait(false);
                accumulate6 = await accumulator6(accumulate6, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    accumulate6).
                ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies seven accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TAccumulate7,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, ValueTask<TAccumulate6>> accumulator6,
        TAccumulate7 seed7,
        Func<TAccumulate7, TSource, ValueTask<TAccumulate7>> accumulator7,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (accumulator7 is null) throw new ArgumentNullException(nameof(accumulator7));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            seed6,
            accumulator6,
            seed7,
            accumulator7,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, ValueTask<TAccumulate6>> accumulator6,
            TAccumulate7 seed7,
            Func<TAccumulate7, TSource, ValueTask<TAccumulate7>> accumulator7,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;
            var accumulate7 = seed7;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
                accumulate3 = await accumulator3(accumulate3, element).ConfigureAwait(false);
                accumulate4 = await accumulator4(accumulate4, element).ConfigureAwait(false);
                accumulate5 = await accumulator5(accumulate5, element).ConfigureAwait(false);
                accumulate6 = await accumulator6(accumulate6, element).ConfigureAwait(false);
                accumulate7 = await accumulator7(accumulate7, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    accumulate6,
                    accumulate7).
                ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Applies eight accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TAccumulate8">The type of eighth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="seed8">The seed value for the eighth accumulator.</param>
    /// <param name="accumulator8">The eighth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TAccumulate7,
        TAccumulate8,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, ValueTask<TAccumulate6>> accumulator6,
        TAccumulate7 seed7,
        Func<TAccumulate7, TSource, ValueTask<TAccumulate7>> accumulator7,
        TAccumulate8 seed8,
        Func<TAccumulate8, TSource, ValueTask<TAccumulate8>> accumulator8,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (accumulator7 is null) throw new ArgumentNullException(nameof(accumulator7));
        if (accumulator8 is null) throw new ArgumentNullException(nameof(accumulator8));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            seed6,
            accumulator6,
            seed7,
            accumulator7,
            seed8,
            accumulator8,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, ValueTask<TAccumulate5>> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, ValueTask<TAccumulate6>> accumulator6,
            TAccumulate7 seed7,
            Func<TAccumulate7, TSource, ValueTask<TAccumulate7>> accumulator7,
            TAccumulate8 seed8,
            Func<TAccumulate8, TSource, ValueTask<TAccumulate8>> accumulator8,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;
            var accumulate7 = seed7;
            var accumulate8 = seed8;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                accumulate1 = await accumulator1(accumulate1, element).ConfigureAwait(false);
                accumulate2 = await accumulator2(accumulate2, element).ConfigureAwait(false);
                accumulate3 = await accumulator3(accumulate3, element).ConfigureAwait(false);
                accumulate4 = await accumulator4(accumulate4, element).ConfigureAwait(false);
                accumulate5 = await accumulator5(accumulate5, element).ConfigureAwait(false);
                accumulate6 = await accumulator6(accumulate6, element).ConfigureAwait(false);
                accumulate7 = await accumulator7(accumulate7, element).ConfigureAwait(false);
                accumulate8 = await accumulator8(accumulate8, element).ConfigureAwait(false);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    accumulate6,
                    accumulate7,
                    accumulate8).
                ConfigureAwait(false);
        }
    }
    
    /// <summary>
    /// Applies two accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        Func<TAccumulate1, TAccumulate2, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            Func<TAccumulate1, TAccumulate2, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    cancellationToken);
        }
    }
    
    /// <summary>
    /// Applies three accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
                accumulate3 = await accumulator3(accumulate3, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    cancellationToken);
        }
    }
    
    /// <summary>
    /// Applies four accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
                accumulate3 = await accumulator3(accumulate3, element, cancellationToken);
                accumulate4 = await accumulator4(accumulate4, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    cancellationToken);
        }
    }
    
    /// <summary>
    /// Applies five accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
                accumulate3 = await accumulator3(accumulate3, element, cancellationToken);
                accumulate4 = await accumulator4(accumulate4, element, cancellationToken);
                accumulate5 = await accumulator5(accumulate5, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    cancellationToken);
        }
    }
    
    /// <summary>
    /// Applies six accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, CancellationToken, ValueTask<TAccumulate6>> accumulator6,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            seed6,
            accumulator6,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, CancellationToken, ValueTask<TAccumulate6>> accumulator6,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
                accumulate3 = await accumulator3(accumulate3, element, cancellationToken);
                accumulate4 = await accumulator4(accumulate4, element, cancellationToken);
                accumulate5 = await accumulator5(accumulate5, element, cancellationToken);
                accumulate6 = await accumulator6(accumulate6, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    accumulate6,
                    cancellationToken);
        }
    }
    
    /// <summary>
    /// Applies seven accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TAccumulate7,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, CancellationToken, ValueTask<TAccumulate6>> accumulator6,
        TAccumulate7 seed7,
        Func<TAccumulate7, TSource, CancellationToken, ValueTask<TAccumulate7>> accumulator7,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (accumulator7 is null) throw new ArgumentNullException(nameof(accumulator7));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            seed6,
            accumulator6,
            seed7,
            accumulator7,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, CancellationToken, ValueTask<TAccumulate6>> accumulator6,
            TAccumulate7 seed7,
            Func<TAccumulate7, TSource, CancellationToken, ValueTask<TAccumulate7>> accumulator7,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;
            var accumulate7 = seed7;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
                accumulate3 = await accumulator3(accumulate3, element, cancellationToken);
                accumulate4 = await accumulator4(accumulate4, element, cancellationToken);
                accumulate5 = await accumulator5(accumulate5, element, cancellationToken);
                accumulate6 = await accumulator6(accumulate6, element, cancellationToken);
                accumulate7 = await accumulator7(accumulate7, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    accumulate6,
                    accumulate7,
                    cancellationToken);
        }
    }

    /// <summary>
    /// Applies eight accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TAccumulate8">The type of eighth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="seed8">The seed value for the eighth accumulator.</param>
    /// <param name="accumulator8">The eighth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>
    public static ValueTask<TResult> AggregateAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TAccumulate7,
        TAccumulate8,
        TResult>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate1 seed1,
        Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
        TAccumulate2 seed2,
        Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
        TAccumulate3 seed3,
        Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
        TAccumulate4 seed4,
        Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
        TAccumulate5 seed5,
        Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
        TAccumulate6 seed6,
        Func<TAccumulate6, TSource, CancellationToken, ValueTask<TAccumulate6>> accumulator6,
        TAccumulate7 seed7,
        Func<TAccumulate7, TSource, CancellationToken, ValueTask<TAccumulate7>> accumulator7,
        TAccumulate8 seed8,
        Func<TAccumulate8, TSource, CancellationToken, ValueTask<TAccumulate8>> accumulator8,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, CancellationToken, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (accumulator1 is null) throw new ArgumentNullException(nameof(accumulator1));
        if (accumulator2 is null) throw new ArgumentNullException(nameof(accumulator2));
        if (accumulator3 is null) throw new ArgumentNullException(nameof(accumulator3));
        if (accumulator4 is null) throw new ArgumentNullException(nameof(accumulator4));
        if (accumulator5 is null) throw new ArgumentNullException(nameof(accumulator5));
        if (accumulator6 is null) throw new ArgumentNullException(nameof(accumulator6));
        if (accumulator7 is null) throw new ArgumentNullException(nameof(accumulator7));
        if (accumulator8 is null) throw new ArgumentNullException(nameof(accumulator8));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(
            source,
            seed1,
            accumulator1,
            seed2,
            accumulator2,
            seed3,
            accumulator3,
            seed4,
            accumulator4,
            seed5,
            accumulator5,
            seed6,
            accumulator6,
            seed7,
            accumulator7,
            seed8,
            accumulator8,
            resultSelector,
            cancellationToken);

        static async ValueTask<TResult> Core(
            IAsyncEnumerable<TSource> source,
            TAccumulate1 seed1,
            Func<TAccumulate1, TSource, CancellationToken, ValueTask<TAccumulate1>> accumulator1,
            TAccumulate2 seed2,
            Func<TAccumulate2, TSource, CancellationToken, ValueTask<TAccumulate2>> accumulator2,
            TAccumulate3 seed3,
            Func<TAccumulate3, TSource, CancellationToken, ValueTask<TAccumulate3>> accumulator3,
            TAccumulate4 seed4,
            Func<TAccumulate4, TSource, CancellationToken, ValueTask<TAccumulate4>> accumulator4,
            TAccumulate5 seed5,
            Func<TAccumulate5, TSource, CancellationToken, ValueTask<TAccumulate5>> accumulator5,
            TAccumulate6 seed6,
            Func<TAccumulate6, TSource, CancellationToken, ValueTask<TAccumulate6>> accumulator6,
            TAccumulate7 seed7,
            Func<TAccumulate7, TSource, CancellationToken, ValueTask<TAccumulate7>> accumulator7,
            TAccumulate8 seed8,
            Func<TAccumulate8, TSource, CancellationToken, ValueTask<TAccumulate8>> accumulator8,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken)
        {
            var accumulate1 = seed1;
            var accumulate2 = seed2;
            var accumulate3 = seed3;
            var accumulate4 = seed4;
            var accumulate5 = seed5;
            var accumulate6 = seed6;
            var accumulate7 = seed7;
            var accumulate8 = seed8;

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                accumulate1 = await accumulator1(accumulate1, element, cancellationToken);
                accumulate2 = await accumulator2(accumulate2, element, cancellationToken);
                accumulate3 = await accumulator3(accumulate3, element, cancellationToken);
                accumulate4 = await accumulator4(accumulate4, element, cancellationToken);
                accumulate5 = await accumulator5(accumulate5, element, cancellationToken);
                accumulate6 = await accumulator6(accumulate6, element, cancellationToken);
                accumulate7 = await accumulator7(accumulate7, element, cancellationToken);
                accumulate8 = await accumulator8(accumulate8, element, cancellationToken);
            }

            return await resultSelector(
                    accumulate1,
                    accumulate2,
                    accumulate3,
                    accumulate4,
                    accumulate5,
                    accumulate6,
                    accumulate7,
                    accumulate8,
                    cancellationToken);
        }
    }
}