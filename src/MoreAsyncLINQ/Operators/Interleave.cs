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
        /// <summary>
        /// Interleaves the elements of two or more sequences into a single sequence, skipping sequences as they are consumed
        /// </summary>
        /// <remarks>
        /// Interleave combines sequences by visiting each in turn, and returning the first element of each, followed
        /// by the second, then the third, and so on.
        /// This operator behaves in a deferred and streaming manner.<br/>
        /// When sequences are of unequal length, this method will skip those sequences that have been fully consumed
        /// and continue interleaving the remaining sequences.<br/>
        /// The sequences are interleaved in the order that they appear in the <paramref name="otherSequences"/>
        /// collection, with <paramref name="sequence"/> as the first sequence.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
        /// <param name="sequence">The first sequence in the interleave group</param>
        /// <param name="otherSequences">The other sequences in the interleave group</param>
        /// <returns>A sequence of interleaved elements from all of the source sequences</returns>
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