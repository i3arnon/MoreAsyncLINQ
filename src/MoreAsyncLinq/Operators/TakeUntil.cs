using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TakeUntil<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, bool> predicate,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return element;

                    if (predicate(element))
                    {
                        yield break;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> TakeUntilAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
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
}