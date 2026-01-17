namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class RankByTests
{
    private static readonly IAsyncEnumerable<string> _source = AsyncEnumerable.Range(0, 10).Select(i => i.ToString());

    [Fact]
    public async Task RankBy_Sync_Compiles()
    {
        await _source.RankBy(x => x.Length).ConsumeAsync();
    }

    [Fact]
    public async Task RankBy_SyncWithComparer_Compiles()
    {
        await _source.RankBy(x => x.Length, Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task RankBy_Async_Compiles()
    {
        await _source.RankBy((x, _) => ValueTask.FromResult(x.Length)).ConsumeAsync();
    }

    [Fact]
    public async Task RankBy_AsyncWithComparer_Compiles()
    {
        await _source.RankBy((x, _) => ValueTask.FromResult(x.Length), Comparer<int>.Default).ConsumeAsync();
    }
}
