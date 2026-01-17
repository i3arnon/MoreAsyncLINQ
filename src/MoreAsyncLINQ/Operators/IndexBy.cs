using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Applies a key-generating function to each element of a sequence and
    /// returns a sequence that contains the elements of the original
    /// sequence as well its key and index inside the group of its key.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element in the source sequence.</param>
    /// <returns>
    /// A sequence of elements paired with their index within the key-group.
    /// </returns>
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.IndexBy(keySelector, comparer: null);
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and
    /// returns a sequence that contains the elements of the original
    /// sequence as well its key and index inside the group of its key.
    /// An additional parameter specifies a comparer to use for testing the
    /// equivalence of keys.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element in the source sequence.</param>
    /// <param name="comparer">
    /// The equality comparer to use to determine whether or not keys are
    /// equal. If <c>null</c>, the default equality comparer for
    /// <typeparamref name="TSource"/> is used.</param>
    /// <returns>
    /// A sequence of elements paired with their index within the key-group.
    /// </returns>
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.
            ScanBy<TSource, TKey, (int index, TSource element)>(
                keySelector,
                _ => (-1, default!),
                (state, _, element) => (state.index + 1, element),
                comparer).
            Select(tuple => (tuple.State.index, tuple.State.element));
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and
    /// returns a sequence that contains the elements of the original
    /// sequence as well its key and index inside the group of its key.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element in the source sequence.</param>
    /// <returns>
    /// A sequence of elements paired with their index within the key-group.
    /// </returns>
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.IndexByAwait(keySelector, comparer: null);
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and
    /// returns a sequence that contains the elements of the original
    /// sequence as well its key and index inside the group of its key.
    /// An additional parameter specifies a comparer to use for testing the
    /// equivalence of keys.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element in the source sequence.</param>
    /// <param name="comparer">
    /// The equality comparer to use to determine whether or not keys are
    /// equal. If <c>null</c>, the default equality comparer for
    /// <typeparamref name="TSource"/> is used.</param>
    /// <returns>
    /// A sequence of elements paired with their index within the key-group.
    /// </returns>
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            
        return source.
            ScanByAwait<TSource, TKey, (int index, TSource element)>(
                keySelector,
                _ => ValueTasks.FromResult((-1, default(TSource)!)),
                (state, _, element) => ValueTasks.FromResult((state.index + 1, element)),
                comparer).
            Select(tuple => (tuple.State.index, tuple.State.element));
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and
    /// returns a sequence that contains the elements of the original
    /// sequence as well its key and index inside the group of its key.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element in the source sequence.</param>
    /// <returns>
    /// A sequence of elements paired with their index within the key-group.
    /// </returns>
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.IndexBy(keySelector, comparer: null);
    }

    /// <summary>
    /// Applies a key-generating function to each element of a sequence and
    /// returns a sequence that contains the elements of the original
    /// sequence as well its key and index inside the group of its key.
    /// An additional parameter specifies a comparer to use for testing the
    /// equivalence of keys.
    /// </summary>
    /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">
    /// Function that projects the key given an element in the source sequence.</param>
    /// <param name="comparer">
    /// The equality comparer to use to determine whether keys are
    /// equal. If <c>null</c>, the default equality comparer for
    /// <typeparamref name="TSource"/> is used.</param>
    /// <returns>
    /// A sequence of elements paired with their index within the key-group.
    /// </returns>
    public static IAsyncEnumerable<(int Index, TSource Element)> IndexBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.
            ScanBy<TSource, TKey, (int index, TSource element)>(
                keySelector,
                (_, _) => ValueTasks.FromResult((-1, default(TSource)!)),
                (state, _, element, _) => ValueTasks.FromResult((state.index + 1, element)),
                comparer).
            Select(tuple => (tuple.State.index, tuple.State.element));
    }
}