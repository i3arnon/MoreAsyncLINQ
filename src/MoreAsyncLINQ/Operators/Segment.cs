using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<IEnumerable<TSource>> Segment<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.Segment((current, _, _) => newSegmentPredicate(current));
        }

        public static IAsyncEnumerable<IEnumerable<TSource>> Segment<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int, bool> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.Segment((current, _, index) => newSegmentPredicate(current, index));
        }

        public static IAsyncEnumerable<IEnumerable<TSource>> Segment<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, int, bool> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return Core(source, newSegmentPredicate);

            static async IAsyncEnumerable<IEnumerable<TSource>> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, int, bool> newSegmentPredicate,
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
                    if (newSegmentPredicate(current, previous, index))
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

        public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.SegmentAwait((current, _, _) => newSegmentPredicate(current));
        }

        public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, int, ValueTask<bool>> newSegmentPredicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (newSegmentPredicate is null) throw new ArgumentNullException(nameof(newSegmentPredicate));

            return source.SegmentAwait((current, _, index) => newSegmentPredicate(current, index));
        }

        public static IAsyncEnumerable<IEnumerable<TSource>> SegmentAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
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
}