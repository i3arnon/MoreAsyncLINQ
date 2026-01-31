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
    /// Returns the Cartesian product of two sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        Func<T1, T2, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of three sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        Func<T1, T2, T3, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of four sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        Func<T1, T2, T3, T4, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();

            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of five sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        Func<T1, T2, T3, T4, T5, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of six sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty() &&
               sixth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            await using var sixthMemo = sixth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    sixthElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of seven sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="T7">
    /// The type of the elements of <paramref name="seventh"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="seventh">The seventh sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        IAsyncEnumerable<T7> seventh,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (seventh is null) throw new ArgumentNullException(nameof(seventh));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty() &&
               sixth.IsKnownEmpty() &&
               seventh.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            await using var sixthMemo = sixth.Memoize();
            await using var seventhMemo = seventh.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken))
            await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    sixthElement,
                    seventhElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of eight sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="T7">
    /// The type of the elements of <paramref name="seventh"/>.</typeparam>
    /// <typeparam name="T8">
    /// The type of the elements of <paramref name="eighth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="seventh">The seventh sequence of elements.</param>
    /// <param name="eighth">The eighth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        IAsyncEnumerable<T7> seventh,
        IAsyncEnumerable<T8> eighth,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (seventh is null) throw new ArgumentNullException(nameof(seventh));
        if (eighth is null) throw new ArgumentNullException(nameof(eighth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty() &&
               sixth.IsKnownEmpty() &&
               seventh.IsKnownEmpty() &&
               eighth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eighth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            IAsyncEnumerable<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            await using var sixthMemo = sixth.Memoize();
            await using var seventhMemo = seventh.Memoize();
            await using var eighthMemo = eighth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken))
            await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken))
            await foreach (var eighthElement in eighthMemo.WithCancellation(cancellationToken))
            {
                yield return resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    sixthElement,
                    seventhElement,
                    eighthElement);
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of two sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        Func<T1, T2, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement).
                        ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of three sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        Func<T1, T2, T3, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, third, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();
            var thirdMemo = third.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement).
                        ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of four sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, third, fourth, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();
            var thirdMemo = third.Memoize();
            var fourthMemo = fourth.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement).
                        ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of five sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        Func<T1, T2, T3, T4, T5, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, third, fourth, fifth, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();
            var thirdMemo = third.Memoize();
            var fourthMemo = fourth.Memoize();
            var fifthMemo = fifth.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement).
                        ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of six sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, T6, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, third, fourth, fifth, sixth, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();
            var thirdMemo = third.Memoize();
            var fourthMemo = fourth.Memoize();
            var fifthMemo = fifth.Memoize();
            var sixthMemo = sixth.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement,
                            sixthElement).
                        ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of seven sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="T7">
    /// The type of the elements of <paramref name="seventh"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="seventh">The seventh sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        IAsyncEnumerable<T7> seventh,
        Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (seventh is null) throw new ArgumentNullException(nameof(seventh));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, third, fourth, fifth, sixth, seventh, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();
            var thirdMemo = third.Memoize();
            var fourthMemo = fourth.Memoize();
            var fifthMemo = fifth.Memoize();
            var sixthMemo = sixth.Memoize();
            var seventhMemo = seventh.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((seventhMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement,
                            sixthElement,
                            seventhElement).
                        ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Returns the Cartesian product of eight sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="T7">
    /// The type of the elements of <paramref name="seventh"/>.</typeparam>
    /// <typeparam name="T8">
    /// The type of the elements of <paramref name="eighth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="seventh">The seventh sequence of elements.</param>
    /// <param name="eighth">The eighth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Cartesian)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        IAsyncEnumerable<T7> seventh,
        IAsyncEnumerable<T8> eighth,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (seventh is null) throw new ArgumentNullException(nameof(seventh));
        if (eighth is null) throw new ArgumentNullException(nameof(eighth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            IAsyncEnumerable<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var secondMemo = second.Memoize();
            var thirdMemo = third.Memoize();
            var fourthMemo = fourth.Memoize();
            var fifthMemo = fifth.Memoize();
            var sixthMemo = sixth.Memoize();
            var seventhMemo = seventh.Memoize();
            var eighthMemo = eighth.Memoize();

            await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((seventhMemo as IAsyncDisposable).ConfigureAwait(false))
            await using ((eighthMemo as IAsyncDisposable).ConfigureAwait(false))
            {
                await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                await foreach (var eighthElement in eighthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement,
                            sixthElement,
                            seventhElement,
                            eighthElement).
                        ConfigureAwait(false);
                }
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of two sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        Func<T1, T2, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of three sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        Func<T1, T2, T3, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of four sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        Func<T1, T2, T3, T4, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of five sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        Func<T1, T2, T3, T4, T5, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of six sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty() &&
               sixth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            await using var sixthMemo = sixth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    sixthElement,
                    cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of seven sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="T7">
    /// The type of the elements of <paramref name="seventh"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="seventh">The seventh sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        IAsyncEnumerable<T7> seventh,
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (seventh is null) throw new ArgumentNullException(nameof(seventh));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty() &&
               sixth.IsKnownEmpty() &&
               seventh.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            await using var sixthMemo = sixth.Memoize();
            await using var seventhMemo = seventh.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken))
            await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    sixthElement,
                    seventhElement,
                    cancellationToken);
            }
        }
    }
    
    /// <summary>
    /// Returns the Cartesian product of eight sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the elements of <paramref name="first"/>.</typeparam>
    /// <typeparam name="T2">
    /// The type of the elements of <paramref name="second"/>.</typeparam>
    /// <typeparam name="T3">
    /// The type of the elements of <paramref name="third"/>.</typeparam>
    /// <typeparam name="T4">
    /// The type of the elements of <paramref name="fourth"/>.</typeparam>
    /// <typeparam name="T5">
    /// The type of the elements of <paramref name="fifth"/>.</typeparam>
    /// <typeparam name="T6">
    /// The type of the elements of <paramref name="sixth"/>.</typeparam>
    /// <typeparam name="T7">
    /// The type of the elements of <paramref name="seventh"/>.</typeparam>
    /// <typeparam name="T8">
    /// The type of the elements of <paramref name="eighth"/>.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name="first">The first sequence of elements.</param>
    /// <param name="second">The second sequence of elements.</param>
    /// <param name="third">The third sequence of elements.</param>
    /// <param name="fourth">The fourth sequence of elements.</param>
    /// <param name="fifth">The fifth sequence of elements.</param>
    /// <param name="sixth">The sixth sequence of elements.</param>
    /// <param name="seventh">The seventh sequence of elements.</param>
    /// <param name="eighth">The eighth sequence of elements.</param>
    /// <param name="resultSelector">A projection function that combines
    /// elements from all the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name="first"/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>
    public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this IAsyncEnumerable<T1> first,
        IAsyncEnumerable<T2> second,
        IAsyncEnumerable<T3> third,
        IAsyncEnumerable<T4> fourth,
        IAsyncEnumerable<T5> fifth,
        IAsyncEnumerable<T6> sixth,
        IAsyncEnumerable<T7> seventh,
        IAsyncEnumerable<T8> eighth,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask<TResult>> resultSelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (third is null) throw new ArgumentNullException(nameof(third));
        if (fourth is null) throw new ArgumentNullException(nameof(fourth));
        if (fifth is null) throw new ArgumentNullException(nameof(fifth));
        if (sixth is null) throw new ArgumentNullException(nameof(sixth));
        if (seventh is null) throw new ArgumentNullException(nameof(seventh));
        if (eighth is null) throw new ArgumentNullException(nameof(eighth));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return first.IsKnownEmpty() &&
               second.IsKnownEmpty() &&
               third.IsKnownEmpty() &&
               fourth.IsKnownEmpty() &&
               fifth.IsKnownEmpty() &&
               sixth.IsKnownEmpty() &&
               seventh.IsKnownEmpty() &&
               eighth.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eighth,
                resultSelector,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            IAsyncEnumerable<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var secondMemo = second.Memoize();
            await using var thirdMemo = third.Memoize();
            await using var fourthMemo = fourth.Memoize();
            await using var fifthMemo = fifth.Memoize();
            await using var sixthMemo = sixth.Memoize();
            await using var seventhMemo = seventh.Memoize();
            await using var eighthMemo = eighth.Memoize();
            
            await foreach (var firstElement in first.WithCancellation(cancellationToken))
            await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken))
            await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken))
            await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken))
            await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken))
            await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken))
            await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken))
            await foreach (var eighthElement in eighthMemo.WithCancellation(cancellationToken))
            {
                yield return await resultSelector(
                    firstElement,
                    secondElement,
                    thirdElement,
                    fourthElement,
                    fifthElement,
                    sixthElement,
                    seventhElement,
                    eighthElement,
                    cancellationToken);
            }
        }
    }
}