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
        public static IAsyncEnumerable<TSource> ExceptBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector)); 
            
            return first.ExceptBy(second, keySelector, keyComparer: null);
        }

        public static IAsyncEnumerable<TSource> ExceptBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? keyComparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return Core(first, second, keySelector, keyComparer);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> first,
                IAsyncEnumerable<TSource> second,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey>? keyComparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var set = 
                    await second.
                        Select(keySelector).
                        ToHashSetAsync(keyComparer, cancellationToken).
                        ConfigureAwait(false);
                await foreach (var element in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var key = keySelector(element);
                    if (set.Add(key))
                    {
                        yield return element;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> ExceptByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector)); 
            
            return first.ExceptBy(second, keySelector, keyComparer: null);
        }

        public static IAsyncEnumerable<TSource> ExceptByAwait<TSource, TKey>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            Func<TSource, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey>? keyComparer)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return Core(first, second, keySelector, keyComparer);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> first,
                IAsyncEnumerable<TSource> second,
                Func<TSource, ValueTask<TKey>> keySelector,
                IEqualityComparer<TKey>? keyComparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var set =
                    await second.
                        SelectAwait(keySelector).
                        ToHashSetAsync(keyComparer, cancellationToken).
                        ConfigureAwait(false);
                await foreach (var element in first.WithCancellation(cancellationToken).ConfigureAwait(false))
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
}