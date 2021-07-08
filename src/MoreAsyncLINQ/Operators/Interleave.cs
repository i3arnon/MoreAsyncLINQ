using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Interleave<TSource>(
            this IAsyncEnumerable<TSource> sequence,
            params IAsyncEnumerable<TSource>[] otherSequences)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));
            if (otherSequences is null) throw new ArgumentNullException(nameof(otherSequences));

            return Core(sequence, otherSequences);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> sequence,
                IAsyncEnumerable<TSource>[] otherSequences,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var sequences = new[] { sequence }.Concat(otherSequences);
                var enumerators = new ConfiguredCancelableAsyncEnumerable<TSource>.Enumerator?[otherSequences.Length + 1];

                try
                {
                    var enumeratorIndex = 0;
                    foreach (var enumerator in sequences.Select(sequence => sequence.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator()))
                    {
                        enumerators[enumeratorIndex] = enumerator;
                        if (await enumerator.MoveNextAsync())
                        {
                            yield return enumerator.Current;
                        }
                        else
                        {
                            enumerators[enumeratorIndex] = null;
                            await enumerator.DisposeAsync();
                        }

                        enumeratorIndex++;
                    }

                    var hasNext = true;
                    while (hasNext)
                    {
                        hasNext = false;
                        for (var index = 0; index < enumerators.Length; index++)
                        {
                            var enumerator = enumerators[index];
                            if (enumerator is null)
                            {
                                continue;
                            }

                            if (await enumerator.Value.MoveNextAsync())
                            {
                                hasNext = true;
                                yield return enumerator.Value.Current;
                            }
                            else
                            {
                                enumerators[index] = null;
                                await enumerator.Value.DisposeAsync();
                            }
                        }
                    }
                }
                finally
                {
                    foreach (var enumerator in enumerators)
                    {
                        if (enumerator is not null)
                        {
                            await enumerator.Value.DisposeAsync();
                        }
                    }
                }
            }
        }
    }
}