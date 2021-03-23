﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<ILookup<TKey, TValue>> ToLookupAsync<TKey, TValue>(
            this IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToLookupAsync(comparer: null, cancellationToken);
        }

        public static ValueTask<ILookup<TKey, TValue>> ToLookupAsync<TKey, TValue>(
            this IAsyncEnumerable<KeyValuePair<TKey, TValue>> source,
            IEqualityComparer<TKey>? comparer,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToLookupAsync(
                pair => pair.Key,
                pair => pair.Value,
                comparer,
                cancellationToken);
        }

        public static ValueTask<ILookup<TKey, TValue>> ToLookupAsync<TKey, TValue>(
            this IAsyncEnumerable<(TKey Key, TValue Value)> source,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToLookupAsync(comparer: null, cancellationToken);
        }

        public static ValueTask<ILookup<TKey, TValue>> ToLookupAsync<TKey, TValue>(
            this IAsyncEnumerable<(TKey Key, TValue Value)> source,
            IEqualityComparer<TKey>? comparer,
            CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.ToLookupAsync(
                tuple => tuple.Key,
                tuple => tuple.Value,
                comparer,
                cancellationToken);
        }
    }
}