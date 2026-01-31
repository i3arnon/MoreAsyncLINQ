#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(CountBy)}.")]
    public static IAsyncEnumerable<(TKey Key, int Count)> CountBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return CountBy(source, keySelector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(CountBy)}.")]
    public static IAsyncEnumerable<(TKey Key, int Count)> CountBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return Core(
            source,
            keySelector,
            comparer ?? EqualityComparer<TKey>.Default);

        static async IAsyncEnumerable<(TKey Key, int Count)> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var (keys, counts) = await CountAsync().ConfigureAwait(false);
            for (var index = 0; index < keys.Count; index++)
            {
                yield return (keys[index], counts[index]);
            }

            async ValueTask<(List<TKey> keys, List<int> counts)> CountAsync()
            {
                var indexMap = new NullableKeyDictionary<TKey, int>(comparer);

                var keys = new List<TKey>();
                var counts = new List<int>();

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var key = keySelector(element);

                    int index;
                    if (indexMap.TryGetValue(key, out var existingIndex))
                    {
                        index = existingIndex;
                    }
                    else
                    {
                        index = keys.Count;
                        indexMap[key] = index;
                        keys.Add(key);
                        counts.Add(0);
                    }

                    counts[index]++;
                }

                return (keys, counts);
            }
        }
    }

    [Obsolete($"Use an overload of {nameof(CountBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(TKey Key, int Count)> CountByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return CountByAwait(source, keySelector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(CountBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(TKey Key, int Count)> CountByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return Core(
            source,
            keySelector,
            comparer ?? EqualityComparer<TKey>.Default);

        static async IAsyncEnumerable<(TKey Key, int Count)> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var (keys, counts) = await CountAsync().ConfigureAwait(false);
            for (var index = 0; index < keys.Count; index++)
            {
                yield return (keys[index], counts[index]);
            }

            async ValueTask<(List<TKey> keys, List<int> counts)> CountAsync()
            {
                var indexMap = new NullableKeyDictionary<TKey, int>(comparer);

                var keys = new List<TKey>();
                var counts = new List<int>();

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var key = await keySelector(element).ConfigureAwait(false);

                    int index;
                    if (indexMap.TryGetValue(key, out var existingIndex))
                    {
                        index = existingIndex;
                    }
                    else
                    {
                        index = keys.Count;
                        indexMap[key] = index;
                        keys.Add(key);
                        counts.Add(0);
                    }

                    counts[index]++;
                }

                return (keys, counts);
            }
        }
    }
}

