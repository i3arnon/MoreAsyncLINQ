#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(ScanBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(TKey Key, TState State)> ScanByAwait<TSource, TKey, TState>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TKey, ValueTask<TState>> seedSelector,
        Func<TState, TKey, TSource, ValueTask<TState>> accumulator)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (seedSelector is null) throw new ArgumentNullException(nameof(seedSelector));
        if (accumulator is null) throw new ArgumentNullException(nameof(accumulator));

        return ScanByAwait(
            source,
            keySelector,
            seedSelector,
            accumulator,
            comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(ScanBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(TKey Key, TState State)> ScanByAwait<TSource, TKey, TState>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TKey, ValueTask<TState>> seedSelector,
        Func<TState, TKey, TSource, ValueTask<TState>> accumulator,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (seedSelector is null) throw new ArgumentNullException(nameof(seedSelector));
        if (accumulator is null) throw new ArgumentNullException(nameof(accumulator));

        return Core(
            source,
            keySelector,
            seedSelector,
            accumulator,
            comparer ?? EqualityComparer<TKey>.Default);

        static async IAsyncEnumerable<(TKey Key, TState State)> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            Func<TKey, ValueTask<TState>> seedSelector,
            Func<TState, TKey, TSource, ValueTask<TState>> accumulator,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var stateMap = new NullableKeyDictionary<TKey, TState>(comparer);

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(element).ConfigureAwait(false);
                var state =
                    stateMap.TryGetValue(key, out var existingState)
                        ? existingState
                        : await seedSelector(key).ConfigureAwait(false);
                state = await accumulator(state, key, element).ConfigureAwait(false);
                stateMap[key] = state;
                yield return (key, state);
            }
        }
    }
}

