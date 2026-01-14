using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    /// is determined via a projection and the default equality comparer for the projected type.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    /// a set of already-seen keys is retained. If a key is seen multiple times,
    /// only the first element with that key is returned.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="keySelector">Projection for determining "distinctness"</param>
    /// <returns>A sequence consisting of distinct elements from the source sequence,
    /// comparing them by the specified key projection.</returns>
    public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.DistinctBy(keySelector, comparer: null);
    }

    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    /// is determined via a projection and the specified comparer for the projected type.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    /// a set of already-seen keys is retained. If a key is seen multiple times,
    /// only the first element with that key is returned.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="keySelector">Projection for determining "distinctness"</param>
    /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
    /// If null, the default equality comparer for <c>TSource</c> is used.</param>
    /// <returns>A sequence consisting of distinct elements from the source sequence,
    /// comparing them by the specified key projection.</returns>
    public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
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

    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    /// is determined via a projection and the default equality comparer for the projected type.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    /// a set of already-seen keys is retained. If a key is seen multiple times,
    /// only the first element with that key is returned.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="keySelector">Projection for determining "distinctness"</param>
    /// <returns>A sequence consisting of distinct elements from the source sequence,
    /// comparing them by the specified key projection.</returns>
    public static IAsyncEnumerable<TSource> DistinctByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.DistinctBy(keySelector, comparer: null);
    }

    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    /// is determined via a projection and the specified comparer for the projected type.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    /// a set of already-seen keys is retained. If a key is seen multiple times,
    /// only the first element with that key is returned.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="keySelector">Projection for determining "distinctness"</param>
    /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
    /// If null, the default equality comparer for <c>TSource</c> is used.</param>
    /// <returns>A sequence consisting of distinct elements from the source sequence,
    /// comparing them by the specified key projection.</returns>
    public static IAsyncEnumerable<TSource> DistinctByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
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