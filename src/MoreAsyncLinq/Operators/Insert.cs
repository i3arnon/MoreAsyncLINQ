using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Insert<TSource>(
            this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            int index)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            return Core(first, second, index);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> first,
                IAsyncEnumerable<TSource> second,
                int index,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator =
                    first.
                        WithCancellation(cancellationToken).
                        ConfigureAwait(false).
                        GetAsyncEnumerator();

                var currentIndex = 0;
                for (; currentIndex < index && await enumerator.MoveNextAsync(); currentIndex++)
                {
                    yield return enumerator.Current;
                }

                if (currentIndex < index)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"{nameof(index)} is greater than the length of {nameof(first)}");
                }

                await foreach (var element in second.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return element;
                }

                while (await enumerator.MoveNextAsync())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}