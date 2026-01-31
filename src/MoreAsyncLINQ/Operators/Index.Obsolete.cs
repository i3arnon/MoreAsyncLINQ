#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Index)}.")]
    public static IAsyncEnumerable<(int Index, TSource Element)> Index<TSource>(IAsyncEnumerable<TSource> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        
        return source.Index(startIndex: 0);
    }
}

