using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<(IAsyncEnumerable<TSource> True, IAsyncEnumerable<TSource> False)> PartitionAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.PartitionAsync(
                predicate,
                static(grouping1, grouping2) => (grouping1, grouping2),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> predicate,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.GroupBy(predicate).PartitionAsync(resultSelector, cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<bool, TSource>> source,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAsync(
                key1: true,
                key2: false,
                (grouping1, grouping2, _) => resultSelector(grouping1, grouping2),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<bool?, TSource>> source,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAsync(
                key1: true,
                key2: false,
                key3: null,
                (grouping1, grouping2, grouping3, _) => resultSelector(grouping1, grouping2, grouping3),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
            return source.PartitionAsync(
                key,
                comparer: null,
                resultSelector,
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAsync(
                count: 1,
                key,
                key,
                key,
                comparer,
                (grouping1, _, _, groupings) => resultSelector(grouping1, groupings),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
            return source.PartitionAsync(
                key1,
                key2,
                comparer: null,
                resultSelector,
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAsync(
                count: 2,
                key1,
                key2,
                key2,
                comparer,
                (grouping1, grouping2, _, groupings) => resultSelector(grouping1, grouping2, groupings),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
            return PartitionAsync(
                source,
                key1,
                key2,
                key3,
                comparer: null,
                resultSelector,
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            
            return PartitionAsync(
                source,
                count: 3,
                key1,
                key2,
                key3,
                comparer,
                resultSelector,
                cancellationToken);
        }

        private static async ValueTask<TResult> PartitionAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            int count,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            Debug.Assert(count is >= 1 and <= 3);

            comparer ??= EqualityComparer<TKey>.Default;

            var grouping1 = count >= 1 ? null : Empty<TSource>();
            var grouping2 = count >= 2 ? null : Empty<TSource>();
            var grouping3 = count == 3 ? null : Empty<TSource>();
            List<IAsyncGrouping<TKey, TSource>>? groupings = null;
            await foreach (var grouping in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (grouping1 is null && comparer.Equals(grouping.Key, key1))
                {
                    grouping1 = grouping;
                }
                else if (grouping2 is null && comparer.Equals(grouping.Key, key2))
                {
                    grouping2 = grouping;
                }
                else if (grouping3 is null && comparer.Equals(grouping.Key, key3))
                {
                    grouping3 = grouping;
                }
                else
                {
                    groupings ??= new List<IAsyncGrouping<TKey, TSource>>();
                    groupings.Add(grouping);
                }
            }

            return resultSelector(
                grouping1 ?? Empty<TSource>(),
                grouping2 ?? Empty<TSource>(),
                grouping3 ?? Empty<TSource>(),
                groupings?.ToAsyncEnumerable() ?? Empty<IAsyncGrouping<TKey, TSource>>());
        }

        public static ValueTask<(IAsyncEnumerable<TSource> True, IAsyncEnumerable<TSource> False)> PartitionAwaitAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.PartitionAwaitAsync(
                predicate,
                static (grouping1, grouping2) => new ValueTask<(IAsyncEnumerable<TSource> True, IAsyncEnumerable<TSource> False)>((grouping1, grouping2)),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.GroupByAwait(predicate).PartitionAwaitAsync(resultSelector, cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<bool, TSource>> source,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAwaitAsync(
                key1: true,
                key2: false,
                (grouping1, grouping2, _) => resultSelector(grouping1, grouping2),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<bool?, TSource>> source,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAwaitAsync(
                key1: true,
                key2: false,
                key3: null,
                (grouping1, grouping2, grouping3, _) => resultSelector(grouping1, grouping2, grouping3),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAwaitAsync(
                key,
                comparer: null,
                resultSelector,
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAwaitAsync(
                count: 1,
                key,
                key,
                key,
                comparer,
                (grouping1, _, _, groupings) => resultSelector(grouping1, groupings),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAwaitAsync(
                key1,
                key2,
                comparer: null,
                resultSelector,
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.PartitionAwaitAsync(
                count: 2,
                key1,
                key2,
                key2,
                comparer,
                (grouping1, grouping2, _, groupings) => resultSelector(grouping1, grouping2, groupings),
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return PartitionAwaitAsync(
                source,
                key1,
                key2,
                key3,
                comparer: null,
                resultSelector,
                cancellationToken);
        }

        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return PartitionAwaitAsync(
                source,
                count: 3,
                key1,
                key2,
                key3,
                comparer,
                resultSelector,
                cancellationToken);
        }

        private static async ValueTask<TResult> PartitionAwaitAsync<TKey, TSource, TResult>(
            this IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> source,
            int count,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IAsyncGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            Debug.Assert(count is >= 1 and <= 3);

            comparer ??= EqualityComparer<TKey>.Default;

            var grouping1 = count >= 1 ? null : Empty<TSource>();
            var grouping2 = count >= 2 ? null : Empty<TSource>();
            var grouping3 = count == 3 ? null : Empty<TSource>();
            List<IAsyncGrouping<TKey, TSource>>? groupings = null;
            await foreach (var grouping in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (grouping1 is null && comparer.Equals(grouping.Key, key1))
                {
                    grouping1 = grouping;
                }
                else if (grouping2 is null && comparer.Equals(grouping.Key, key2))
                {
                    grouping2 = grouping;
                }
                else if (grouping3 is null && comparer.Equals(grouping.Key, key3))
                {
                    grouping3 = grouping;
                }
                else
                {
                    groupings ??= new List<IAsyncGrouping<TKey, TSource>>();
                    groupings.Add(grouping);
                }
            }

            return await resultSelector(
                    grouping1 ?? Empty<TSource>(),
                    grouping2 ?? Empty<TSource>(),
                    grouping3 ?? Empty<TSource>(),
                    groupings?.ToAsyncEnumerable() ?? Empty<IAsyncGrouping<TKey, TSource>>()).
                ConfigureAwait(false);
        }
    }
}