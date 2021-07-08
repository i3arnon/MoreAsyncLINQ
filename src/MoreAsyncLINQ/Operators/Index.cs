using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<(int Index, TSource Element)> Index<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            
            return source.Index(startIndex: 0);
        }

        public static IAsyncEnumerable<(int Index, TSource Element)> Index<TSource>(this IAsyncEnumerable<TSource> source, int startIndex)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Select((element, index) => (startIndex + index, element));
        }
    }
}