using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> GenerateAwait<TResult>(
            TResult initial,
            Func<TResult, ValueTask<TResult>> generator)
        {
            if (generator is null) throw new ArgumentNullException(nameof(generator));

            return Core(initial, generator);

            static async IAsyncEnumerable<TResult> Core(
                TResult initial,
                Func<TResult, ValueTask<TResult>> generator,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var current = initial;
                while (true)
                {
                    yield return current;
                    current = await generator(current).ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }
    }
}