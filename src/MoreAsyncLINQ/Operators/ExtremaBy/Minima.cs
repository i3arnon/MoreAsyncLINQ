using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns the minimal elements of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// This overload uses the default comparer for the projected type.
    /// This operator uses deferred execution. The results are evaluated
    /// and cached on first use to returned sequence.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The sequence of minimal elements, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static IExtremaAsyncEnumerable<TSource> Minima<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return MinBy(source, selector, comparer: null);
    }

    /// <summary>
    /// Returns the minimal elements of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution. The results are evaluated
    /// and cached on first use to returned sequence.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The sequence of minimal elements, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
    /// or <paramref name="comparer"/> is null</exception>
    public static IExtremaAsyncEnumerable<TSource> Minima<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> selector,
        IComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return source.IsKnownEmpty()
            ? ExtremaAsyncEnumerable.Empty<TSource>()
            : new ExtremaAsyncEnumerable<TSource, TKey>(
                source,
                selector,
                GetMinimaComparer(comparer));
    }

    /// <summary>
    /// Returns the minimal elements of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// This overload uses the default comparer for the projected type.
    /// This operator uses deferred execution. The results are evaluated
    /// and cached on first use to returned sequence.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The sequence of minimal elements, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static IExtremaAsyncEnumerable<TSource> Minima<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> selector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return source.Minima(selector, comparer: null);
    }

    /// <summary>
    /// Returns the minimal elements of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution. The results are evaluated
    /// and cached on first use to returned sequence.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The sequence of minimal elements, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
    /// or <paramref name="comparer"/> is null</exception>
    public static IExtremaAsyncEnumerable<TSource> Minima<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> selector,
        IComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return source.IsKnownEmpty()
            ? ExtremaAsyncEnumerable.Empty<TSource>()
            : new ExtremaAsyncEnumerableWithTask<TSource, TKey>(
                source,
                selector,
                GetMinimaComparer(comparer));
    }

    private static Func<T, T, int> GetMinimaComparer<T>(IComparer<T>? comparer)
    {
        comparer ??= Comparer<T>.Default;
        return (first, second) => -Math.Sign(comparer.Compare(first, second));
    }
}