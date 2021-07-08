using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> GenerateByIndexAwait<TResult>(Func<int, ValueTask<TResult>> generator)
        {
            if (generator is null) throw new ArgumentNullException(nameof(generator));

            return Range(start: 0, int.MaxValue).ToAsyncEnumerable().SelectAwait(generator);
        }
    }
}