using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> FillBackward<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.FillBackward(static element => element is not null);
        }

        public static IAsyncEnumerable<TSource> FillBackward<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.FillBackwardCore(predicate, fillSelector: null);
        }

        public static IAsyncEnumerable<TSource> FillBackward<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TSource, TSource, TSource> fillSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

            return source.FillBackwardCore(predicate, fillSelector);
        }

        private static async IAsyncEnumerable<TSource> FillBackwardCore<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<TSource, TSource, TSource>? fillSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            List<TSource>? holes = null;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (predicate(element))
                {
                    holes ??= new List<TSource>();
                    holes.Add(element);
                }
                else
                {
                    if (holes is { Count: > 0 })
                    {
                        foreach (var hole in holes)
                        {
                            yield return fillSelector is not null
                                ? fillSelector(hole, element)
                                : element;
                        }

                        holes.Clear();
                    }

                    yield return element;
                }
            }

            if (holes is { Count: > 0 })
            {
                foreach (var hole in holes)
                {
                    yield return hole;
                }
            }
        }

        public static IAsyncEnumerable<TSource> FillBackwardAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.FillBackwardCoreAwait(predicate, fillSelector: null);
        }

        public static IAsyncEnumerable<TSource> FillBackwardAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, TSource, ValueTask<TSource>> fillSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (fillSelector is null) throw new ArgumentNullException(nameof(fillSelector));

            return source.FillBackwardCoreAwait(predicate, fillSelector);
        }

        private static async IAsyncEnumerable<TSource> FillBackwardCoreAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, TSource, ValueTask<TSource>>? fillSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            List<TSource>? holes = null;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (await predicate(element).ConfigureAwait(false))
                {
                    holes ??= new List<TSource>();
                    holes.Add(element);
                }
                else
                {
                    if (holes is { Count: > 0 })
                    {
                        foreach (var hole in holes)
                        {
                            yield return fillSelector is not null
                                ? await fillSelector(hole, element).ConfigureAwait(false)
                                : element;
                        }

                        holes.Clear();
                    }

                    yield return element;
                }
            }

            if (holes is { Count: > 0 })
            {
                foreach (var hole in holes)
                {
                    yield return hole;
                }
            }
        }
    }
}