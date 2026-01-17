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
    /// specified index.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
    /// <param name="first">The source sequence.</param>
    /// <param name="second">The sequence that will be inserted.</param>
    /// <param name="index">
    /// The zero-based index at which to insert elements from
    /// <paramref name="second"/>.</param>
    /// <returns>
    /// A sequence that contains the elements of <paramref name="first"/>
    /// plus the elements of <paramref name="second"/> inserted at
    /// the given index.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="index"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown lazily if <paramref name="index"/> is greater than the
    /// length of <paramref name="first"/>. The validation occurs when
    /// yielding the next element after having iterated
    /// <paramref name="first"/> entirely.
    /// </exception>
    public static IAsyncEnumerable<TSource> Insert<TSource>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        int index)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TSource>()
            : Core(first, second, index, default);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second,
            int index,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator =
                first.
                    WithCancellation(cancellationToken).
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

            await foreach (var element in second.WithCancellation(cancellationToken))
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