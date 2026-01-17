namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ScanByTests
{
    private static readonly IAsyncEnumerable<string> _source = AsyncEnumerable.Range(0, 10).Select(i => i.ToString());

    [Fact]
    public async Task ScanBy_Sync_Compiles()
    {
        await _source.ScanBy(
            x => x.Length,
            _ => 0,
            (state, _, item) => state + item.Length).ConsumeAsync();
    }

    [Fact]
    public async Task ScanBy_SyncWithComparer_Compiles()
    {
        await _source.ScanBy(
            x => x.Length,
            _ => 0,
            (state, _, item) => state + item.Length,
            EqualityComparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task ScanBy_Async_Compiles()
    {
        await _source.ScanBy(
            (x, _) => ValueTask.FromResult(x.Length),
            (_, _) => ValueTask.FromResult(0),
            (state, _, item, _) => ValueTask.FromResult(state + item.Length)).ConsumeAsync();
    }

    [Fact]
    public async Task ScanBy_AsyncWithComparer_Compiles()
    {
        await _source.ScanBy(
            (x, _) => ValueTask.FromResult(x.Length),
            (_, _) => ValueTask.FromResult(0),
            (state, _, item, _) => ValueTask.FromResult(state + item.Length),
            EqualityComparer<int>.Default).ConsumeAsync();
    }
}

