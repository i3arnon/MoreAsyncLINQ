using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    static partial class MoreAsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                Func<T1, T2, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                Func<T1, T2, T3, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, TResult>(
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

            return Core(first, second, third, fourth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                Func<T1, T2, T3, T4, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                Func<T1, T2, T3, T4, T5, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (sixth is null) throw new ArgumentNullException(nameof(sixth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, sixth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                IAsyncEnumerable<T6> sixth,
                Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();
                var sixthMemo = sixth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement,
                            sixthElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (sixth is null) throw new ArgumentNullException(nameof(sixth));
            if (seventh is null) throw new ArgumentNullException(nameof(seventh));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, sixth, seventh, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                IAsyncEnumerable<T6> sixth,
                IAsyncEnumerable<T7> seventh,
                Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();
                var sixthMemo = sixth.Memoize();
                var seventhMemo = seventh.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((seventhMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement,
                            sixthElement,
                            seventhElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            IAsyncEnumerable<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (sixth is null) throw new ArgumentNullException(nameof(sixth));
            if (seventh is null) throw new ArgumentNullException(nameof(seventh));
            if (eighth is null) throw new ArgumentNullException(nameof(eighth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                IAsyncEnumerable<T6> sixth,
                IAsyncEnumerable<T7> seventh,
                IAsyncEnumerable<T8> eighth,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();
                var sixthMemo = sixth.Memoize();
                var seventhMemo = seventh.Memoize();
                var eighthMemo = eighth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((seventhMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((eighthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var eighthElement in eighthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return resultSelector(
                            firstElement,
                            secondElement,
                            thirdElement,
                            fourthElement,
                            fifthElement,
                            sixthElement,
                            seventhElement,
                            eighthElement);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            Func<T1, T2, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                Func<T1, T2, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            Func<T1, T2, T3, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                Func<T1, T2, T3, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement,
                                thirdElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, TResult>(
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

            return Core(first, second, third, fourth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement,
                                thirdElement,
                                fourthElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                Func<T1, T2, T3, T4, T5, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement,
                                thirdElement,
                                fourthElement,
                                fifthElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, T6, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (sixth is null) throw new ArgumentNullException(nameof(sixth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, sixth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                IAsyncEnumerable<T6> sixth,
                Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();
                var sixthMemo = sixth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement,
                                thirdElement,
                                fourthElement,
                                fifthElement,
                                sixthElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (sixth is null) throw new ArgumentNullException(nameof(sixth));
            if (seventh is null) throw new ArgumentNullException(nameof(seventh));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, sixth, seventh, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                IAsyncEnumerable<T6> sixth,
                IAsyncEnumerable<T7> seventh,
                Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();
                var sixthMemo = sixth.Memoize();
                var seventhMemo = seventh.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((seventhMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement,
                                thirdElement,
                                fourthElement,
                                fifthElement,
                                sixthElement,
                                seventhElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }

        public static IAsyncEnumerable<TResult> CartesianAwait<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3> third,
            IAsyncEnumerable<T4> fourth,
            IAsyncEnumerable<T5> fifth,
            IAsyncEnumerable<T6> sixth,
            IAsyncEnumerable<T7> seventh,
            IAsyncEnumerable<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>> resultSelector)
        {
            if (first is null) throw new ArgumentNullException(nameof(first));
            if (second is null) throw new ArgumentNullException(nameof(second));
            if (third is null) throw new ArgumentNullException(nameof(third));
            if (fourth is null) throw new ArgumentNullException(nameof(fourth));
            if (fifth is null) throw new ArgumentNullException(nameof(fifth));
            if (sixth is null) throw new ArgumentNullException(nameof(sixth));
            if (seventh is null) throw new ArgumentNullException(nameof(seventh));
            if (eighth is null) throw new ArgumentNullException(nameof(eighth));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return Core(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector);

            static async IAsyncEnumerable<TResult> Core(
                IAsyncEnumerable<T1> first,
                IAsyncEnumerable<T2> second,
                IAsyncEnumerable<T3> third,
                IAsyncEnumerable<T4> fourth,
                IAsyncEnumerable<T5> fifth,
                IAsyncEnumerable<T6> sixth,
                IAsyncEnumerable<T7> seventh,
                IAsyncEnumerable<T8> eighth,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>> resultSelector,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var secondMemo = second.Memoize();
                var thirdMemo = third.Memoize();
                var fourthMemo = fourth.Memoize();
                var fifthMemo = fifth.Memoize();
                var sixthMemo = sixth.Memoize();
                var seventhMemo = seventh.Memoize();
                var eighthMemo = eighth.Memoize();

                await using ((secondMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((thirdMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fourthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((fifthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((sixthMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((seventhMemo as IAsyncDisposable).ConfigureAwait(false))
                await using ((eighthMemo as IAsyncDisposable).ConfigureAwait(false))
                {
                    await foreach (var firstElement in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var secondElement in secondMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var thirdElement in thirdMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fourthElement in fourthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var fifthElement in fifthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var sixthElement in sixthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var seventhElement in seventhMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    await foreach (var eighthElement in eighthMemo.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await resultSelector(
                                firstElement,
                                secondElement,
                                thirdElement,
                                fourthElement,
                                fifthElement,
                                sixthElement,
                                seventhElement,
                                eighthElement).
                            ConfigureAwait(false);
                    }
                }
            }
        }
    }
}