using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<bool> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<byte> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<char> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<decimal> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<double> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<float> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<int> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<long> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<sbyte> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<short> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<string> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<uint> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<ulong> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync(
            this IAsyncEnumerable<ushort> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        public static ValueTask<string> ToDelimitedStringAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            string delimiter,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (delimiter is null) throw new ArgumentNullException(nameof(delimiter));

            return source.ToDelimitedStringAsync(
                delimiter,
                static (builder, element) => builder.Append(element),
                cancellationToken);
        }

        private static async ValueTask<string> ToDelimitedStringAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            string delimiter,
            Action<StringBuilder, TSource> appender,
            CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                return string.Empty;
            }

            var element = enumerator.Current;
            var firstString = element?.ToString();
            if (!await enumerator.MoveNextAsync())
            {
                return firstString ?? string.Empty;
            }

            var builder = new StringBuilder(firstString);

            do
            {
                element = enumerator.Current;
                builder.Append(delimiter);
                appender(builder, element);
            } while (await enumerator.MoveNextAsync());

            return builder.ToString();
        }
    }
}