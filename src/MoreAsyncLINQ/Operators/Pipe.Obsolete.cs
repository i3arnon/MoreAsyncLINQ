#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Pipe)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PipeAwait<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> action)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (action is null) throw new ArgumentNullException(nameof(action));

        return Core(source, action);

        static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                await action(element).ConfigureAwait(false);
                    
                yield return element;
            }
        }
    }
}

