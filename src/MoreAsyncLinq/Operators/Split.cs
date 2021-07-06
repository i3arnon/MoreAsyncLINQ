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
        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> Split<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Split(separator, int.MaxValue);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> Split<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.Split(separator, count, static enumerable => enumerable);
        }

        public static IAsyncEnumerable<TResult> Split<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            Func<IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Split(separator, int.MaxValue, resultSelector);
        }

        public static IAsyncEnumerable<TResult> Split<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            int count,
            Func<IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Split(separator, comparer: null, count, resultSelector);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> Split<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.Split(separator, comparer, int.MaxValue);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> Split<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.Split(separator, comparer, count, static enumerable => enumerable);
        }

        public static IAsyncEnumerable<TResult> Split<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer,
            Func<IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Split(separator, comparer, int.MaxValue, resultSelector);
        }

        public static IAsyncEnumerable<TResult> Split<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer,
            int count,
            Func<IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            comparer ??= EqualityComparer<TSource>.Default;
            return source.Split(
                element => comparer.Equals(element, separator),
                count,
                resultSelector);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> Split<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));

            return source.Split(separatorFunc, int.MaxValue);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> Split<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.Split(separatorFunc, count, static enumerable => enumerable);
        }

        public static IAsyncEnumerable<TResult> Split<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc,
            Func<IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Split(separatorFunc, int.MaxValue, resultSelector);
        }

        public static IAsyncEnumerable<TResult> Split<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc,
            int count,
            Func<IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(source, separatorFunc, count, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<TSource> source,
                Func<TSource, bool> separatorFunc,
                int count,
                Func<IAsyncEnumerable<TSource>, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                if (count == 0)
                {
                    yield return resultSelector(source);

                    yield break;
                }

                List<TSource>? items = null;
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (count > 0 && separatorFunc(element))
                    {
                        yield return resultSelector(items?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TSource>());

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
                    yield return resultSelector(items.ToAsyncEnumerable());
                }
            }
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.SplitAwait(separator, int.MaxValue);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.SplitAwait(separator, count, ValueTasks.FromResult);
        }

        public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SplitAwait(separator, int.MaxValue, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            int count,
            Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SplitAwait(separator, comparer: null, count, resultSelector);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.SplitAwait(separator, comparer, int.MaxValue);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.SplitAwait(separator, comparer, count, ValueTasks.FromResult);
        }

        public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer,
            Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SplitAwait(separator, comparer, int.MaxValue, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            TSource separator,
            IEqualityComparer<TSource>? comparer,
            int count,
            Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            comparer ??= EqualityComparer<TSource>.Default;
            return source.SplitAwait(
                element => ValueTasks.FromResult(comparer.Equals(element, separator)),
                count,
                resultSelector);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> separatorFunc)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));

            return source.SplitAwait(separatorFunc, int.MaxValue);
        }

        public static IAsyncEnumerable<IAsyncEnumerable<TSource>> SplitAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> separatorFunc,
            int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            return source.SplitAwait(separatorFunc, count, ValueTasks.FromResult);
        }

        public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, ValueTask<bool>> separatorFunc,
            Func<IAsyncEnumerable<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (separatorFunc is null) throw new ArgumentNullException(nameof(separatorFunc));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SplitAwait(separatorFunc, int.MaxValue, resultSelector);
        }

        public static IAsyncEnumerable<TResult> SplitAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
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
}