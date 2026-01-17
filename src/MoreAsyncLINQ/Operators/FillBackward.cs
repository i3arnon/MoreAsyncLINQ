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
    /// replaced with the following non-null reference or value in
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
    /// results. If references or values are null at the end of the
    /// sequence then they remain null.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackward<TSource>(this IAsyncEnumerable<TSource> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.FillBackward(static element => element is not null);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the following non-missing element in that sequence. An
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
    /// results. If elements are missing at the end of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillBackwardCore(
                source,
                predicate,
                fillSelector: null,
                default);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the following non-missing element in that sequence. Additional
    /// parameters specify two functions, one used to determine if an
    /// element is considered missing or not and another to provide the
    /// replacement for the missing element.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <param name="fillSelector">The function used to produce the element
    /// that will replace the missing one. Its first argument receives the
    /// current element considered missing while the second argument
    /// receives the next non-missing element.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the end of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Func<TSource, TSource, TSource> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillBackwardCore(
                source,
                predicate,
                fillSelector,
                default);
    }

    private static async IAsyncEnumerable<TSource> FillBackwardCore<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Func<TSource, TSource, TSource>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        List<TSource>? holes = null;

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (predicate(element))
            {
                holes ??= [];
                holes.Add(element);
            }
            else
            {
                if (holes is { Count: > 0 })
                {
                    foreach (var hole in holes)
                    {
                        yield return fillSelector is not null
                            ? fillSelector(hole, element)
                            : element;
                    }

                    holes.Clear();
                }

                yield return element;
            }
        }

        if (holes is { Count: > 0 })
        {
            foreach (var hole in holes)
            {
                yield return hole;
            }
        }
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the following non-missing element in that sequence. An
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
    /// results. If elements are missing at the end of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackwardAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.FillBackwardCoreAwait(predicate, fillSelector: null);
    }

    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the following non-missing element in that sequence. Additional
    /// parameters specify two functions, one used to determine if an
    /// element is considered missing or not and another to provide the
    /// replacement for the missing element.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <param name="fillSelector">The function used to produce the element
    /// that will replace the missing one. Its first argument receives the
    /// current element considered missing while the second argument
    /// receives the next non-missing element.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the end of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackwardAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, TSource, ValueTask<TSource>> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return source.FillBackwardCoreAwait(predicate, fillSelector);
    }

    private static async IAsyncEnumerable<TSource> FillBackwardCoreAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, TSource, ValueTask<TSource>>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        List<TSource>? holes = null;

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (await predicate(element).ConfigureAwait(false))
            {
                holes ??= new List<TSource>();
                holes.Add(element);
            }
            else
            {
                if (holes is { Count: > 0 })
                {
                    foreach (var hole in holes)
                    {
                        yield return fillSelector is not null
                            ? await fillSelector(hole, element).ConfigureAwait(false)
                            : element;
                    }

                    holes.Clear();
                }

                yield return element;
            }
        }

        if (holes is { Count: > 0 })
        {
            foreach (var hole in holes)
            {
                yield return hole;
            }
        }
    }
    
    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the following non-missing element in that sequence. An
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
    /// results. If elements are missing at the end of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillBackwardCore(
                source,
                predicate,
                fillSelector: null,
                default);
    }
    
    /// <summary>
    /// Returns a sequence with each missing element in the source replaced
    /// with the following non-missing element in that sequence. Additional
    /// parameters specify two functions, one used to determine if an
    /// element is considered missing or not and another to provide the
    /// replacement for the missing element.
    /// </summary>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">The function used to determine if
    /// an element in the sequence is considered missing.</param>
    /// <param name="fillSelector">The function used to produce the element
    /// that will replace the missing one. Its first argument receives the
    /// current element considered missing while the second argument
    /// receives the next non-missing element.</param>
    /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
    /// </returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results. If elements are missing at the end of the sequence then
    /// they remain missing.
    /// </remarks>
    public static IAsyncEnumerable<TSource> FillBackward<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate,
        Func<TSource, TSource, CancellationToken, ValueTask<TSource>> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FillBackwardCore(
                source,
                predicate,
                fillSelector,
                default);
    }
    
    private static async IAsyncEnumerable<TSource> FillBackwardCore<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<bool>> predicate,
        Func<TSource, TSource, CancellationToken, ValueTask<TSource>>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        List<TSource>? holes = null;

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (await predicate(element, cancellationToken))
            {
                holes ??= [];
                holes.Add(element);
            }
            else
            {
                if (holes is { Count: > 0 })
                {
                    foreach (var hole in holes)
                    {
                        yield return fillSelector is not null
                            ? await fillSelector(hole, element, cancellationToken)
                            : element;
                    }

                    holes.Clear();
                }

                yield return element;
            }
        }

        if (holes is { Count: > 0 })
        {
            foreach (var hole in holes)
            {
                yield return hole;
            }
        }
    }
}