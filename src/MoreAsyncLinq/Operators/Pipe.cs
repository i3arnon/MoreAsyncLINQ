using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Pipe<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (action is null) throw new ArgumentNullException(nameof(action));

            return Core(source, action);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Action<TSource> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    action(element);
                   
                    yield return element;
                }
            }
        }

        public static IAsyncEnumerable<TSource> PipeAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> action)
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
}