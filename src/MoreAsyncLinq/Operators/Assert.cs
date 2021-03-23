using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> AssertAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate) =>
            source.AssertAsync(predicate, errorSelector: null);

        public static IAsyncEnumerable<TSource> AssertAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TSource, Exception>? errorSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return Core(
                source,
                predicate,
                errorSelector ?? (static _ => new InvalidOperationException("Sequence contains an invalid item.")));

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, bool> predicate,
                Func<TSource, Exception> errorSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return predicate(element)
                        ? element
                        : throw errorSelector(element);
                }
            }
        }

        public static IAsyncEnumerable<TSource> AssertAwaitAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate) =>
            source.AssertAwaitAsync(predicate, errorSelector: null);

        public static IAsyncEnumerable<TSource> AssertAwaitAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, ValueTask<Exception>>? errorSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return Core(
                source,
                predicate,
                errorSelector ?? (static _ => new ValueTask<Exception>(new InvalidOperationException("Sequence contains an invalid item."))));

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, ValueTask<bool>> predicate,
                Func<TSource, ValueTask<Exception>> errorSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return await predicate(element).ConfigureAwait(false)
                        ? element
                        : throw (await errorSelector(element).ConfigureAwait(false));
                }
            }
        }
    }
}