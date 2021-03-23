using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Backsert<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            int index)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            return index == 0
                ? first.Concat(second)
                : Core(first, second, index);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> first,
                IAsyncEnumerable<TSource> second,
                int index,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = first.CountDown(index).WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                if (!await enumerator.MoveNextAsync())
                {
                    yield break;
                }

                var (countdown, element) = enumerator.Current;
                if (countdown is not null && countdown != index - 1)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"{nameof(index)} is greater than the length of {nameof(first)}.");
                }

                do
                {
                    (countdown, element) = enumerator.Current;
                    if (countdown == index - 1)
                    {
                        // ReSharper disable once PossibleMultipleEnumeration
                        await foreach (var secondElement in second.WithCancellation(cancellationToken).ConfigureAwait(false))
                        {
                            yield return secondElement;
                        }
                    }

                    yield return element;
                } while (await enumerator.MoveNextAsync());
            }
        }
    }
}