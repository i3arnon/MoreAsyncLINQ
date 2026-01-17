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
    /// Returns a sequence resulting from applying a function to each
    /// element in the source sequence and its
    /// predecessor, except the first element which is
    /// only returned as the predecessor of the second element.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="resultSelector">A transform function to apply to
    /// each pair of sequence.</param>
    /// <returns>
    /// Returns the resulting sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> Pairwise<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, TResult> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
        
        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(source, resultSelector, default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                var previous = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    yield return resultSelector(previous, enumerator.Current);

                    previous = enumerator.Current;
                }
            }
        }
    }

    /// <summary>
    /// Returns a sequence resulting from applying a function to each
    /// element in the source sequence and its
    /// predecessor, with the exception of the first element which is
    /// only returned as the predecessor of the second element.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="resultSelector">A transform function to apply to
    /// each pair of sequence.</param>
    /// <returns>
    /// Returns the resulting sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Pairwise)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> PairwiseAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                var previous = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    yield return await resultSelector(previous, enumerator.Current).ConfigureAwait(false);

                    previous = enumerator.Current;
                }
            }
        }
    }
    
    /// <summary>
    /// Returns a sequence resulting from applying a function to each
    /// element in the source sequence and its
    /// predecessor, except the first element which is
    /// only returned as the predecessor of the second element.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="resultSelector">A transform function to apply to
    /// each pair of sequence.</param>
    /// <returns>
    /// Returns the resulting sequence.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> Pairwise<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(source, resultSelector, default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                var previous = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    yield return await resultSelector(previous, enumerator.Current, cancellationToken);

                    previous = enumerator.Current;
                }
            }
        }
    }
}