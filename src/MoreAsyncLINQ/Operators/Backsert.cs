using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Inserts the elements of a sequence into another sequence at a
    /// specified index from the tail of the sequence, where zero always
    /// represents the last position, one represents the second-last
    /// element, two represents the third-last element and so on.
    /// </summary>
    /// <typeparam name="TSource">
    /// Type of elements in all sequences.</typeparam>
    /// <param name="first">The source sequence.</param>
    /// <param name="second">The sequence that will be inserted.</param>
    /// <param name="index">
    /// The zero-based index from the end of <paramref name="first"/> where
    /// elements from <paramref name="second"/> should be inserted.
    /// <paramref name="second"/>.</param>
    /// <returns>
    /// A sequence that contains the elements of <paramref name="first"/>
    /// plus the elements of <paramref name="second"/> inserted at
    /// the given index from the end of <paramref name="first"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="index"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown lazily if <paramref name="index"/> is greater than the
    /// length of <paramref name="first"/>. The validation occurs when
    /// the resulting sequence is iterated.
    /// </exception>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
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