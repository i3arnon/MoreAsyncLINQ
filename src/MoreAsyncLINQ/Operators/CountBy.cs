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
    /// Applies a key-generating function to each element of a sequence and returns a sequence of
    /// unique keys and their number of occurrences in the original sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TKey">Type of the projected element.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
    /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
    [Obsolete($"Use an overload of {nameof(CountBy)}.")]
    public static IAsyncEnumerable<(TKey Key, int Count)> CountBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return CountBy(source, keySelector, comparer: null);
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and returns a sequence of
    /// unique keys and their number of occurrences in the original sequence.
    /// An additional argument specifies a comparer to use for testing equivalence of keys.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TKey">Type of the projected element.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
    /// <param name="comparer">The equality comparer to use to determine whether keys are equal.
    /// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
    /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
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

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and returns a sequence of
    /// unique keys and their number of occurrences in the original sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TKey">Type of the projected element.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
    /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
    [Obsolete($"Use an overload of {nameof(CountBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<(TKey Key, int Count)> CountByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.CountByAwait(keySelector, comparer: null);
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and returns a sequence of
    /// unique keys and their number of occurrences in the original sequence.
    /// An additional argument specifies a comparer to use for testing equivalence of keys.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TKey">Type of the projected element.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
    /// <param name="comparer">The equality comparer to use to determine whether keys are equal.
    /// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
    /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
    [Obsolete($"Use an overload of {nameof(CountBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
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