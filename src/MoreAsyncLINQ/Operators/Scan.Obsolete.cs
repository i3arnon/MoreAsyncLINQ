#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Scan)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> ScanAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, ValueTask<TSource>> transformation)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (transformation is null) throw new ArgumentNullException(nameof(transformation));

        return Core(source, transformation);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TSource>> transformation,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var seed = enumerator.Current;
            yield return seed;

            while (await enumerator.MoveNextAsync())
            {
                seed = await transformation(seed, enumerator.Current).ConfigureAwait(false);
                yield return seed;
            }
        }
    }

    [Obsolete($"Use an overload of {nameof(Scan)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TState> ScanAwait<TSource, TState>(
        IAsyncEnumerable<TSource> source,
        TState seed,
        Func<TState, TSource, ValueTask<TState>> transformation)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (transformation is null) throw new ArgumentNullException(nameof(transformation));

        return Core(source, seed, transformation);

        static async IAsyncEnumerable<TState> Core(
            IAsyncEnumerable<TSource> source,
            TState seed,
            Func<TState, TSource, ValueTask<TState>> transformation,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            yield return seed;

            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                seed = await transformation(seed, element).ConfigureAwait(false);
                yield return seed;
            }
        }
    }
}

