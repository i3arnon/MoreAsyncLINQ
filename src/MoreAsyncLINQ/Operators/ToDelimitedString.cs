using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<bool> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<byte> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<char> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<decimal> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<double> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<float> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<int> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<long> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<sbyte> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<short> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<string> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<uint> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<ulong> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync(
        this IAsyncEnumerable<ushort> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static ValueTask<string> ToDelimitedStringAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        string delimiter,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

        return source.ToDelimitedStringAsync(
            delimiter,
            static (builder, element) => builder.Append(element),
            cancellationToken);
    }

    private static async ValueTask<string> ToDelimitedStringAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        string delimiter,
        Action<StringBuilder, TSource> appender,
        CancellationToken cancellationToken)
    {
        await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

        if (!await enumerator.MoveNextAsync())
        {
            return string.Empty;
        }

        var element = enumerator.Current;
        var firstString = element?.ToString();
        if (!await enumerator.MoveNextAsync())
        {
            return firstString ?? string.Empty;
        }

        var builder = new StringBuilder(firstString);

        do
        {
            element = enumerator.Current;
            builder.Append(delimiter);
            appender(builder, element);
        } while (await enumerator.MoveNextAsync());

        return builder.ToString();
    }
}