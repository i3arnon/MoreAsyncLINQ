using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    static partial class MoreAsyncEnumerable
    {
        private static async IAsyncEnumerable<TResult> Zip<T1, T2, T3, T4, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3>? third,
            IAsyncEnumerable<T4>? fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector,
            int limit,
            Func<bool[], Exception>? errorSelector = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ConfiguredCancelableAsyncEnumerable<T1>.Enumerator? firstEnumerator = null;
            ConfiguredCancelableAsyncEnumerable<T2>.Enumerator? secondEnumerator = null;
            ConfiguredCancelableAsyncEnumerable<T3>.Enumerator? thirdEnumerator = null;
            ConfiguredCancelableAsyncEnumerable<T4>.Enumerator? fourthEnumerator = null;
            var terminations = 0;

            try
            {
                firstEnumerator = first.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                secondEnumerator = second.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                thirdEnumerator = third?.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                fourthEnumerator = fourth?.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                while (true)
                {
                    T1 firstElement;
                    T2 secondElement;
                    T3 thirdElement;
                    T4 fourthElement;
                    bool terminated;
                    var calls = 0;

                    calls++;
                    (firstElement, terminated) = await GetElementAsync(firstEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        firstEnumerator = null;
                        Validate(calls);
                    }

                    calls++;
                    (secondElement, terminated) = await GetElementAsync(secondEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        secondEnumerator = null;
                        Validate(calls);
                    }

                    calls++;
                    (thirdElement, terminated) = await GetElementAsync(thirdEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        thirdEnumerator = null;
                        Validate(calls);
                    }

                    calls++;
                    (fourthElement, terminated) = await GetElementAsync(fourthEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        fourthEnumerator = null;
                        Validate(calls);
                    }

                    if (terminations <= limit)
                    {
                        yield return resultSelector(firstElement, secondElement, thirdElement, fourthElement);
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
            finally
            {
                if (firstEnumerator is not null)
                {
                    await firstEnumerator.Value.DisposeAsync();
                }

                if (secondEnumerator is not null)
                {
                    await secondEnumerator.Value.DisposeAsync();
                }

                if (thirdEnumerator is not null)
                {
                    await thirdEnumerator.Value.DisposeAsync();
                }

                if (fourthEnumerator is not null)
                {
                    await fourthEnumerator.Value.DisposeAsync();
                }
            }

            async ValueTask<(T element, bool terminated)> GetElementAsync<T>(ConfiguredCancelableAsyncEnumerable<T>.Enumerator? enumerator)
            {
                if (enumerator is null || terminations > limit)
                {
                    return (default!, false);
                }

                if (await enumerator.Value.MoveNextAsync())
                {
                    var element = enumerator.Value.Current;
                    return (element, false);
                }

                await enumerator.Value.DisposeAsync();
                terminations++;
                return (default!, true);
            }

            void Validate(int calls)
            {
                if (errorSelector is not null && terminations > 0 && terminations < calls)
                {
                    throw errorSelector(
                        new[]
                        {
                            firstEnumerator is null,
                            secondEnumerator is null,
                            thirdEnumerator is null,
                            fourthEnumerator is null
                        });
                }
            }
        }

        private static async IAsyncEnumerable<TResult> ZipAwait<T1, T2, T3, T4, TResult>(
            this IAsyncEnumerable<T1> first,
            IAsyncEnumerable<T2> second,
            IAsyncEnumerable<T3>? third,
            IAsyncEnumerable<T4>? fourth,
            Func<T1, T2, T3, T4, ValueTask<TResult>> resultSelector,
            int limit,
            Func<bool[], Exception>? errorSelector = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ConfiguredCancelableAsyncEnumerable<T1>.Enumerator? firstEnumerator = null;
            ConfiguredCancelableAsyncEnumerable<T2>.Enumerator? secondEnumerator = null;
            ConfiguredCancelableAsyncEnumerable<T3>.Enumerator? thirdEnumerator = null;
            ConfiguredCancelableAsyncEnumerable<T4>.Enumerator? fourthEnumerator = null;
            var terminations = 0;

            try
            {
                firstEnumerator = first.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                secondEnumerator = second.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                thirdEnumerator = third?.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();
                fourthEnumerator = fourth?.WithCancellation(cancellationToken).ConfigureAwait(false).GetAsyncEnumerator();

                while (true)
                {
                    T1 firstElement;
                    T2 secondElement;
                    T3 thirdElement;
                    T4 fourthElement;
                    bool terminated;
                    var calls = 0;

                    calls++;
                    (firstElement, terminated) = await GetElementAsync(firstEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        firstEnumerator = null;
                        Validate(calls);
                    }

                    calls++;
                    (secondElement, terminated) = await GetElementAsync(secondEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        secondEnumerator = null;
                        Validate(calls);
                    }

                    calls++;
                    (thirdElement, terminated) = await GetElementAsync(thirdEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        thirdEnumerator = null;
                        Validate(calls);
                    }

                    calls++;
                    (fourthElement, terminated) = await GetElementAsync(fourthEnumerator).ConfigureAwait(false);
                    if (terminated)
                    {
                        fourthEnumerator = null;
                        Validate(calls);
                    }

                    if (terminations <= limit)
                    {
                        yield return await resultSelector(firstElement, secondElement, thirdElement, fourthElement).ConfigureAwait(false);
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
            finally
            {
                if (firstEnumerator is not null)
                {
                    await firstEnumerator.Value.DisposeAsync();
                }

                if (secondEnumerator is not null)
                {
                    await secondEnumerator.Value.DisposeAsync();
                }

                if (thirdEnumerator is not null)
                {
                    await thirdEnumerator.Value.DisposeAsync();
                }

                if (fourthEnumerator is not null)
                {
                    await fourthEnumerator.Value.DisposeAsync();
                }
            }

            async ValueTask<(T element, bool terminated)> GetElementAsync<T>(ConfiguredCancelableAsyncEnumerable<T>.Enumerator? enumerator)
            {
                if (enumerator is null || terminations > limit)
                {
                    return (default!, false);
                }

                if (await enumerator.Value.MoveNextAsync())
                {
                    var element = enumerator.Value.Current;
                    return (element, false);
                }

                await enumerator.Value.DisposeAsync();
                terminations++;
                return (default!, true);
            }

            void Validate(int calls)
            {
                if (errorSelector is not null && terminations > 0 && terminations < calls)
                {
                    throw errorSelector(
                        new[]
                        {
                            firstEnumerator is null,
                            secondEnumerator is null,
                            thirdEnumerator is null,
                            fourthEnumerator is null
                        });
                }
            }
        }
    }
}