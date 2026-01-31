#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(SkipUntil)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> SkipUntilAwait<TSource>(
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
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            do
            {
                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }
            }
            while (!await predicate(enumerator.Current).ConfigureAwait(false));

            while (await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }
    }
}

