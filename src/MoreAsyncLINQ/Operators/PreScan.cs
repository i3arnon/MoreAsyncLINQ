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
    /// Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
    /// </summary>
    /// <remarks>
    /// An exclusive prefix sum returns an equal-length sequence where the
    /// N-th element is the sum of the first N-1 input elements (the first
    /// element is a special case, it is set to the identity). More
    /// generally, the pre-scan allows any commutative binary operation,
    /// not just a sum.
    /// The inclusive version of PreScan is <see cref="MoreAsyncEnumerable.Scan{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, TSource})"/>.
    /// This operator uses deferred execution and streams its result.
    /// </remarks>
    /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="transformation">Transformation operation</param>
    /// <param name="identity">Identity element (see remarks)</param>
    /// <returns>The scanned sequence</returns>
    public static IAsyncEnumerable<TSource> PreScan<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, TSource> transformation,
        TSource identity)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (transformation is null) throw new ArgumentNullException(nameof(transformation));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(
                source,
                transformation,
                identity,
                default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource> transformation,
            TSource identity,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            var aggregator = identity;
            if (await enumerator.MoveNextAsync())
            {
                yield return aggregator;

                var element = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    aggregator = transformation(aggregator, element);
                    yield return aggregator;

                    element = enumerator.Current;
                }
            }
        }
    }

    /// <summary>
    /// Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
    /// </summary>
    /// <remarks>
    /// An exclusive prefix sum returns an equal-length sequence where the
    /// N-th element is the sum of the first N-1 input elements (the first
    /// element is a special case, it is set to the identity). More
    /// generally, the pre-scan allows any commutative binary operation,
    /// not just a sum.
    /// The inclusive version of PreScan is <see cref="MoreAsyncEnumerable.Scan{TSource}"/>.
    /// This operator uses deferred execution and streams its result.
    /// </remarks>
    /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="transformation">Transformation operation</param>
    /// <param name="identity">Identity element (see remarks)</param>
    /// <returns>The scanned sequence</returns>
    public static IAsyncEnumerable<TSource> PreScanAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TSource>> transformation,
        TSource identity)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (transformation is null) throw new ArgumentNullException(nameof(transformation));

        return Core(source, transformation, identity);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TSource>> transformation,
            TSource identity,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                
            var aggregator = identity;
            if (await enumerator.MoveNextAsync())
            {
                yield return aggregator;

                var element = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    aggregator = await transformation(aggregator, element).ConfigureAwait(false);
                    yield return aggregator;

                    element = enumerator.Current;
                }
            }
        }
    }
    
    /// <summary>
    /// Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
    /// </summary>
    /// <remarks>
    /// An exclusive prefix sum returns an equal-length sequence where the
    /// N-th element is the sum of the first N-1 input elements (the first
    /// element is a special case, it is set to the identity). More
    /// generally, the pre-scan allows any commutative binary operation,
    /// not just a sum.
    /// The inclusive version of PreScan is <see cref="MoreAsyncEnumerable.Scan{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, CancellationToken, ValueTask{TSource}})"/>.
    /// This operator uses deferred execution and streams its result.
    /// </remarks>
    /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="transformation">Transformation operation</param>
    /// <param name="identity">Identity element (see remarks)</param>
    /// <returns>The scanned sequence</returns>
    public static IAsyncEnumerable<TSource> PreScan<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, CancellationToken, ValueTask<TSource>> transformation,
        TSource identity)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (transformation is null) throw new ArgumentNullException(nameof(transformation));

        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(
                source,
                transformation,
                identity,
                default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, CancellationToken, ValueTask<TSource>> transformation,
            TSource identity,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();
                
            var aggregator = identity;
            if (await enumerator.MoveNextAsync())
            {
                yield return aggregator;

                var element = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    aggregator = await transformation(aggregator, element, cancellationToken);
                    yield return aggregator;

                    element = enumerator.Current;
                }
            }
        }
    }
}