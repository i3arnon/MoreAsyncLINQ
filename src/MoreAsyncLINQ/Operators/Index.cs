using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns a sequence of tuples of an element 
    /// and its zero-based index in the source sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <returns>A sequence of <see cref="ValueTuple{T1,T2}"/>.</returns>
    /// <remarks>This operator uses deferred execution and streams its
    /// results.</remarks>
    [Obsolete($"Use an overload of {nameof(Index)}.")]
    public static IAsyncEnumerable<(int Index, TSource Element)> Index<TSource>(IAsyncEnumerable<TSource> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        
        return source.Index(startIndex: 0);
    }

    /// <summary>
    /// Returns a sequence of tuples of an element
    /// and its index in the source sequence.
    /// An additional parameter specifies the starting index.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="startIndex"></param>
    /// <returns>A sequence of <see cref="ValueTuple{T1,T2}"/>.</returns>
    /// <remarks>This operator uses deferred execution and streams its
    /// results.</remarks>
    public static IAsyncEnumerable<(int Index, TSource Element)> Index<TSource>(this IAsyncEnumerable<TSource> source, int startIndex)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Select((element, index) => (startIndex + index, element));
    }
}