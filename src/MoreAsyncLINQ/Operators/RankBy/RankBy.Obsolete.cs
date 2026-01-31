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
    [Obsolete($"Use an overload of {nameof(RankBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<int> RankByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
        where TSource : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return RankByAwait(source, keySelector, comparer: null);
    }

    [Obsolete($"Use an overload of {nameof(RankBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<int> RankByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
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

