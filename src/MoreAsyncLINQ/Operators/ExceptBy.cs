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
    /// Returns the set of elements in the first sequence which aren't
    /// in the second sequence, according to a given key selector.
    /// </summary>
    /// <remarks>
    /// This is a set operation; if multiple elements in <paramref name="first"/> have
    /// equal keys, only the first such element is returned.
    /// This operator uses deferred execution and streams the results, although
    /// a set of keys from <paramref name="second"/> is immediately selected and retained.
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="first">The sequence of potentially included elements.</param>
    /// <param name="second">The sequence of elements whose keys may prevent elements in
    /// <paramref name="first"/> from being returned.</param>
    /// <param name="keySelector">The mapping from source element to key.</param>
    /// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
    /// any element in <paramref name="second"/>.</returns>
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

    /// <summary>
    /// Returns the set of elements in the first sequence which aren't
    /// in the second sequence, according to a given key selector.
    /// </summary>
    /// <remarks>
    /// This is a set operation; if multiple elements in <paramref name="first"/> have
    /// equal keys, only the first such element is returned.
    /// This operator uses deferred execution and streams the results, although
    /// a set of keys from <paramref name="second"/> is immediately selected and retained.
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="first">The sequence of potentially included elements.</param>
    /// <param name="second">The sequence of elements whose keys may prevent elements in
    /// <paramref name="first"/> from being returned.</param>
    /// <param name="keySelector">The mapping from source element to key.</param>
    /// <param name="keyComparer">The equality comparer to use to determine whether or not keys are equal.
    /// If null, the default equality comparer for <c>TSource</c> is used.</param>
    /// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
    /// any element in <paramref name="second"/>.</returns>
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

    /// <summary>
    /// Returns the set of elements in the first sequence which aren't
    /// in the second sequence, according to a given key selector.
    /// </summary>
    /// <remarks>
    /// This is a set operation; if multiple elements in <paramref name="first"/> have
    /// equal keys, only the first such element is returned.
    /// This operator uses deferred execution and streams the results, although
    /// a set of keys from <paramref name="second"/> is immediately selected and retained.
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="first">The sequence of potentially included elements.</param>
    /// <param name="second">The sequence of elements whose keys may prevent elements in
    /// <paramref name="first"/> from being returned.</param>
    /// <param name="keySelector">The mapping from source element to key.</param>
    /// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
    /// any element in <paramref name="second"/>.</returns>
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

    /// <summary>
    /// Returns the set of elements in the first sequence which aren't
    /// in the second sequence, according to a given key selector.
    /// </summary>
    /// <remarks>
    /// This is a set operation; if multiple elements in <paramref name="first"/> have
    /// equal keys, only the first such element is returned.
    /// This operator uses deferred execution and streams the results, although
    /// a set of keys from <paramref name="second"/> is immediately selected and retained.
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="first">The sequence of potentially included elements.</param>
    /// <param name="second">The sequence of elements whose keys may prevent elements in
    /// <paramref name="first"/> from being returned.</param>
    /// <param name="keySelector">The mapping from source element to key.</param>
    /// <param name="keyComparer">The equality comparer to use to determine whether or not keys are equal.
    /// If null, the default equality comparer for <c>TSource</c> is used.</param>
    /// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
    /// any element in <paramref name="second"/>.</returns>
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