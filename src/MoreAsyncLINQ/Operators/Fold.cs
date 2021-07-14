using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 1, cancellationToken).ConfigureAwait(false);
                return folder(elements[0]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 2, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 3, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 4, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 5, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 6, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 7, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 8, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 9, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 10, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 11, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 12, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 13, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 14, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12],
                    elements[13]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 15, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12],
                    elements[13],
                    elements[14]);
            }
        }

        public static ValueTask<TResult> FoldAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 16, cancellationToken).ConfigureAwait(false);
                return folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12],
                    elements[13],
                    elements[14],
                    elements[15]);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 1, cancellationToken).ConfigureAwait(false);
                return await folder(elements[0]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 2, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 3, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 4, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 5, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 6, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 7, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 8, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 9, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 10, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 11, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 12, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 13, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 14, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12],
                    elements[13]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 15, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12],
                    elements[13],
                    elements[14]).ConfigureAwait(false);
            }
        }

        public static ValueTask<TResult> FoldAwaitAsync<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
            CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (folder is null) throw new ArgumentNullException(nameof(folder));

            return Core(source, folder, cancellationToken);

            static async ValueTask<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, ValueTask<TResult>> folder,
                CancellationToken cancellationToken)
            {
                var elements = await GetFoldElementsAsync(source, count: 16, cancellationToken).ConfigureAwait(false);
                return await folder(
                    elements[0],
                    elements[1],
                    elements[2],
                    elements[3],
                    elements[4],
                    elements[5],
                    elements[6],
                    elements[7],
                    elements[8],
                    elements[9],
                    elements[10],
                    elements[11],
                    elements[12],
                    elements[13],
                    elements[14],
                    elements[15]).ConfigureAwait(false);
            }
        }

        private static async ValueTask<TSource[]> GetFoldElementsAsync<TSource>(
            IAsyncEnumerable<TSource> source,
            int count,
            CancellationToken cancellationToken)
        {
            var elements = new TSource[count];
            await foreach (var (index, element) in source.Index().AssertCount(count).WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                elements[index] = element;
            }

            return elements;
        }
    }
}