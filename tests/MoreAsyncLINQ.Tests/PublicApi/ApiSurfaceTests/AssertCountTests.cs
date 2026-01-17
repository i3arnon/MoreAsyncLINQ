namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class AssertCountTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task AssertCount_Compiles()
    {
        await _source.AssertCount(10).ConsumeAsync();
    }

    [Fact]
    public async Task AssertCount_SyncErrorSelector_Compiles()
    {
        await _source.AssertCount(
                10,
                (expected, actual) => new InvalidOperationException($"Expected {expected}, got {actual}")).
            ConsumeAsync();
    }

    [Fact]
    public async Task AssertCount_AsyncErrorSelector_Compiles()
    {
        await _source.AssertCount(
                10,
                (expected, actual, _) =>
                    ValueTask.FromResult<Exception>(new InvalidOperationException($"Expected {expected}, got {actual}"))).
            ConsumeAsync();
    }
}

