using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, TKey> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortBy(count, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, TKey> keySelector,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortBy(count, keySelector, comparer: null, direction);
        }

        public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortBy(count, keySelector, Comparers.Get(comparer, direction));
        }

        public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortBy(count, keySelector, comparer, comparer: null);
        }

        private static async IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, TKey>? keySelector,
            IComparer<TKey>? keyComparer,
            IComparer<TSource>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var keys =
                keySelector is not null
                    ? new List<TKey>(count)
                    : null;
            var top = new List<TSource>(count);

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (keys is not null)
                {
                    var key = keySelector!(element);
                    if (Insert(keys, key, keyComparer) is { } index)
                    {
                        if (top.Count == count)
                        {
                            top.RemoveAt(count - 1);
                        }

                        top.Insert(index, element);
                    }
                }
                else
                {
                    Insert(top, element, comparer);
                }
            }

            foreach (var element in top)
            {
                yield return element;
            }

            int? Insert<T>(List<T> list, T item, IComparer<T>? comparer)
            {
                var index = list.BinarySearch(item, comparer);
                if (index < 0)
                {
                    index = ~index;
                    if (index >= count)
                    {
                        return null;
                    }
                }

                if (list.Count == count)
                {
                    list.RemoveAt(count - 1);
                }

                list.Insert(index, item);
                return index;
            }
        }

        public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortByAwait(count, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, ValueTask<TKey>> keySelector,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortByAwait(count, keySelector, comparer: null, direction);
        }

        public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer,
            OrderByDirection direction)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortByAwait(count, keySelector, Comparers.Get(comparer, direction));
        }

        public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return source.PartialSortByAwait(count, keySelector, comparer, comparer: null);
        }

        private static async IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            int count,
            Func<TSource, ValueTask<TKey>>? keySelector,
            IComparer<TKey>? keyComparer,
            IComparer<TSource>? comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var keys =
                keySelector is not null
                    ? new List<TKey>(count)
                    : null;
            var top = new List<TSource>(count);

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (keys is not null)
                {
                    var key = await keySelector!(element).ConfigureAwait(false);
                    if (Insert(keys, key, keyComparer) is { } index)
                    {
                        if (top.Count == count)
                        {
                            top.RemoveAt(count - 1);
                        }

                        top.Insert(index, element);
                    }
                }
                else
                {
                    Insert(top, element, comparer);
                }
            }

            foreach (var element in top)
            {
                yield return element;
            }

            int? Insert<T>(List<T> list, T item, IComparer<T>? comparer)
            {
                var index = list.BinarySearch(item, comparer);
                if (index < 0)
                {
                    index = ~index;
                    if (index >= count)
                    {
                        return null;
                    }
                }

                if (list.Count == count)
                {
                    list.RemoveAt(count - 1);
                }

                list.Insert(index, item);
                return index;
            }
        }
    }
}