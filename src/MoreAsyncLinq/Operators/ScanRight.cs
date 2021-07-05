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
        public static IAsyncEnumerable<TSource> ScanRight<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (func is null) throw new ArgumentNullException(nameof(func));

            return source.ScanRight(
                func,
                list =>
                    list.Count > 0
                        ? (list[list.Count - 1], list.Count - 1)
                        : null);
        }

        public static IAsyncEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(
            this IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, TAccumulate> func)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (func is null) throw new ArgumentNullException(nameof(func));

            return source.ScanRight(func, list => (seed, list.Count));
        }

        private static async IAsyncEnumerable<TResult> ScanRight<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TResult, TResult> func,
            Func<List<TSource>, (TResult seed, int count)?> seeder,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            if (seeder(list) is not var (accumulator, count))
            {
                yield break;
            }

            var stack = new Stack<TResult>(count + 1);
            stack.Push(accumulator);
            while (count > 0)
            {
                count--;
                accumulator = func(list[count], accumulator);
                stack.Push(accumulator);
            }

            foreach (var result in stack)
            {
                yield return result;
            }
        }

        public static IAsyncEnumerable<TSource> ScanRightAwait<TSource>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TSource, ValueTask<TSource>> func)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (func is null) throw new ArgumentNullException(nameof(func));

            return source.ScanRightAwait(
                func,
                list =>
                    list.Count > 0
                        ? (list[list.Count - 1], list.Count - 1)
                        : null);
        }

        public static IAsyncEnumerable<TAccumulate> ScanRightAwait<TSource, TAccumulate>(
            this IAsyncEnumerable<TSource> source,
            TAccumulate seed,
            Func<TSource, TAccumulate, ValueTask<TAccumulate>> func)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (func is null) throw new ArgumentNullException(nameof(func));

            return source.ScanRightAwait(func, list => (seed, list.Count));
        }

        private static async IAsyncEnumerable<TResult> ScanRightAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TResult, ValueTask<TResult>> func,
            Func<List<TSource>, (TResult seed, int count)?> seeder,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            if (seeder(list) is not var (accumulator, count))
            {
                yield break;
            }

            var stack = new Stack<TResult>(count + 1);
            stack.Push(accumulator);
            while (count > 0)
            {
                count--;
                accumulator = await func(list[count], accumulator).ConfigureAwait(false);
                stack.Push(accumulator);
            }

            foreach (var result in stack)
            {
                yield return result;
            }
        }
    }
}