using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IExtremaAsyncEnumerable<TSource> MaxBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return source.MaxBy(selector, comparer: null);
        }

        public static IExtremaAsyncEnumerable<TSource> MaxBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return new ExtremaAsyncEnumerable<TSource, TKey>(
                source,
                selector,
                GetMaxByComparer(comparer));
        }

        public static IExtremaAsyncEnumerable<TSource> MaxByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return source.MaxByAwait(selector, comparer: null);
        }

        public static IExtremaAsyncEnumerable<TSource> MaxByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> selector,
            IComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return new ExtremaAsyncEnumerableWithTask<TSource, TKey>(
                source,
                selector,
                GetMaxByComparer(comparer));
        }

        private static Func<T, T, int> GetMaxByComparer<T>(IComparer<T>? comparer)
        {
            comparer ??= Comparer<T>.Default;
            return (first, second) => comparer.Compare(first, second);
        }
    }
}