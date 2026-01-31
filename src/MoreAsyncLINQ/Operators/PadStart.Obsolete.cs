#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(PadStart)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PadStartAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        int width,
        Func<int, ValueTask<TSource>> paddingSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

        return PadStartAwaitCore(source, width, paddingSelector);
    }

    [Obsolete]
    private static async IAsyncEnumerable<TSource> PadStartAwaitCore<TSource>(
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

