using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns all duplicate elements of the given source, using the specified equality
    /// comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="comparer">
    /// The equality comparer to use to determine whether one <typeparamref name="TSource"/>
    /// equals another. If <see langword="null"/>, the default equality comparer for
    /// <typeparamref name="TSource"/> is used.</param>
    /// <returns>All elements that are duplicated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <remarks>This operator uses deferred execution and streams its results.</remarks>
    public static IAsyncEnumerable<TSource> Duplicates<TSource>(
        this IAsyncEnumerable<TSource> source,
        IEqualityComparer<TSource>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ScanBy(
                static element => element,
                static _ => 0,
                static (count, _, _) => unchecked(Math.Min(count + 1, 3)),
                comparer).
            Where(static element => element.State is 2).
            Select(static element => element.Key);
    }
}

