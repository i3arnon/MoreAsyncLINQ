using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<(TKey Key, TState State)> ScanBy<TSource, TKey, TState>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, TState> seedSelector,
            Func<TState, TKey, TSource, TState> accumulator)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (seedSelector is null) throw new ArgumentNullException(nameof(seedSelector));
            if (accumulator is null) throw new ArgumentNullException(nameof(accumulator));

            return source.ScanBy(
                keySelector,
                seedSelector,
                accumulator, 
                comparer: null);
        }

        public static IAsyncEnumerable<(TKey Key, TState State)> ScanBy<TSource, TKey, TState>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, TState> seedSelector,
            Func<TState, TKey, TSource, TState> accumulator,
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
                Func<TSource, TKey> keySelector,
                Func<TKey, TState> seedSelector,
                Func<TState, TKey, TSource, TState> accumulator,
                IEqualityComparer<TKey> comparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var stateMap = new NullableKeyDictionary<TKey, TState>(comparer);

                (TKey, TState)? previous = null;
                await foreach (var element in source.WithCancellation(cancellationToken))
                {
                    var key = keySelector(element);

                    TState state;
                    if (previous is ({ } previousKey, { } previousState)
                        && comparer.GetHashCode(previousKey) == comparer.GetHashCode(key)
                        && comparer.Equals(previousKey, key))
                    {
                        state = previousState;
                    }
                    else if (stateMap.TryGetValue(key, out var existingState))
                    {
                        state = existingState;
                    }
                    else
                    {
                        state = seedSelector(key);
                    }

                    state = accumulator(state, key, element);
                    stateMap[key] = state;
                    yield return (key, state);

                    previous = (key, state);
                }
            }
        }

        public static IAsyncEnumerable<(TKey Key, TState State)> ScanByAwait<TSource, TKey, TState>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            Func<TKey, ValueTask<TState>> seedSelector,
            Func<TState, TKey, TSource, ValueTask<TState>> accumulator)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (seedSelector is null) throw new ArgumentNullException(nameof(seedSelector));
            if (accumulator is null) throw new ArgumentNullException(nameof(accumulator));

            return source.ScanByAwait(
                keySelector,
                seedSelector,
                accumulator,
                comparer: null);
        }

        public static IAsyncEnumerable<(TKey Key, TState State)> ScanByAwait<TSource, TKey, TState>(
            this IAsyncEnumerable<TSource> source,
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

                (TKey, TState)? previous = null;
                await foreach (var element in source.WithCancellation(cancellationToken))
                {
                    var key = await keySelector(element).ConfigureAwait(false);

                    TState state;
                    if (previous is ({ } previousKey, { } previousState)
                        && comparer.GetHashCode(previousKey) == comparer.GetHashCode(key)
                        && comparer.Equals(previousKey, key))
                    {
                        state = previousState;
                    }
                    else if (stateMap.TryGetValue(key, out var existingState))
                    {
                        state = existingState;
                    }
                    else
                    {
                        state = await seedSelector(key).ConfigureAwait(false);
                    }

                    state = await accumulator(state, key, element).ConfigureAwait(false);
                    stateMap[key] = state;
                    yield return (key, state);

                    previous = (key, state);
                }
            }
        }
    }
}