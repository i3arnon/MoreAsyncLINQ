#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(FillBackward)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> FillBackwardAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return source.FillBackwardCoreAwait(predicate, fillSelector: null);
    }

    [Obsolete($"Use an overload of {nameof(FillBackward)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> FillBackwardAwait<TSource>(
        IAsyncEnumerable<TSource> source,
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
}

