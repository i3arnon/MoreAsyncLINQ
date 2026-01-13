using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        /// <summary>
        /// Partitions or splits a sequence in two using a predicate.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">Type of source elements.</typeparam>
        /// <returns>
        /// A tuple of elements satisfying the predicate and those that do not,
        /// respectively.
        /// </returns>
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

        /// <summary>
        /// Partitions or splits a sequence in two using a predicate and then
        /// projects a result from the two.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of elements that
        /// satisfy the predicate and those that do not, respectively, passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">Type of source elements.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
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

        /// <summary>
        /// Partitions a grouping by Boolean keys into a projection of true
        /// elements and false elements, respectively.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of true elements
        /// and false elements, respectively, passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
            this IAsyncEnumerable<IGrouping<bool, TSource>> source,
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

        /// <summary>
        /// Partitions a grouping by nullable Boolean keys into a projection of
        /// true elements, false elements and null elements, respectively.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of true elements,
        /// false elements and null elements, respectively, passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TSource, TResult>(
            this IAsyncEnumerable<IGrouping<bool?, TSource>> source,
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

        /// <summary>
        /// Partitions a grouping and projects a result from group elements
        /// matching a key and those groups that do not.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key">The key to partition.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of elements
        /// matching <paramref name="key"/> and those groups that do not (in
        /// the order in which they appear in <paramref name="source"/>),
        /// passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from group elements
        /// matching a key and those groups that do not. An additional parameter
        /// specifies how to compare keys for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key">The key to partition on.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of the group
        /// matching <paramref name="key"/> and those groups that do not (in
        /// the order in which they appear in <paramref name="source"/>),
        /// passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements of
        /// groups matching a set of two keys and those groups that do not.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of the group
        /// matching <paramref name="key1"/>, elements of the group matching
        /// <paramref name="key2"/> and those groups that do not (in the order
        /// in which they appear in <paramref name="source"/>), passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements of
        /// groups matching a set of two keys and those groups that do not.
        /// An additional parameter specifies how to compare keys for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of the group
        /// matching <paramref name="key1"/>, elements of the group matching
        /// <paramref name="key2"/> and those groups that do not (in the order
        /// in which they appear in <paramref name="source"/>), passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements groups
        /// matching a set of three keys and those groups that do not.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="key3">The third key to partition on.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of groups
        /// matching <paramref name="key1"/>, <paramref name="key2"/> and
        /// <paramref name="key3"/> and those groups that do not (in the order
        /// in which they appear in <paramref name="source"/>), passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements groups
        /// matching a set of three keys and those groups that do not. An
        /// additional parameter specifies how to compare keys for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="key3">The third key to partition on.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of groups
        /// matching <paramref name="key1"/>, <paramref name="key2"/> and
        /// <paramref name="key3"/> and those groups that do not (in
        /// the order in which they appear in <paramref name="source"/>),
        /// passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector,
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
            this IAsyncEnumerable<IGrouping<TKey, TSource>> source,
            int count,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IGrouping<TKey, TSource>>, TResult> resultSelector,
            CancellationToken cancellationToken = default)
        {
            Debug.Assert(count is >= 1 and <= 3);

            comparer ??= EqualityComparer<TKey>.Default;

            var grouping1 = count >= 1 ? null : Empty<TSource>();
            var grouping2 = count >= 2 ? null : Empty<TSource>();
            var grouping3 = count == 3 ? null : Empty<TSource>();
            List<IGrouping<TKey, TSource>>? groupings = null;
            await foreach (var grouping in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (grouping1 is null && comparer.Equals(grouping.Key, key1))
                {
                    grouping1 = grouping.ToAsyncEnumerable();
                }
                else if (grouping2 is null && comparer.Equals(grouping.Key, key2))
                {
                    grouping2 = grouping.ToAsyncEnumerable();
                }
                else if (grouping3 is null && comparer.Equals(grouping.Key, key3))
                {
                    grouping3 = grouping.ToAsyncEnumerable();
                }
                else
                {
                    groupings ??= new List<IGrouping<TKey, TSource>>();
                    groupings.Add(grouping);
                }
            }

            return resultSelector(
                grouping1 ?? Empty<TSource>(),
                grouping2 ?? Empty<TSource>(),
                grouping3 ?? Empty<TSource>(),
                groupings?.ToAsyncEnumerable() ?? Empty<IGrouping<TKey, TSource>>());
        }

        /// <summary>
        /// Partitions or splits a sequence in two using a predicate.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">Type of source elements.</typeparam>
        /// <returns>
        /// A tuple of elements satisfying the predicate and those that do not,
        /// respectively.
        /// </returns>
        public static ValueTask<(IAsyncEnumerable<TSource> True, IAsyncEnumerable<TSource> False)> PartitionAwaitAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.PartitionAwaitAsync(
                predicate,
                static (grouping1, grouping2) => ValueTasks.FromResult((grouping1, grouping2)),
                cancellationToken);
        }

        /// <summary>
        /// Partitions or splits a sequence in two using a predicate and then
        /// projects a result from the two.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of elements that
        /// satisfy the predicate and those that do not, respectively, passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">Type of source elements.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return source.GroupBy((element, _) => predicate(element)).PartitionAwaitAsync(resultSelector, cancellationToken);
        }

        /// <summary>
        /// Partitions a grouping by Boolean keys into a projection of true
        /// elements and false elements, respectively.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of true elements
        /// and false elements, respectively, passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<IGrouping<bool, TSource>> source,
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

        /// <summary>
        /// Partitions a grouping by nullable Boolean keys into a projection of
        /// true elements, false elements and null elements, respectively.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of true elements,
        /// false elements and null elements, respectively, passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<IGrouping<bool?, TSource>> source,
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

        /// <summary>
        /// Partitions a grouping and projects a result from group elements
        /// matching a key and those groups that do not.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key">The key to partition.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from sequences of elements
        /// matching <paramref name="key"/> and those groups that do not (in
        /// the order in which they appear in <paramref name="source"/>),
        /// passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from group elements
        /// matching a key and those groups that do not. An additional parameter
        /// specifies how to compare keys for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key">The key to partition on.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of the group
        /// matching <paramref name="key"/> and those groups that do not (in
        /// the order in which they appear in <paramref name="source"/>),
        /// passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements of
        /// groups matching a set of two keys and those groups that do not.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of the group
        /// matching <paramref name="key1"/>, elements of the group matching
        /// <paramref name="key2"/> and those groups that do not (in the order
        /// in which they appear in <paramref name="source"/>), passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements of
        /// groups matching a set of two keys and those groups that do not.
        /// An additional parameter specifies how to compare keys for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of the group
        /// matching <paramref name="key1"/>, elements of the group matching
        /// <paramref name="key2"/> and those groups that do not (in the order
        /// in which they appear in <paramref name="source"/>), passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements groups
        /// matching a set of three keys and those groups that do not.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="key3">The third key to partition on.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of groups
        /// matching <paramref name="key1"/>, <paramref name="key2"/> and
        /// <paramref name="key3"/> and those groups that do not (in the order
        /// in which they appear in <paramref name="source"/>), passed as
        /// arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
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

        /// <summary>
        /// Partitions a grouping and projects a result from elements groups
        /// matching a set of three keys and those groups that do not. An
        /// additional parameter specifies how to compare keys for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
        /// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="key1">The first key to partition on.</param>
        /// <param name="key2">The second key to partition on.</param>
        /// <param name="key3">The third key to partition on.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from elements of groups
        /// matching <paramref name="key1"/>, <paramref name="key2"/> and
        /// <paramref name="key3"/> and those groups that do not (in
        /// the order in which they appear in <paramref name="source"/>),
        /// passed as arguments.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>
        /// The return value from <paramref name="resultSelector"/>.
        /// </returns>
        public static ValueTask<TResult> PartitionAwaitAsync<TKey, TElement, TResult>(
            this IAsyncEnumerable<IGrouping<TKey, TElement>> source,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IGrouping<TKey, TElement>>, ValueTask<TResult>> resultSelector,
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
            this IAsyncEnumerable<IGrouping<TKey, TSource>> source,
            int count,
            TKey key1,
            TKey key2,
            TKey key3,
            IEqualityComparer<TKey>? comparer,
            Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<TSource>, IAsyncEnumerable<IGrouping<TKey, TSource>>, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default)
        {
            Debug.Assert(count is >= 1 and <= 3);

            comparer ??= EqualityComparer<TKey>.Default;

            var grouping1 = count >= 1 ? null : Empty<TSource>();
            var grouping2 = count >= 2 ? null : Empty<TSource>();
            var grouping3 = count == 3 ? null : Empty<TSource>();
            List<IGrouping<TKey, TSource>>? groupings = null;
            await foreach (var grouping in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (grouping1 is null && comparer.Equals(grouping.Key, key1))
                {
                    grouping1 = grouping.ToAsyncEnumerable();
                }
                else if (grouping2 is null && comparer.Equals(grouping.Key, key2))
                {
                    grouping2 = grouping.ToAsyncEnumerable();
                }
                else if (grouping3 is null && comparer.Equals(grouping.Key, key3))
                {
                    grouping3 = grouping.ToAsyncEnumerable();
                }
                else
                {
                    groupings ??= new List<IGrouping<TKey, TSource>>();
                    groupings.Add(grouping);
                }
            }

            return await resultSelector(
                    grouping1 ?? Empty<TSource>(),
                    grouping2 ?? Empty<TSource>(),
                    grouping3 ?? Empty<TSource>(),
                    groupings?.ToAsyncEnumerable() ?? Empty<IGrouping<TKey, TSource>>()).
                ConfigureAwait(false);
        }
    }
}