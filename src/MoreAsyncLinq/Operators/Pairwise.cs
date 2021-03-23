using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Pairwise<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (await enumerator.MoveNextAsync())
                {
                    var previous = enumerator.Current;
                    while (await enumerator.MoveNextAsync())
                    {
                        yield return resultSelector(previous, enumerator.Current);

                        previous = enumerator.Current;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> PairwiseAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
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
}