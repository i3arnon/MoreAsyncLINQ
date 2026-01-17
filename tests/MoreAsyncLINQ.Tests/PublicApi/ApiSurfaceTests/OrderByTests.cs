namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class OrderByTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task OrderBy_Sync_Compiles()
    {
        await _source.OrderBy(x => x, OrderByDirection.Ascending).ConsumeAsync();
    }

    [Fact]
    public async Task OrderBy_SyncWithComparer_Compiles()
    {
        await _source.OrderBy(x => x, Comparer<int>.Default, OrderByDirection.Descending).ConsumeAsync();
    }

    [Fact]
    public async Task OrderBy_Async_Compiles()
    {
        await _source.OrderBy((x, _) => ValueTask.FromResult(x), OrderByDirection.Ascending).ConsumeAsync();
    }

    [Fact]
    public async Task OrderBy_AsyncWithComparer_Compiles()
    {
        await _source.OrderBy((x, _) => ValueTask.FromResult(x), Comparer<int>.Default, OrderByDirection.Descending).ConsumeAsync();
    }
}

