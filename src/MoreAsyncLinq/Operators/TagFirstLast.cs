using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> TagFirstLast<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool, bool, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Index().CountDown(
                count: 1,
                (indexedElement, countDownCount) =>
                    resultSelector(
                        indexedElement.Element,
                        indexedElement.Index == 0,
                        countDownCount == 0));
        }

        public static IAsyncEnumerable<TResult> TagFirstLastAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool, bool, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Index().CountDownAwait(
                count: 1,
                (indexedElement, countDownCount) =>
                    resultSelector(
                        indexedElement.Element,
                        indexedElement.Index == 0,
                        countDownCount == 0));
        }
    }
}