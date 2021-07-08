using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TakeEvery<TSource>(this IAsyncEnumerable<TSource> source, int step)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));

            return source.Where((_, index) => index % step == 0);
        }
    }
}