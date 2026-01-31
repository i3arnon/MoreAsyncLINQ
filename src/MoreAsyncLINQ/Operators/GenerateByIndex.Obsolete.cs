#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(GenerateByIndex)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> GenerateByIndexAwait<TResult>(Func<int, ValueTask<TResult>> generator)
    {
        if (generator is null) throw new ArgumentNullException(nameof(generator));

        return Range(start: 0, int.MaxValue).ToAsyncEnumerable().Select((int index, CancellationToken _) => generator(index));
    }
}

