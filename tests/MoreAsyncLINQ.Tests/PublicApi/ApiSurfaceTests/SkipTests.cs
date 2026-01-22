namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class SkipTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task SkipLast_Compiles()
    {
        await _source.SkipLast(3).ConsumeAsync();
    }

    [Fact]
    public async Task SkipUntil_Sync_Compiles()
    {
        await _source.SkipUntil(x => x > 5).ConsumeAsync();
    }

    [Fact]
    public async Task SkipUntil_Async_Compiles()
    {
        await _source.SkipUntil((x, _) => ValueTask.FromResult(x > 5)).ConsumeAsync();
    }
}


