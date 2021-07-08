using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<IList<TSource>> WindowLeft<TSource>(
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
                    if (window.Count == size)
                    {
                        var nextWindow = new List<TSource>(size);
                        nextWindow.AddRange(window.Skip(count: 1));
                        yield return window;

                        window = nextWindow;
                    }
                }

                while (window.Count > 0)
                {
                    var nextWindow = new List<TSource>(size);
                    nextWindow.AddRange(window.Skip(count: 1));
                    yield return window;

                    window = nextWindow;
                }
            }
        }
    }
}