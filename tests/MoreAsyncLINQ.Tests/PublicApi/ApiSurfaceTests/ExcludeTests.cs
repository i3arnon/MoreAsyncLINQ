namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class ExcludeTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 10);

    [Fact]
    public async Task Exclude_Compiles()
    {
        await _source.Exclude(2, 5).ConsumeAsync();
    }
}

