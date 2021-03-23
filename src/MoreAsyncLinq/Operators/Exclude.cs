using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Exclude<TSource>(this IAsyncEnumerable<TSource> source, int startIndex, int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return count == 0
                ? source
                : Core(source, startIndex, count);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int startIndex,
                int count,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var index = 0;
                for (; index < startIndex && await enumerator.MoveNextAsync(); index++)
                {
                    yield return enumerator.Current;
                }

                var endIndex = startIndex + count;
                for (; index < endIndex && await enumerator.MoveNextAsync(); index++)
                {
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}