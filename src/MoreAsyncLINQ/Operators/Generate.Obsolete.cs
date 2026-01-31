#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Generate)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> GenerateAwait<TResult>(
        TResult initial,
        Func<TResult, ValueTask<TResult>> generator)
    {
        if (generator is null) throw new ArgumentNullException(nameof(generator));

        return Core(initial, generator);

        static async IAsyncEnumerable<TResult> Core(
            TResult initial,
            Func<TResult, ValueTask<TResult>> generator)
        {
            var current = initial;
            while (true)
            {
                yield return current;

                current = await generator(current).ConfigureAwait(false);
            }
        }
    }
}

