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
    /// Asserts that a source sequence contains a given count of elements.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Count to assert.</param>
    /// <returns>
    /// Returns the original sequence as long it is contains the
    /// number of elements specified by <paramref name="count"/>.
    /// Otherwise it throws <see cref="Exception" />.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> AssertCount<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        return source.AssertCount(
            count,
            static (comparison, count) => new InvalidOperationException($"Sequence contains too {(comparison < 0 ? "few" : "many")} elements when exactly {count:N0} {(count == 1 ? "was" : "were")} expected."));
    }

    /// <summary>
    /// Asserts that a source sequence contains a given count of elements.
    /// A parameter specifies the exception to be thrown.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Count to assert.</param>
    /// <param name="errorSelector">
    /// Function that receives a comparison (a negative integer if actual
    /// count is less than <paramref name="count"/> and a positive integer
    /// if actual count is greater than <paramref name="count"/>) and
    /// <paramref name="count"/> as arguments and which returns the
    /// <see cref="Exception"/> object to throw.</param>
    /// <returns>
    /// Returns the original sequence as long it is contains the
    /// number of elements specified by <paramref name="count"/>.
    /// Otherwise it throws the <see cref="Exception" /> object
    /// returned by calling <paramref name="errorSelector"/>.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> AssertCount<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<int, int, Exception> errorSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        
        if (source.IsKnownEmpty())
        {
            return count == 0
                ? AsyncEnumerable.Empty<TSource>()
                : throw errorSelector(-1, count);
        }

        return Core(
            source,
            count,
            errorSelector,
            default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<int, int, Exception> errorSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var currentCount = 0;
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                currentCount++;
                if (currentCount > count)
                {
                    throw errorSelector(1, count);
                }

                yield return element;
            }

            if (currentCount != count)
            {
                throw errorSelector(-1, count);
            }
        }
    }

    /// <summary>
    /// Asserts that a source sequence contains a given count of elements.
    /// A parameter specifies the exception to be thrown.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Count to assert.</param>
    /// <param name="errorSelector">
    /// Function that receives a comparison (a negative integer if actual
    /// count is less than <paramref name="count"/> and a positive integer
    /// if actual count is greater than <paramref name="count"/>) and
    /// <paramref name="count"/> as arguments and which returns the
    /// <see cref="Exception"/> object to throw.</param>
    /// <returns>
    /// Returns the original sequence as long it is contains the
    /// number of elements specified by <paramref name="count"/>.
    /// Otherwise it throws the <see cref="Exception" /> object
    /// returned by calling <paramref name="errorSelector"/>.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(AssertCount)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> AssertCountAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<int, int, ValueTask<Exception>> errorSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        return Core(source, count, errorSelector);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<int, int, ValueTask<Exception>> errorSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var currentCount = 0;
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                currentCount++;
                if (currentCount > count)
                {
                    throw await errorSelector(1, count).ConfigureAwait(false);
                }

                yield return element;
            }

            if (currentCount != count)
            {
                throw await errorSelector(-1, count).ConfigureAwait(false);
            }
        }
    }
    
    /// <summary>
    /// Asserts that a source sequence contains a given count of elements.
    /// A parameter specifies the exception to be thrown.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Count to assert.</param>
    /// <param name="errorSelector">
    /// Function that receives a comparison (a negative integer if actual
    /// count is less than <paramref name="count"/> and a positive integer
    /// if actual count is greater than <paramref name="count"/>) and
    /// <paramref name="count"/> as arguments and which returns the
    /// <see cref="Exception"/> object to throw.</param>
    /// <returns>
    /// Returns the original sequence as long it is contains the
    /// number of elements specified by <paramref name="count"/>.
    /// Otherwise it throws the <see cref="Exception" /> object
    /// returned by calling <paramref name="errorSelector"/>.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> AssertCount<TSource>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<int, int, CancellationToken, ValueTask<Exception>> errorSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        return Core(
            source,
            count,
            errorSelector,
            default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<int, int, CancellationToken, ValueTask<Exception>> errorSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var currentCount = 0;
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                currentCount++;
                if (currentCount > count)
                {
                    throw await errorSelector(1, count, cancellationToken);
                }

                yield return element;
            }

            if (currentCount != count)
            {
                throw await errorSelector(-1, count, cancellationToken);
            }
        }
    }
}