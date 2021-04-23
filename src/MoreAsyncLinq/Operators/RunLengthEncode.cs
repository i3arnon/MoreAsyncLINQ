using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<(TSource Element, int RunCount)> RunLengthEncode<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.RunLengthEncode(comparer: null);
        }

        public static IAsyncEnumerable<(TSource Element, int RunCount)> RunLengthEncode<TSource>(
            this IAsyncEnumerable<TSource> source,
            IEqualityComparer<TSource>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return Core(source, comparer ?? EqualityComparer<TSource>.Default);

            static async IAsyncEnumerable<(TSource, int)> Core(
                IAsyncEnumerable<TSource> source,
                IEqualityComparer<TSource> comparer,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }

                var previousItem = enumerator.Current;
                var runCount = 1;
                while (await enumerator.MoveNextAsync())
                {
                    if (comparer.Equals(previousItem, enumerator.Current))
                    {
                        runCount++;
                    }
                    else
                    {
                        yield return (previousItem, runCount);

                        previousItem = enumerator.Current;
                        runCount = 1;
                    }
                }

                yield return (previousItem, runCount);
            }
        }
    }
}