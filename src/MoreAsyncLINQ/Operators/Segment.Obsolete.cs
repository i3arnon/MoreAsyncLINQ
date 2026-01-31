#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    [Obsolete($"Use an overload of {nameof(Segment)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> newSegmentPredicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

        return SegmentAwait(source, (current, _, _) => newSegmentPredicate(current));
    }

    [Obsolete($"Use an overload of {nameof(Segment)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, int, ValueTask<bool>> newSegmentPredicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

        return SegmentAwait(source, (current, _, index) => newSegmentPredicate(current, index));
    }

    [Obsolete($"Use an overload of {nameof(Segment)} that accepts an async delegate with a {nameof(CancellationToken)} parameter.")]
    public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, TSource, int, ValueTask<bool>> newSegmentPredicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

        return Core(source, newSegmentPredicate);

        static async IAsyncEnumerable<IEnumerable<TSource>> Core(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, int, ValueTask<bool>> newSegmentPredicate,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            var previous = enumerator.Current;
            var segment = new List<TSource> { previous };
            var index = 0;
            while (await enumerator.MoveNextAsync())
            {
                var current = enumerator.Current;
                index++;
                if (await newSegmentPredicate(current, previous, index).ConfigureAwait(false))
                {
                    yield return segment;

                    segment = new List<TSource>();
                }

                segment.Add(current);
                previous = current;
            }

            yield return segment;
        }
    }
}

