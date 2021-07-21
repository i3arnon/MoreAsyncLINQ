using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> ZipShortest<T1, T2, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.Zip<T1, T2, T2, T2, TResult>(
                second,
                third: null,
                fourth: null,
                (firstElement, secondElement, _, _) => resultSelector(firstElement, secondElement),
                limit: 0);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> ZipShortest<T1, T2, T3, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.Zip<T1, T2, T3, T3, TResult>(
                second,
                third,
                fourth: null,
                (firstElement, secondElement, thirdElement, _) => resultSelector(firstElement, secondElement, thirdElement),
                limit: 0);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="fourth">The fourth sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> ZipShortest<T1, T2, T3, T4, TResult>(
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

            return first.Zip(second, third, fourth, resultSelector, limit: 0);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> ZipShortestAwait<T1, T2, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.ZipAwait<T1, T2, T2, T2, TResult>(
                second,
                third: null,
                fourth: null,
                (firstElement, secondElement, _, _) => resultSelector(firstElement, secondElement),
                limit: 0);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> ZipShortestAwait<T1, T2, T3, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.ZipAwait<T1, T2, T3, T3, TResult>(
                second,
                third,
                fourth: null,
                (firstElement, secondElement, thirdElement, _) => resultSelector(firstElement, secondElement, thirdElement),
                limit: 0);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="fourth">The fourth sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        public static IAsyncEnumerable<TResult> ZipShortestAwait<T1, T2, T3, T4, TResult>(
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

            return first.ZipAwait(second, third, fourth, resultSelector, limit: 0);
        }
    }
}