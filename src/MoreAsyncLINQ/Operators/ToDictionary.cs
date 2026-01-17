using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
    /// <see cref="KeyValuePair{TKey,TValue}" /> elements.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source sequence of key-value pairs.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A <see cref="Dictionary{TKey, TValue}"/> containing the values
    /// mapped to their keys.
    /// </returns>
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(comparer: null, cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
    /// <see cref="KeyValuePair{TKey,TValue}" /> elements. An additional
    /// parameter specifies a comparer for keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source sequence of key-value pairs.</param>
    /// <param name="comparer">The comparer for keys.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A <see cref="Dictionary{TKey, TValue}"/> containing the values
    /// mapped to their keys.
    /// </returns>
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
        IEqualityComparer<TKey>? comparer,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(
            static pair => pair.Key,
            static pair => pair.Value,
            comparer,
            cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
    /// tuples of 2 where the first item is the key and the second the
    /// value.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source sequence of couples (tuple of 2).</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A <see cref="Dictionary{TKey, TValue}"/> containing the values
    /// mapped to their keys.
    /// </returns>
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<(TKey Key, TValue Value)> source,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(comparer: null, cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
    /// tuples of 2 where the first item is the key and the second the
    /// value. An additional parameter specifies a comparer for keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source sequence of couples (tuple of 2).</param>
    /// <param name="comparer">The comparer for keys.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A <see cref="Dictionary{TKey, TValue}"/> containing the values
    /// mapped to their keys.
    /// </returns>
    public static ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(
        IAsyncEnumerable<(TKey Key, TValue Value)> source,
        IEqualityComparer<TKey>? comparer,
        CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionaryAsync(
            static tuple => tuple.Key,
            static tuple => tuple.Value,
            comparer,
            cancellationToken);
    }
}