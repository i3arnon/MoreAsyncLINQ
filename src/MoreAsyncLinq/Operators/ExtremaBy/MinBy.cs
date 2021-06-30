using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IExtremaAsyncEnumerable<TSource> MinBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return source.MinBy(selector, comparer: null);
        }

        public static IExtremaAsyncEnumerable<TSource> MinBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return new ExtremaAsyncEnumerable<TSource, TKey>(
                source,
                selector,
                GetMinByComparer(comparer));
        }

        public static IExtremaAsyncEnumerable<TSource> MinByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return source.MinByAwait(selector, comparer: null);
        }

        public static IExtremaAsyncEnumerable<TSource> MinByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TKey>> selector,
            IComparer<TKey>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return new ExtremaAsyncEnumerableWithTask<TSource, TKey>(
                source,
                selector,
                GetMinByComparer(comparer));
        }

        private static Func<T, T, int> GetMinByComparer<T>(IComparer<T>? comparer)
        {
            comparer ??= Comparer<T>.Default;
            return (first, second) => -Math.Sign(comparer.Compare(first, second));
        }
    }
}