using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource[]> Transpose<TSource>(IAsyncEnumerable<IAsyncEnumerable<TSource>> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return Core(source);

            static async IAsyncEnumerable<TSource[]> Core(
                IAsyncEnumerable<IAsyncEnumerable<TSource>> source,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                IAsyncEnumerator<TSource>?[] enumerators =
                    await source.
                        Select(enumerable => enumerable.GetAsyncEnumerator(cancellationToken)).
                        AcquireAsync(cancellationToken).
                        ConfigureAwait(false);
                
                try
                {
                    while (true)
                    {
                        var column = new TSource[enumerators.Length];
                        var count = 0;
                        for (var index = 0; index < enumerators.Length; index++)
                        {
                            var enumerator = enumerators[index];
                            if (enumerator is null)
                            {
                                continue;
                            }

                            if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                column[count] = enumerator.Current;
                                count++;
                            }
                            else
                            {
                                await enumerator.DisposeAsync().ConfigureAwait(false);
                                enumerators[index] = null;
                            }
                        }

                        if (count == 0)
                        {
                            yield break;
                        }

                        Array.Resize(ref column, count);
                        yield return column;
                    }
                }
                finally
                {
                    foreach (var enumerator in enumerators)
                    {
                        if (enumerator is not null)
                        {
                            await enumerator.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}