#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(DistinctBy)}.")]
    public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.DistinctBy(keySelector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(DistinctBy)}.")]
    public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return Core(source, keySelector, comparer);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var set = new HashSet<TKey>(comparer);
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = keySelector(element);
                if (set.Add(key))
                {
                    yield return element;
                }
            }
        }
    }

    [Obsolete($"Use an overload of {nameof(DistinctBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> DistinctByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.DistinctBy(keySelector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(DistinctBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> DistinctByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return Core(source, keySelector, comparer);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var set = new HashSet<TKey>(comparer);
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(element).ConfigureAwait(false);
                if (set.Add(key))
                {
                    yield return element;
                }
            }
        }
    }
}

