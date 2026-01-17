using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// If the resulting sequence is enumerated multiple times, the functions will be
    /// evaluated multiple times too.
    /// </remarks>
    /// <typeparam name="TResult">The type of the object returned by the functions.</typeparam>
    /// <param name="functions">The functions to evaluate.</param>
    /// <returns>A sequence with results from invoking <paramref name="functions"/>.</returns>
    /// <exception cref="ArgumentNullException">when <paramref name="functions"/> is <c>null</c>.</exception>
    public static IAsyncEnumerable<TResult> Evaluate<TResult>(this IAsyncEnumerable<Func<TResult>> functions)
    {
        if (functions is null) throw new ArgumentNullException(nameof(functions));

        return Core(functions);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<Func<TResult>> functions,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var function in functions.WithCancellation(cancellationToken))
            {
                yield return function();
            }
        }
    }

    /// <summary>
    /// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// If the resulting sequence is enumerated multiple times, the functions will be
    /// evaluated multiple times too.
    /// </remarks>
    /// <typeparam name="TResult">The type of the object returned by the functions.</typeparam>
    /// <param name="functions">The functions to evaluate.</param>
    /// <returns>A sequence with results from invoking <paramref name="functions"/>.</returns>
    /// <exception cref="ArgumentNullException">when <paramref name="functions"/> is <c>null</c>.</exception>
    public static IAsyncEnumerable<TResult> Evaluate<TResult>(this IAsyncEnumerable<Func<ValueTask<TResult>>> functions)
    {
        if (functions is null) throw new ArgumentNullException(nameof(functions));

        return Core(functions);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<Func<ValueTask<TResult>>> functions,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var function in functions.WithCancellation(cancellationToken))
            {
                yield return await function();
            }
        }
    }
    
    /// <summary>
    /// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results.
    /// If the resulting sequence is enumerated multiple times, the functions will be
    /// evaluated multiple times too.
    /// </remarks>
    /// <typeparam name="TResult">The type of the object returned by the functions.</typeparam>
    /// <param name="functions">The functions to evaluate.</param>
    /// <returns>A sequence with results from invoking <paramref name="functions"/>.</returns>
    /// <exception cref="ArgumentNullException">when <paramref name="functions"/> is <c>null</c>.</exception>
    public static IAsyncEnumerable<TResult> Evaluate<TResult>(this IAsyncEnumerable<Func<CancellationToken, ValueTask<TResult>>> functions)
    {
        if (functions is null) throw new ArgumentNullException(nameof(functions));

        return functions.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(functions, default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<Func<CancellationToken, ValueTask<TResult>>> functions,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var function in functions.WithCancellation(cancellationToken))
            {
                yield return await function(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}