using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;
using static System.Math;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TakeLast<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return count <= 0
                ? Empty<TSource>()
                : Core(source, count);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int count,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
                var result =
                    collectionCount is null
                        ? source.
                            CountDown(count).
                            SkipWhile(tuple => tuple.Countdown is null).
                            Select(tuple => tuple.Element)
                        : source.Slice(Max(0, collectionCount.Value - count), int.MaxValue);

                await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return element;
                }
            }
        }
    }
}