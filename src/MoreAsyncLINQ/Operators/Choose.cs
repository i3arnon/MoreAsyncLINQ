using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Choose<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, (bool, TResult)> chooser)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (chooser is null) throw new ArgumentNullException(nameof(chooser));

            return Core(source, chooser);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, (bool, TResult)> chooser,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    var (choose, result) = chooser(element);
                    if (choose)
                    {
                        yield return result;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> ChooseAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
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
}