using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource fallback)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.FallbackIfEmpty<TSource>(
                count: 1,
                fallback,
                default!,
                default!,
                default!,
                fallback: null);
        }

        public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource fallback1,
            TSource fallback2)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.FallbackIfEmpty<TSource>(
                count: 2,
                fallback1,
                fallback2,
                default!,
                default!,
                fallback: null);
        }

        public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource fallback1,
            TSource fallback2,
            TSource fallback3)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.FallbackIfEmpty<TSource>(
                count: 3,
                fallback1,
                fallback2,
                fallback3,
                default!,
                fallback: null);
        }

        public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource fallback1,
            TSource fallback2,
            TSource fallback3,
            TSource fallback4)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.FallbackIfEmpty(
                count: 4,
                fallback1,
                fallback2,
                fallback3,
                fallback4,
                fallback: null);
        }

        public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            params TSource[] fallback)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (fallback is null) throw new ArgumentNullException(nameof(fallback));
            
            return source.FallbackIfEmpty(fallback.ToAsyncEnumerable());
        }

        public static IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            IAsyncEnumerable<TSource> fallback)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (fallback is null) throw new ArgumentNullException(nameof(fallback));

            return source.FallbackIfEmpty<TSource>(
                count: null,
                default!,
                default!,
                default!,
                default!,
                fallback);
        }

        private static async IAsyncEnumerable<TSource> FallbackIfEmpty<TSource>(
            this IAsyncEnumerable<TSource> source,
            int? count,
            TSource fallback1,
            TSource fallback2,
            TSource fallback3,
            TSource fallback4,
            IAsyncEnumerable<TSource>? fallback,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collectionCount = await source.TryGetCollectionCountAsync(cancellationToken).ConfigureAwait(false);
            var result =
                collectionCount switch
                {
                    0 => FallbackCollection(),
                    > 0 => source,
                    _ => Core()
                };

            await foreach (var element in result.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return element;
            }

            async IAsyncEnumerable<TSource> Core()
            {
                await using (var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator())
                {
                    if (await enumerator.MoveNextAsync())
                    {
                        do
                        {
                            yield return enumerator.Current;
                        } while (await enumerator.MoveNextAsync());

                        yield break;
                    }
                }

                await foreach (var element in FallbackCollection().WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return element;
                }
            }

            IAsyncEnumerable<TSource> FallbackCollection()
            {
                return fallback ?? FallbackArguments().ToAsyncEnumerable();

                IEnumerable<TSource> FallbackArguments()
                {
                    switch (count)
                    {
                        case 1:
                            yield return fallback1;

                            break;
                        case 2:
                            yield return fallback1;
                            yield return fallback2;

                            break;
                        case 3:
                            yield return fallback1;
                            yield return fallback2;
                            yield return fallback3;

                            break;
                        case 4:
                            yield return fallback1;
                            yield return fallback2;
                            yield return fallback3;
                            yield return fallback4;

                            break;
                        default:
                            Debug.Fail(count.ToString());
                            break;
                    }
                }
            }
        }
    }
}