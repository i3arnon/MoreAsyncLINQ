#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TResult>(
        IAsyncEnumerable<TSource> source,
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

    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TResult>(
        IAsyncEnumerable<TSource> source,
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

    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TResult>(
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

    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TResult>(
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

    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult> AggregateAwaitAsync<
        TSource,
        TAccumulate1,
        TAccumulate2,
        TAccumulate3,
        TAccumulate4,
        TAccumulate5,
        TAccumulate6,
        TResult>(
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

    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    [Obsolete($"Use an overload of {nameof(AggregateAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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
}
