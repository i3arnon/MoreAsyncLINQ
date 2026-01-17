namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class AssertTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Assert_Sync_Compiles()
    {
        await _source.Assert(x => x >= 0).ConsumeAsync();
    }

    [Fact]
    public async Task Assert_SyncWithErrorSelector_Compiles()
    {
        await _source.Assert(x => x >= 0, x => new InvalidOperationException($"Invalid: {x}")).ConsumeAsync();
    }

    [Fact]
    public async Task Assert_Async_Compiles()
    {
        await _source.Assert((x, _) => ValueTask.FromResult(x >= 0)).ConsumeAsync();
    }

    [Fact]
    public async Task Assert_AsyncWithErrorSelector_Compiles()
    {
        await _source.Assert(
            (x, _) => ValueTask.FromResult(x >= 0), 
            (x, _) => ValueTask.FromResult<Exception>(new InvalidOperationException($"Invalid: {x}"))).ConsumeAsync();
    }
}

