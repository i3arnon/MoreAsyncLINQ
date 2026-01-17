namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PreScanTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task PreScan_Sync_Compiles()
    {
        await _source.PreScan((acc, x) => acc + x, 0).ConsumeAsync();
    }

    [Fact]
    public async Task PreScan_Async_Compiles()
    {
        await _source.PreScan((acc, x, _) => ValueTask.FromResult(acc + x), 0).ConsumeAsync();
    }
}

