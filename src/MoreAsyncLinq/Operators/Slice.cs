using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Slice<TSource>(
            this IAsyncEnumerable<TSource> source,
            int startIndex,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.Skip(startIndex).Take(count);
        }
    }
}