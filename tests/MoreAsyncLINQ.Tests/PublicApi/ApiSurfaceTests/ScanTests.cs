namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ScanTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Scan_Sync_Compiles()
    {
        await _source.Scan((x, y) => x + y).ConsumeAsync();
    }

    [Fact]
    public async Task Scan_Async_Compiles()
    {
        await _source.Scan((x, y, _) => ValueTask.FromResult(x + y)).ConsumeAsync();
    }

    [Fact]
    public async Task Scan_SyncWithSeed_Compiles()
    {
        await _source.Scan(0, (acc, x) => acc + x).ConsumeAsync();
    }

    [Fact]
    public async Task Scan_AsyncWithSeed_Compiles()
    {
        await _source.Scan(0, (acc, x, _) => ValueTask.FromResult(acc + x)).ConsumeAsync();
    }
}


