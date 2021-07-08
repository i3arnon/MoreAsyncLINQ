using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
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
}