namespace MoreAsyncLINQ.Tests;

public abstract class AsyncEnumerableTests
{
    protected static async Task AssertEqual<T>(IEnumerable<T> expected, IAsyncEnumerable<T> actual) =>
        Assert.Equal(
            expected.ToArray(),
            await actual.ToArrayAsync());

    protected static void AssertKnownEmpty<T>(IAsyncEnumerable<T> actual) => 
        Assert.StartsWith("EmptyAsyncEnumerable", actual.GetType().Name);
}