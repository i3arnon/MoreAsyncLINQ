using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderBy{TSource,TKey}(IAsyncEnumerable{TSource},Func{TSource,TKey})"/>,
    /// where each element is its key, and <see cref="AsyncEnumerable.Take{TSource}"/>
    /// in a single operation.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in their ascending order.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSort<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.PartialSort(count, comparer: null);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}, OrderByDirection)"/>,
    /// where each element is its key, and <see cref="AsyncEnumerable.Take{TSource}"/>
    /// in a single operation.
    /// An additional parameter specifies the direction of the sort
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSort<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.PartialSort(count, comparer: null, direction);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}, OrderByDirection)"/>,
    /// where each element is its key, and <see cref="AsyncEnumerable.Take{TSource}"/>
    /// in a single operation.
    /// Additional parameters specify how the elements compare to each other and
    /// the direction of the sort.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSort<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        IComparer<TSource>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.PartialSort(count, Comparers.Get(comparer, direction));
    }

    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderBy{TSource,TKey}(IAsyncEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
    /// where each element is its key, and <see cref="AsyncEnumerable.Take{TSource}"/>
    /// in a single operation. An additional parameter specifies how the
    /// elements compare to each other.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in their ascending order.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSort<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        IComparer<TSource>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.PartialSortBy<TSource, TSource>(
            count,
            keySelector: null,
            keyComparer: null,
            comparer);
    }
}