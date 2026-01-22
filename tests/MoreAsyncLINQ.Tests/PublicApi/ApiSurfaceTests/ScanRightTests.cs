namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ScanRightTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task ScanRight_Sync_Compiles()
    {
        await _source.ScanRight((x, y) => x + y).ConsumeAsync();
    }

    [Fact]
    public async Task ScanRight_Async_Compiles()
    {
        await _source.ScanRight((x, y, _) => ValueTask.FromResult(x + y)).ConsumeAsync();
    }

    [Fact]
    public async Task ScanRight_SyncWithSeed_Compiles()
    {
        await _source.ScanRight(0, (x, acc) => x + acc).ConsumeAsync();
    }

    [Fact]
    public async Task ScanRight_AsyncWithSeed_Compiles()
    {
        await _source.ScanRight(0, (x, acc, _) => ValueTask.FromResult(x + acc)).ConsumeAsync();
    }
}


