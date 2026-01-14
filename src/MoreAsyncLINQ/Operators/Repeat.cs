using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Repeats the sequence the specified number of times.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in sequence</typeparam>
    /// <param name="source">The sequence to repeat</param>
    /// <param name="count">Number of times to repeat the sequence</param>
    /// <returns>A sequence produced from the repetition of the original source sequence</returns>
    public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be greater or equal to zero.");

        return source.RepeatCore(count);
    }


    /// <summary>
    /// Repeats the sequence forever.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in sequence</typeparam>
    /// <param name="source">The sequence to repeat</param>
    /// <returns>A sequence produced from the infinite repetition of the original source sequence</returns>
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