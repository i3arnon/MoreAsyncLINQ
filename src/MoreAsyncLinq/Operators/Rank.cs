using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<int> Rank<TSource>(this IAsyncEnumerable<TSource> source)
            where TSource : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Rank(comparer: null);
        }

        public static IAsyncEnumerable<int> Rank<TSource>(
            this IAsyncEnumerable<TSource> source,
            IComparer<TSource>? comparer)
            where TSource : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.RankBy(element => element, comparer);
        }

        public static IAsyncEnumerable<int> RankBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
            where TSource : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.RankBy(keySelector, comparer: null);
        }

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
                            cancellationToken).
                        ConfigureAwait(false);
                foreach (var element in list)
                {
                    yield return rankMap[element];
                }
            }
        }

        public static IAsyncEnumerable<int> RankByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> keySelector)
            where TSource : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.RankByAwait(keySelector, comparer: null);
        }


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
                        OrderByDescendingAwait(keySelector, comparer).
                        Index(startIndex: 1).
                        ToDictionaryAsync(
                            tuple => tuple.Element,
                            tuple => tuple.Index,
                            cancellationToken).
                        ConfigureAwait(false);
                foreach (var element in list)
                {
                    yield return rankMap[element];
                }
            }
        }
    }
}