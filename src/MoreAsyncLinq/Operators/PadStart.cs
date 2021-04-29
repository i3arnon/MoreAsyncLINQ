using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> PadStart<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            return source.PadStart<TSource>(width, default!, paddingSelector: null);
        }

        public static IAsyncEnumerable<TSource> PadStart<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            TSource padding)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            return source.PadStart<TSource>(width, padding, paddingSelector: null);
        }

        public static IAsyncEnumerable<TSource> PadStart<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            Func<int, TSource> paddingSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

            return source.PadStart<TSource>(width, default!, paddingSelector);
        }

        private static async IAsyncEnumerable<TSource> PadStart<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            TSource padding,
            Func<int, TSource>? paddingSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
            var result = 
                collectionCount is not null
                ? collectionCount >= width
                    ? source
                    : Enumerable.Range(start: 0, width - collectionCount.Value).
                        Select(index => paddingSelector is null ? padding : paddingSelector(index)).
                        ToAsyncEnumerable().
                        Concat(source)
                : Core(source, width, padding, paddingSelector);

            await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;
            }

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int width,
                TSource padding,
                Func<int, TSource>? paddingSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var window = new TSource[width];
                var count = 0;
                await using (var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator())
                {
                    for (; count < width && await enumerator.MoveNextAsync(); count++)
                    {
                        window[count] = enumerator.Current;
                    }

                    if (count == width)
                    {
                        for (var index = 0; index < count; index++)
                        {
                            yield return window[index];
                        }

                        while (await enumerator.MoveNextAsync())
                        {
                            yield return enumerator.Current;
                        }

                        yield break;
                    }
                }

                var paddingLength = width - count;
                for (var index = 0; index < paddingLength; index++)
                {
                    yield return paddingSelector is null ? padding : paddingSelector(index);
                }

                for (var index = 0; index < count; index++)
                {
                    yield return window[index];
                }
            }
        }

        public static IAsyncEnumerable<TSource> PadStartAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            Func<int, ValueTask<TSource>> paddingSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

            return source.PadStartAwaitCore<TSource>(width, paddingSelector);
        }

        private static async IAsyncEnumerable<TSource> PadStartAwaitCore<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            Func<int, ValueTask<TSource>> paddingSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
            var result =
                collectionCount is not null
                ? collectionCount >= width
                    ? source
                    : Enumerable.Range(start: 0, width - collectionCount.Value).
                        ToAsyncEnumerable().
                        SelectAwait(paddingSelector).
                        Concat(source)
                : Core(source, width, paddingSelector);

            await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;
            }

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                int width,
                Func<int, ValueTask<TSource>> paddingSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var window = new TSource[width];
                var count = 0;
                await using (var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator())
                {
                    for (; count < width && await enumerator.MoveNextAsync(); count++)
                    {
                        window[count] = enumerator.Current;
                    }

                    if (count == width)
                    {
                        for (var index = 0; index < count; index++)
                        {
                            yield return window[index];
                        }

                        while (await enumerator.MoveNextAsync())
                        {
                            yield return enumerator.Current;
                        }

                        yield break;
                    }
                }

                var paddingLength = width - count;
                for (var index = 0; index < paddingLength; index++)
                {
                    yield return await paddingSelector(index).ConfigureAwait(false);
                }

                for (var index = 0; index < count; index++)
                {
                    yield return window[index];
                }
            }
        }
    }
}