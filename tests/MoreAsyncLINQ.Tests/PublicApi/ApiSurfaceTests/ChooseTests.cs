namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ChooseTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Choose_Sync_Compiles()
    {
        await _source.Choose(x => (x > 0, x.ToString())).ConsumeAsync();
    }

    [Fact]
    public async Task Choose_Async_Compiles()
    {
        await _source.Choose((x, _) => ValueTask.FromResult((x > 0, x.ToString()))).ConsumeAsync();
    }
}