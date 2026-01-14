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
    /// Ranks each item in the sequence in descending ordering by a specified key using a default comparer
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
    /// <param name="source">The sequence of items to rank</param>
    /// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
    /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
    public static IAsyncEnumerable<int> RankBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.RankBy(keySelector, comparer: null);
    }

    /// <summary>
    /// Ranks each item in a sequence using a specified key and a caller-supplied comparer
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
    /// <param name="source">The sequence of items to rank</param>
    /// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
    /// <param name="comparer">An object that defines the comparison semantics for keys used to rank items</param>
    /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
    public static IAsyncEnumerable<int> RankBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return Core(source, keySelector, comparer ?? Comparer<TKey>.Default);

        static async IAsyncEnumerable<int> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            var rankMap =
                await list.
                    Distinct().
                    ToAsyncEnumerable().
                    OrderByDescending(keySelector, comparer).
                    Index(startIndex: 1).
                    ToDictionaryAsync(
                        tuple => tuple.Element,
                        tuple => tuple.Index,
                        comparer: null,
                        cancellationToken).
                    ConfigureAwait(false);
            foreach (var element in list)
            {
                yield return rankMap[element];
            }
        }
    }

    /// <summary>
    /// Ranks each item in the sequence in descending ordering by a specified key using a default comparer
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
    /// <param name="source">The sequence of items to rank</param>
    /// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
    /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
    public static IAsyncEnumerable<int> RankByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.RankByAwait(keySelector, comparer: null);
    }

    /// <summary>
    /// Ranks each item in a sequence using a specified key and a caller-supplied comparer
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
    /// <param name="source">The sequence of items to rank</param>
    /// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
    /// <param name="comparer">An object that defines the comparison semantics for keys used to rank items</param>
    /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
    public static IAsyncEnumerable<int> RankByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return Core(source, keySelector, comparer ?? Comparer<TKey>.Default);

        static async IAsyncEnumerable<int> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector,
            IComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            var rankMap =
                await list.
                    Distinct().
                    ToAsyncEnumerable().
                    OrderByDescending((element, _) => keySelector(element), comparer).
                    Index(startIndex: 1).
                    ToDictionaryAsync(
                        tuple => tuple.Element,
                        tuple => tuple.Index,
                        comparer: null,
                        cancellationToken).
                    ConfigureAwait(false);
            foreach (var element in list)
            {
                yield return rankMap[element];
            }
        }
    }
}