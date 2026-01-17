using static System.Linq.AsyncEnumerable;

namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ExceptByTests
{
    private static readonly IAsyncEnumerable<int> _first = Range(0, 10);
    private static readonly IAsyncEnumerable<int> _second = Range(0, 10);

    [Fact]
    public async Task ExceptBy_Sync_Compiles()
    {
        await _first.ExceptBy(_second, x => x).ConsumeAsync();
    }

    [Fact]
    public async Task ExceptBy_SyncWithComparer_Compiles()
    {
        await _first.ExceptBy(_second, x => x, EqualityComparer<int>.Default).ConsumeAsync();
    }

    [Fact]
    public async Task ExceptBy_Async_Compiles()
    {
        await _first.ExceptBy(_second, (x, _) => ValueTask.FromResult(x)).ConsumeAsync();
    }

    [Fact]
    public async Task ExceptBy_AsyncWithComparer_Compiles()
    {
        await _first.ExceptBy(_second, (x, _) => ValueTask.FromResult(x), EqualityComparer<int>.Default).ConsumeAsync();
    }
}

