#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Pad)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PadAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        int width,
        Func<int, ValueTask<TSource>> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return PadAwait(
            source,
            width,
            padding: default,
            paddingSelector);
    }

    [Obsolete]
    private static async IAsyncEnumerable<TSource> PadAwait<TSource>(
        IAsyncEnumerable<TSource> source,
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

