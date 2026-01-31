#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(TagFirstLast)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> TagFirstLastAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, bool, bool, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return CountDownAwait(
            source.Index(startIndex: 0),
            count: 1,
            (indexedElement, countDownCount) =>
                resultSelector(
                    indexedElement.Element,
                    indexedElement.Index == 0,
                    countDownCount == 0));
    }
}

