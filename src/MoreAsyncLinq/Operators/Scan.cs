using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Scan<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource> transformation)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, transformation);

            static async IAsyncEnumerable<TSource> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource> transformation,
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
                    seed = transformation(seed, enumerator.Current);
                    yield return seed;
                }
            }
        }

        public static IAsyncEnumerable<TState> Scan<TSource, TState>(
            this IAsyncEnumerable<TSource> source,
            TState seed,
            Func<TState, TSource, TState> transformation)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (transformation is null) throw new ArgumentNullException(nameof(transformation));

            return Core(source, seed, transformation);

            static async IAsyncEnumerable<TState> Core(
                IAsyncEnumerable<TSource> source,
                TState seed,
                Func<TState, TSource, TState> transformation,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                yield return seed;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    seed = transformation(seed, element);
                    yield return seed;
                }
            }
        }

        public static IAsyncEnumerable<TSource> ScanAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
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

        public static IAsyncEnumerable<TState> ScanAwait<TSource, TState>(
            this IAsyncEnumerable<TSource> source,
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
}