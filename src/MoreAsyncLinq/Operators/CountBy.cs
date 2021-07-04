using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<(TKey Key, int Count)> CountBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.CountBy(keySelector, comparer: null);
        }

        public static IAsyncEnumerable<(TKey Key, int Count)> CountBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
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
                var (keys, counts) = await CountAsync();
                for (var index = 0; index < keys.Count; index++)
                {
                    yield return (keys[index], counts[index]);
                }

                async ValueTask<(List<TKey> keys, List<int> counts)> CountAsync()
                {
                    var indexMap = new NullableKeyDictionary<TKey, int>(comparer);

                    var keys = new List<TKey>();
                    var counts = new List<int>();

                    (TKey, int)? previous = null;
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        var key = keySelector(element);

                        int index;
                        if (previous is ({ } previousKey, var previousIndex)
                            && comparer.GetHashCode(previousKey) == comparer.GetHashCode(key)
                            && comparer.Equals(previousKey, key))
                        {
                            index = previousIndex;
                        }
                        else if (indexMap.TryGetValue(key, out var existingIndex))
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
                        previous = (key, index);
                    }

                    return (keys, counts);
                }
            }
        }

        public static IAsyncEnumerable<(TKey Key, int Count)> CountByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.CountByAwait(keySelector, comparer: null);
        }

        public static IAsyncEnumerable<(TKey Key, int Count)> CountByAwait<TSource, TKey>(
           this IAsyncEnumerable<TSource> source,
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
                var (keys, counts) = await CountAsync();
                for (var index = 0; index < keys.Count; index++)
                {
                    yield return (keys[index], counts[index]);
                }

                async ValueTask<(List<TKey> keys, List<int> counts)> CountAsync()
                {
                    var indexMap = new NullableKeyDictionary<TKey, int>(comparer);

                    var keys = new List<TKey>();
                    var counts = new List<int>();

                    (TKey, int)? previous = null;
                    await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        var key = await keySelector(element).ConfigureAwait(false);

                        int index;
                        if (previous is ({ } previousKey, var previousIndex)
                            && comparer.GetHashCode(previousKey) == comparer.GetHashCode(key)
                            && comparer.Equals(previousKey, key))
                        {
                            index = previousIndex;
                        }
                        else if (indexMap.TryGetValue(key, out var existingIndex))
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
                        previous = (key, index);
                    }

                    return (keys, counts);
                }
            }
        }
    }
}