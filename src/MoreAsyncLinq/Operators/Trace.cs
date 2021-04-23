using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Trace<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Trace(format: null);
        }

        public static IAsyncEnumerable<TSource> Trace<TSource>(
            this IAsyncEnumerable<TSource> source,
            string? format)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return format is null
                ? source.Trace(element => element?.ToString() ?? string.Empty)
                : source.Trace(element => string.Format(format, element));
        }

        public static IAsyncEnumerable<TSource> Trace<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, string> formatter)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            return source.Pipe(element => System.Diagnostics.Trace.WriteLine(formatter(element)));
        }

        public static IAsyncEnumerable<TSource> TraceAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<string>> formatter)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            return source.PipeAwait(async element => System.Diagnostics.Trace.WriteLine(await formatter(element).ConfigureAwait(false)));
        }
    }
}