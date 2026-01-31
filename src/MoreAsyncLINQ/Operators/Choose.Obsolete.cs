#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Choose)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> ChooseAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<(bool, TResult)>> chooser)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (chooser is null) throw new ArgumentNullException(nameof(chooser));

        return Core(source, chooser);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<(bool, TResult)>> chooser,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var (choose, result) = await chooser(element).ConfigureAwait(false);
                if (choose)
                {
                    yield return result;
                }
            }
        }
    }
}

