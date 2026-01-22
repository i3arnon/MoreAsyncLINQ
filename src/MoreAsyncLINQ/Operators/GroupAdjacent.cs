using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Groups the adjacent elements of a sequence according to a
    /// specified key selector function and compares the keys by using a
    /// specified comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <param name="source">A sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each
    /// element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to
    /// compare keys.</param>
    /// <returns>A sequence of groupings where each grouping
    /// (<see cref="IGrouping{TKey,TElement}"/>) contains the key
    /// and the adjacent elements in the same order as found in the
    /// source sequence.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution and
    /// streams the groupings. The grouping elements, however, are
    /// buffered. Each grouping is therefore yielded as soon as it
    /// is complete and before the next grouping occurs.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<TKey, TSource>> GroupAdjacent<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.GroupAdjacent(
            keySelector,
            static element => element,
            comparer);
    }

    /// <summary>
    /// Groups the adjacent elements of a sequence according to a
    /// specified key selector function. The keys are compared by using
    /// a comparer and each group's elements are projected by using a
    /// specified function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the
    /// resulting groupings.</typeparam>
    /// <param name="source">A sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each
    /// element.</param>
    /// <param name="elementSelector">A function to map each source
    /// element to an element in the resulting grouping.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to
    /// compare keys.</param>
    /// <returns>A sequence of groupings where each grouping
    /// (<see cref="IGrouping{TKey,TElement}"/>) contains the key
    /// and the adjacent elements (of type <typeparamref name="TElement"/>)
    /// in the same order as found in the source sequence.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution and
    /// streams the groupings. The grouping elements, however, are
    /// buffered. Each grouping is therefore yielded as soon as it
    /// is complete and before the next grouping occurs.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<TKey, TElement>> GroupAdjacent<TSource, TKey, TElement>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (elementSelector is null) throw new ArgumentNullException(nameof(elementSelector));

        return GroupAdjacent(
            source,
            keySelector,
            elementSelector,
            static (key, elements) =>
                Grouping.Create(
                    key,
                    elements.IsReadOnly
                        ? elements
                        : new ReadOnlyCollection<TElement>(elements)),
            comparer);
    }

    /// <summary>
    /// Groups the adjacent elements of a sequence according to a
    /// specified key selector function and creates a result value from
    /// each group and its key. The keys are compared by using a
    /// specified comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the
    /// resulting sequence.</typeparam>
    /// <param name="source">A sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each
    /// element.</param>
    /// <param name="resultSelector">A function to map each key and
    /// associated source elements to a result object.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to
    /// compare keys.</param>
    /// <returns>A collection of elements of type
    /// <typeparamref name="TResult" /> where each element represents
    /// a projection over a group and its key.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution and
    /// streams the groupings. The grouping elements, however, are
    /// buffered. Each grouping is therefore yielded as soon as it
    /// is complete and before the next grouping occurs.
    /// </remarks>
    public static IAsyncEnumerable<TResult> GroupAdjacent<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return GroupAdjacent(
            source,
            keySelector,
            static element => element,
            resultSelector,
            comparer);
    }

    private static IAsyncEnumerable<TResult> GroupAdjacent<TSource, TKey, TElement, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        Func<TKey, IList<TElement>, TResult> resultSelector,
        IEqualityComparer<TKey>? comparer)
    {
        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                source,
                keySelector,
                elementSelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IList<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var groupKey = keySelector(enumerator.Current);
            List<TElement> group = [elementSelector(enumerator.Current)];

            while (await enumerator.MoveNextAsync())
            {
                var key = keySelector(enumerator.Current);
                var element = elementSelector(enumerator.Current);

                if (comparer.Equals(groupKey, key))
                {
                    group.Add(element);
                }
                else
                {
                    yield return resultSelector(groupKey, group);

                    groupKey = key;
                    group = [element];
                }
            }

            yield return resultSelector(groupKey, group);
        }
    }

    /// <summary>
    /// Groups the adjacent elements of a sequence according to a
    /// specified key selector function and compares the keys by using a
    /// specified comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <param name="source">A sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each
    /// element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to
    /// compare keys.</param>
    /// <returns>A sequence of groupings where each grouping
    /// (<see cref="IGrouping{TKey,TElement}"/>) contains the key
    /// and the adjacent elements in the same order as found in the
    /// source sequence.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution and
    /// streams the groupings. The grouping elements, however, are
    /// buffered. Each grouping is therefore yielded as soon as it
    /// is complete and before the next grouping occurs.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<TKey, TSource>> GroupAdjacent<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.GroupAdjacent(
            keySelector,
            static (element, _) => ValueTasks.FromResult(element),
            comparer);
    }

    /// <summary>
    /// Groups the adjacent elements of a sequence according to a
    /// specified key selector function. The keys are compared by using
    /// a comparer and each group's elements are projected by using a
    /// specified function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the
    /// resulting groupings.</typeparam>
    /// <param name="source">A sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each
    /// element.</param>
    /// <param name="elementSelector">A function to map each source
    /// element to an element in the resulting grouping.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to
    /// compare keys.</param>
    /// <returns>A sequence of groupings where each grouping
    /// (<see cref="IGrouping{TKey,TElement}"/>) contains the key
    /// and the adjacent elements (of type <typeparamref name="TElement"/>)
    /// in the same order as found in the source sequence.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution and
    /// streams the groupings. The grouping elements, however, are
    /// buffered. Each grouping is therefore yielded as soon as it
    /// is complete and before the next grouping occurs.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<TKey, TElement>> GroupAdjacent<TSource, TKey, TElement>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (elementSelector is null) throw new ArgumentNullException(nameof(elementSelector));

        return GroupAdjacent(
            source,
            keySelector,
            elementSelector,
            static (key, elements, _) =>
                ValueTasks.FromResult(
                    Grouping.Create(
                        key,
                        elements.IsReadOnly
                            ? elements
                            : new ReadOnlyCollection<TElement>(elements))),
            comparer);
    }

    /// <summary>
    /// Groups the adjacent elements of a sequence according to a
    /// specified key selector function and creates a result value from
    /// each group and its key. The keys are compared by using a
    /// specified comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the
    /// resulting sequence.</typeparam>
    /// <param name="source">A sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each
    /// element.</param>
    /// <param name="resultSelector">A function to map each key and
    /// associated source elements to a result object.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to
    /// compare keys.</param>
    /// <returns>A collection of elements of type
    /// <typeparamref name="TResult" /> where each element represents
    /// a projection over a group and its key.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution and
    /// streams the groupings. The grouping elements, however, are
    /// buffered. Each grouping is therefore yielded as soon as it
    /// is complete and before the next grouping occurs.
    /// </remarks>
    public static IAsyncEnumerable<TResult> GroupAdjacent<TSource, TKey, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TKey, IEnumerable<TSource>, CancellationToken, ValueTask<TResult>> resultSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return GroupAdjacent(
            source,
            keySelector,
            static (element, _) => ValueTasks.FromResult(element),
            resultSelector,
            comparer);
    }

    private static IAsyncEnumerable<TResult> GroupAdjacent<TSource, TKey, TElement, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector,
        Func<TKey, IList<TElement>, CancellationToken, ValueTask<TResult>> resultSelector,
        IEqualityComparer<TKey>? comparer)
    {
        return source.IsKnownEmpty()
            ? AsyncEnumerable.Empty<TResult>()
            : Core(
                source,
                keySelector,
                elementSelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default,
                default);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
            Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector,
            Func<TKey, IList<TElement>, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey> comparer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var groupKey = await keySelector(enumerator.Current, cancellationToken);
            List<TElement> group = [await elementSelector(enumerator.Current, cancellationToken)];

            while (await enumerator.MoveNextAsync())
            {
                var key = await keySelector(enumerator.Current, cancellationToken);
                var element = await elementSelector(enumerator.Current, cancellationToken);

                if (comparer.Equals(groupKey, key))
                {
                    group.Add(element);
                }
                else
                {
                    yield return await resultSelector(groupKey, group, cancellationToken);

                    groupKey = key;
                    group = [element];
                }
            }

            yield return await resultSelector(groupKey, group, cancellationToken);
        }
    }

    private static class Grouping
    {
        public static Grouping<TKey, TElement> Create<TKey, TElement>(TKey key, IEnumerable<TElement> elements) =>
            new(key, elements);
    }

    private sealed class Grouping<TKey, TElement>(TKey key, IEnumerable<TElement> elements) : IGrouping<TKey, TElement>
    {
        public TKey Key { get; } = key;
        public IEnumerator<TElement> GetEnumerator() => elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

