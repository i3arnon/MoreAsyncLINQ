using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

/// <summary>
/// Exposes the enumerator, which supports asynchronous iteration over a sequence of
/// some extremum property (maximum or minimum) of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects to enumerate.</typeparam>
public interface IExtremaAsyncEnumerable<out T> : IAsyncEnumerable<T>
{
    /// <summary>
    /// Returns a specified number of contiguous elements from the start of
    /// the sequence.
    /// </summary>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that contains the specified number
    /// of elements from the start of the input sequence.
    /// </returns>
    IAsyncEnumerable<T> Take(int count);

    /// <summary>
    /// Returns a specified number of contiguous elements at the end of the
    /// sequence.
    /// </summary>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that contains the specified number
    /// of elements at the end of the input sequence.
    /// </returns>
    IAsyncEnumerable<T> TakeLast(int count);
}

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns the first element of a sequence.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The input sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <exception cref="InvalidOperationException">
    /// The input sequence is empty.</exception>
    /// <returns>
    /// The first element of the input sequence.
    /// </returns>
    public static ValueTask<TSource> FirstAsync<TSource>(
        this IExtremaAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Take(count: 1).FirstAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the
    /// sequence contains no elements.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The input sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// Default value of type <typeparamref name="TSource"/> if source is empty;
    /// otherwise, the first element in source.
    /// </returns>
    public static ValueTask<TSource?> FirstOrDefaultAsync<TSource>(
        this IExtremaAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Take(count: 1).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the last element of a sequence.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The input sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <exception cref="InvalidOperationException">
    /// The input sequence is empty.</exception>
    /// <returns>
    /// The last element of the input sequence.
    /// </returns>
    public static ValueTask<TSource> LastAsync<TSource>(
        this IExtremaAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.TakeLast(count: 1).LastAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the last element of a sequence, or a default value if the
    /// sequence contains no elements.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The input sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// Default value of type <typeparamref name="TSource"/> if source is empty;
    /// otherwise, the last element in source.
    /// </returns>
    public static ValueTask<TSource?> LastOrDefaultAsync<TSource>(
        this IExtremaAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.TakeLast(count: 1).LastOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the only element of a sequence, and throws an exception if
    /// there is not exactly one element in the sequence.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The input sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <exception cref="InvalidOperationException">
    /// The input sequence contains more than one element.</exception>
    /// <returns>
    /// The single element of the input sequence.
    /// </returns>
    public static ValueTask<TSource> SingleAsync<TSource>(
        this IExtremaAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Take(count: 2).SingleAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the only element of a sequence, or a default value if the
    /// sequence is empty; this method throws an exception if there is more
    /// than one element in the sequence.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The input sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// The single element of the input sequence, or default value of type
    /// <typeparamref name="TSource"/> if the sequence contains no elements.
    /// </returns>
    public static ValueTask<TSource?> SingleOrDefaultAsync<TSource>(
        this IExtremaAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Take(count: 2).SingleOrDefaultAsync(cancellationToken);
    }
}