using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Evaluate<TResult>(this IAsyncEnumerable<Func<TResult>> functions)
        {
            if (functions is null) throw new ArgumentNullException(nameof(functions));

            return Core(functions);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<Func<TResult>> functions,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var function in functions.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return function();
                }
            }
        }

        public static IAsyncEnumerable<TResult> Evaluate<TResult>(this IAsyncEnumerable<Func<ValueTask<TResult>>> functions)
        {
            if (functions is null) throw new ArgumentNullException(nameof(functions));

            return Core(functions);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<Func<ValueTask<TResult>>> functions,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var function in functions.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await function().ConfigureAwait(false);
                }
            }
        }
    }
}