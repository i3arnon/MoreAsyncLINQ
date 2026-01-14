using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ;

static partial class MoreAsyncEnumerable
{
    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 1 element.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 1 element</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 2 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 2 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 3 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 3 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 4 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 4 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 5 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 5 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 6 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 6 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 7 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 7 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 8 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 8 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 9 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 9 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 10 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 10 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 11 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 11 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 12 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 12 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 13 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 13 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 14 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 14 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 15 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 15 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 16 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 16 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 1 element.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 1 element</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 2 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 2 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 3 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 3 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 4 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 4 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 5 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 5 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 6 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 6 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 7 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 7 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 8 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 8 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 9 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 9 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 10 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 10 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 11 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 11 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 12 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 12 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 13 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 13 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 14 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 14 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 15 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 15 elements</exception>
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

    /// <summary>
    /// Returns the result of applying a function to a sequence of
    /// 16 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name="TSource">Type of element in the source sequence</typeparam>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <param name="source">The sequence of items to fold.</param>
    /// <param name="folder">Function to apply to the elements in the sequence.</param>
    /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
    /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
    /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 16 elements</exception>
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