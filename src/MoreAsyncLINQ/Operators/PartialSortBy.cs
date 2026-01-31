using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey})"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in ascending order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, TKey> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortBy(count, keySelector, comparer: null);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, TKey}, OrderByDirection)"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// An additional parameter specifies the direction of the sort
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, TKey> keySelector,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortBy(count, keySelector, comparer: null, direction);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, TKey}, OrderByDirection)"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// Additional parameters specify how the elements compare to each other and
    /// the direction of the sort.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortBy(
            count,
            keySelector,
            Comparers.Get(comparer, direction));
    }

    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey})"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// An additional parameter specifies how the keys compare to each other.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in ascending order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return PartialSortBy(
            source,
            count,
            keySelector,
            comparer,
            comparer: null,
            default);
    }

    private static async IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, TKey>? keySelector,
        IComparer<TKey>? keyComparer,
        IComparer<TSource>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var keys =
            keySelector is not null
                ? new List<TKey>(count)
                : null;
        var top = new List<TSource>(count);

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (keys is not null)
            {
                var key = keySelector!(element);
                if (Insert(keys, key, keyComparer) is { } index)
                {
                    if (top.Count == count)
                    {
                        top.RemoveAt(count - 1);
                    }

                    top.Insert(index, element);
                }
            }
            else
            {
                Insert(top, element, comparer);
            }
        }

        foreach (var element in top)
        {
            yield return element;
        }

        int? Insert<T>(List<T> list, T item, IComparer<T>? comparer)
        {
            var index = list.BinarySearch(item, comparer);
            if (index < 0)
            {
                index = ~index;
                if (index >= count)
                {
                    return null;
                }
            }

            if (list.Count == count)
            {
                list.RemoveAt(count - 1);
            }

            list.Insert(index, item);
            return index;
        }
    }

    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderByAwait{TSource,TKey}(IAsyncEnumerable{TSource},Func{TSource,ValueTask{TKey}},IComparer{TKey})"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}"/> in a single operation.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in ascending order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(PartialSortBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortByAwait(count, keySelector, comparer: null);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderByAwait{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, ValueTask{TKey}}, OrderByDirection)"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}"/> in a single operation.
    /// An additional parameter specifies the direction of the sort
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(PartialSortBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, ValueTask<TKey>> keySelector,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortByAwait(count, keySelector, comparer: null, direction);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderByAwait{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, ValueTask{TKey}}, OrderByDirection)"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}"/> in a single operation.
    /// Additional parameters specify how the elements compare to each other and
    /// the direction of the sort.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(PartialSortBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortByAwait(count, keySelector, Comparers.Get(comparer, direction));
    }

    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderByAwait{TSource,TKey}(IAsyncEnumerable{TSource},Func{TSource,ValueTask{TKey}},IComparer{TKey})"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}"/> in a single operation.
    /// An additional parameter specifies how the keys compare to each other.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in ascending order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    [Obsolete($"Use an overload of {nameof(PartialSortBy)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortByAwait(count, keySelector, comparer, comparer: null);
    }

    private static async IAsyncEnumerable<TSource> PartialSortByAwait<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, ValueTask<TKey>>? keySelector,
        IComparer<TKey>? keyComparer,
        IComparer<TSource>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var keys =
            keySelector is not null
                ? new List<TKey>(count)
                : null;
        var top = new List<TSource>(count);

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (keys is not null)
            {
                var key = await keySelector!(element).ConfigureAwait(false);
                if (Insert(keys, key, keyComparer) is { } index)
                {
                    if (top.Count == count)
                    {
                        top.RemoveAt(count - 1);
                    }

                    top.Insert(index, element);
                }
            }
            else
            {
                Insert(top, element, comparer);
            }
        }

        foreach (var element in top)
        {
            yield return element;
        }

        int? Insert<T>(List<T> list, T item, IComparer<T>? comparer)
        {
            var index = list.BinarySearch(item, comparer);
            if (index < 0)
            {
                index = ~index;
                if (index >= count)
                {
                    return null;
                }
            }

            if (list.Count == count)
            {
                list.RemoveAt(count - 1);
            }

            list.Insert(index, item);
            return index;
        }
    }

    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderBy{TSource,TKey}(IAsyncEnumerable{TSource},Func{TSource, CancellationToken, ValueTask{TKey}},IComparer{TKey})"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in ascending order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortBy(count, keySelector, comparer: null);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, CancellationToken, ValueTask{TKey}}, OrderByDirection)"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// An additional parameter specifies the direction of the sort
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortBy(count, keySelector, comparer: null, direction);
    }

    /// <summary>
    /// Combines <see cref="MoreAsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, CancellationToken, ValueTask{TKey}}, OrderByDirection)"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// Additional parameters specify how the elements compare to each other and
    /// the direction of the sort.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <param name="direction">The direction in which to sort the elements</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in the specified order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer,
        OrderByDirection direction)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return source.PartialSortBy(
            count,
            keySelector,
            Comparers.Get(comparer, direction));
    }

    /// <summary>
    /// Combines <see cref="AsyncEnumerable.OrderBy{TSource, TKey}(IAsyncEnumerable{TSource}, Func{TSource, CancellationToken, ValueTask{TKey}}, IComparer{TKey})"/>,
    /// and <see cref="AsyncEnumerable.Take{TSource}(IAsyncEnumerable{TSource}, int)"/> in a single operation.
    /// An additional parameter specifies how the keys compare to each other.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="count">Number of (maximum) elements to return.</param>
    /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>A sequence containing at most top <paramref name="count"/>
    /// elements from source, in ascending order of their keys.</returns>
    /// <remarks>
    /// This operator uses deferred execution and streams it results.
    /// </remarks>
    public static IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        this IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        return PartialSortBy(
            source,
            count,
            keySelector,
            comparer,
            comparer: null,
            default);
    }

    private static async IAsyncEnumerable<TSource> PartialSortBy<TSource, TKey>(
        IAsyncEnumerable<TSource> source,
        int count,
        Func<TSource, CancellationToken, ValueTask<TKey>>? keySelector,
        IComparer<TKey>? keyComparer,
        IComparer<TSource>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var keys =
            keySelector is not null
                ? new List<TKey>(count)
                : null;
        var top = new List<TSource>(count);

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (keys is not null)
            {
                var key = await keySelector!(element, cancellationToken).ConfigureAwait(false);
                if (Insert(keys, key, keyComparer) is { } index)
                {
                    if (top.Count == count)
                    {
                        top.RemoveAt(count - 1);
                    }

                    top.Insert(index, element);
                }
            }
            else
            {
                Insert(top, element, comparer);
            }
        }

        foreach (var element in top)
        {
            yield return element;
        }

        int? Insert<T>(List<T> list, T item, IComparer<T>? comparer)
        {
            var index = list.BinarySearch(item, comparer);
            if (index < 0)
            {
                index = ~index;
                if (index >= count)
                {
                    return null;
                }
            }

            if (list.Count == count)
            {
                list.RemoveAt(count - 1);
            }

            list.Insert(index, item);
            return index;
        }
    }
}