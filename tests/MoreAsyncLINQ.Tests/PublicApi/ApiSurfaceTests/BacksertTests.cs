namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class BacksertTests
{
    private static readonly IAsyncEnumerable<int> _first = AsyncEnumerable.Range(0, 10);
    private static readonly IAsyncEnumerable<int> _second = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Backsert_Compiles()
    {
        await _first.Backsert(_second, 2).ConsumeAsync();
    }
}

