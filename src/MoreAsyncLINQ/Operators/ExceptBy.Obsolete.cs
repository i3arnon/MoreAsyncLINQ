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
    [Obsolete($"Use an overload of {nameof(ExceptBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> ExceptByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector)); 
            
        return ExceptByAwait(first, second, keySelector, keyComparer: null);
    }

    [Obsolete($"Use an overload of {nameof(ExceptBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> ExceptByAwait<TSource, TKey>(
        IAsyncEnumerable<TSource> first,
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
                    Select((TSource element, CancellationToken _) => keySelector(element)).
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

