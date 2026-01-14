using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Pads a sequence with default values if it is narrower (shorter
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
    public static IAsyncEnumerable<TSource?> Pad<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

        return source.Pad(
            width,
            padding: default,
            paddingSelector: null);
    }

    /// <summary>
    /// Pads a sequence with a given filler value if it is narrower (shorter
    /// in length) than a given width.
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
    public static IAsyncEnumerable<TSource> Pad<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        TSource padding)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

        return source.Pad(
            width,
            padding,
            paddingSelector: null);
    }

    /// <summary>
    /// Pads a sequence with a dynamic filler value if it is narrower (shorter
    /// in length) than a given width.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <param name="paddingSelector">Function to calculate padding.</param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Pad<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        Func<int, TSource> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return source.Pad(
            width,
            padding: default,
            paddingSelector);
    }

    private static async IAsyncEnumerable<TSource> Pad<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        TSource? padding,
        Func<int, TSource>? paddingSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var count = 0;
        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return element;

            count++;
        }

        while (count < width)
        {
            yield return paddingSelector is null
                ? padding!
                : paddingSelector(count);

            count++;
        }
    }

    /// <summary>
    /// Pads a sequence with a dynamic filler value if it is narrower (shorter
    /// in length) than a given width.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to pad.</param>
    /// <param name="width">The width/length below which to pad.</param>
    /// <param name="paddingSelector">Function to calculate padding.</param>
    /// <returns>
    /// Returns a sequence that is at least as wide/long as the width/length
    /// specified by the <paramref name="width"/> parameter.
    /// </returns>
    /// <remarks>
    /// This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PadAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        Func<int, ValueTask<TSource>> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return source.PadAwait(
            width,
            padding: default,
            paddingSelector);
    }

    private static async IAsyncEnumerable<TSource> PadAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        int width,
        TSource? padding,
        Func<int, ValueTask<TSource>>? paddingSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var count = 0;
        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return element;

            count++;
        }

        while (count < width)
        {
            yield return paddingSelector is null
                ? padding!
                : await paddingSelector(count).ConfigureAwait(false);

            count++;
        }
    }
}