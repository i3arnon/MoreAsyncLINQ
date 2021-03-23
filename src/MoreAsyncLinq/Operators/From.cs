using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> FromAwait<TResult>(Func<ValueTask<TResult>> function)
        {
            if (function is null) throw new ArgumentNullException(nameof(function));

            return Core(function);

            static async IAsyncEnumerable<TResult> Core(
                Func<ValueTask<TResult>> function,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                yield return await function().ConfigureAwait(false);
            }
        }

        public static IAsyncEnumerable<TResult> FromAwait<TResult>(
            Func<ValueTask<TResult>> function1,
            Func<ValueTask<TResult>> function2)
        {
            if (function1 is null) throw new ArgumentNullException(nameof(function1));
            if (function2 is null) throw new ArgumentNullException(nameof(function2));

            return Core(function1, function2);

            static async IAsyncEnumerable<TResult> Core(
                Func<ValueTask<TResult>> function1,
                Func<ValueTask<TResult>> function2,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                yield return await function1().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                
                yield return await function2().ConfigureAwait(false);
            }
        }

        public static IAsyncEnumerable<TResult> FromAwait<TResult>(
            Func<ValueTask<TResult>> function1,
            Func<ValueTask<TResult>> function2,
            Func<ValueTask<TResult>> function3)
        {
            if (function1 is null) throw new ArgumentNullException(nameof(function1));
            if (function2 is null) throw new ArgumentNullException(nameof(function2));
            if (function3 is null) throw new ArgumentNullException(nameof(function3));

            return Core(function1, function2, function3);

            static async IAsyncEnumerable<TResult> Core(
                Func<ValueTask<TResult>> function1,
                Func<ValueTask<TResult>> function2,
                Func<ValueTask<TResult>> function3,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                yield return await function1().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                
                yield return await function2().ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                
                yield return await function3().ConfigureAwait(false);
            }
        }

        public static IAsyncEnumerable<TResult> FromAwait<TResult>(params Func<ValueTask<TResult>>[] functions)
        {
            if (functions is null) throw new ArgumentNullException(nameof(functions));

            return functions.ToAsyncEnumerable().Evaluate();
        }
    }
}