namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class TakeTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 20);

    [Fact]
    public async Task TakeEvery_Compiles()
    {
        await _source.TakeEvery(3).ConsumeAsync();
    }

    [Fact]
    public async Task TakeLast_Compiles()
    {
        await _source.TakeLast(5).ConsumeAsync();
    }

    [Fact]
    public async Task TakeUntil_Sync_Compiles()
    {
        await _source.TakeUntil(x => x > 10).ConsumeAsync();
    }

    [Fact]
    public async Task TakeUntil_Async_Compiles()
    {
        await _source.TakeUntil((x, _) => ValueTask.FromResult(x > 10)).ConsumeAsync();
    }
}

