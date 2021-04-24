using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Pad<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            return source.Pad<TSource>(
                width,
                default!,
                paddingSelector: null);
        }


        public static IAsyncEnumerable<TSource> Pad<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            TSource padding)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            return source.Pad<TSource>(
                width,
                padding,
                paddingSelector: null);
        }

        public static IAsyncEnumerable<TSource> Pad<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            Func<int, TSource>? paddingSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

            return source.Pad<TSource>(
                width,
                default!,
                paddingSelector);
        }

        private static async IAsyncEnumerable<TSource> Pad<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            TSource padding,
            Func<int, TSource>? paddingSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var count = 0;
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;

                count++;
            }

            while (count < width)
            {
                yield return paddingSelector is null
                    ? padding
                    : paddingSelector(count);

                count++;
            }
        }

        public static IAsyncEnumerable<TSource> PadAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            Func<int, ValueTask<TSource>>? paddingSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (paddingSelector is null) throw new ArgumentNullException(nameof(paddingSelector));

            return source.PadAwait<TSource>(
                width,
                default!,
                paddingSelector);
        }

        private static async IAsyncEnumerable<TSource> PadAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            int width,
            TSource padding,
            Func<int, ValueTask<TSource>>? paddingSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var count = 0;
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;

                count++;
            }

            while (count < width)
            {
                yield return paddingSelector is null
                    ? padding
                    : await paddingSelector(count).ConfigureAwait(false);

                count++;
            }
        }
    }
}