using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Creates an array from an <see cref="IAsyncEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <returns>
        /// An array that contains the elements from the input sequence. The
        /// size of the array will be as large as the highest index returned
        /// by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TSource[]> ToArrayByIndexAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int> indexSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));

            return source.ToArrayByIndexAsync(
                indexSelector,
                static (element, _) => element,
                cancellationToken);
        }

        /// <summary>
        /// Creates an array from an <see cref="IAsyncEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array. The elements are projected into the array
        /// via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array that contains the projected elements from the input
        /// sequence. The size of the array will be as large as the highest
        /// index returned by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int> indexSelector,
            Func<TSource, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.ToArrayByIndexAsync(
                indexSelector,
                (element, _) => resultSelector(element),
                cancellationToken);
        }

        /// <summary>
        /// Creates an array from an <see cref="IAsyncEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array. The elements are projected into the array
        /// via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array that contains the projected elements from the input
        /// sequence. The size of the array will be as large as the highest
        /// index returned by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int> indexSelector,
            Func<TSource, int, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, indexSelector, resultSelector, cancellationToken);

            static async ValueTask<TResult[]> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, int> indexSelector,
                Func<TSource, int, TResult> resultSelector,
                CancellationToken cancellationToken)
            {
                List<(int index, TSource element)>? indexedElements = null;

                var maxIndex = int.MinValue;
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var index = indexSelector(element);
                    if (index < 0)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    maxIndex = Math.Max(index, maxIndex);
                    indexedElements ??= new List<(int, TSource)>();
                    indexedElements.Add((index, element));
                }

                if (indexedElements is null)
                {
                    return Array.Empty<TResult>();
                }

                return await indexedElements.
                    ToAsyncEnumerable().
                    ToArrayByIndexAsync(
                        maxIndex + 1,
                        tuple => tuple.index,
                        tuple => resultSelector(tuple.element, tuple.index),
                        cancellationToken).
                    ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IAsyncEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TSource[]> ToArrayByIndexAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int length,
            Func<TSource, int> indexSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));

            return source.ToArrayByIndexAsync(
                length,
                indexSelector,
                static(element, _) => element,
                cancellationToken);
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IAsyncEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array. The
        /// elements are projected into the array via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// projected elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int length,
            Func<TSource, int> indexSelector,
            Func<TSource, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.ToArrayByIndexAsync(
                length,
                indexSelector,
                (element, _) => resultSelector(element),
                cancellationToken);
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IAsyncEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array. The
        /// elements are projected into the array via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// projected elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int length,
            Func<TSource, int> indexSelector,
            Func<TSource, int, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, length, indexSelector, resultSelector, cancellationToken);

            static async ValueTask<TResult[]> Core(
                IAsyncEnumerable<TSource> source,
                int length,
                Func<TSource, int> indexSelector,
                Func<TSource, int, TResult> resultSelector,
                CancellationToken cancellationToken)
            {
                var array = new TResult[length];
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var index = indexSelector(element);
                    if (index < 0 || index > array.Length)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    array[index] = resultSelector(element, index);
                }

                return array;
            }
        }

        /// <summary>
        /// Creates an array from an <see cref="IAsyncEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <returns>
        /// An array that contains the elements from the input sequence. The
        /// size of the array will be as large as the highest index returned
        /// by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TSource[]> ToArrayByIndexAwaitAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<int>> indexSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));

            return source.ToArrayByIndexAwaitAsync(
                indexSelector,
                static (element, _) => ValueTasks.FromResult(element),
                cancellationToken);
        }

        /// <summary>
        /// Creates an array from an <see cref="IAsyncEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array. The elements are projected into the array
        /// via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array that contains the projected elements from the input
        /// sequence. The size of the array will be as large as the highest
        /// index returned by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<int>> indexSelector,
            Func<TSource, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.ToArrayByIndexAwaitAsync(
                indexSelector,
                (element, _) => resultSelector(element),
                cancellationToken);
        }

        /// <summary>
        /// Creates an array from an <see cref="IAsyncEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array. The elements are projected into the array
        /// via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array that contains the projected elements from the input
        /// sequence. The size of the array will be as large as the highest
        /// index returned by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<int>> indexSelector,
            Func<TSource, int, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, indexSelector, resultSelector, cancellationToken);

            static async ValueTask<TResult[]> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, ValueTask<int>> indexSelector,
                Func<TSource, int, ValueTask<TResult>> resultSelector,
                CancellationToken cancellationToken)
            {
                List<(int index, TSource element)>? indexedElements = null;

                var maxIndex = int.MinValue;
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var index = await indexSelector(element).ConfigureAwait(false);
                    if (index < 0)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    maxIndex = Math.Max(index, maxIndex);
                    indexedElements ??= new List<(int, TSource)>();
                    indexedElements.Add((index, element));
                }

                if (indexedElements is null)
                {
                    return Array.Empty<TResult>();
                }

                return await indexedElements.
                    ToAsyncEnumerable().
                    ToArrayByIndexAwaitAsync(
                        maxIndex + 1,
                        tuple => ValueTasks.FromResult(tuple.index),
                        tuple => resultSelector(tuple.element, tuple.index),
                        cancellationToken).
                    ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IAsyncEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TSource[]> ToArrayByIndexAwaitAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            int length,
            Func<TSource, ValueTask<int>> indexSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));

            return source.ToArrayByIndexAwaitAsync(
                length,
                indexSelector,
                static (element, _) => ValueTasks.FromResult(element),
                cancellationToken);
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IAsyncEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array. The
        /// elements are projected into the array via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// projected elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int length,
            Func<TSource, ValueTask<int>> indexSelector,
            Func<TSource, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.ToArrayByIndexAwaitAsync(
                length,
                indexSelector,
                (element, _) => resultSelector(element),
                cancellationToken);
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IAsyncEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array. The
        /// elements are projected into the array via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// projected elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>
        public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            int length,
            Func<TSource, ValueTask<int>> indexSelector,
            Func<TSource, int, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, length, indexSelector, resultSelector, cancellationToken);

            static async ValueTask<TResult[]> Core(
                IAsyncEnumerable<TSource> source,
                int length,
                Func<TSource, ValueTask<int>> indexSelector,
                Func<TSource, int, ValueTask<TResult>> resultSelector,
                CancellationToken cancellationToken)
            {
                var array = new TResult[length];
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var index = await indexSelector(element).ConfigureAwait(false);
                    if (index < 0 || index > array.Length)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    array[index] = await resultSelector(element, index).ConfigureAwait(false);
                }

                return array;
            }
        }
    }
}