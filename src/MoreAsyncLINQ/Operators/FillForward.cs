using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> FillForward<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.FillForward(static element => element is null);
        }

        public static IAsyncEnumerable<TSource> FillForward<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.FillForwardCore(predicate, fillSelector: null);
        }

        public static IAsyncEnumerable<TSource> FillForward<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TSource, TSource, TSource> fillSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

            return source.FillForwardCore(predicate, fillSelector);
        }

        private static async IAsyncEnumerable<TSource> FillForwardCore<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TSource, TSource, TSource>? fillSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            (bool, TSource) nullableSeed = default;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (predicate(element))
                {
                    yield return nullableSeed is (true, { } seed)
                        ? fillSelector is not null
                            ? fillSelector(element, seed)
                            : seed
                        : element;
                }
                else
                {
                    nullableSeed = (true, element);
                    yield return element;
                }
            }
        }

        public static IAsyncEnumerable<TSource> FillForwardAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.FillForwardCoreAwait(predicate, fillSelector: null);
        }

        public static IAsyncEnumerable<TSource> FillForwardAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, TSource, ValueTask<TSource>> fillSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

            return source.FillForwardCoreAwait(predicate, fillSelector);
        }

        private static async IAsyncEnumerable<TSource> FillForwardCoreAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, TSource, ValueTask<TSource>>? fillSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            (bool, TSource) nullableSeed = default;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (await predicate(element).ConfigureAwait(false))
                {
                    yield return nullableSeed is (true, { } seed)
                        ? fillSelector is not null
                            ? await fillSelector(element, seed).ConfigureAwait(false)
                            : seed
                        : element;
                }
                else
                {
                    nullableSeed = (true, element);
                    yield return element;
                }
            }
        }
    }
}