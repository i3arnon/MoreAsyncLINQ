#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(PartialSortBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PreScanAwait<TSource>(
        IAsyncEnumerable<TSource> source,
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
}

