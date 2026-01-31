#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(FillForward)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> FillForwardAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return FillForwardCoreAwait(source, predicate, fillSelector: null);
    }

    [Obsolete($"Use an overload of {nameof(FillForward)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> FillForwardAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, TSource, ValueTask<TSource>> fillSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

        return FillForwardCoreAwait(source, predicate, fillSelector);
    }

    [Obsolete]
    private static async IAsyncEnumerable<TSource> FillForwardCoreAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, TSource, ValueTask<TSource>>? fillSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        (bool, TSource) nullableSeed = default;

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (await predicate(element).ConfigureAwait(false))
            {
                yield return nullableSeed is (true, { } seed)
                    ? fillSelector is not null
                        ? await fillSelector(element, seed).ConfigureAwait(false)
                        : seed
                    : element;
            }
            else
            {
                nullableSeed = (true, element);
                yield return element;
            }
        }
    }
}

