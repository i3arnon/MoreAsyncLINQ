using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns the elements of the specified sequence or the specified
    /// value in a singleton collection if the sequence is empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequences.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="fallback">The value to return in a singleton
    /// collection if <paramref name="source"/> is empty.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that contains <paramref name="fallback"/>
    /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
    /// </returns>
    public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        this IAsyncEnumerable<TSource> source,
        TSource fallback)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return FallbackIfEmpty(
            source,
            count: 1,
            fallback,
            fallback2: default,
            fallback3: default,
            fallback4: default,
            fallback: null,
            default);
    }

    /// <summary>
    /// Returns the elements of a sequence, but if it is empty then
    /// returns an alternate sequence of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequences.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="fallback1">First value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <param name="fallback2">Second value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
    /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
    /// </returns>
    public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        this IAsyncEnumerable<TSource> source,
        TSource fallback1,
        TSource fallback2)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return FallbackIfEmpty(
            source,
            count: 2,
            fallback1,
            fallback2,
            fallback3: default,
            fallback4: default,
            fallback: null,
            default);
    }

    /// <summary>
    /// Returns the elements of a sequence, but if it is empty then
    /// returns an alternate sequence of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequences.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="fallback1">First value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <param name="fallback2">Second value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <param name="fallback3">Third value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
    /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
    /// </returns>
    public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        this IAsyncEnumerable<TSource> source,
        TSource fallback1,
        TSource fallback2,
        TSource fallback3)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return FallbackIfEmpty(
            source,
            count: 3,
            fallback1,
            fallback2,
            fallback3,
            fallback4: default,
            fallback: null,
            default);
    }

    /// <summary>
    /// Returns the elements of a sequence, but if it is empty then
    /// returns an alternate sequence of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequences.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="fallback1">First value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <param name="fallback2">Second value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <param name="fallback3">Third value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <param name="fallback4">Fourth value of the alternate sequence that
    /// is returned if <paramref name="source"/> is empty.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
    /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
    /// </returns>
    public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        this IAsyncEnumerable<TSource> source,
        TSource fallback1,
        TSource fallback2,
        TSource fallback3,
        TSource fallback4)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return FallbackIfEmpty(
            source,
            count: 4,
            fallback1,
            fallback2,
            fallback3,
            fallback4,
            fallback: null,
            default);
    }

    /// <summary>
    /// Returns the elements of a sequence, but if it is empty then
    /// returns an alternate sequence from an array of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequences.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="fallback">The array that is returned as the alternate
    /// sequence if <paramref name="source"/> is empty.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
    /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
    /// </returns>
    public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        this IAsyncEnumerable<TSource> source,
        params TSource[] fallback)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));

        return source.FallbackIfEmpty(fallback.ToAsyncEnumerable());
    }

    /// <summary>
    /// Returns the elements of a sequence, but if it is empty then
    /// returns an alternate sequence of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the sequences.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="fallback">The alternate sequence that is returned
    /// if <paramref name="source"/> is empty.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
    /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
    /// </returns>
    public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        this IAsyncEnumerable<TSource> source,
        IAsyncEnumerable<TSource> fallback)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));

        return source.IsKnownEmpty() &&
               fallback.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : FallbackIfEmpty(
                source,
                count: null,
                fallback1: default,
                fallback2: default,
                fallback3: default,
                fallback4: default,
                fallback,
                default);
    }

    private static async IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
        IAsyncEnumerable<TSource> source,
        int? count,
        TSource? fallback1,
        TSource? fallback2,
        TSource? fallback3,
        TSource? fallback4,
        IAsyncEnumerable<TSource>? fallback,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using (var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator())
        {
            if (await enumerator.MoveNextAsync())
            {
                do
                {
                    yield return enumerator.Current;
                } while (await enumerator.MoveNextAsync());

                yield break;
            }
        }

        await foreach (var element in FallbackCollection().WithCancellation(cancellationToken))
        {
            yield return element;
        }

        yield break;

        IAsyncEnumerable<TSource> FallbackCollection()
        {
            return fallback ?? FallbackArguments().ToAsyncEnumerable();

            IEnumerable<TSource> FallbackArguments()
            {
                Debug.Assert(count is >= 1 and <= 4);

                yield return fallback1!;

                if (count >= 2)
                {
                    yield return fallback2!;
                }

                if (count >= 3)
                {
                    yield return fallback3!;
                }

                if (count == 4)
                {
                    yield return fallback4!;
                }
            }
        }
    }
}