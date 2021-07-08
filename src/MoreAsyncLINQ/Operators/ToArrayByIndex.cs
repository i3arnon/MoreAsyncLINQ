using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
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