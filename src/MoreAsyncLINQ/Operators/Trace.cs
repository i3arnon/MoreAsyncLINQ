using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Traces the elements of a source sequence for diagnostics.
    /// </summary>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <param name="source">Source sequence whose elements to trace.</param>
    /// <returns>
    /// Return the source sequence unmodified.
    /// </returns>
    /// <remarks>
    /// This a pass-through operator that uses deferred execution and
    /// streams the results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Trace<TSource>(this IAsyncEnumerable<TSource> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Trace(format: null);
    }

    /// <summary>
    /// Traces the elements of a source sequence for diagnostics using
    /// custom formatting.
    /// </summary>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <param name="source">Source sequence whose elements to trace.</param>
    /// <param name="format">
    /// String to use to format the trace message. If null then the
    /// element value becomes the traced message.
    /// </param>
    /// <returns>
    /// Return the source sequence unmodified.
    /// </returns>
    /// <remarks>
    /// This a pass-through operator that uses deferred execution and
    /// streams the results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Trace<TSource>(
        this IAsyncEnumerable<TSource> source,
        string? format)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return format is null
            ? source.Trace(element => element?.ToString() ?? string.Empty)
            : source.Trace(element => string.Format(format, element));
    }

    /// <summary>
    /// Traces the elements of a source sequence for diagnostics using
    /// a custom formatter.
    /// </summary>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <param name="source">Source sequence whose elements to trace.</param>
    /// <param name="formatter">Function used to format each source element into a string.</param>
    /// <returns>
    /// Return the source sequence unmodified.
    /// </returns>
    /// <remarks>
    /// This a pass-through operator that uses deferred execution and
    /// streams the results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Trace<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, string> formatter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (formatter is null) throw new ArgumentNullException(nameof(formatter));

        return source.Pipe(element => System.Diagnostics.Trace.WriteLine(formatter(element)));
    }

    /// <summary>
    /// Traces the elements of a source sequence for diagnostics using
    /// a custom formatter.
    /// </summary>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <param name="source">Source sequence whose elements to trace.</param>
    /// <param name="formatter">Function used to format each source element into a string.</param>
    /// <returns>
    /// Return the source sequence unmodified.
    /// </returns>
    /// <remarks>
    /// This a pass-through operator that uses deferred execution and
    /// streams the results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(Trace)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> TraceAwait<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<string>> formatter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (formatter is null) throw new ArgumentNullException(nameof(formatter));

        return source.PipeAwait(async element => System.Diagnostics.Trace.WriteLine(await formatter(element).ConfigureAwait(false)));
    }
    
    /// <summary>
    /// Traces the elements of a source sequence for diagnostics using
    /// a custom formatter.
    /// </summary>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <param name="source">Source sequence whose elements to trace.</param>
    /// <param name="formatter">Function used to format each source element into a string.</param>
    /// <returns>
    /// Return the source sequence unmodified.
    /// </returns>
    /// <remarks>
    /// This a pass-through operator that uses deferred execution and
    /// streams the results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> Trace<TSource>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<string>> formatter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (formatter is null) throw new ArgumentNullException(nameof(formatter));

        return source.Pipe(async (element, cancellationToken) => System.Diagnostics.Trace.WriteLine(await formatter(element, cancellationToken)));
    }
}