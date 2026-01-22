namespace MoreAsyncLINQ.Tests;

public static class BreakingFunc
{
    public static Func<T, TResult> Of<T, TResult>() =>
        _ => throw new InvalidOperationException("Function should not be invoked.");

    public static Func<T1, T2, TResult> Of<T1, T2, TResult>() =>
        (_, _) => throw new InvalidOperationException("Function should not be invoked.");

    public static Func<T1, T2, T3, TResult> Of<T1, T2, T3, TResult>() =>
        (_, _, _) => throw new InvalidOperationException("Function should not be invoked.");

    public static Func<T, CancellationToken, ValueTask<TResult>> OfAsync<T, TResult>() =>
        (_, _) => throw new InvalidOperationException("Function should not be invoked.");

    public static Func<T1, T2, CancellationToken, ValueTask<TResult>> OfAsync<T1, T2, TResult>() =>
        (_, _, _) => throw new InvalidOperationException("Function should not be invoked.");

    public static Func<T1, T2, T3, CancellationToken, ValueTask<TResult>> OfAsync<T1, T2, T3, TResult>() =>
        (_, _, _, _) => throw new InvalidOperationException("Function should not be invoked.");
}