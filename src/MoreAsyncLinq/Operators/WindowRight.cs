using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<IList<TSource>> WindowRight<TSource>(
            this IAsyncEnumerable<TSource> source,
            int size)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            return Core(source, size);

            static async IAsyncEnumerable<IList<TSource>> Core(
                IAsyncEnumerable<TSource> source,
                int size,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var window = new List<TSource>(size);
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    window.Add(element);

                    var nextWindow = new List<TSource>(size);
                    nextWindow.AddRange(window.Count == size ? window.Skip(count: 1) : window);
                    yield return window;

                    window = nextWindow;
                }
            }
        }
    }
}