using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    private static IAsyncEnumerable<TSource> ExtremaBy<TSource, TKey, TStore>(
        this IAsyncEnumerable<TSource> source,
        Extrema<TStore, TSource> extrema,
        int? limit,
        Func<TSource, TKey> selector,
        Func<TKey, TKey, int> comparer)
    {
        return Core(
            source,
            extrema,
            limit,
            selector,
            comparer);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Extrema<TStore, TSource> extrema,
            int? limit,
            Func<TSource, TKey> selector,
            Func<TKey, TKey, int> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var store = extrema.InitializeStore();
            extrema.AddItem(ref store, limit, enumerator.Current);
            var extremaKey = selector(enumerator.Current);
            while (await enumerator.MoveNextAsync())
            {
                var element = enumerator.Current;
                var key = selector(element);
                switch (comparer(key, extremaKey))
                {
                    case > 0:
                        extrema.ResetStore(ref store);
                        extrema.AddItem(ref store, limit, element);
                        extremaKey = key;
                        break;
                    case 0:
                        extrema.AddItem(ref store, limit, element);
                        break;
                }
            }

            foreach (var element in extrema.GetEnumerable(store))
            {
                yield return element;
            }
        }
    }

    private static async IAsyncEnumerable<TSource> ExtremaByAwait<TSource, TKey, TStore>(
        this IAsyncEnumerable<TSource> source,
        Extrema<TStore, TSource> extrema,
        int? limit,
        Func<TSource, ValueTask<TKey>> selector,
        Func<TKey, TKey, int> comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

        if (!await enumerator.MoveNextAsync())
        {
            yield break;
        }

        var store = extrema.InitializeStore();
        extrema.AddItem(ref store, limit, enumerator.Current);
        var extremaKey = await selector(enumerator.Current).ConfigureAwait(false);
        while (await enumerator.MoveNextAsync())
        {
            var element = enumerator.Current;
            var key = await selector(element).ConfigureAwait(false);
            switch (comparer(key, extremaKey))
            {
                case > 0:
                    extrema.ResetStore(ref store);
                    extrema.AddItem(ref store, limit, element);
                    extremaKey = key;
                    break;
                case 0:
                    extrema.AddItem(ref store, limit, element);
                    break;
            }
        }

        foreach (var element in extrema.GetEnumerable(store))
        {
            yield return element;
        }
    }
    
    private static IAsyncEnumerable<TSource> ExtremaBy<TSource, TKey, TStore>(
        this IAsyncEnumerable<TSource> source,
        Extrema<TStore, TSource> extrema,
        int? limit,
        Func<TSource, CancellationToken, ValueTask<TKey>> selector,
        Func<TKey, TKey, int> comparer)
    {
        return Core(
            source,
            extrema,
            limit,
            selector,
            comparer);

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Extrema<TStore, TSource> extrema,
            int? limit,
            Func<TSource, CancellationToken, ValueTask<TKey>> selector,
            Func<TKey, TKey, int> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var store = extrema.InitializeStore();
            extrema.AddItem(ref store, limit, enumerator.Current);
            var extremaKey = await selector(enumerator.Current, cancellationToken);
            while (await enumerator.MoveNextAsync())
            {
                var element = enumerator.Current;
                var key = await selector(element, cancellationToken);
                switch (comparer(key, extremaKey))
                {
                    case > 0:
                        extrema.ResetStore(ref store);
                        extrema.AddItem(ref store, limit, element);
                        extremaKey = key;
                        break;
                    case 0:
                        extrema.AddItem(ref store, limit, element);
                        break;
                }
            }

            foreach (var element in extrema.GetEnumerable(store))
            {
                yield return element;
            }
        }
    }
}