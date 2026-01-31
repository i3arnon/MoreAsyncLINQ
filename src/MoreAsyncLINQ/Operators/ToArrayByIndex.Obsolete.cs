#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(ToArrayByIndexAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TSource[]> ToArrayByIndexAwaitAsync<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<int>> indexSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));

        return ToArrayByIndexAwaitAsync(
            source,
            indexSelector,
            static (element, _) => ValueTasks.FromResult(element),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToArrayByIndexAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<int>> indexSelector,
        Func<TSource, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return ToArrayByIndexAwaitAsync(
            source,
            indexSelector,
            (element, _) => resultSelector(element),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToArrayByIndexAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
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

            return await ToArrayByIndexAwaitAsync(
                    indexedElements.
                        ToAsyncEnumerable(),
                    maxIndex + 1,
                    tuple => ValueTasks.FromResult(tuple.index),
                    tuple => resultSelector(tuple.element, tuple.index),
                    cancellationToken).
                ConfigureAwait(false);
        }
    }

    [Obsolete($"Use an overload of {nameof(ToArrayByIndexAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TSource[]> ToArrayByIndexAwaitAsync<TSource>(
        IAsyncEnumerable<TSource> source,
        int length,
        Func<TSource, ValueTask<int>> indexSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));

        return ToArrayByIndexAwaitAsync(
            source,
            length,
            indexSelector,
            static (element, _) => ValueTasks.FromResult(element),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToArrayByIndexAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        int length,
        Func<TSource, ValueTask<int>> indexSelector,
        Func<TSource, ValueTask<TResult>> resultSelector,
        CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (indexSelector is null) throw new ArgumentNullException(nameof(indexSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return ToArrayByIndexAwaitAsync(
            source,
            length,
            indexSelector,
            (element, _) => resultSelector(element),
            cancellationToken);
    }

    [Obsolete($"Use an overload of {nameof(ToArrayByIndexAsync)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static ValueTask<TResult[]> ToArrayByIndexAwaitAsync<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
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

