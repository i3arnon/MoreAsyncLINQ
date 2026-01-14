using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
    /// <param name="source">The sequence to order</param>
    /// <param name="keySelector">A key selector function</param>
    /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
    /// <returns>An ordered copy of the source sequence</returns>
    public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            
        return source.OrderBy(keySelector,comparer: null, direction);
    }

    /// <summary>
    /// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
    /// <param name="source">The sequence to order</param>
    /// <param name="keySelector">A key selector function</param>
    /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
    /// <param name="comparer">A comparer used to define the semantics of element comparison</param>
    /// <returns>An ordered copy of the source sequence</returns>
    public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        comparer ??= Comparer<TKey>.Default;
        return direction == Ascending
            ? source.OrderBy(keySelector, comparer)
            : source.OrderByDescending(keySelector, comparer);
    }

    /// <summary>
    /// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
    /// <param name="source">The sequence to order</param>
    /// <param name="keySelector">A key selector function</param>
    /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
    /// <returns>An ordered copy of the source sequence</returns>
    public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.OrderByAwait(keySelector, comparer: null, direction);
    }

    /// <summary>
    /// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
    /// <param name="source">The sequence to order</param>
    /// <param name="keySelector">A key selector function</param>
    /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
    /// <param name="comparer">A comparer used to define the semantics of element comparison</param>
    /// <returns>An ordered copy of the source sequence</returns>
    public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        comparer ??= Comparer<TKey>.Default;
        return direction == Ascending
            ? source.OrderBy((element, _) => keySelector(element), comparer)
            : source.OrderByDescending((element, _) => keySelector(element), comparer);
    }
}