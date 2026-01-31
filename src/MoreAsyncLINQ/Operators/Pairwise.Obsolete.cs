#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Pairwise)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> PairwiseAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                var previous = enumerator.Current;
                while (await enumerator.MoveNextAsync())
                {
                    yield return await resultSelector(previous, enumerator.Current).ConfigureAwait(false);

                    previous = enumerator.Current;
                }
            }
        }
    }
}

