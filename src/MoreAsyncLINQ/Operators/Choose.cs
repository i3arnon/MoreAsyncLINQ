using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Applies a function to each element of the source sequence and
    /// returns a new sequence of result elements for source elements
    /// where the function returns a couple (2-tuple) having a <c>true</c>
    /// as its first element and result as the second.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements in the returned sequence.</typeparam>
    /// <param name="source"> The source sequence.</param>
    /// <param name="chooser">The function that is applied to each source
    /// element.</param>
    /// <returns>A sequence <typeparamref name="TResult"/> elements.</returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> Choose<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, (bool, TResult)> chooser)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (chooser is null) throw new ArgumentNullException(nameof(chooser));

        return Core(source, chooser);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, (bool, TResult)> chooser,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var (choose, result) = chooser(element);
                if (choose)
                {
                    yield return result;
                }
            }
        }
    }

    /// <summary>
    /// Applies a function to each element of the source sequence and
    /// returns a new sequence of result elements for source elements
    /// where the function returns a couple (2-tuple) having a <c>true</c>
    /// as its first element and result as the second.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements in the returned sequence.</typeparam>
    /// <param name="source"> The source sequence.</param>
    /// <param name="chooser">The function that is applied to each source
    /// element.</param>
    /// <returns>A sequence <typeparamref name="TResult"/> elements.</returns>
    /// <remarks>
    /// This method uses deferred execution semantics and streams its
    /// results.
    /// </remarks>
    public static IAsyncEnumerable<TResult> ChooseAwait<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<(bool, TResult)>> chooser)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (chooser is null) throw new ArgumentNullException(nameof(chooser));

        return Core(source, chooser);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<(bool, TResult)>> chooser,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var (choose, result) = await chooser(element).ConfigureAwait(false);
                if (choose)
                {
                    yield return result;
                }
            }
        }
    }
}