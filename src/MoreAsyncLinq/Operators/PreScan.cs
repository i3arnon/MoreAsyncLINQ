using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> PreScan<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource> transformation,
            TSource identity)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, transformation, identity);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource> transformation,
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
                        aggregator = transformation(aggregator, element);
                        yield return aggregator;

                        element = enumerator.Current;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> PreScanAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
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
}