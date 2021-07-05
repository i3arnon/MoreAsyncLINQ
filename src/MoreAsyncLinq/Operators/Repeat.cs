using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be greater or equal to zero.");

            return source.RepeatCore(count);
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.RepeatCore(count: null);
        }

        private static async IAsyncEnumerable<TSource> RepeatCore<TSource>(
            this IAsyncEnumerable<TSource> source,
            int? count,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var memo = source.Memoize();
            await using ((memo as IAsyncDisposable).ConfigureAwait(false))
            {
                while (true)
                {
                    if (count is not null)
                    {
                        if (count == 0)
                        {
                            break;
                        }

                        count--;
                    }

                    await foreach (var element in memo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return element;
                    }
                }
            }
        }
    }
}