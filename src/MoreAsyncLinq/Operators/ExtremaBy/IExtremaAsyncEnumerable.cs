using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    public interface IExtremaAsyncEnumerable<out TElement> : IAsyncEnumerable<TElement>
    {
        IAsyncEnumerable<TElement> Take(int count);
        IAsyncEnumerable<TElement> TakeLast(int count);
    }

    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<TSource> FirstAsync<TSource>(
            this IExtremaAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Take(count: 1).FirstAsync(cancellationToken);
        }

        public static ValueTask<TSource?> FirstOrDefaultAsync<TSource>(
            this IExtremaAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Take(count: 1).FirstOrDefaultAsync(cancellationToken);
        }

        public static ValueTask<TSource> LastAsync<TSource>(
            this IExtremaAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.TakeLast(count: 1).LastAsync(cancellationToken);
        }

        public static ValueTask<TSource?> LastOrDefaultAsync<TSource>(
            this IExtremaAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.TakeLast(count: 1).LastOrDefaultAsync(cancellationToken);
        }

        public static ValueTask<TSource> SingleAsync<TSource>(
            this IExtremaAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Take(count: 2).SingleAsync(cancellationToken);
        }

        public static ValueTask<TSource?> SingleOrDefaultAsync<TSource>(
            this IExtremaAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Take(count: 2).SingleOrDefaultAsync(cancellationToken);
        }
    }
}