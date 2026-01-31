#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Assert)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> AssertAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate) =>
        AssertAwait(source, predicate, errorSelector: null);

    [Obsolete($"Use an overload of {nameof(Assert)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<TSource> AssertAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate,
        Func<TSource, ValueTask<Exception>>? errorSelector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return Core(
            source,
            predicate,
            errorSelector ?? (static _ => ValueTasks.FromResult<Exception>(new InvalidOperationException("Sequence contains an invalid item."))));

        static async IAsyncEnumerable<TSource> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> predicate,
            Func<TSource, ValueTask<Exception>> errorSelector,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return await predicate(element).ConfigureAwait(false)
                    ? element
                    : throw (await errorSelector(element).ConfigureAwait(false));
            }
        }
    }
}

