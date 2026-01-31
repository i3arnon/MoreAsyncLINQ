#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        TSource separator)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return SplitAwait(source, separator, int.MaxValue);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        return SplitAwait(source, separator, count, ValueTasks.FromResult);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SplitAwait(source, separator, int.MaxValue, resultSelector);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        int count,
        Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SplitAwait(source, separator, comparer: null, count, resultSelector);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        IEqualityComparer<TSource>? comparer)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return SplitAwait(source, separator, comparer, int.MaxValue);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        IEqualityComparer<TSource>? comparer,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        return SplitAwait(source, separator, comparer, count, ValueTasks.FromResult);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        IEqualityComparer<TSource>? comparer,
        Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SplitAwait(source, separator, comparer, int.MaxValue, resultSelector);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        TSource separator,
        IEqualityComparer<TSource>? comparer,
        int count,
        Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        comparer ??= EqualityComparer<TSource>.Default;
        return SplitAwait(
            source,
            element => ValueTasks.FromResult(comparer.Equals(element, separator)),
            count,
            resultSelector);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> separatorFunc)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));

        return SplitAwait(source, separatorFunc, int.MaxValue);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> separatorFunc,
        int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        return SplitAwait(source, separatorFunc, count, ValueTasks.FromResult);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> separatorFunc,
        Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SplitAwait(source, separatorFunc, int.MaxValue, resultSelector);
    }

    [Obsolete($"Use an overload of {nameof(Split)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> separatorFunc,
        int count,
        Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return Core(source, separatorFunc, count, resultSelector);

        static async IAsyncEnumerable<TResult> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> separatorFunc,
            int count,
            Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (count == 0)
            {
                yield return await resultSelector(source).ConfigureAwait(false);

                yield break;
            }

            List<TSource>? items = null;
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (count > 0 && await separatorFunc(element).ConfigureAwait(false))
                {
                    yield return await resultSelector(items?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TSource>()).ConfigureAwait(false);

                    count--;
                    items = null;
                }
                else
                {
                    items ??= new List<TSource>();
                    items.Add(element);
                }
            }

            if (items is { Count: > 0 })
            {
                yield return await resultSelector(items.ToAsyncEnumerable()).ConfigureAwait(false);
            }
        }
    }
}

