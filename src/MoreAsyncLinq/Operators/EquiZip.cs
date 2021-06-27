using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        private static readonly string[] _ordinalNumbers =
        {
            "First",
            "Second",
            "Third",
            "Fourth",
        };

        public static IAsyncEnumerable<TResult> EquiZip<T1, T2, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.EquiZipCore<T1, T2, T2, T2, TResult>(
                second,
                third: null,
                fourth: null,
                (firstElement, secondElement, _, _) => resultSelector(firstElement, secondElement));
        }

        public static IAsyncEnumerable<TResult> EquiZip<T1, T2, T3, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.EquiZipCore<T1, T2, T3, T3, TResult>(
                second,
                third,
                fourth: null,
                (firstElement, secondElement, thirdElement, _) => resultSelector(firstElement, secondElement, thirdElement));
        }

        public static IAsyncEnumerable<TResult> EquiZip<T1, T2, T3, T4, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.EquiZipCore(second, third, fourth, resultSelector);
        }

        private static IAsyncEnumerable<TResult> EquiZipCore<T1, T2, T3, T4, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3>? third,
            IAsyncEnumerable<T4>? fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            var limit = 1;

            if (third is not null)
            {
                limit++;
            }

            if (fourth is not null)
            {
                limit++;
            }

            return first.Zip(
                second,
                third,
                fourth,
                resultSelector,
                limit,
                EquiZipErrorSelector);
        }

        public static IAsyncEnumerable<TResult> EquiZipAwait<T1, T2, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.EquiZipAwaitCore<T1, T2, T2, T2, TResult>(
                second,
                third: null,
                fourth: null,
                (firstElement, secondElement, _, _) => resultSelector(firstElement, secondElement));
        }

        public static IAsyncEnumerable<TResult> EquiZipAwait<T1, T2, T3, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.EquiZipAwaitCore<T1, T2, T3, T3, TResult>(
                second,
                third,
                fourth: null,
                (firstElement, secondElement, thirdElement, _) => resultSelector(firstElement, secondElement, thirdElement));
        }

        public static IAsyncEnumerable<TResult> EquiZipAwait<T1, T2, T3, T4, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return first.EquiZipAwaitCore(second, third, fourth, resultSelector);
        }

        private static IAsyncEnumerable<TResult> EquiZipAwaitCore<T1, T2, T3, T4, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3>? third,
            IAsyncEnumerable<T4>? fourth,
            Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector)
        {
            var limit = 1;

            if (third is not null)
            {
                limit++;
            }

            if (fourth is not null)
            {
                limit++;
            }

            return first.ZipAwait(
                second,
                third,
                fourth,
                resultSelector,
                limit,
                EquiZipErrorSelector);
        }

        private static Exception EquiZipErrorSelector(bool[] terminations)
        {
            var index =
                terminations.
                    Select((termination, index) => (termination, index)).
                    First(tuple => tuple.termination).
                    index;
            return new InvalidOperationException($"{_ordinalNumbers[index]} sequence too short.");
        }
    }
}