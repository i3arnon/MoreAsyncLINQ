using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Transposes a sequence of rows into a sequence of columns.
        /// </summary>
        /// <typeparam name="TSource">Type of source sequence elements.</typeparam>
        /// <param name="source">Source sequence to transpose.</param>
        /// <returns>
        /// Returns a sequence of columns in the source swapped into rows.
        /// </returns>
        /// <remarks>
        /// If a rows is shorter than a follow it then the shorter row's
        /// elements are skipped in the corresponding column sequences.
        /// This operator uses deferred execution and streams its results.
        /// Source sequence is consumed greedily when an iteration begins.
        /// The inner sequences representing rows are consumed lazily and
        /// resulting sequences of columns are streamed.
        /// </remarks>
        public static IAsyncEnumerable<TSource[]> Transpose<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> source)
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