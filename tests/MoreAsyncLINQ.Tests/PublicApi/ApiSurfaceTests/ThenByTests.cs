using static MoreAsyncLINQ.OrderByDirection;

namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ThenByTests
{
    private static readonly IAsyncEnumerable<(int, string)> _source =
        AsyncEnumerable.Range(0, 10).Select(i => (i % 3, i.ToString()));

    [Fact]
    public async Task ThenBy_Sync_Compiles()
    {
        await _source.OrderBy(x => x.Item1, Ascending)
            .ThenBy(x => x.Item2, Descending)
            .ConsumeAsync();
    }

    [Fact]
    public async Task ThenBy_SyncWithComparer_Compiles()
    {
        await _source.OrderBy(x => x.Item1, Ascending)
            .ThenBy(x => x.Item2, Comparer<string>.Default, Descending)
            .ConsumeAsync();
    }

    [Fact]
    public async Task ThenBy_Async_Compiles()
    {
        await _source.OrderBy((x, _) => ValueTask.FromResult(x.Item1), Ascending)
            .ThenBy((x, _) => ValueTask.FromResult(x.Item2), Descending)
            .ConsumeAsync();
    }

    [Fact]
    public async Task ThenBy_AsyncWithComparer_Compiles()
    {
        await _source.OrderBy((x, _) => ValueTask.FromResult(x.Item1), Ascending)
            .ThenBy((x, _) => ValueTask.FromResult(x.Item2), Comparer<string>.Default, Descending)
            .ConsumeAsync();
    }
}

