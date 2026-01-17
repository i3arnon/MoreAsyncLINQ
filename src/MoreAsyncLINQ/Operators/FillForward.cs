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
    /// Returns a sequence with each null reference or value in the source
    /// replaced with the previous non-null reference or value seen in
    /// that sequence.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with null references or values
    /// replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If references or values are null at the start of the
    /// sequence then they remain null.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillForward<TSource>(this IAsyncEnumerable<TSource> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.FillForward(static element => element is null);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the previous non-missing element seen in that sequence. An
    /// additional parameter specifies a function used to determine if an
    /// element is considered missing or not.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the start of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillForward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillForwardCore(
                source,
                predicate,
                fillSelector: null,
                default);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with one based on the previous non-missing element seen in that
    /// sequence. Additional parameters specify two functions, one used to
    /// determine if an element is considered missing or not and another
    /// to provide the replacement for the missing element.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <param name="fillSelector">The function used to produce the element
    /// that will replace the missing one. Its first argument receives the
    /// current element considered missing while the second argument
    /// receives the previous non-missing element.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the start of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillForward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Func<TSource, TSource, TSource> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillForwardCore(
                source,
                predicate,
                fillSelector,
                default);
    }

    private static async IAsyncEnumerable<TSource> FillForwardCore<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Func<TSource, TSource, TSource>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        (bool, TSource) nullableSeed = default;

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (predicate(element))
            {
                yield return nullableSeed is (true, { } seed)
                    ? fillSelector is not null
                        ? fillSelector(element, seed)
                        : seed
                    : element;
            }
            else
            {
                nullableSeed = (true, element);
                yield return element;
            }
        }
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the previous non-missing element seen in that sequence. An
    /// additional parameter specifies a function used to determine if an
    /// element is considered missing or not.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the start of the sequence then
    /// they remain missing.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(FillForward)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> FillForwardAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.FillForwardCoreAwait(predicate, fillSelector: null);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with one based on the previous non-missing element seen in that
    /// sequence. Additional parameters specify two functions, one used to
    /// determine if an element is considered missing or not and another
    /// to provide the replacement for the missing element.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <param name="fillSelector">The function used to produce the element
    /// that will replace the missing one. Its first argument receives the
    /// current element considered missing while the second argument
    /// receives the previous non-missing element.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the start of the sequence then
    /// they remain missing.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(FillForward)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> FillForwardAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, TSource, ValueTask<TSource>> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return source.FillForwardCoreAwait(predicate, fillSelector);
    }

    [Obsolete]
    private static async IAsyncEnumerable<TSource> FillForwardCoreAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, TSource, ValueTask<TSource>>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        (bool, TSource) nullableSeed = default;

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (await predicate(element).ConfigureAwait(false))
            {
                yield return nullableSeed is (true, { } seed)
                    ? fillSelector is not null
                        ? await fillSelector(element, seed).ConfigureAwait(false)
                        : seed
                    : element;
            }
            else
            {
                nullableSeed = (true, element);
                yield return element;
            }
        }
    }
    
    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the previous non-missing element seen in that sequence. An
    /// additional parameter specifies a function used to determine if an
    /// element is considered missing or not.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the start of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillForward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillForwardCore(
                source,
                predicate,
                fillSelector: null,
                default);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with one based on the previous non-missing element seen in that
    /// sequence. Additional parameters specify two functions, one used to
    /// determine if an element is considered missing or not and another
    /// to provide the replacement for the missing element.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <param name="fillSelector">The function used to produce the element
    /// that will replace the missing one. Its first argument receives the
    /// current element considered missing while the second argument
    /// receives the previous non-missing element.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the start of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillForward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate,
        Func<TSource, TSource, CancellationToken, ValueTask<TSource>> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillForwardCore(
                source,
                predicate,
                fillSelector,
                default);
    }

    private static async IAsyncEnumerable<TSource> FillForwardCore<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate,
        Func<TSource, TSource, CancellationToken, ValueTask<TSource>>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        (bool, TSource) nullableSeed = default;

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (await predicate(element, cancellationToken))
            {
                yield return nullableSeed is (true, { } seed)
                    ? fillSelector is not null
                        ? await fillSelector(element, seed, cancellationToken)
                        : seed
                    : element;
            }
            else
            {
                nullableSeed = (true, element);
                yield return element;
            }
        }
    }
}