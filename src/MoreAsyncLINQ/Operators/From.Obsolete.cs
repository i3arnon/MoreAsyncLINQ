#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(From)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> FromAwait<TResult>(Func<ValueTask<TResult>> function)
    {
        if (function is null) throw new ArgumentNullException(nameof(function));

        return Core(function);

        static async IAsyncEnumerable<TResult> Core(Func<ValueTask<TResult>> function)
        {
            yield return await function().ConfigureAwait(false);
        }
    }

    [Obsolete($"Use an overload of {nameof(From)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    [Obsolete($"Use an overload of {nameof(From)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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

    [Obsolete($"Use an overload of {nameof(From)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> FromAwait<TResult>(params Func<ValueTask<TResult>>[] functions)
    {
        if (functions is null) throw new ArgumentNullException(nameof(functions));

        return functions.ToAsyncEnumerable().Evaluate();
    }
}

