namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class IndexByTests
{
    private static readonly IAsyncEnumerable<string> _source = AsyncEnumerable.Range(0, 10).Select(i => i.ToString());

    [Fact]
    public async Task IndexBy_Sync_Compiles()
    {
        await _source.IndexBy(x => x.Length).ConsumeAsync();
    }

    [Fact]
    public async Task IndexBy_SyncWithComparer_Compiles()
    {
        await _source.IndexBy(x => x.Length, EqualityComparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task IndexBy_Async_Compiles()
    {
        await _source.IndexBy((x, _) => ValueTask.FromResult(x.Length)).ConsumeAsync();
    }

    [Fact]
    public async Task IndexBy_AsyncWithComparer_Compiles()
    {
        await _source.IndexBy((x, _) => ValueTask.FromResult(x.Length), EqualityComparer<int>.Default).ConsumeAsync();
    }
}

