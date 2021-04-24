using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<IList<TSource>> Window<TSource>(
            this IAsyncEnumerable<TSource> source,
            int size)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            return Core(source, size);

            static async IAsyncEnumerable<TSource[]> Core(
                IAsyncEnumerable<TSource> source,
                int size,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                var window = new TSource[size];
                int index;
                for (index = 0; index < size && await enumerator.MoveNextAsync(); index++)
                {
                    window[index] = enumerator.Current;
                }

                if (index < size)
                {
                    yield break;
                }

                while (await enumerator.MoveNextAsync())
                {
                    var nextWindow = new TSource[size];
                    Array.Copy(window, sourceIndex: 1, nextWindow, destinationIndex: 0, size - 1);
                    nextWindow[size - 1] = enumerator.Current;

                    yield return window;

                    window = nextWindow;
                }

                yield return window;
            }
        }
    }
}