#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(ZipShortest)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> ZipShortestAwait<T1, T2, TResult>(
        IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        Func<T1, T2, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.ZipAwait<T1, T2, T2, T2, TResult>(
            second,
            third: null,
            fourth: null,
            (firstElement, secondElement, _, _) => resultSelector(firstElement, secondElement),
            limit: 0);
    }

    [Obsolete($"Use an overload of {nameof(ZipShortest)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> ZipShortestAwait<T1, T2, T3, TResult>(
        IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        Func<T1, T2, T3, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.ZipAwait<T1, T2, T3, T3, TResult>(
            second,
            third,
            fourth: null,
            (firstElement, secondElement, thirdElement, _) => resultSelector(firstElement, secondElement, thirdElement),
            limit: 0);
    }

    [Obsolete($"Use an overload of {nameof(ZipShortest)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> ZipShortestAwait<T1, T2, T3, T4, TResult>(
        IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.ZipAwait(second, third, fourth, resultSelector, limit: 0);
    }
}

