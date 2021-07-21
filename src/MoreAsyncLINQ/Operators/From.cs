using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Returns a single-element sequence containing the result of invoking the function.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the function will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="TResult">The type of the object returned by the function.</typeparam>
        /// <param name="function">The function to evaluate.</param>
        /// <returns>A sequence with the value resulting from invoking <paramref name="function"/>.</returns>
        public static IAsyncEnumerable<TResult> FromAwait<TResult>(Func<ValueTask<TResult>> function)
        {
            if (function is null) throw new ArgumentNullException(nameof(function));

            return Core(function);

            static async IAsyncEnumerable<TResult> Core(Func<ValueTask<TResult>> function)
            {
                yield return await function().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Returns a sequence containing the result of invoking each parameter function in order.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="TResult">The type of the object returned by the functions.</typeparam>
        /// <param name="function1">The first function to evaluate.</param>
        /// <param name="function2">The second function to evaluate.</param>
        /// <returns>A sequence with the values resulting from invoking <paramref name="function1"/> and <paramref name="function2"/>.</returns>
        public static IAsyncEnumerable<TResult> FromAwait<TResult>(
            Func<ValueTask<TResult>> function1,
            Func<ValueTask<TResult>> function2)
        {
            if (function1 is null) throw new ArgumentNullException(nameof(function1));
            if (function2 is null) throw new ArgumentNullException(nameof(function2));

            return Core(function1, function2);

            static async IAsyncEnumerable<TResult> Core(
                Func<ValueTask<TResult>> function1,
                Func<ValueTask<TResult>> function2)
            {
                yield return await function1().ConfigureAwait(false);
                yield return await function2().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Returns a sequence containing the result of invoking each parameter function in order.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="TResult">The type of the object returned by the functions.</typeparam>
        /// <param name="function1">The first function to evaluate.</param>
        /// <param name="function2">The second function to evaluate.</param>
        /// <param name="function3">The third function to evaluate.</param>
        /// <returns>A sequence with the values resulting from invoking <paramref name="function1"/>, <paramref name="function2"/> and <paramref name="function3"/>.</returns>
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
                Func<ValueTask<TResult>> function3)
            {
                yield return await function1().ConfigureAwait(false);
                yield return await function2().ConfigureAwait(false);
                yield return await function3().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// If the resulting sequence is enumerated multiple times, the functions will be
        /// invoked multiple times too.
        /// </remarks>
        /// <typeparam name="TResult">The type of the object returned by the functions.</typeparam>
        /// <param name="functions">The functions to evaluate.</param>
        /// <returns>A sequence with the values resulting from invoking all of the <paramref name="functions"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="functions"/> is <c>null</c>.</exception>
        public static IAsyncEnumerable<TResult> FromAwait<TResult>(params Func<ValueTask<TResult>>[] functions)
        {
            if (functions is null) throw new ArgumentNullException(nameof(functions));

            return functions.ToAsyncEnumerable().Evaluate();
        }
    }
}