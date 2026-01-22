namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class IndexTests
{
    private static readonly IAsyncEnumerable<string> _source = AsyncEnumerable.Range(0, 5).Select(i => i.ToString());

    [Fact]
    public async Task Index_Compiles()
    {
        await _source.Index().ConsumeAsync();
        await _source.Index(10).ConsumeAsync();
    }
}


