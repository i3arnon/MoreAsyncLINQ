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
    /// Pads a sequence with default values in the beginning if it is narrower (shorter
    /// in length) than a given width.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PadStart<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

        return PadStartCore(
            source,
            width,
            default!,
            paddingSelector: null,
            default);
    }

    /// <summary>
    /// Pads a sequence with a given filler value in the beginning if it is narrower (shorter
    /// in length) than a given width.
    /// An additional parameter specifies the value to use for padding.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <param name="padding">The value to use for padding.</param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PadStart<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        TSource padding)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

        return PadStartCore(
            source,
            width,
            padding,
            paddingSelector: null,
            default);
    }

    /// <summary>
    /// Pads a sequence with a dynamic filler value in the beginning if it is narrower (shorter
    /// in length) than a given width.
    /// An additional parameter specifies the function to calculate padding.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <param name="paddingSelector">
    /// Function to calculate padding given the index of the missing element.
    /// </param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PadStart<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        Func<int, TSource> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return PadStartCore(
            source,
            width,
            default!,
            paddingSelector,
            default);
    }

    private static async IAsyncEnumerable<TSource> PadStartCore<TSource>(
        IAsyncEnumerable<TSource> source,
        int width,
        TSource padding,
        Func<int, TSource>? paddingSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var window = new TSource[width];
        var count = 0;
        await using (var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator())
        {
            for (; count < width && await enumerator.MoveNextAsync(); count++)
            {
                window[count] = enumerator.Current;
            }

            if (count == width)
            {
                for (var index = 0; index < count; index++)
                {
                    yield return window[index];
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return enumerator.Current;
                }

                yield break;
            }
        }

        var paddingLength = width - count;
        for (var index = 0; index < paddingLength; index++)
        {
            yield return paddingSelector is null ? padding : paddingSelector(index);
        }

        for (var index = 0; index < count; index++)
        {
            yield return window[index];
        }
    }

    /// <summary>
    /// Pads a sequence with a dynamic filler value in the beginning if it is narrower (shorter
    /// in length) than a given width.
    /// An additional parameter specifies the function to calculate padding.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <param name="paddingSelector">
    /// Function to calculate padding given the index of the missing element.
    /// </param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(PadStart)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PadStartAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        Func<int, ValueTask<TSource>> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return source.PadStartAwaitCore<TSource>(width, paddingSelector);
    }

    [Obsolete]
    private static async IAsyncEnumerable<TSource> PadStartAwaitCore<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        Func<int, ValueTask<TSource>> paddingSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
        var result =
            collectionCount is not null
                ? collectionCount >= width
                    ? source
                    : Enumerable.Range(start: 0, width - collectionCount.Value).
                        ToAsyncEnumerable().
                        Select((int index, CancellationToken _) => paddingSelector(index)).
                        Concat(source)
                : Core(source, width, paddingSelector);

        await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return element;
        }

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int width,
            Func<int, ValueTask<TSource>> paddingSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var window = new TSource[width];
            var count = 0;
            await using (var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator())
            {
                for (; count < width && await enumerator.MoveNextAsync(); count++)
                {
                    window[count] = enumerator.Current;
                }

                if (count == width)
                {
                    for (var index = 0; index < count; index++)
                    {
                        yield return window[index];
                    }

                    while (await enumerator.MoveNextAsync())
                    {
                        yield return enumerator.Current;
                    }

                    yield break;
                }
            }

            var paddingLength = width - count;
            for (var index = 0; index < paddingLength; index++)
            {
                yield return await paddingSelector(index).ConfigureAwait(false);
            }

            for (var index = 0; index < count; index++)
            {
                yield return window[index];
            }
        }
    }

    /// <summary>
    /// Pads a sequence with a dynamic filler value in the beginning if it is narrower (shorter
    /// in length) than a given width.
    /// An additional parameter specifies the function to calculate padding.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <param name="paddingSelector">
    /// Function to calculate padding given the index of the missing element.
    /// </param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PadStart<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        Func<int, CancellationToken, ValueTask<TSource>> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return PadStartCore(
            source,
            width,
            paddingSelector,
            default);
    }

    private static async IAsyncEnumerable<TSource> PadStartCore<TSource>(
        IAsyncEnumerable<TSource> source,
        int width,
        Func<int, CancellationToken, ValueTask<TSource>> paddingSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var window = new TSource[width];
        var count = 0;
        await using (var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator())
        {
            for (; count < width && await enumerator.MoveNextAsync(); count++)
            {
                window[count] = enumerator.Current;
            }

            if (count == width)
            {
                for (var index = 0; index < count; index++)
                {
                    yield return window[index];
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return enumerator.Current;
                }

                yield break;
            }
        }

        var paddingLength = width - count;
        for (var index = 0; index < paddingLength; index++)
        {
            yield return await paddingSelector(index, cancellationToken);
        }

        for (var index = 0; index < count; index++)
        {
            yield return window[index];
        }
    }
}