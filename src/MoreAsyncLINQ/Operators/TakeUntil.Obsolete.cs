#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(TakeUntil)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> TakeUntilAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return Core(source, predicate);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;

                if (await predicate(element).ConfigureAwait(false))
                {
                    yield break;
                }
            }
        }
    }
}

