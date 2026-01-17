namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class PartialSortByTests
{
    private static readonly IAsyncEnumerable<string> _source = AsyncEnumerable.Range(0, 100).Select(i => i.ToString());

    [Fact]
    public async Task PartialSortBy_Sync_Compiles()
    {
        await _source.PartialSortBy(10, x => x.Length).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_SyncWithComparer_Compiles()
    {
        await _source.PartialSortBy(10, x => x.Length, Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_SyncWithDirection_Compiles()
    {
        await _source.PartialSortBy(10, x => x.Length, OrderByDirection.Descending).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_SyncWithComparerAndDirection_Compiles()
    {
        await _source.PartialSortBy(10, x => x.Length, Comparer<int>.Default, OrderByDirection.Descending).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_Async_Compiles()
    {
        await _source.PartialSortBy(10, (x, _) => ValueTask.FromResult(x.Length)).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_AsyncWithComparer_Compiles()
    {
        await _source.PartialSortBy(10, (x, _) => ValueTask.FromResult(x.Length), Comparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_AsyncWithDirection_Compiles()
    {
        await _source.PartialSortBy(10, (x, _) => ValueTask.FromResult(x.Length), OrderByDirection.Descending).ConsumeAsync();
    }

    [Fact]
    public async Task PartialSortBy_AsyncWithComparerAndDirection_Compiles()
    {
        await _source.PartialSortBy(10, (x, _) => ValueTask.FromResult(x.Length), Comparer<int>.Default, OrderByDirection.Descending).ConsumeAsync();
    }
}

