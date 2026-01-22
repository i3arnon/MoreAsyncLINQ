namespace MoreAsyncLINQ.Tests.PublicApi.ApiSurfaceTests;

public class SliceTests
{
    private static readonly IAsyncEnumerable<int> _source = AsyncEnumerable.Range(0, 20);

    [Fact]
    public async Task Slice_Compiles()
    {
        await _source.Slice(5, 10).ConsumeAsync();
    }
}


