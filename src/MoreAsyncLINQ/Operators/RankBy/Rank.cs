using System;
using System.Collections.Generic;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Ranks each item in the sequence in descending ordering using a default comparer.
    /// </summary>
    /// <typeparam name="TSource">Type of item in the sequence</typeparam>
    /// <param name="source">The sequence whose items will be ranked</param>
    /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
    public static IAsyncEnumerable<int> Rank<TSource>(this IAsyncEnumerable<TSource> source)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Rank(comparer: null);
    }

    /// <summary>
    /// Rank each item in the sequence using a caller-supplied comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <param name="source">The sequence of items to rank</param>
    /// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
    /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
    public static IAsyncEnumerable<int> Rank<TSource>(
        this IAsyncEnumerable<TSource> source,
        IComparer<TSource>? comparer)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.RankBy(element => element, comparer);
    }
}