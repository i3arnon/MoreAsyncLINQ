#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Unfold)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> UnfoldAwait<TState, T, TResult>(
        TState state,
        Func<TState, ValueTask<T>> generator,
        Func<T, ValueTask<bool>> predicate,
        Func<T, ValueTask<TState>> stateSelector,
        Func<T, ValueTask<TResult>> resultSelector)
    {
        if (generator is null) throw new ArgumentNullException(nameof(generator));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (stateSelector is null) throw new ArgumentNullException(nameof(stateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(state, generator, predicate, stateSelector, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            TState state,
            Func<TState, ValueTask<T>> generator,
            Func<T, ValueTask<bool>> predicate,
            Func<T, ValueTask<TState>> stateSelector,
            Func<T, ValueTask<TResult>> resultSelector)
        {
            while (true)
            {
                var element = await generator(state).ConfigureAwait(false);
                if (!await predicate(element).ConfigureAwait(false))
                {
                    yield break;
                }

                yield return await resultSelector(element).ConfigureAwait(false);

                state = await stateSelector(element).ConfigureAwait(false);
            }
        }
    }
}

