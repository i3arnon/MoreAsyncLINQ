#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Trace)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> TraceAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<string>> formatter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (formatter is null) throw new ArgumentNullException(nameof(formatter));

        return PipeAwait(source, async element => System.Diagnostics.Trace.WriteLine(await formatter(element).ConfigureAwait(false)));
    }
}

