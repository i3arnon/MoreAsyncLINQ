#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(AssertCount)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> AssertCountAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        int count,
        Func<int, int, ValueTask<Exception>> errorSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        return Core(source, count, errorSelector);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            int count,
            Func<int, int, ValueTask<Exception>> errorSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var currentCount = 0;
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                currentCount++;
                if (currentCount > count)
                {
                    throw await errorSelector(1, count).ConfigureAwait(false);
                }

                yield return element;
            }

            if (currentCount != count)
            {
                throw await errorSelector(-1, count).ConfigureAwait(false);
            }
        }
    }
}

