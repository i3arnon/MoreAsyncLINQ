using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Bypasses a specified number of elements at the end of the sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="count">The number of elements to bypass at the end of the source sequence.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
    /// </returns>
    public static IAsyncEnumerable<TSource> SkipLast<TSource>(
        IAsyncEnumerable<TSource> source,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        return count == 0
            ? source
            : source.
                CountDown(count).
                TakeWhile(tuple => tuple.Countdown is null).
                Select(tuple => tuple.Element);
    }
}